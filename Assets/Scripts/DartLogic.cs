using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartLogic : MonoBehaviour {

	public GameObject gamePlayCamera;

    
		void OnCollisionEnter(Collision collision)
    {
			if(collision.transform.gameObject.tag == "DartCube") 
			{
				Debug.Log("Case 1");
				Destroy(gameObject.GetComponent<CapsuleCollider>());
				Destroy(gameObject.GetComponent<Rigidbody>());
				//Camera mainCamera= Camera.main;
				Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
				mainCamera.GetComponent<GamePlayManager>().SendMessage("HitOnDartBoard", collision.transform.gameObject);
			}
			else if(collision.transform.gameObject.tag == "DartBoard") 
			{
				Debug.Log("Case 2");
				Destroy(gameObject.GetComponent<CapsuleCollider>());
				Destroy(gameObject.GetComponent<Rigidbody>());
				Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
				mainCamera.GetComponent<GamePlayManager>().SendMessage("HitOnBoard");
			}
			else if(collision.transform.gameObject.tag == "Dart") 
			{
				Debug.Log("Case 3");
				//DoNothing
			}
			else{
				Debug.Log("Case 4");
				Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
				mainCamera.GetComponent<GamePlayManager>().SendMessage("ProcessNonHitThrowResult");
				Destroy(gameObject);
			}
			
		}
}
