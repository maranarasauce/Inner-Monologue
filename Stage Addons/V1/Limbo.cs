using BepInEx;
using ProjectProphet.Patching;
using System.Collections;
using UnityEngine;

namespace ProjectProphet.StageAddons
{
    public static class Limbo
    {
        [StageAddon(12)]
        public static void BurningWorld()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("12");
            GameObject line = inst.transform.GetChild(0).gameObject;
            line.SetActive(false);
            GameObject.Find("FirstRoom").transform.Find("Room").Find("FinalDoor").GetComponent<FinalDoor>().doors[0].onFullyOpened.AddListener(() => FinalizeScene(line));

            CancerousRodent[] rodents = Utils.FindScriptsInScene<CancerousRodent>();
            foreach (CancerousRodent rodent in rodents)
            {
                if (rodent.harmless)
                {
                    Utils.DeathLine(rodent.GetComponent<EnemyIdentifier>(), inst.transform.GetChild(1).gameObject, 0.5f);
                    break;
                }
            }
        }

        private static void FinalizeScene(GameObject toActivate)
        {
            Transform entry = GameObject.Find("1 - First Room").transform;
            GameObject obac = entry.GetChild(0).GetChild(2).gameObject;
            Utils.LineOnActivate(obac, toActivate, 0.5f);
        }

        [StageAddon(13)]
        public static void Sister()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("13");
            GameObject line = inst.transform.GetChild(0).gameObject;

            Mass mass = Utils.FindScriptInScene<Mass>();

            Utils.LineOnActivate(mass.gameObject, line, 0.3f);
        }

        [StageAddon(14)]
        public static void Mirror()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("14");
            GameObject line = inst.transform.GetChild(0).gameObject;

            V2 mass = Utils.FindScriptInScene<V2>();

            Utils.LineOnActivate(mass.gameObject, line, 0.6f);

            WeaponPickUp wpu = Utils.FindScriptInScene<WeaponPickUp>();
            Utils.LineOnActivate(wpu.activateOnPickup, inst.transform.GetChild(1).gameObject);
        }

    }
}
