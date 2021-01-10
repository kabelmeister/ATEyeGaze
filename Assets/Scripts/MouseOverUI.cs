using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	float timer;
	bool mouseInside;

	public UnityEvent onHoverActivate;

	void Update()
	{
		timer -= Time.unscaledDeltaTime;
		if (timer <= 0f && mouseInside)
		{
			onHoverActivate.Invoke();
			mouseInside = false;
			Debug.Log("Activate!");
		}
	}

	void Start()
	{
		mouseInside = false;
		timer = Configuration.MouseHoverTime;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mouseInside = false;
		Debug.Log("Reset!");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		mouseInside = true;
		timer = Configuration.MouseHoverTime;
	}
}
