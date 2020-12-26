using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject[] asteroids;

    float timer;
    Rect cameraVisibility;

    // Start is called before the first frame update
    void Start()
    {
        timer = GenerateSpawnTime();

        Vector2 camSize = new Vector2();
        camSize.y = mainCamera.orthographicSize;
        camSize.x = camSize.y * mainCamera.aspect; 
        Vector2 camPos = mainCamera.transform.position;

        cameraVisibility = new Rect(camPos - camSize, camSize + camSize);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
		{
            timer = GenerateSpawnTime();
        }
        
        if (Intersection(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition), cameraVisibility, out float t))
		{
			Debug.DrawLine(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition), Color.green);
		}
		else
		{
			Debug.DrawLine(mainCamera.transform.position, mainCamera.ScreenToWorldPoint(Input.mousePosition), Color.red);
		}
	}

    float GenerateSpawnTime()
	{
        return Random.Range(0.5f, 1.8f);
	}

    public static bool Intersection(Vector2 s, Vector2 e, Rect r, out float t)
	{
        Vector2 dir = e - s;

        if (dir.y != 0)
		{
            Vector2 s2e = r.position - s;
            t = -s2e.y * r.width / (-dir.y * r.width);
            if (t >= 0 && t <= 1)
                return true;

            s2e += new Vector2(0, r.height);
            t = -s2e.y * r.width / (-dir.y * r.width);
            if (t >= 0 && t <= 1)
                return true;
        }
        if (dir.x != 0)
		{
            Vector2 s2e = r.position - s;
			t = s2e.x * r.height / (dir.x * r.height);
			if (t >= 0 && t <= 1)
				return true;
            s2e += new Vector2(r.width, 0);
			t = s2e.x * r.height / (dir.x * r.height);
            if (t >= 0 && t <= 1)
                return true;
		}
        t = -1f;
        return false;
	}

    public static bool IntersectionRay(Vector2 o, Vector2 dir, Rect r, out float t)
	{
		if (dir.y != 0)
		{
			Vector2 s2e = r.position - o;
			t = -s2e.y * r.width / (-dir.y * r.width);
			if (t >= 0)
				return true;

			s2e += new Vector2(0, r.height);
			t = -s2e.y * r.width / (-dir.y * r.width);
			if (t >= 0)
				return true;
		}
		if (dir.x != 0)
		{
			Vector2 s2e = r.position - o;
			t = s2e.x * r.height / (dir.x * r.height);
			if (t >= 0)
				return true;
			s2e += new Vector2(r.width, 0);
			t = s2e.x * r.height / (dir.x * r.height);
			if (t >= 0)
				return true;
		}
		t = -1f;
		return false;
	}

    public static Vector2 RandomUnitVector()
	{
        float angle = Random.value * 2 * Mathf.PI;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}

    GameObject GenerateAsteroid()
	{
        Vector2 dir = RandomUnitVector();
        Vector2 start = mainCamera.transform.position;
        float t;
        IntersectionRay(start, dir, cameraVisibility, out t);

        Vector2 newPos = start + dir * (t + 2.5f);


        GameObject newAsteroid = Instantiate(asteroids[Random.Range(0, asteroids.Length)], newPos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        return newAsteroid;
	}
}
