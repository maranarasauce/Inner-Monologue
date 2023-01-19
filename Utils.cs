using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

namespace ProjectProphet
{
    public static class Utils
    {
        public static bool notifLaunched;
        public static StyleHUD shud { get => StyleHUD.Instance; }
        public static GunControl gc { get => GunControl.Instance; }
        public static WeaponCharges wc { get => WeaponCharges.Instance; }
        public static TimeController timeController { get => TimeController.Instance; }
        public const string bundleProjectPath = "Assets\\Maranara\\Projects\\InnerMonologue";

        public static string PackedPath {
            get
            {
                if (string.IsNullOrEmpty(_packedPath))
                    _packedPath = $"{Path.GetDirectoryName(Application.dataPath)}\\BepInEx\\plugins\\Inner Monologue\\bundles";

                return _packedPath;
            }
        }
        private static string _packedPath;

        public static void InitShaders()
        {
            Shader vertShader = Shader.Find("psx/vertexlit/vertexlit");
            shaders = new Dictionary<string, Shader>();
            shaders.Add("psx/vertexlit/vertexlit", vertShader);
            shaders.Add("Standard", vertShader);
            shaders.Add("psx/unlit/transparent/nocull-fresnel", Shader.Find("psx/unlit/transparent/nocull-fresnel"));
            shaders.Add("psx/unlit/ambient", Shader.Find("psx/unlit/ambient"));
            shaders.Add("psx/unlit/transparent/unlit-scrolling", Shader.Find("psx/unlit/transparent/unlit-scrolling"));
        }

        public static void FixShaders(this GameObject go)
        {
            Renderer[] smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            Renderer[] mrs = go.GetComponentsInChildren<MeshRenderer>();
            foreach (Renderer smr in smrs)
            {
                FixRenderer(smr);
            }
            foreach (Renderer mr in mrs)
            {
                FixRenderer(mr);
            }
        }

        public static Dictionary<string, Shader> shaders;
        private static void FixRenderer(Renderer rend)
        {
            foreach (Material mat in rend.sharedMaterials)
            {
                if (mat == null)
                    continue;

                if (shaders.ContainsKey(mat.shader.name))
                {
                    mat.shader = shaders[mat.shader.name];
                }

            }
        }

        public static T FindScriptInScene<T>()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] roots = scene.GetRootGameObjects();
            foreach (GameObject root in roots)
            {
                T type = root.GetComponentInChildren<T>(true);
                if (type != null)
                    return type;
            }
            return default(T);
        }

        public static T[] FindScriptsInScene<T>()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] roots = scene.GetRootGameObjects();
            List<T> allScripts = new List<T>();
            foreach (GameObject root in roots)
            {
                T[] type = root.GetComponentsInChildren<T>(true);
                allScripts.AddRange(type);
            }
            return allScripts.ToArray();
        }

        public static Vector3 ClampMagnitude(this Vector3 v, float max, float min)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * (double)max) return v.normalized * max;
            else if (sm < (double)min * (double)min) return v.normalized * min;
            return v;
        }

        public static ObjectActivator LineOnActivate(GameObject target, GameObject lineObj)
        {
            return LineOnActivate(target, lineObj, 0.1f);
        }

        public static ObjectActivator LineOnActivate(GameObject target, GameObject lineObj, float delay)
        {
            lineObj.SetActive(false);
            lineObj.GetComponent<AudioSource>().playOnAwake = true;

            GameObject obacObj = new GameObject("Line OBAC");
            obacObj.transform.parent = target.transform;
            obacObj.transform.localPosition = Vector3.zero;
            ObjectActivator obac = obacObj.AddComponent<ObjectActivator>();
            obac.delay = delay;
            obac.events = new UltrakillEvent();
            obac.events.toActivateObjects = new GameObject[1];
            obac.events.toActivateObjects[0] = lineObj.gameObject;
            return obac;
        }
        public static UnityAction LineEvent(GameObject lineObj)
        {
            lineObj.SetActive(false);
            lineObj.GetComponent<AudioSource>().playOnAwake = true;

            return () => lineObj.SetActive(true);
        }

        public static void DeathLine(EnemyIdentifier id, GameObject line, float delay)
        {
            line.SetActive(false);
            line.GetComponent<AudioSource>().playOnAwake = true;

            GameObject obacObj = new GameObject("Line OBAC");
            obacObj.SetActive(false);
            ObjectActivator obac = obacObj.AddComponent<ObjectActivator>();
            obac.delay = delay;

            obac.events = new UltrakillEvent();
            obac.events.toActivateObjects = new GameObject[1];
            obac.events.toActivateObjects[0] = line.gameObject;

            List<GameObject> obj = new List<GameObject>();
            if (id.activateOnDeath != null)
                obj.AddRange(id.activateOnDeath);
            obj.Add(obacObj);
            id.activateOnDeath = obj.ToArray();
        }


        public static void HurtButDontKill(int damage)
        {
            NewMovement nmov = NewMovement.Instance;
            int realDamage = 0;
            if (nmov.hp == 1)
                return;

            if (nmov.hp <= damage)
            {
                realDamage = nmov.hp - 1;
            }
            else realDamage = damage;

            nmov.GetHurt(realDamage, false);
        }

        public static AssetBundle curStageBundle { get; private set; }
        public static GameObject LoadAndInstantiateStage(string levelName)
        {
            string bundleDir = $"{Utils.PackedPath}\\{levelName}.lvl";

            if (curStageBundle == null)
            {
                curStageBundle = AssetBundle.LoadFromFile(bundleDir);
            }
            else
            {
                Debug.Log(curStageBundle.name);
                if (curStageBundle.name != $"{levelName}.lvl")
                {
                    if (curStageBundle != null)
                        curStageBundle.Unload(true);
                    curStageBundle = AssetBundle.LoadFromFile(bundleDir);
                    curStageBundle.name = levelName;
                    Debug.Log("Loading stage bundle...");
                }
                else
                {
                    Debug.Log("Stage bundle already loaded");
                }
            }

            if (curStageBundle.isStreamedSceneAssetBundle)
                return null;

            GameObject prefab = curStageBundle.LoadAsset<GameObject>($"{Utils.bundleProjectPath}/LevelAddons/{levelName}.prefab");
            GameObject inst = GameObject.Instantiate(prefab);
            return inst;
        }
    }
}
