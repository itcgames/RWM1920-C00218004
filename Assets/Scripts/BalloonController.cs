using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    private CapsuleCollider2D coll;

    [Range(0, 0.9f)]
    [Tooltip("Bouciness strength of the balloon between 0 and 0.9\nDefault = 0.5")]
    public float bounciness = 0.5f;

    [Tooltip("Is the balloon meant to float or not")]
    public bool @float = false;
    [Range(0, 10)]
    [Tooltip("How light or heavy the gas is in the balloon, between 0 and 10. At 0 balloon will neither go up or down on its own\nDefault = 1")]
    public int gasStrenght = 1;
    private int gravityMultiplier = 1;

    [Range(-1, 1)]
    [Tooltip("DOES NOTHING - NOT IMPLEMENTED\nBalloon behaviour upon breaking \n 1 = Explode\n 0 = Nothing\n-1 = Implode")]
    public int balloonType = 0;

    [Tooltip("Anchor point to keep the balloon connect to a spot or position")]
    public GameObject anchorPoint;
    [Range(0, 100)]
    [Tooltip("Leash distance for the balloon if anchor point is set\nDefault = 3")]
    public int leashDistance = 10;

    private SpringJoint2D spring;
    bool noRb;

    private void Start()
    {
        //get the rigidbody
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        //make it kinematic while we apply changes to the physics material
        rigidbody.isKinematic = true;
        //make the rigidbody asleep while we apply changes to the physics material
        rigidbody.Sleep();

        //get the collider
        coll = gameObject.GetComponent<CapsuleCollider2D>();

        //clamp bouciness value to be between 0.0f and 0.9f
        Mathf.Clamp(bounciness, 0.0f, 0.9f);
        //apply bouciness to the physics material
        coll.sharedMaterial.bounciness = bounciness;

        //check if the balloon is set to floating or not
        if (@float)
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

        spring = gameObject.GetComponent<SpringJoint2D>();
        if (anchorPoint != null)
        {
            Vector2 anchor;
            if (anchorPoint.GetComponent<Rigidbody2D>() != null)
            {
                spring.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
                noRb = false;
            }
            else
            {
                anchor = new Vector2(anchorPoint.transform.position.x, anchorPoint.transform.position.y);
                spring.connectedAnchor = anchor;
                noRb = true;
            }
            spring.distance = leashDistance;
            spring.anchor = new Vector2(-0.05f, -0.93f);
            spring.enableCollision = true;
            spring.dampingRatio = 1;


            //spring.breakForce = 50;
        }
        else
        {
            spring.enabled = false;
        }
        //wake the rigidbody up after apply physics changes
        rigidbody.WakeUp();
    }

    private void Update()
    {
        if (anchorPoint != null && spring != null)
        {
            float dst = Vector3.Distance(anchorPoint.transform.position, transform.GetChild(0).position);
            //Debug.Log("Distance: " + dst + " Force: " + spring.reactionForce);
            spring.distance = leashDistance;
            if (dst >= leashDistance)
            {
                spring.enabled = true;
                Vector2 anchor;
                if (!noRb)
                {
                    //spring.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
                }
                else
                {
                    anchor = new Vector2(anchorPoint.transform.position.x, anchorPoint.transform.position.y);
                    spring.connectedAnchor = anchor;
                }
                spring.distance = leashDistance;
                spring.anchor = new Vector2(-0.05f, -0.93f);
            }
            else if (spring != null)
            {
                spring.enabled = false;
            }
        }
    }

    private void OnJointBreak2D(Joint2D brokenJoint)
    {
        brokenJoint = null;
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
        coll = gameObject.GetComponent<CapsuleCollider2D>();

        //clamp bouciness value to be between 0.0f and 0.9f
        Mathf.Clamp(bounciness, 0.0f, 0.9f);
        //apply bouciness to the physics material
        coll.sharedMaterial.bounciness = bounciness;

        //check if the balloon is set to floating or not
        if (@float)
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
