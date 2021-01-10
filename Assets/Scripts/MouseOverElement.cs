using UnityEngine;
using UnityEngine.Events;

public class MouseOverElement : MonoBehaviour
{
	float timer;
	bool actionActivated;

	public UnityEvent onHoverActivate;

	void Start()
	{
		actionActivated = false;
		timer = Configuration.MouseHoverTime;
	}

	void OnMouseOver()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f && !actionActivated)
		{
			onHoverActivate.Invoke();
			actionActivated = true;
		}
	}

	void OnMouseExit()
	{
		timer = Configuration.MouseHoverTime;
		actionActivated = false;
	}
}
