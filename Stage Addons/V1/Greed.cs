using ProjectProphet.Patching;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectProphet.StageAddons
{
    public static class Greed
    {
        [StageAddon(42)]
        public static void Sisyphus()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("42");
            GameObject line = inst.transform.GetChild(1).gameObject;

            Sisyphus mass = Utils.FindScriptInScene<Sisyphus>();

            Utils.LineOnActivate(mass.gameObject, line);
            mass.GetComponent<Machine>().onDeath.AddListener(Utils.LineEvent(inst.transform.GetChild(2).gameObject));
        }

        [StageAddon(43)]
        public static void TorchOn()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("43");
            GameObject line = inst.transform.GetChild(1).gameObject;
            line.SetActive(false);
            line.GetComponent<AudioSource>().playOnAwake = true;

            ItemPlaceZone[] zones = Utils.FindScriptsInScene<ItemPlaceZone>();
            foreach (ItemPlaceZone zone in zones)
            {
                if (zone.transform.root.name == "7 - Generator Room")
                {
                    List<GameObject> objs = new List<GameObject>();
                    objs.AddRange(zone.activateOnSuccess);
                    objs.Add(line);
                    zone.activateOnSuccess = objs.ToArray();
                }
            }

            Mandalore man = Utils.FindScriptInScene<Mandalore>();
            Utils.DeathLine(man.GetComponent<EnemyIdentifier>(), inst.transform.GetChild(2).gameObject, 3f);
        }

        [StageAddon(44)]
        public static void V22()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("44");
            GameObject line = inst.transform.GetChild(2).gameObject;

            HudMessage[] huds = Utils.FindScriptsInScene<HudMessage>();
            foreach (HudMessage hud in huds)
            {
                if (hud.gameObject.name == "Message" && hud.timed)
                {
                    Utils.LineOnActivate(hud.gameObject, line);
                    break;
                }
            }

            
        }
    }
}
