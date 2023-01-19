using ProjectProphet.Patching;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ProjectProphet.StageAddons
{
    public static class Special
    {
        [StageAddon("0S")]
        public static void SomethingWicked()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("0S");
            GameObject entry = GameObject.Find("MessageParent").transform.GetChild(0).gameObject;
            Utils.LineOnActivate(entry, inst.transform.GetChild(0).gameObject, 4f);
        }

        [StageAddon("1S")]
        public static void Puzzle()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("1S");
        }

        [StageAddon("4S")]
        public static void Clash()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("4S");
            GameObject line = inst.transform.GetChild(0).gameObject;
            line.SetActive(false);

            List<GameObject> obj = new List<GameObject>();
            Door dr = GameObject.Find("FirstRoom Secret").transform.Find("Room").Find("FinalDoor").GetComponent<FinalDoor>().doors[0];
            obj.AddRange(dr.activatedRooms);
            obj.Add(line);
            dr.activatedRooms = obj.ToArray();
        }


        [StageAddon("P1")]
        public static void PrimeSanctum()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("P1");
            MinosPrime peenos = Utils.FindScriptInScene<MinosPrime>();
            CutsceneSkip[] skips = Utils.FindScriptsInScene<CutsceneSkip>();
            foreach (CutsceneSkip skip in skips)
            {
                if (skip.gameObject.name == "MinosPrimeIntro")
                {
                    List<GameObject> obj = new List<GameObject>();
                    obj.AddRange(skip.onSkip.toActivateObjects);
                    obj.Add(inst.transform.GetChild(1).gameObject);
                    skip.onSkip.toActivateObjects = obj.ToArray();

                    Utils.LineOnActivate(skip.transform.GetChild(6).gameObject, inst.transform.GetChild(1).gameObject, 24f);

                    break;
                }
            }

            List<GameObject> obj2 = new List<GameObject>();
            obj2.AddRange(peenos.onOutroEnd.toActivateObjects);
            obj2.Add(inst.transform.GetChild(2).gameObject);
            peenos.onOutroEnd.toActivateObjects = obj2.ToArray();
        }
    }
}
