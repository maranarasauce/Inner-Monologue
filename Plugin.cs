using BepInEx;
using HarmonyLib;
using ProjectProphet.Patching;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.UI;
using ProjectProphet.StageAddons;
using Random = UnityEngine.Random;

namespace ProjectProphet
{
    [BepInPlugin("maranara_inner_monologue", "Inner Monologue", "0.0.1")]
    public class InternalMonologue : BaseUnityPlugin
    {
        public static Harmony harmony;
        private void OnEnable()
        {
            Utils.InitShaders();

            harmony = new Harmony("maranara_inner_monologue");

            harmony.PatchAll(typeof(InternalMonologue));

            InitStageAddons();

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            //var cheatsHarmony = new Harmony("maranara_project_prophet-cheats");
            //cheatsHarmony.PatchAll(typeof(CheatsPatch));

            string bundleDir = $"{Utils.PackedPath}\\death.bd";
            AssetBundle deathBundle = AssetBundle.LoadFromFile(bundleDir);
            deathBundle.hideFlags = HideFlags.DontUnloadUnusedAsset;
            deathVoicePrefab = deathBundle.LoadAsset<GameObject>($"{Utils.bundleProjectPath}/LevelAddons/Death.prefab");
            ultrakillVoicePrefab = deathBundle.LoadAsset<GameObject>($"{Utils.bundleProjectPath}/LevelAddons/Ultrakill.prefab");
        }

        [HarmonyPatch(typeof(NewMovement), "Start")]
        [HarmonyPostfix]
        public static void NewMovStart(NewMovement __instance)
        {
            GameObject curVox = GameObject.Instantiate(deathVoicePrefab);
            curDeathVoice = curVox.GetComponent<GabrielVoice>();
            GameObject curVox2 = GameObject.Instantiate(ultrakillVoicePrefab);
            curStyleVoice = curVox2.GetComponent<GabrielVoice>();
        }

        [HarmonyPatch(typeof(Punch), "ForceHold")]
        [HarmonyPostfix]
        public static void PunchHold(Punch __instance, ItemIdentifier itid)
        {
            Wrath.PickUpFlorpCheck(itid);
        }
        
        [HarmonyPatch(typeof(Punch), "PlaceHeldObject")]
        [HarmonyPostfix]
        public static void PlaceFlorpCheck(Punch __instance)
        {
            Wrath.PlaceFlorpCheck();
        }


        [HarmonyPatch(typeof(NewMovement), "GetHurt")]
        [HarmonyPostfix]
        public static void GetHurt(NewMovement __instance)
        {
            if (__instance.dead)
            {
                curDeathVoice.Taunt();
            }
        }

        [HarmonyPatch(typeof(NewMovement), "Update")]
        [HarmonyPostfix]
        public static void RankUpdate(NewMovement __instance)
        {
            if (styleTimer > 0f)
                styleTimer -= Time.deltaTime;
        }

        [HarmonyPatch(typeof(StyleHUD), "AscendRank")]
        [HarmonyPostfix]
        public static void RankUp(StyleHUD __instance)
        {
            if (__instance.currentRank.sprite.name == "RankU" && styleTimer <= 0f)
            {
                styleTimer = 15f;
                curStyleVoice.Taunt();
            }
        }

        private static float styleTimer;
        static GabrielVoice curDeathVoice;
        static GabrielVoice curStyleVoice;
        static GameObject deathVoicePrefab;
        static GameObject ultrakillVoicePrefab;
        private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            string name = arg1.name;
            name = name.Substring(name.Length - 3);
            name = name.Remove(1, 1);

            if (int.TryParse(name, out int result))
            {
                string bundleDir = $"{Utils.PackedPath}\\{name}.lvl";
                if (sceneToBundle.TryGetValue(result, out StageAddon addon))
                {
                    addon.RunAddon();
                }
                else if (File.Exists(bundleDir))
                {
                    Utils.LoadAndInstantiateStage(name);
                }
            }
            else if(specialToBundle.TryGetValue(name, out StageAddon addon))
            {
                addon.RunAddon();
            }
        }

        public static Dictionary<int, StageAddon> sceneToBundle;
        public static Dictionary<string, StageAddon> specialToBundle;
        private void InitStageAddons()
        {
            sceneToBundle = new Dictionary<int, StageAddon>();
            specialToBundle = new Dictionary<string, StageAddon>();
            Assembly assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace != "ProjectProphet.StageAddons")
                    continue;

                MethodInfo[] methods = type.GetMethods();
                foreach (MethodInfo method in methods)
                {
                    StageAddon addon = method.GetCustomAttribute<StageAddon>();
                    if (addon != null)
                    {
                        addon.SetMethodInfo(method);
                        if (addon.stage != 0)
                            sceneToBundle.Add(addon.stage, addon);
                        else specialToBundle.Add(addon.specialStageName, addon);
                    }
                }
            }
        }
    }
}