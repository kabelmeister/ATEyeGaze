using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public const float DefaultHealth = 100f;
    bool visible;
    float hp, maxHp;

    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        float scale = Random.Range(0.8f, 1.5f);
        transform.localScale = new Vector3(scale, scale, 1f);
        maxHp = DefaultHealth * scale * scale;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Damage(float dmg)
	{
        if (!visible)
            return false;

        hp -= dmg;
        if (hp <= 0f)
		{
            GetComponent<Collider2D>().enabled = false;
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
        if (collision.gameObject.tag == "MainCamera")
            Destroy(gameObject);
	}
}
