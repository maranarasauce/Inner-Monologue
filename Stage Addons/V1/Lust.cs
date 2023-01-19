using ProjectProphet.Patching;
using System.Collections;
using UnityEngine;

namespace ProjectProphet.StageAddons
{
    public static class Lust
    {
        [StageAddon(23)]
        public static void Mindflayer()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("23");
            GameObject line = inst.transform.GetChild(0).gameObject;

            Mindflayer mass = Utils.FindScriptInScene<Mindflayer>();

            Utils.LineOnActivate(mass.gameObject, line, 1f);
        }


        [StageAddon(24)]
        public static void MinosCorpse()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("24");
            GameObject line = inst.transform.GetChild(0).gameObject;

            MinosBoss mass = Utils.FindScriptInScene<MinosBoss>();

            Utils.LineOnActivate(mass.gameObject, line, 1f);

            FinalRoom room = Utils.FindScriptInScene<FinalRoom>();
            Utils.LineOnActivate(room.gameObject, inst.transform.GetChild(1).gameObject, 0f);
        }
    }
}
