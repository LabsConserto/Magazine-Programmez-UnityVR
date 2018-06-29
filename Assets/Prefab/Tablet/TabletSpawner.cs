using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletSpawner : MonoBehaviour {
    [SerializeField]
    GameObject controllerLeft;

    [SerializeField]
    GameObject tabletPrefab;

    private SteamVR_TrackedController trackedController_left;
    private GameObject spawned;

    void Start()
    {

        GameObject tablet = Instantiate(tabletPrefab, controllerLeft.transform);
        this.spawned = tablet.transform.Find("Tablet").gameObject;
        this.spawned.SetActive(false);


        trackedController_left = controllerLeft.GetComponent<SteamVR_TrackedController>();

        trackedController_left.TriggerClicked += (object sender, ClickedEventArgs e) => {
            SpawnIt();
        };
        trackedController_left.TriggerUnclicked += (object sender, ClickedEventArgs e) => {
            UnspawnIt();
        };
    }

    void SpawnIt()
    {
        if (this.spawned == null)
        {
            GameObject tablet = Instantiate(tabletPrefab, controllerLeft.transform);
            this.spawned = tablet.transform.Find("Tablet").gameObject;
        }
        else
        {
            this.spawned.SetActive(true);
        }
    }

    void UnspawnIt()
    {
        this.spawned.SetActive(false);
    }

}
