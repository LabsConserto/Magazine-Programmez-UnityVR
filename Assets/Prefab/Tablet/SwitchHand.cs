using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchHand : MonoBehaviour {

    private bool leftHanded = false;

    public void Start()
    {
        int isLefthanded = PlayerPrefs.GetInt("isLeftHanded", 0);

        if (isLefthanded == 1) { 
            SwitchControllers();
        }
    }

    public void ChangeHand(Toggle toggle)
    {
        if (toggle.isOn)
        {
            if(toggle.gameObject.name == "Toggle_left" && !leftHanded)
            {
                print("Activate left handed");
                PlayerPrefs.SetInt("isLeftHanded", 1);
                SwitchControllers();

            }
            if (toggle.gameObject.name == "Toggle_right" && leftHanded)
            {
                print("Activate right handed");
                PlayerPrefs.SetInt("isLeftHanded", 0);
                SwitchControllers();
            }
        }
    }
    private void SwitchControllers()
    {

        leftHanded = !leftHanded;
        Transform toggleParent = GameObject.Find("Tablet(Clone)").
            transform.Find("Tablet").
            Find("FrontSide").
            Find("Canvas").
            Find("UI_changeHand");
        GameObject rightToggle = toggleParent.Find("Toggle_right").gameObject;
        GameObject leftToggle = toggleParent.Find("Toggle_left").gameObject;

        if (leftHanded)
        {
            rightToggle.GetComponent<Toggle>().isOn = false;
            leftToggle.GetComponent<Toggle>().isOn = true;
        }

        SteamVR_TrackedObject lo = GameObject.Find("Controller (left)").GetComponent<SteamVR_TrackedObject>();
        SteamVR_TrackedController lc = GameObject.Find("Controller (left)").GetComponent<SteamVR_TrackedController>();

        SteamVR_TrackedObject ro = GameObject.Find("Controller (right)").GetComponent<SteamVR_TrackedObject>();
        SteamVR_TrackedController rc = GameObject.Find("Controller (right)").GetComponent<SteamVR_TrackedController>();


        int tmp = (int)ro.index;

        ro.SetDeviceIndex((int)lo.index);
        rc.SetDeviceIndex((int)lo.index);

        lo.SetDeviceIndex(tmp);
        lc.SetDeviceIndex(tmp);


        // Decalle tablet
        GameObject tablet = GameObject.Find("Tablet(Clone)").transform.Find("Tablet").gameObject;
        if (leftHanded)
        {
            GameObject leftPoint = GameObject.Find("Tablet(Clone)").transform.Find("LeftPoint").gameObject;
            tablet.transform.position = leftPoint.transform.position;
        }
        else
        {
            GameObject rightPoint = GameObject.Find("Tablet(Clone)").transform.Find("RightPoint").gameObject;
            tablet.transform.position = rightPoint.transform.position;
        }

    }
}
