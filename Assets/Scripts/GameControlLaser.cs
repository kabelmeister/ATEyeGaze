using UnityEngine;

public class GameControlLaser : MonoBehaviour
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
        mainCamera.GetComponent<BoxCollider2D>().size = camSize + camSize;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
		{
            timer = GenerateSpawnTime();
            GenerateAsteroid();
        }
	}

    float GenerateSpawnTime()
	{
        return Random.Range(0.8f, 1.35f);
	}

    public static bool Intersection(Vector2 s, Vector2 e, Rect r, out float t)
	{
        Vector2 dir = e - s;
        float tmp;
        t = 1e30f;

        if (dir.y != 0)
		{
            Vector2 s2e = r.position - s;
            tmp = -s2e.y * r.width / (-dir.y * r.width);
            if (tmp >= 0 && tmp <= 1)
                t = Mathf.Min(t, tmp);

            s2e += new Vector2(0, r.height);
            tmp = -s2e.y * r.width / (-dir.y * r.width);
            if (tmp >= 0 && tmp <= 1)
                t = Mathf.Min(t, tmp);
        }
        if (dir.x != 0)
		{
            Vector2 s2e = r.position - s;
			tmp = s2e.x * r.height / (dir.x * r.height);
            if (tmp >= 0 && tmp <= 1)
                t = Mathf.Min(t, tmp);

            s2e += new Vector2(r.width, 0);
			tmp = s2e.x * r.height / (dir.x * r.height);
            if (tmp >= 0 && tmp <= 1)
                t = Mathf.Min(t, tmp);
		}
        return t <= 1f;
	}

    public static float IntersectionRay(Vector2 o, Vector2 dir, Rect r)
	{
        float tmp, t = 1e30f;
		if (dir.y != 0)
		{
			Vector2 s2e = r.position - o;
			tmp = -s2e.y * r.width / (-dir.y * r.width);
            if (tmp >= 0)
                t = Mathf.Min(t, tmp);

			s2e += new Vector2(0, r.height);
            tmp = -s2e.y * r.width / (-dir.y * r.width);
            if (tmp >= 0)
                t = Mathf.Min(t, tmp);
        }
		if (dir.x != 0)
		{
			Vector2 s2e = r.position - o;
			tmp = s2e.x * r.height / (dir.x * r.height);
			if (tmp >= 0)
				t = Mathf.Min(t, tmp);

			s2e += new Vector2(r.width, 0);
            tmp = s2e.x * r.height / (dir.x * r.height);
            if (tmp >= 0)
                t = Mathf.Min(t, tmp);
        }
        return t;
	}

    public static Vector2 FromUnitPolar(float angle)
	{
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}

    public static int Quadrant(Vector2 d)
	{
        int px = (d.x < 0f) ? 1 : 0;
        int py = (d.y < 0f) ? 1 : 0;
        return px + py + (py & (1 - px));
	}

    public static Vector2 CalculatePointToRectangleDir(Vector2 point, Rect r)
	{
        Vector2 d0, d1, d2, d3;
        d0 = r.position - point;
        d1.x = d0.x + r.width; d1.y = d0.y;
        d2.x = d0.x; d2.y = d0.y + r.height;
        d3 = d0 + r.size;

        float minAngle = 10f, maxAngle = -10f;
        float a = Mathf.Atan2(d0.y, d0.x);
        minAngle = Mathf.Min(minAngle, a);
        maxAngle = Mathf.Max(maxAngle, a);
        a = Mathf.Atan2(d1.y, d1.x);
        minAngle = Mathf.Min(minAngle, a);
        maxAngle = Mathf.Max(maxAngle, a);
        a = Mathf.Atan2(d2.y, d2.x);
        minAngle = Mathf.Min(minAngle, a);
        maxAngle = Mathf.Max(maxAngle, a);
        a = Mathf.Atan2(d3.y, d3.x);
        minAngle = Mathf.Min(minAngle, a);
        maxAngle = Mathf.Max(maxAngle, a);

        if (maxAngle - minAngle > Mathf.PI)
		{
            float tmp = minAngle + 2f * Mathf.PI;
            minAngle = maxAngle;
            maxAngle = tmp;
		}
        return new Vector2(minAngle, maxAngle);
    }

    GameObject GenerateAsteroid()
	{
        Vector2 dir = FromUnitPolar(Random.value * 2f * Mathf.PI);
        Vector2 start = mainCamera.transform.position;
        float t = IntersectionRay(start, dir, cameraVisibility);
        Vector2 newPos = start + dir * (t + 3f);
        Vector2 angles = CalculatePointToRectangleDir(newPos, cameraVisibility);

        GameObject newAsteroid = Instantiate(asteroids[Random.Range(0, asteroids.Length)], newPos, Quaternion.identity);
        Rigidbody2D body = newAsteroid.GetComponent<Rigidbody2D>();
        body.velocity = FromUnitPolar(Random.Range(angles.x, angles.y)) * Random.Range(1.2f, 2.0f);
        body.angularVelocity = Random.Range(-150f, 150f);
        return newAsteroid;
	}
}
