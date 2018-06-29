using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MovementManager : MonoBehaviour {

    [SerializeField]
    private GameObject leftController;
    [SerializeField]
    private GameObject rightController;
    [SerializeField]
    private GameObject hmd;
    [SerializeField]
    private GameObject player;

    private bool isBehaviorActive = true;
    public int stateTranslationController = 0;
    /*
     * 0 -> libre
     * 1 -> dash
     * 2 -> teleport
    */
    public int stateRotationController = 0;
    /*
     * 0 -> libre
     * 1 -> incremental
    */
    private LineRenderer lineRenderer;
    private bool sendRayTeleport = false;
    private Vector3? touchPoint = null;

    private bool joyPressedDashTransl = false; // memorize if joy still press or not
    private bool joyPressedDashRot = false; // memorize if joy still press or not




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
            TriggerComponent(true);
        };
        rightcontroller.TriggerUnclicked += (object sender, ClickedEventArgs e) => {
            TriggerComponent(false);
        };

        lineRenderer = GetComponent<LineRenderer>();

    }

    void active(bool activate)
    {
        isBehaviorActive = activate;
    }

    void TriggerComponent(bool activate)
    {
        if (isBehaviorActive)
        {
            if (stateTranslationController == 0)
            {
                print("free move");
            }
            if (stateTranslationController == 1)
            {
                print("dash move");
            }
            if (stateTranslationController == 2)
            {
                if (activate) {
                    sendRayTeleport = true;
                }
                else
                {
                    sendRayTeleport = false;
                    if (touchPoint != null)
                    {
                        Vector3 localPosition = player.transform.Find("[CameraRig]").transform.Find("Camera (eye)").transform.localPosition;
                        player.transform.position = new Vector3(
                            touchPoint.Value[0] - localPosition[0],
                            touchPoint.Value[1],
                            touchPoint.Value[2] - localPosition[2]);

                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (stateTranslationController == 0)
        {
            UpdateFreeMove();
        }
        else if (stateTranslationController == 1)
        {
            UpdateDashMove();
        }
        else if (stateTranslationController == 2)
        {
            UpdateTeleportMove();
        }

        if (stateRotationController == 0)
        {
            UpdateFreeRotation();
        }
        else if (stateRotationController == 1)
        {
            UpdateTeleportRotation();
        }


    }

    void UpdateFreeMove() {
        // get joystick left position
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)leftController.GetComponent<SteamVR_TrackedController>().controllerIndex);
        Vector2 sticks = new Vector2();

        if (UnityEngine.XR.XRDevice.model == "Vive MV")
        {
            bool pressTouchpad = device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
            Vector2 touch = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            sticks = pressTouchpad ? touch : new Vector2(0, 0); // to emulate joystick press hard the touchpad
        }

        else if (UnityEngine.XR.XRDevice.model == "Lenovo Explorer")
        {
            sticks = device.GetAxis(EVRButtonId.k_EButton_Axis2);
        }
        else if (UnityEngine.XR.XRDevice.model == "Oculus Rift CV1")
        {
            sticks = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        }

        //print(sticks);

        // get hmd y orientation 
        float angle = hmd.transform.rotation.eulerAngles.y - player.transform.rotation.eulerAngles.y;

        int speedReducer = 40;

        float x = sticks[0]/speedReducer;
        float y = 0;
        float z = sticks[1]/speedReducer;

        Vector3 vector = new Vector3(x, y, z);
        player.transform.Translate(Quaternion.AngleAxis(angle, Vector3.up) * vector, Space.Self);

    }
    void UpdateDashMove() {
        Vector2 sticks = new Vector2();
        bool pressed = false;

        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)leftController.GetComponent<SteamVR_TrackedController>().controllerIndex);
        if (UnityEngine.XR.XRDevice.model == "Vive MV")
        {
            bool pressTouchpad = device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
            Vector2 touch = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            pressed = pressTouchpad;
            sticks = pressTouchpad ? touch : new Vector2(0, 0); // to emulate joystick press hard the touchpad
        }
        else if (UnityEngine.XR.XRDevice.model == "Lenovo Explorer")
        {
            pressed = device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
            sticks = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        }
        else if (UnityEngine.XR.XRDevice.model == "Vive MV")
        {
            pressed = device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
            sticks = device.GetAxis(EVRButtonId.k_EButton_Axis2);
        }
        
        //print(sticks);

        if (joyPressedDashTransl)
        {
            if (!pressed) { joyPressedDashTransl = false; }
        }
        else
        {
            if (pressed)
            {
                joyPressedDashTransl = true;

                float angle = hmd.transform.rotation.eulerAngles.y - player.transform.rotation.eulerAngles.y;

                int speedReducer = 1;

                float x = sticks[0] / speedReducer;
                float y = 0;
                float z = sticks[1] / speedReducer;

                Vector3 vector = new Vector3(x, y, z);
                player.transform.Translate(Quaternion.AngleAxis(angle, Vector3.up) * vector, Space.Self);
            }
        }
    }
    void UpdateTeleportMove() {
        if (sendRayTeleport)
        {
            Transform originPoint = rightController.transform.Find("Model").transform.Find("tip").transform.Find("attach");
            Ray ray = new Ray(originPoint.position, originPoint.forward);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

            Vector3? touchPoint = null;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                touchPoint = hit.point;
                //print(hit.collider.gameObject.name);
                this.touchPoint = touchPoint;
            }
            else
            {
                this.touchPoint = null;
            }

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
        else
        {
            lineRenderer.enabled = false;
        }
    }
    void UpdateFreeRotation()
    {
        Vector2 sticks = new Vector2();
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)rightController.GetComponent<SteamVR_TrackedController>().controllerIndex);
        if (UnityEngine.XR.XRDevice.model == "Vive MV")
        {
            bool pressTouchpad = device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
            Vector2 touch = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            sticks = pressTouchpad ? touch : new Vector2(0, 0); // to emulate joystick press hard the touchpad
        }
        else if (UnityEngine.XR.XRDevice.model == "Lenovo Explorer")
        {
            sticks = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        }
        else if (UnityEngine.XR.XRDevice.model == "Vive MV")
        {
            sticks = device.GetAxis(EVRButtonId.k_EButton_Axis2);
        }

        //print(sticks);
        GameObject pivot = new GameObject();
        pivot.transform.position = player.transform.Find("[CameraRig]").Find("Camera (eye)").transform.position;

        player.transform.SetParent(pivot.transform);

        pivot.transform.Rotate(Vector3.up, sticks[0]);
        player.transform.SetParent(null);
        Destroy(pivot);
    }
    void UpdateTeleportRotation()
    {
        Vector2 sticks = new Vector2();
        bool pressed = false;

        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)rightController.GetComponent<SteamVR_TrackedController>().controllerIndex);
        if (UnityEngine.XR.XRDevice.model == "Vive MV")
        {
            bool pressTouchpad = device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
            Vector2 touch = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            pressed = pressTouchpad;
            sticks = pressTouchpad ? touch : new Vector2(0, 0); // to emulate joystick press hard the touchpad
        }
        //print(sticks);

        if (joyPressedDashRot)
        {
            if (!pressed) { joyPressedDashRot = false; }
        }
        else
        {
            if (pressed)
            {
                joyPressedDashRot = true;
                GameObject pivot = new GameObject();
                pivot.transform.position = player.transform.Find("[CameraRig]").Find("Camera (eye)").transform.position;

                player.transform.SetParent(pivot.transform);

                pivot.transform.Rotate(Vector3.up, sticks[0]*20);
                player.transform.SetParent(null);
                Destroy(pivot);
            }
        }
    }

}
