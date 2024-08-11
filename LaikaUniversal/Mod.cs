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
        // public override string GitHubLink => "https://github.com/Jalopy-Mods/LaikaUniversal";
        public override WhenToInit WhenToInit => WhenToInit.InGame;

        // public override List<(string, string, string)> Dependencies => new List<(string, string, string)>();

        public override bool UseAssets => true;

        public override void OnEnable()
        {
            base.OnEnable();
            LoadInGame();
        }

        private void LoadInMainMenu()
        {
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
            var seat = Instantiate(assetBundle.LoadAsset<GameObject>("seat"), parent: car.transform.Find("Plane_1862"));
            seat.transform.localPosition = new Vector3(0, 0, 0);
            assetBundle.Unload(false);
        }

        public void LoadInGame()
        {
            Debug.LogWarning("Started");
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
                if (obj.name != "Boot" || obj.transform.childCount == 0) continue;
                switch (obj.transform.GetChild(0).name)
                {
                    case "Boot":
                        Destroy(obj.GetComponent<MeshFilter>());
                        universal.transform.GetChild(0).parent = obj.transform;
                        car.transform.Find("R_LicensePlate").localPosition = new Vector3(-5.65f, -3.55f, 0);
                        obj.AddComponent<TrunkOpener>();
                        break;
                    case "BootLock":
                        obj.transform.localPosition = new Vector3(-0.07f, -0.525f, -0.3f);
                        Destroy(obj.transform.GetChild(0).GetComponent<MeshFilter>());
                        obj.transform.GetChild(0).GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        break;
                    case "Slot 1":
                        Destroy(obj.GetComponent<MeshFilter>());
                        Destroy(obj);
                        Debug.LogWarning("Destroyed Boot Slot 1");
                        break;
                }
            }

            var seat = Instantiate(assetBundle.LoadAsset<GameObject>("seat"), parent: car.transform.Find("Plane_1862"));
            seat.transform.localPosition = new Vector3(0, 0, 0);
            assetBundle.Unload(false);

            // Rotation not managed by mesh origin, but by script iTween and DoorLogicC
            // Can maybe create a harmony prefix to change the position of the door while the true component is doing its thing in the meantime.
        }
    }

    public class TrunkOpener : MonoBehaviour
    {
        private bool _isOpen;
        private float _time;
        private readonly Vector3 _openPos = new Vector3(-5.8f, -1.7f, 0);
        private readonly Vector3 _closedPos = new Vector3(-3.93f, -1.77f, 0);
        public int speedMultiplier = 6;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                _isOpen = !_isOpen;
                _time = 0;
            }

            if (!(_time < 1)) return;
            _time += Time.deltaTime * speedMultiplier;
            transform.localPosition =
                _isOpen ? Vector3.Slerp(_closedPos, _openPos, _time) : Vector3.Slerp(_openPos, _closedPos, _time);
        }
    }
}