using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHelper : MonoBehaviour {

    // Use this for initialization
    void Start() {

        if (UnityEngine.XR.XRDevice.model == "Vive MV")
        {
            UpdateImage(0);
        }
        else if (UnityEngine.XR.XRDevice.model == "Oculus Rift CV1")
        {
            UpdateImage(1);
        }
        else if (UnityEngine.XR.XRDevice.model == "Lenovo Explorer")
        {
            UpdateImage(2);
        }
    }

    // ChangeImageOrder
    void UpdateImage (int number) {
        GameObject im0 = this.gameObject.transform.Find("Canvas").Find("Huge").gameObject; //vive
        GameObject im1 = this.gameObject.transform.Find("Canvas").Find("Little1").gameObject; // oculus
        GameObject im2 = this.gameObject.transform.Find("Canvas").Find("Little2").gameObject; //mr
        if(number == 0)
        {
            // keep like this
        }
        else if(number == 1)
        {
            SwitchImage(im0,im1);
        }
        else if (number == 2)
        {
            SwitchImage(im0, im2);
        }
    }
    void SwitchImage(GameObject im1, GameObject im2)
    {
        Sprite tmp = im1.GetComponent<Image>().sprite;
        im1.GetComponent<Image>().sprite = im2.GetComponent<Image>().sprite;
        im2.GetComponent<Image>().sprite = tmp;

    }
}
