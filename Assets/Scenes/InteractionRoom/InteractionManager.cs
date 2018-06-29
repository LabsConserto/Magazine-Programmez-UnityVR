using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InteractionManager : MonoBehaviour {

    [SerializeField]
    private GameObject leftController;
    [SerializeField]
    private GameObject rightController;
    [SerializeField]
    private GameObject gunPrefab;
    private GameObject spawnedGun;

    [SerializeField]
    private GameObject hitPrefab;

    private GameObject objectTouched;


    private bool isBehaviorActive = true;
    public int stateController = 0;
    // O -> nothing
    // 1 -> take
    // 2 -> gun


	// Use this for initialization
	void Start () {
        SteamVR_TrackedController leftcontroller = leftController.GetComponent<SteamVR_TrackedController>();
        SteamVR_TrackedController rightcontroller = rightController.GetComponent<SteamVR_TrackedController>();


        leftcontroller.TriggerClicked += (object sender, ClickedEventArgs e) => {
            active(false); //active behavior when tablet is not enable
        };
        leftcontroller.TriggerUnclicked += (object sender, ClickedEventArgs e) => {
            active(true);
        };

        rightcontroller.TriggerClicked += (object sender, ClickedEventArgs e) => {
            triggerComponent(true);
        };
        rightcontroller.TriggerUnclicked += (object sender, ClickedEventArgs e) => {
            triggerComponent(false);
        };
        
    }

    void active(bool activate)
    {
        isBehaviorActive = activate;

        if(activate == true)
        {


            if (stateController == 2)
            {
                //Transform tip = rightController.transform.
                // hide controller
                rightController.transform.Find("Model").gameObject.SetActive(false);
                if (!spawnedGun)
                {
                    spawnedGun = Instantiate(gunPrefab, rightController.transform);
                }
                else { spawnedGun.SetActive(true); }
            }
        }
        else
        {
            print("hide gun on tip");
            rightController.transform.Find("Model").gameObject.SetActive(true);
            if (spawnedGun) { spawnedGun.SetActive(false); }
        }
    }


    void triggerComponent(bool activate)
    {
        print("isBehavioractive : " + isBehaviorActive);
        print("state: " + stateController);


        if (isBehaviorActive == true)
        {
            //print("trigger component " + activate+"  : "+ stateController);
            if (stateController == 1)
            {
                if (activate) { Take(); }
                else { Drop(); }
            }
            else if (stateController == 2)
            {
                if (activate) { Shoot(); }
            }
        }
    }

    void Take()
    {
        print("take");
        Collider colliderTrigger = rightController.GetComponent<Collider>();
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        GameObject touched = null;
        foreach (GameObject go in interactables){
            Collider col = go.GetComponent<Collider>();
            if (colliderTrigger.bounds.Intersects(col.bounds))
            {
                print("objectTouched interactables");
                touched = col.gameObject;
            }
        }
        if (touched)
        {
            objectTouched = touched;
            objectTouched.GetComponent<Rigidbody>().isKinematic = true;
            //objectTouched.GetComponent<Rigidbody>().useGravity = false;
            objectTouched.transform.SetParent(rightController.transform.Find("Model"));
        }
        

    }
    void Drop()
    {
        print("drop");
        if (objectTouched)
        {
            objectTouched.transform.SetParent(null);
            objectTouched.GetComponent<Rigidbody>().isKinematic = false;

            SteamVR_TrackedObject rightcontroller = rightController.GetComponent<SteamVR_TrackedObject>();
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)rightcontroller.index); //rightController.GetComponent<Rigidbody>().velocity;

            objectTouched.GetComponent<Rigidbody>().velocity = controller.velocity;//rightController.GetComponent<Rigidbody>().velocity;
            objectTouched.GetComponent<Rigidbody>().angularVelocity = controller.angularVelocity;//rightController.GetComponent<Rigidbody>().velocity;


        }
    }

    void Shoot()
    {
        print("gun shoot");

        //show particle
        ParticleSystem ps = spawnedGun.transform.Find("PM-40_Top").Find("tip").Find("MuzzleFlashEffect").gameObject.GetComponent<ParticleSystem>();
        print(ps);
        ps.Play();

        // try ray
        Transform originPoint = spawnedGun.transform.Find("PM-40_Top").Find("tip");
        Ray ray = new Ray(originPoint.position, originPoint.up);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject spawnedDecal = GameObject.Instantiate(hitPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            spawnedDecal.transform.SetParent(hit.collider.transform);

            if(hit.collider.gameObject.tag == "Interactable")
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().velocity = ray.direction*4;
            }
        }

    }

}
