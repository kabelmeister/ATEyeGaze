using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject optionsMenu;

	int hoverOptionSet;

	void Awake()
	{
		Configuration.Load();
		hoverOptionSet = Mathf.RoundToInt(Configuration.MouseHoverTime * 4f) - 1;
		Application.targetFrameRate = Screen.currentResolution.refreshRate;
	}

	public void MainMenuButton(int buttonId)
	{
		switch (buttonId)
		{
			case 0:
				SceneManager.LoadScene(1);
				break;
			case 1:
				SceneManager.LoadScene(2);
				break;
			case 2:
				SetOptionsMenuActive(true);
				break;
			case 3:
				Application.Quit();
				break;
		}
	}

	public void HoverTimeButton(int buttonId)
	{
		hoverOptionSet = buttonId;
		Configuration.MouseHoverTime = (buttonId + 1) * 0.25f;
		SetHoverChecks();
	}

	public void SetOptionsMenuActive(bool active)
	{
		mainMenu.SetActive(!active);
		optionsMenu.SetActive(active);
		if (active)
			SetHoverChecks();
	}

	void SetHoverChecks()
	{
		Transform tr = optionsMenu.transform;
		for (int i = 0; i < 4; i++)
		{
			tr.GetChild(i).GetChild(0).gameObject.SetActive(i == hoverOptionSet);
		}
	}

	void OnDestroy()
	{
		Configuration.Save();
	}
}
