using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartLogic : MonoBehaviour {

	public GameObject gamePlayCamera;

    
		void OnCollisionEnter(Collision collision)
    {
			if(collision.transform.gameObject.tag == "DartCube") 
			{
				Destroy(gameObject.GetComponent<CapsuleCollider>());
				Destroy(gameObject.GetComponent<Rigidbody>());
				//Camera mainCamera= Camera.main;
				Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
				mainCamera.GetComponent<GamePlayManager>().SendMessage("HitOnDartBoard", collision.transform.gameObject);
			}
			// else{
			// 	Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
			// 	mainCamera.GetComponent<GamePlayManager>().SendMessage("ProcessNonHitThrowResult");
			// }
			
		}
}
