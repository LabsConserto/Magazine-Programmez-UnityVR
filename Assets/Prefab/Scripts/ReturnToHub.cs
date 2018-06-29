using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToHub : MonoBehaviour {

	public void ToHub () {
        SceneManager.LoadScene("Hub");
    }
}
