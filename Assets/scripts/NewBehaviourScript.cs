 using UnityEngine;
 using System.Collections;
 
 public class NewBehaviourScript : MonoBehaviour
 {
     public float speed = 0.1f;
     public void FixedUpdate()
     {
         if(Input.GetKey(KeyCode.RightArrow))
         {
             transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
         }
         if(Input.GetKey(KeyCode.LeftArrow))
         {
             transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
         }
         if(Input.GetKey(KeyCode.DownArrow))
         {
             transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
         }
         if(Input.GetKey(KeyCode.UpArrow))
         {
             transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
         }
     }
 }