using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public Transform position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (position)
        {
            Vector3 tempVec;
            tempVec = position.position;
            tempVec.z = -1;

            transform.position = tempVec;
        }
    }
}
