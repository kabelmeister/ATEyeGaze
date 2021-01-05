using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public const float DefaultHealth = 100f;
    public const float DeathAnimationTime = 0.8f;
    bool visible, dead;
    float hp, maxHp, deathAnimTimer;

    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        dead = false;
        float scale = Random.Range(0.75f, 1.25f);
        transform.localScale = new Vector3(scale, scale, 1f);
        maxHp = DefaultHealth * scale;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
		{
            deathAnimTimer -= Time.deltaTime;
            if (deathAnimTimer >= 0f)
                GetComponent<SpriteRenderer>().material.SetFloat("_Fade", 1f - deathAnimTimer / DeathAnimationTime);
            else
                Destroy(gameObject);
		}
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
            GetComponent<Collider2D>().enabled = false;
            deathAnimTimer = DeathAnimationTime;
		}
        return true;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "MainCamera")
            visible = true;
	}

	void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "MainCamera" && !dead)
            Destroy(gameObject);
	}
}
