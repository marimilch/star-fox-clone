using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipOnTrigger : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //triggerEnter only relevant for player
    //private void OnTriggerEnter(Collider other)
    //{
    //    var o = other.gameObject;

    //    if (o.CompareTag("Player Wing"))
    //    {
    //        Debug.Log("Player collided.");

    //        //flip rotation
    //        //var initialRotation =
    //        //    PlayerController.SharedComponent.transform.eulerAngles;

    //        //PlayerController.SharedComponent.transform.eulerAngles =
    //        //    new Vector3(
    //        //        -initialRotation.x,
    //        //        initialRotation.y,
    //        //        initialRotation.z
    //        //    )
    //        // ;

    //        //flip y direction
    //        var initialMovement =
    //            PlayerController.SharedComponent.movementVector;

    //        //PlayerController.SharedComponent.ForceMovement(new Vector3(
    //        //    initialMovement.x,
    //        //    -initialMovement.y,
    //        //    initialMovement.z
    //        //), .5f);

    //        var direction = new Vector3(
    //            initialMovement.x,
    //            -0.5f,
    //            initialMovement.z
    //        );

    //        PlayerHealth.SharedComponent.ReceiveDamage(new Damage(direction));
    //    }
    //}
}
