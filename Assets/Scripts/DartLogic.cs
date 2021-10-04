using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartLogic : MonoBehaviour {

    [SerializeField] CapsuleCollider capsuleColliderComponent;
    [SerializeField] Rigidbody rigidBodyComponent;
    GamePlayManager gameplayManager;

    private void Start()
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        gameplayManager = mainCamera.GetComponent<GamePlayManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        rigidBodyComponent.isKinematic = true;
        Destroy(rigidBodyComponent);
        Destroy(capsuleColliderComponent);

        if (collision.transform.gameObject.tag == "DartCube")
        {
            transform.parent = collision.transform;
            //gameplayManager.HitOnDartBoard(collision.transform.gameObject);
        }
        else if (collision.transform.gameObject.tag == "DartBoard")
        {
            //gameplayManager.HitOnBoard();
            gameplayManager.ProcessNonHitThrowResult();
        }
        else if (collision.transform.gameObject.tag == "Dart")
        {
            Debug.Log("Case 3");
            //DoNothing
        }
        else
        {
            Debug.Log("Case 4");
            gameplayManager.ProcessNonHitThrowResult();
            Destroy(gameObject);
        }
    }
}
