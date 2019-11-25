using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    private bool m_isHeldDown;
    private float startPosX;
    private float startPosY;

    // Update is called once per frame
    void Update()
    {
        if (m_isHeldDown)
        {
            Rigidbody2D rb2 = gameObject.GetComponent<Rigidbody2D>();
            rb2.velocity = Vector2.zero;
            rb2.angularVelocity = 0.0f;
            rb2.Sleep();

            Vector2 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);


            gameObject.transform.localPosition = new Vector2(mousePos.x - startPosX, mousePos.y - startPosY);
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;

            m_isHeldDown = true;
        }
    }

    private void OnMouseUp()
    {
        m_isHeldDown = false;
        Rigidbody2D rb2 = gameObject.GetComponent<Rigidbody2D>();
        rb2.WakeUp();
    }
}
