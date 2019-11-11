using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    [Range(0, 0.9f)]
    [Tooltip("Bouciness strength of the balloon between 0 and 0.9")]
    public float bouciness = 0.5f;

    [Tooltip("Is the balloon meant to float or not")]
    public bool doesItFloat = false;
    [Range(0, 10)]
    [Tooltip("Floatiness value for the balloon between 0 and 10. At 0 balloon will neither go up or down on its own")]
    public int gasStrenght = 1;
    private int gravityMultiplier = 1;

    private PolygonCollider2D coll;
    private void Start()
    {
        //get the rigidbody
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        //make it kinematic while we apply changes to the physics material
        rigidbody.isKinematic = true;
        //make the rigidbody asleep while we apply changes to the physics material
        rigidbody.Sleep();

        //get the collider
        coll = gameObject.GetComponent<PolygonCollider2D>();

        //clamp bouciness value to be between 0.0f and 0.9f
        Mathf.Clamp(bouciness, 0.0f, 0.9f);
        //apply bouciness to the physics material
        coll.sharedMaterial.bounciness = bouciness;

        //check if the balloon is set to floating or not
        if (doesItFloat)
        {
            //flip the gravity modifier if balloon is meant to float
            gravityMultiplier = -1;
        }

        //clamp the floatiness value
        Mathf.Clamp(gasStrenght, 0, 10);
        //set the 'floatiness' value
        rigidbody.gravityScale = (gravityMultiplier * gasStrenght);

        //make the body not kinematic
        rigidbody.isKinematic = false;
        //wake the rigidbody up after apply physics changes
        rigidbody.WakeUp();
    }

    private void OnValidate()
    {
        //get the rigidbody
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        //make it kinematic while we apply changes to the physics material
        rigidbody.isKinematic = true;
        //make the rigidbody asleep while we apply changes to the physics material
        rigidbody.Sleep();

        //get the collider
        coll = gameObject.GetComponent<PolygonCollider2D>();

        //clamp bouciness value to be between 0.0f and 0.9f
        Mathf.Clamp(bouciness, 0.0f, 0.9f);
        //apply bouciness to the physics material
        coll.sharedMaterial.bounciness = bouciness;

        //check if the balloon is set to floating or not
        if (doesItFloat)
        {
            //flip the gravity modifier if balloon is meant to float
            gravityMultiplier = -1;
        }
        else
        {
            gravityMultiplier = 1;
        }

        //clamp the floatiness value
        Mathf.Clamp(gasStrenght, 0, 10);
        //set the 'floatiness' value
        rigidbody.gravityScale = (gravityMultiplier * gasStrenght);

        //make the body not kinematic
        rigidbody.isKinematic = false;
        //wake the rigidbody up after apply physics changes
        rigidbody.WakeUp();
    }
}
