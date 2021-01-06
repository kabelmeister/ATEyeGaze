using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    const float Damage = 90f / 50f;
    const float TurnRate = 240f;
    const float ForceStrength = 0.65f;
    const float RaycastDist = 16f;
    public Camera mainCamera;
    Transform laserTransform;
    float rotation;
    // Start is called before the first frame update
    void Start()
    {
        rotation = GetTargetRotation();
        laserTransform = transform.GetChild(0);

        Vector3 laserScale = laserTransform.localScale;
        laserScale.y = RaycastDist;
        laserTransform.localScale = laserScale;
    }

    // Update is called once per frame
    void Update()
    {
        float targetRotation = GetTargetRotation();
        rotation = Mathf.MoveTowardsAngle(rotation, targetRotation, TurnRate * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

	void FixedUpdate()
	{
        RaycastHit2D hit = Physics2D.Raycast(Vector2.zero, GameControlLaser.FromUnitPolar((rotation + 90f) * Mathf.Deg2Rad), RaycastDist);
        Vector3 laserScale = laserTransform.localScale;
        if (hit.collider)
        {
            Asteroid hitAsteroid = hit.collider.gameObject.GetComponent<Asteroid>();
            if (hitAsteroid)
            {
                if (hitAsteroid.Damage(Damage))
				{
                    laserScale.y = Vector2.Distance(hit.point, laserTransform.position) + 0.1f;
                    laserTransform.localScale = laserScale;
                    hit.rigidbody.AddForce(ForceStrength * (hit.point - (Vector2)transform.position).normalized);
				}
            }
        }
        else
		{
            laserScale.y = RaycastDist;
            laserTransform.localScale = laserScale;
        }
    }

	float GetTargetRotation()
	{
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shipToMouse = mousePos - (Vector2)transform.position;
        return Mathf.Atan2(shipToMouse.y, shipToMouse.x) * Mathf.Rad2Deg - 90f;
    }
}
