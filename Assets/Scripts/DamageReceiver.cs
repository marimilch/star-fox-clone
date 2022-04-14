using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private string location;
    [SerializeField] private float dampenFactor = 1f;
    [SerializeField] private Vector2 addVector;
    [SerializeField] private bool keepInitialX;
    [SerializeField] private bool keepInitialY;

    //called by hostile object
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy")){
            //get enemy damage data
            Damage mainDamage = collider.gameObject.GetComponent<Enemy>().GetDamage();

            //if this enemy just damaged player, null is returned and player wont lose health
            if (mainDamage == null)
            {
                return;
            }

            //apply addVector (respecting local coordinate System); apply dampening
            var addVectorTransformed3D = transform.TransformVector(addVector);
            var addVectorTransformed = new Vector2(
                addVectorTransformed3D.x,
                addVectorTransformed3D.y
            );

            //prioritze negative keepInitial of enemy
            var damage = new Damage(
                mainDamage.amount * dampenFactor,
                mainDamage.forceDirection + addVectorTransformed,
                !mainDamage.keepInitialX ? mainDamage.keepInitialX :
                    keepInitialX,
                !mainDamage.keepInitialY ? mainDamage.keepInitialY :
                    keepInitialY
            );

            //communicate upwards
            PlayerHealth.SharedComponent.ReceiveDamage(damage);

            //show visual feedback dependant on location
            FXHandler.SharedComponent.ShowFX(location);
            //Debug.Log("Hit by Enemy.");
        }
    }
}
