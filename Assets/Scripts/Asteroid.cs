﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float baseHealth = 100f;
    public float deathAnimationTime = 1f;
    bool visible, dead;
    float hp, maxHp, deathAnimTimer;

    public float MaxHP { get => maxHp; }

    public AudioSource disintegrateSound;

    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        dead = false;
        float scale = Random.Range(0.75f, 1.32f);
        transform.localScale = new Vector3(scale, scale, 1f);
        maxHp = baseHealth * scale * scale;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
		{
            deathAnimTimer -= Time.deltaTime;
            if (deathAnimTimer >= 0f)
                GetComponent<SpriteRenderer>().material.SetFloat("_Fade", 1f - deathAnimTimer / deathAnimationTime);
            else if (!disintegrateSound.isPlaying)
                Destroy(gameObject);
		}
    }

	void FixedUpdate()
	{
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Vector2.Dot(rb.velocity, rb.position) > 0f && rb.position.sqrMagnitude > Ship.RaycastDist * Ship.RaycastDist)
            Destroy(gameObject);
	}

	public bool Damage(float dmg)
	{
        if (!visible || dead)
            return false;

        hp -= dmg;
        GetComponent<SpriteRenderer>().material.SetFloat("_Destruction", 1f - hp / maxHp);
        if (hp <= 0f)
		{
            dead = true;
            GameControlLaser.DestroyedAsteroid(this);
            disintegrateSound.Play();
            GetComponent<Collider2D>().enabled = false;
            deathAnimTimer = deathAnimationTime;
		}
        return true;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("MainCamera"))
            visible = true;
	}

	void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("MainCamera"))
            Destroy(gameObject);
	}
}
