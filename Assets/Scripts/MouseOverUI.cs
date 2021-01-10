using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	float timer;
	float enabledTime;
	bool mouseInside;

	public UnityEvent onHoverActivate;

	void OnEnable()
	{
		enabledTime = Time.unscaledTime;
	}

	void Update()
	{
		timer -= Time.unscaledDeltaTime;
		if (timer <= 0f && mouseInside)
		{
			onHoverActivate.Invoke();
			mouseInside = false;
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
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (Time.unscaledTime - enabledTime > 0.125f)
		{
			mouseInside = true;
			timer = Configuration.MouseHoverTime;
		}
	}
}
