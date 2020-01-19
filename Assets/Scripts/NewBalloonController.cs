﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBalloonController : MonoBehaviour
{
    private CapsuleCollider2D coll;

    [Tooltip("This will draw gizmos in edit mode for debug purposes\nRed circle = Area of Effect\nCyan = Leash Distance" +
        "\nMagenta = Vector applied to affected objects")]
    public bool showDebugGizmos = false;

    [Range(0, 1.0f)]
    [Tooltip("Bouciness strength of the balloon between 0 and 1\nDefault = 0.5")]
    public float bounciness = 0.5f;

    [Tooltip("Is the balloon meant to float or not")]
    public bool @float = false;

    [Range(0, 10)]
    [Tooltip("How fast the balloon with rise or fall. At 0 balloon will neither go up or down on its own\nDefault = 1")]
    public float gasStrenght = 1;
    private int gravityMultiplier = 1;

    [Range(-1, 1)]
    [Tooltip("Balloon behaviour upon breaking \n 1 = Explode\n 0 = Nothing\n-1 = Implode")]
    public int balloonType = 0;

    [Tooltip("Distance from the balloon that will affect objects")]
    public float distanceOfAffection;

    [Tooltip("How strong the affection will be")]
    public float affectionStrenght;

    [Tooltip("Force needed to break the balloon")]
    public int breakForce = 0;

    [Tooltip("Anchor point to keep the balloon connect to a spot or position")]
    public GameObject anchorPoint;

    [Tooltip("Leash distance for the balloon if anchor point is set")]
    public float leashDistance;

    public AudioClip implodeSound;
    public AudioClip popSound;

    private Vector2 m_localAnchor;
    private List<Rigidbody2D> m_collidedRigidBodies;

    private SpringJoint2D spring;

    private LineRenderer line;

    private bool quitting = false;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.startColor = Color.black;
        line.endColor = Color.black;
        
        //clamp all values
        affectionStrenght = Mathf.Max(0.0f, affectionStrenght);
        distanceOfAffection = Mathf.Max(0.0f, distanceOfAffection);
        breakForce = Mathf.Max(0, breakForce);
        leashDistance = Mathf.Max(0.0f, leashDistance);
        gasStrenght = Mathf.Clamp(gasStrenght, 0, 10);
        bounciness = Mathf.Clamp(bounciness, 0.0f, 1.0f);

        m_collidedRigidBodies = new List<Rigidbody2D>();
        m_localAnchor = new Vector2(transform.GetChild(0).transform.localPosition.x, transform.GetChild(0).transform.localPosition.y);

        //get the rigidbody
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        //make it kinematic while we apply changes to the physics material
        rigidbody.isKinematic = true;
        //make the rigidbody asleep while we apply changes to the physics material
        rigidbody.Sleep();

        //get the collider
        coll = gameObject.GetComponent<CapsuleCollider2D>();


        //apply bouciness to the physics material
        //coll.sharedMaterial.bounciness = bounciness;

        //check if the balloon is set to floating or not
        if (@float)
        {
            //flip the gravity modifier if balloon is meant to float
            gravityMultiplier = -1;
        }


        //set the 'floatiness' value
        rigidbody.gravityScale = (gravityMultiplier * gasStrenght);

        //make the body not kinematic
        rigidbody.isKinematic = false;

        spring = gameObject.GetComponent<SpringJoint2D>();
        if (anchorPoint != null)
        {
            Vector2 anchor;
            if (anchorPoint.GetComponentInChildren<Rigidbody2D>() != null)
            {
                //anchor = new Vector2(anchorPoint.transform.position.x - transform.GetChild(0).transform.position.x, anchorPoint.transform.position.y - transform.GetChild(0).transform.position.y);
                spring.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
                spring.connectedAnchor = new Vector2(0.0f, 0.0f);
            }
            else
            {
                anchor = new Vector2(anchorPoint.transform.position.x, anchorPoint.transform.position.y);
                spring.connectedAnchor = anchor;
            }

            spring.autoConfigureDistance = false;
            spring.distance = leashDistance;
            spring.anchor = m_localAnchor;
            spring.enableCollision = true;
            spring.dampingRatio = 1;
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
            //distance between balloon's local anchor point and the center of the object its anchored to
            float dst = Vector3.Distance(anchorPoint.transform.position, transform.GetChild(0).position);

            if (dst > leashDistance)
            {
                spring.enabled = true;
            }
            else if (spring != null)
            {
                spring.enabled = false;
            }

            line.SetPosition(0, transform.GetChild(0).position);
            line.SetPosition(1, anchorPoint.transform.position);
        }
    }

    /// <summary>
    /// This can be called when making an interactive balloon.
    /// 
    /// A GameObject is used as the anchor point.
    /// The anchor point is set to that object's local (0,0) position
    /// 
    /// It also sets the leash distance to the current distance from
    /// the local anchor to the newly connected anchor
    /// 
    /// Eg. setting an anchor point with RMB to a GameObject
    /// with or without a RB2D
    /// </summary>
    /// <param name="t_newAnchor">a GameObject with or without RB2D</param>
    public void SetAnchor(GameObject t_newAnchor)
    {
        anchorPoint = t_newAnchor;

        //if gameObject has a RB2D
        if (anchorPoint.GetComponent<Rigidbody2D>() != null)
        {
            //make the anchor connect to the RB2D
            spring.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
            //set the anchor to the local center of that body
            spring.connectedAnchor = new Vector2(0.0f, 0.0f);
        }
        else
        {
            spring.connectedAnchor = anchorPoint.transform.position;
        }

        spring.distance = Vector3.Distance(transform.GetChild(0).transform.position, anchorPoint.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody)
        {
            bool alreadyCollided = false;

            foreach (var collidee in m_collidedRigidBodies)
            {
                if (collision.rigidbody == collidee)
                {
                    alreadyCollided = true;
                    break;
                }
            }

            if (!alreadyCollided)
            {
                collision.rigidbody.velocity *= (1.0f + bounciness);
                m_collidedRigidBodies.Add(collision.rigidbody);
            }
            if (collision.relativeVelocity.magnitude > breakForce)
            {
                Debug.Log("Breaking force: " + collision.relativeVelocity.magnitude);
                Destroy(gameObject);
            }
        }
        else if (collision.rigidbody)
        {
            Debug.Log("Force experienced: " + collision.relativeVelocity.magnitude);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody)
        {
            for (int i = m_collidedRigidBodies.Count - 1; i >= 0; i--)
            {
                if (collision.rigidbody == m_collidedRigidBodies[i])
                {
                    m_collidedRigidBodies.RemoveAt(i);
                }
            }
        }
    }

    private void OnJointBreak2D(Joint2D brokenJoint)
    {
        brokenJoint = null;
    }

    private void OnApplicationQuit()
    {
        quitting = true;
    }

    private void OnDestroy()
    {
        if (!quitting)
        {
            switch (balloonType)
            {
                case -1:
                    AudioSource.PlayClipAtPoint(implodeSound, transform.position);
                    break;
                case 1:
                    AudioSource.PlayClipAtPoint(popSound, transform.position);
                    break;
                default:
                    break;
            }

            Collider2D[] hitColliders;
            if (balloonType != 0)
            {
                hitColliders = Physics2D.OverlapCircleAll(transform.position, distanceOfAffection);

                foreach (var affectedObject in hitColliders)
                {
                    if (affectedObject.GetComponent<Rigidbody2D>())
                    {
                        var vectorToBalloon = Vector3.Normalize(affectedObject.transform.position - transform.position);

                        if (balloonType == 1)
                        {
                            affectedObject.GetComponent<Rigidbody2D>().velocity = (vectorToBalloon * affectionStrenght);
                        }
                        else if (balloonType == -1)
                        {
                            affectedObject.GetComponent<Rigidbody2D>().velocity = -(vectorToBalloon * affectionStrenght);
                        }

                    }
                }
            } 
        }
    }

    private void OnDrawGizmosSelected()
    {

#if UNITY_EDITOR
        if (showDebugGizmos)
        {
            Gizmos.color = Color.red;

            //draw force application point
            Gizmos.DrawWireSphere(transform.position, distanceOfAffection);

            Gizmos.color = Color.cyan;
            //draw force application point
            Gizmos.DrawWireSphere(transform.GetChild(0).position, leashDistance);

            Gizmos.color = Color.magenta;
            Collider2D[] hitColliders;
            if (balloonType != 0)
            {
                hitColliders = Physics2D.OverlapCircleAll(transform.position, distanceOfAffection);

                foreach (var affectedObject in hitColliders)
                {
                    if (affectedObject.GetComponent<Rigidbody2D>())
                    {
                        var vectorToBalloon = Vector3.Normalize(affectedObject.transform.position - transform.position);

                        if (balloonType == 1)
                        {
                            Gizmos.DrawLine(affectedObject.transform.position, (vectorToBalloon * affectionStrenght) + affectedObject.transform.position);
                        }
                        else if (balloonType == -1)
                        {
                            Gizmos.DrawLine(affectedObject.transform.position, -(vectorToBalloon * affectionStrenght) + affectedObject.transform.position);
                        }
                    }
                }
            }
            Gizmos.color = Color.white;
        }
#endif
    }
}