using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class RayInput : MonoBehaviour {

    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private GameObject leftController;



    private Ray ray;
    private Transform originPoint;
    private LineRenderer lineRenderer;

    private GameObject focused;

    private bool isTabletOpen = false;


    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        SteamVR_TrackedController controller = rightController.GetComponent<SteamVR_TrackedController>();
        controller.TriggerUnclicked += (object sender, ClickedEventArgs e) => {
            if (isTabletOpen) { ClickIt(); }
        };


        SteamVR_TrackedController leftcontroller = leftController.GetComponent<SteamVR_TrackedController>();
        leftcontroller.TriggerClicked += (object sender, ClickedEventArgs e) => {
            isTabletOpen = true;
        };
        leftcontroller.TriggerUnclicked += (object sender, ClickedEventArgs e) => {
            isTabletOpen = false;
        };

    }


    void Update () {
        GameObject hand1 = null;
        try
        {
            hand1 = rightController.transform.Find("Model").Find("tip").gameObject;
        }
        catch (NullReferenceException)
        {
            lineRenderer.enabled = false;
            return;
        }

        if( !isTabletOpen)
        {  // Pas de ray si la tablette n'est pas ouverte
            lineRenderer.enabled = false;
            return;
        }

        originPoint = hand1.transform.Find("attach");
        ray = new Ray(originPoint.position, originPoint.forward);
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        Vector3? touchPoint = null;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            touchPoint = hit.point;
            if (hit.collider.gameObject.tag == "UI_buttons")
            {
                if (this.focused != hit.collider.gameObject)
                {
                    //print(hit.collider.name);
                    this.focused = hit.collider.gameObject;
                    EventSystem.current.SetSelectedGameObject(this.focused);
                }
            }
            else
            {
                this.focused = null;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }


        // If the ray touch something -> stop ray to touchpoint, else go very far
        // prevent ray go through objects
        Vector3[] positions = new Vector3[2];
        positions[0] = ray.origin;
        if (touchPoint == null)
        {
            positions[1] = ray.direction * 1000;
        }
        else
        {
            positions[1] = touchPoint.Value;
        }
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;




    }

    private void ClickIt()
    {
        if (this.focused)
        {
            //button.GetComponent<AudioSource>().Play();
            try
            {
                this.focused.GetComponent<Button>().onClick.Invoke();
            }
            catch (NullReferenceException) {}

            try
            {
                this.focused.GetComponent<Toggle>().isOn = !this.focused.GetComponent<Toggle>().isOn;
            }
            catch (NullReferenceException) {}
    }
    }
}
