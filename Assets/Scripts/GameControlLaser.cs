using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlLaser : MonoBehaviour
{
    public Camera mainCamera;
    public Text scoreText;
    public Text pauseStatusText;
    public Text pauseLabelText;
    public GameObject gameOverScreen;
    public GameObject[] asteroids;

    int score = 0;
    float timer;
    Rect cameraVisibility;
    bool paused;
    bool playerDied;

    static GameControlLaser instance;

    public static void DestroyedAsteroid(Asteroid ast)
	{
        if (instance != null)
		{
            instance.score += Mathf.RoundToInt(ast.MaxHP);
            instance.scoreText.text = instance.score.ToString();
		}
	}

    public static void PlayerDied()
	{
        if (instance != null)
		{
            instance.StartCoroutine(instance.GameOver());
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerDied = false;
        timer = 0f;

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
        if (timer <= 0f && !playerDied)
		{
            timer = GenerateSpawnTime();
            GenerateAsteroid();
        }
	}

    float GenerateSpawnTime()
	{
        return Random.Range(0.68f, 1.2f);
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

    IEnumerator GameOver()
	{
        yield return new WaitForSecondsRealtime(3f);
        Configuration.HighScore = Mathf.Max(score, Configuration.HighScore);

        Transform gosTransform = gameOverScreen.transform;
        gosTransform.GetChild(1).GetComponent<Text>().text = "Your score: " + score;
        gosTransform.GetChild(2).GetComponent<Text>().text = "High score: " + Configuration.HighScore;
        gameOverScreen.SetActive(true);
	}

    public void GameOverButtons(int buttonCode)
	{
        if (buttonCode == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            SceneManager.LoadScene(0);
	}

	public void Pause()
	{
        paused = !paused;
        if (paused)
		{
            Time.timeScale = 0f;
            AudioListener.pause = true;
            pauseStatusText.enabled = true;
            pauseLabelText.text = "Unpause";
		}
        else
		{
            Time.timeScale = 1f;
            AudioListener.pause = false;
            pauseStatusText.enabled = false;
            pauseLabelText.text = "Pause";
        }
	}

	GameObject GenerateAsteroid()
	{
        Vector2 dir = FromUnitPolar(Random.value * 2f * Mathf.PI);
        Vector2 start = mainCamera.transform.position;
        float t = IntersectionRay(start, dir, cameraVisibility);
        Vector2 newPos = start + dir * (t + 1.5f);
        Vector2 angles = CalculatePointToRectangleDir(newPos, cameraVisibility);

        GameObject newAsteroid = Instantiate(asteroids[Random.Range(0, asteroids.Length)], new Vector3(newPos.x, newPos.y, -1f), Quaternion.identity);
        Rigidbody2D body = newAsteroid.GetComponent<Rigidbody2D>();
        body.AddForce(FromUnitPolar(Random.Range(angles.x, angles.y)) * Random.Range(1.3f, 3f), ForceMode2D.Impulse);
        body.angularVelocity = Random.Range(-150f, 150f);
        return newAsteroid;
	}
}
