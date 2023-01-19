using BepInEx;
using ProjectProphet.Patching;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

namespace ProjectProphet.StageAddons
{
    public static class Wrath
    {
        [StageAddon(52)]
        public static void Ferryman()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("52");
            GameObject line = inst.transform.GetChild(0).gameObject;

            Ferryman mass = Utils.FindScriptInScene<Ferryman>();

            Utils.LineOnActivate(mass.gameObject, line);
            Idol[] idolu = Utils.FindScriptsInScene<Idol>();
            foreach (Idol idol in idolu)
            {
                if (idol.transform.root.name == "3 - Ferryman's Cabin")
                {
                    idol.GetComponent<EnemyIdentifier>().onDeath.AddListener(Utils.LineEvent(inst.transform.GetChild(1).gameObject));
                    break;
                }
            }

            ItemPlaceZone jakitoZone = null;
            ItemPlaceZone shipZone = null;
            ItemPlaceZone[] zones = Utils.FindScriptsInScene<ItemPlaceZone>();
            foreach (ItemPlaceZone zone in zones)
            {
                if (zone.transform.root.name == "JakitoCaged" && zone.transform.parent.name == "Altar")
                {
                    jakitoZone = zone;
                } else if (zone.transform.root.name == "8 - Ship")
                {
                    shipZone = zone;
                }
            }

            //Florp pickup
            florpLoop = inst.transform.GetChild(2).gameObject;
            florpId = shipZone.GetComponentInChildren<ItemIdentifier>();

            //Florp death
            List<GameObject> objDeath = new List<GameObject>();
            objDeath.AddRange(jakitoZone.activateOnSuccess);
            objDeath.Add(inst.transform.GetChild(3).gameObject);
            jakitoZone.activateOnSuccess = objDeath.ToArray();

            List<GameObject> objDeath2 = new List<GameObject>();
            objDeath2.AddRange(jakitoZone.deactivateOnSuccess);
            objDeath2.Add(florpLoop);
            jakitoZone.deactivateOnSuccess = objDeath2.ToArray();

            ThreadingHelper.Instance.StartCoroutine(WaitWaitWait(florpLoop));
        }

        private static IEnumerator WaitWaitWait(GameObject loooop)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            loooop.SetActive(false);
        }

        private static GameObject florpLoop;
        private static ItemIdentifier florpId;
        public static void PickUpFlorpCheck(ItemIdentifier id)
        {
            if (florpLoop == null || florpId == null)
                return;

            if (id != florpId)
                return;

            florpLoop.SetActive(florpId.pickedUp);
        }

        public static void PlaceFlorpCheck()
        {

            if (florpLoop == null || florpId == null)
                return;


            florpLoop.SetActive(false);
        }

        [StageAddon(53)]
        public static void RocketLauncherPickup()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("53");
            GameObject line = inst.transform.GetChild(1).gameObject;
            line.SetActive(false);
            AudioSource src = line.GetComponent<AudioSource>();
            src.playOnAwake = true;

            WeaponPickUp mass = Utils.FindScriptInScene<WeaponPickUp>();
            mass.activateOnPickup = line;
        }

        [StageAddon(54)]
        public static void Leviathan()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("54");
            GameObject line = inst.transform.GetChild(0).gameObject;

            LeviathanController mass = Utils.FindScriptInScene<LeviathanController>();

            Utils.LineOnActivate(mass.gameObject, line, 1f);
            EnemyIdentifier eid = mass.GetComponent<EnemyIdentifier>();
            Utils.DeathLine(eid, inst.transform.GetChild(1).gameObject, 0.7f);
        }
    }
}
