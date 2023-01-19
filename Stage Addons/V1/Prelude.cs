using ProjectProphet.Patching;
using UnityEngine;

namespace ProjectProphet.StageAddons
{
    public static class Prelude
    {
        [StageAddon(03)]
        public static void ShotgunPickup()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("03");
            GameObject line = inst.transform.GetChild(1).gameObject;
            line.SetActive(false);
            AudioSource src = line.GetComponent<AudioSource>();
            src.playOnAwake = true;

            WeaponPickUp mass = Utils.FindScriptInScene<WeaponPickUp>();
            mass.activateOnPickup = line;
        }

        [StageAddon(05)]
        public static void Cerberus()
        {
            GameObject inst = Utils.LoadAndInstantiateStage("05");
            StatueFake[] fakes = Utils.FindScriptsInScene<StatueFake>();
            StatueFake cerb1 = null;
            StatueFake cerb2 = null;

            foreach (StatueFake fake in fakes)
            {
                if (fake.quickSpawn)
                    cerb2 = fake;
                else cerb1 = fake;
            }

            SubscribeCerb(cerb1, inst.transform.GetChild(0));
            SubscribeCerb(cerb2, inst.transform.GetChild(1));
        }

        private static void SubscribeCerb(StatueFake cerb, Transform line)
        {
            line.gameObject.SetActive(false);
            line.GetComponent<AudioSource>().playOnAwake = true;

            ObjectActivator obac = cerb.toActivate[1].AddComponent<ObjectActivator>();
            obac.events = new UltrakillEvent();
            obac.events.toActivateObjects = new GameObject[1];
            obac.events.toActivateObjects[0] = line.gameObject;
        }
    }
}
