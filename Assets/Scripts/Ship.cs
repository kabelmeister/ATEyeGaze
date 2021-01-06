using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float damage = 90f;
    public float turnRate = 240f;
    public float pushForce = 34f;
    const float RaycastDist = 16f;

    public Camera mainCamera;
    Transform laserTransform;
    float rotation;
    float targetRotation;

    public ParticleSystem hitParticles;
    public AudioSource fireSound;
    public AudioSource hitSound;
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
        targetRotation = GetTargetRotation();
    }

	void FixedUpdate()
	{
        rotation = Mathf.MoveTowardsAngle(rotation, targetRotation, turnRate * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
        RaycastHit2D hit = Physics2D.Raycast(Vector2.zero, GameControlLaser.FromUnitPolar((rotation + 90f) * Mathf.Deg2Rad), RaycastDist);
        Vector3 laserScale = laserTransform.localScale;
        if (hit.collider)
        {
            Asteroid hitAsteroid = hit.collider.gameObject.GetComponent<Asteroid>();
            if (hitAsteroid)
            {
                if (hitAsteroid.Damage(damage * Time.fixedDeltaTime))
				{
                    hitParticles.transform.position = hit.point;
                    hitParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, hit.normal);
                    if (!hitParticles.isPlaying)
                        hitParticles.Play();

                    hitSound.transform.position = hit.point;
                    if (!hitSound.isPlaying)
                        hitSound.Play();

                    laserScale.y = Vector2.Distance(hit.point, laserTransform.position) + 0.1f;
                    laserTransform.localScale = laserScale;
                    hit.rigidbody.AddForce(pushForce * Time.fixedDeltaTime * (hit.point - (Vector2)transform.position).normalized);
                }
            }
        }
        else
		{
            laserScale.y = RaycastDist;
            laserTransform.localScale = laserScale;
            if (hitParticles.isPlaying)
                hitParticles.Stop();
            if (hitSound.isPlaying)
                hitSound.Pause();
        }
    }

	float GetTargetRotation()
	{
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shipToMouse = mousePos - (Vector2)transform.position;
        return Mathf.Atan2(shipToMouse.y, shipToMouse.x) * Mathf.Rad2Deg - 90f;
    }
}
