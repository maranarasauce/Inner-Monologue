using ProjectProphet.Patching;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectProphet.StageAddons
{
    public static class Gluttony
    {
        [StageAddon(32)]
        public static void Gabriel()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("32");
            GameObject line = inst.transform.GetChild(1).gameObject;

            ScaleTransform mass = Utils.FindScriptInScene<ScaleTransform>();
            Utils.LineOnActivate(mass.transform.GetChild(3).gameObject, line, 0.3f);

            CutsceneSkip skip = Utils.FindScriptInScene<CutsceneSkip>();
            List<GameObject> obj = new List<GameObject>();
            obj.AddRange(skip.onSkip.toActivateObjects);
            obj.Add(line);
            skip.onSkip.toActivateObjects = obj.ToArray();

        }

        [StageAddon(62)]
        public static void Gabriel2()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("62");
            GameObject line = inst.transform.GetChild(1).gameObject;

            ScaleTransform mass = Utils.FindScriptInScene<ScaleTransform>();
            GameObject outro = mass.transform.parent.GetChild(1).GetChild(0).gameObject;
            Utils.LineOnActivate(outro, line, 7f);
        }
    }
}
