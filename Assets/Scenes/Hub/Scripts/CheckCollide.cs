using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckCollide : MonoBehaviour {
    [SerializeField]
    private string sceneName;
	// Use this for initialization

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Controller (right)" || col.gameObject.name == "Controller (left)")
        {
            if(sceneName != "")
            {
                SceneManager.LoadScene(this.sceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError("SceneName argument must put filled");
            }
        }
    }

}
