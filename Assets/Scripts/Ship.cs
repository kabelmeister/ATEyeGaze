using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shipToMouse = mousePos - (Vector2)transform.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shipToMouse.y, shipToMouse.x) * Mathf.Rad2Deg - 90f);

        RaycastHit2D hit = Physics2D.Raycast(Vector2.zero, shipToMouse, 20f);
        if (hit.collider)
		{
            Asteroid hitAsteroid = hit.collider.gameObject.GetComponent<Asteroid>();
            if (hitAsteroid)
			{
                hitAsteroid.Damage(85f * Time.deltaTime);
                hitAsteroid.GetComponent<Rigidbody2D>().AddForce((hit.point - (Vector2)transform.position).normalized * 0.035f);
			}
        }
    }
}
