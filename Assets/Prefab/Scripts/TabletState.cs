using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TabletState : MonoBehaviour {
    private string sceneName = null;

    private int interactBehavior = 0;
    private int translationBehavior = 0;
    private int rotationBehavior = 0;


    private InteractionManager interactionManager = null;
    private MovementManager movementManager = null;

    // Class used to store and represent the tablet state
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "InteractionRoom")
        {
            interactionManager = GameObject.Find("InteractionManager").GetComponent<InteractionManager>();
        }
        else if (sceneName == "MovementRoom")
        {
            movementManager = GameObject.Find("MovementManager").GetComponent<MovementManager>();
        }


        GameObject tablet = GameObject.FindGameObjectsWithTag("Tablet")[0].transform.Find("Tablet").gameObject;
        //print(tablet);

        GameObject ui_text_hub = tablet.transform.Find("FrontSide").Find("Canvas").Find("TextsHub").gameObject;
        GameObject ui_text_interact = tablet.transform.Find("FrontSide").Find("Canvas").Find("UI_interaction").gameObject;
        GameObject ui_text_move = tablet.transform.Find("FrontSide").Find("Canvas").Find("UI_move").gameObject;


        ui_text_hub.SetActive(false);
        ui_text_interact.SetActive(false);
        ui_text_move.SetActive(false);
        if (sceneName == "Hub")
        {
            ui_text_hub.SetActive(true);
        }
        else if (sceneName == "InteractionRoom")
        {
            ui_text_interact.SetActive(true);
        }
        else if (sceneName == "MovementRoom")
        {
            ui_text_move.SetActive(true);
        }
    }

    public void setInteract_None(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("set nothing behavior");

            interactBehavior = 0;
            interactionManager.stateController = 0;
            updateInfoPanels();
        }
    }
    public void setInteract_Take(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("set take behavior");

            interactBehavior = 1;
            interactionManager.stateController = 1;
            updateInfoPanels();
        }
    }
    public void setInteract_Gun(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("set gun behavior");
            interactBehavior = 2;
            interactionManager.stateController = 2;
            updateInfoPanels();
        }
    }

    public void setMovementTranslation_free(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("setMovementTranslation_free");
            translationBehavior = 0;
            movementManager.stateTranslationController = 0;
            updateInfoPanels();
        }
    }
    public void setMovementTranslation_dash(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("setMovementTranslation_dash");
            translationBehavior = 1;
            movementManager.stateTranslationController = 1;
            updateInfoPanels();
        }
    }
    public void setMovementTranslation_teleport(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("setMovementTranslation_teleport");
            translationBehavior = 2;
            movementManager.stateTranslationController = 2;
            updateInfoPanels();
        }
    }
    public void setMovementRotation_free(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("setMovementRotation_free");
            rotationBehavior = 0;
            movementManager.stateRotationController = 0;
            updateInfoPanels();
        }
    }
    public void setMovementRotation_telport(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            print("setMovementRotation_telport");
            rotationBehavior = 1;
            movementManager.stateRotationController = 1;
            updateInfoPanels();
        }
    }

    public void updateInfoPanels()
    {
        if (sceneName == "InteractionRoom")
        {
            updateInteractPanel();
        }
        else
        {
            updateMovementPanel();
        }


    }

    private void updateMovementPanel() {
        print("updateMovementPanel : " + translationBehavior+" "+rotationBehavior);


        GameObject info_panels = GameObject.FindGameObjectsWithTag("Info_Panel")[0];
        GameObject info_tablet_interact = info_panels.transform.Find("info_tablet").gameObject;
        info_tablet_interact.SetActive(false);

        GameObject info_translation_free = info_panels.transform.Find("info_translation_free").gameObject;
        GameObject info_translation_dash = info_panels.transform.Find("info_translation_dash").gameObject;
        GameObject info_translation_teleport = info_panels.transform.Find("info_translation_teleport").gameObject;
        GameObject info_rotation_free = info_panels.transform.Find("info_rotation_free").gameObject;
        GameObject info_rotation_teleport = info_panels.transform.Find("info_rotation_teleport").gameObject;

        info_translation_free.SetActive(false);
        info_translation_dash.SetActive(false);
        info_translation_teleport.SetActive(false);
        info_rotation_free.SetActive(false);
        info_rotation_teleport.SetActive(false);

        switch (translationBehavior){
            case 0:
                info_translation_free.SetActive(true);
                break;
            case 1:
                info_translation_dash.SetActive(true);
                break;
            case 2:
                info_translation_teleport.SetActive(true);
                break;
        }

        switch (rotationBehavior)
        {
            case 0:
                info_rotation_free.SetActive(true);
                break;
            case 1:
                info_rotation_teleport.SetActive(true);
                break;
        }

    }

    private void updateInteractPanel()
    {
        GameObject info_panels = GameObject.FindGameObjectsWithTag("Info_Panel")[0];

        GameObject info_tablet = info_panels.transform.Find("info_tablet").gameObject;
        GameObject info_take = info_panels.transform.Find("info_take").gameObject;
        GameObject info_gun = info_panels.transform.Find("info_gun").gameObject;

        info_tablet.SetActive(false);
        info_take.SetActive(false);
        info_gun.SetActive(false);

        if (interactBehavior == 0)
        {
            //print("choice 0");
            info_tablet.SetActive(true);
        }
        else if (interactBehavior == 1)
        {
            //print("choice 1");
            info_take.SetActive(true);
        }
        else if (interactBehavior == 2)
        {
            //print("choice 2");
            info_gun.SetActive(true);
        }
    }

}
