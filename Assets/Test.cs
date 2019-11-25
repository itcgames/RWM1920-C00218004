using UnityEngine;
using System.Collections;


public class Test : MonoBehaviour
{


    //Restricts the position of this object to be within a distance of "distance" of "center"
    public float distance = 3;
    public Transform center;
    private void Update()
    {
        float dst = Vector3.Distance(center.position, transform.position);
        if (dst > distance)
        {
            Vector3 vect = center.position - transform.position;
            vect = vect.normalized;
            vect *= (dst - distance);
            transform.position += vect;
        }
    }

}