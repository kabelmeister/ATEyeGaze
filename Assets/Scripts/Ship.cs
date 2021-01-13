using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float damage = 90f;
    public float turnRate = 240f;
    public float pushForce = 34f;
    public const float RaycastDist = 18f;

    public Camera mainCamera;
    Transform laserTransform;
    float rotation;
    float targetRotation;
    bool dead;

    public AudioSource fireSound;
    public AudioSource hitSound;
    public ParticleSystem hitParticles;
    public ParticleSystem explodeEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
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
        RaycastHit2D hit = Physics2D.Raycast(laserTransform.position, laserTransform.up, RaycastDist);
        Vector3 laserScale = laserTransform.localScale;
        if (hit.collider)
        {
            Asteroid hitAsteroid = hit.collider.gameObject.GetComponent<Asteroid>();
            if (hitAsteroid)
            {
                if (hitAsteroid.Damage(damage * Time.fixedDeltaTime))
				{
                    Vector3 particleLocation = new Vector3(hit.point.x, hit.point.y, hitParticles.transform.position.z);
                    hitParticles.transform.SetPositionAndRotation(particleLocation, Quaternion.LookRotation(Vector3.forward, hit.normal));
                    hitParticles.Play();

                    if (!hitSound.isPlaying)
                        hitSound.Play();

                    laserScale.y = Vector2.Distance(hit.point, laserTransform.position) + 0.08f;
                    laserTransform.localScale = laserScale;
                    hit.rigidbody.AddForce(pushForce * Time.fixedDeltaTime * (hit.point - (Vector2)transform.position).normalized);
                }
            }
        }
        else
		{
            laserScale.y = RaycastDist;
            laserTransform.localScale = laserScale;
            hitParticles.Stop();
            hitSound.Pause();
        }
    }

	float GetTargetRotation()
	{
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shipToMouse = mousePos - (Vector2)transform.position;
        return Mathf.Atan2(shipToMouse.y, shipToMouse.x) * Mathf.Rad2Deg - 90f;
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
        GameObject collidee = collision.gameObject;
        if (!dead && collidee.CompareTag("Asteroid"))
		{
            dead = true;
            GameObject particles = Instantiate(explodeEffectPrefab, transform.position, Quaternion.identity).gameObject;         
            Destroy(gameObject, 0.15f);
            Destroy(particles, 4f);
            GameControlLaser.PlayerDied();
		}
	}

	void OnDestroy()
	{
        if (hitParticles != null)
            hitParticles.Stop();
        if (hitSound != null)
            hitSound.Stop();
    }
}
