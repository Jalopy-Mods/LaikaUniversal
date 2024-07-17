using JaLoader;
using UnityEngine;

namespace Universal
{
    public class Universal : Mod
    {
        public override string ModID => "laikauniversal";
        public override string ModName => "Laika Unviersal";
        public override string ModAuthor => "itisgt & Meb";
        public override string ModDescription => "Turn the Trabby into a Laika Universal";
        public override string ModVersion => "1.0.0";
        public override string GitHubLink => "";
        public override WhenToInit WhenToInit => WhenToInit.InMenu;

        // public override List<(string, string, string)> Dependencies => new List<(string, string, string)>();

        public override bool UseAssets => true;

        public override void OnEnable()
        {
            base.OnEnable();

            var car = GameObject.Find("FrameHolder/TweenHolder/Frame");
            var assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/universal");
            if (assetBundle == null)
            {
                Debug.LogError("Failed to load Asset Bundle");
                return;
            }

            var universal = Instantiate(assetBundle.LoadAsset<GameObject>("LaikaUniversal"), parent: car.transform);

            Destroy(car.GetComponent<MeshFilter>());
            Destroy(car.transform.Find("Roof").GetComponent<MeshFilter>());
            Destroy(car.transform.GetChild(car.transform.childCount - 3).gameObject);
            Destroy(car.transform.Find("rearWindow").GetComponent<MeshFilter>());
            Destroy(car.transform.Find("Plane_1862").GetComponent<MeshFilter>());
            Destroy(car.transform.Find("RL_Window").GetComponent<MeshFilter>());
            Destroy(car.transform.Find("RR_Window").GetComponent<MeshFilter>());

            universal.transform.localPosition = new Vector3(0, -4.35f, 0);

            foreach (var obj in FindObjectsOfType<GameObject>())
            {
                if (obj.name != "Boot") continue;
                switch (obj.transform.GetChild(0).name)
                {
                    case "Boot":
                        Destroy(obj.GetComponent<MeshFilter>());
                        universal.transform.GetChild(1).parent = obj.transform;
                        car.transform.Find("LicensePlateRear").localPosition = new Vector3(-14.2f, -1.1f, 0);
                        break;
                    case "BootLock":
                        obj.transform.localPosition = new Vector3(-0.07f, -0.525f, -0.3f);
                        Destroy(obj.transform.GetChild(0).GetComponent<MeshFilter>());
                        obj.transform.GetChild(0).GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        break;
                    case "Slot 1":
                        Destroy(obj.GetComponent<MeshFilter>());
                        break;
                }
            }

            Instantiate(assetBundle.LoadAsset<GameObject>("seat"), parent: car.transform.Find("Plane_1862"));
            assetBundle.Unload(false);
        }
    }
}