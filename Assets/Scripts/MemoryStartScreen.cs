using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemoryStartScreen : MonoBehaviour
{
    public float hoverTime = Configuration.MouseHoverTime;

    // Start is called before the first frame update
    void Start()
    {
        ButtonsSetUp();

        GameObject.FindGameObjectWithTag("Music").GetComponent<MemoryMusic>().PlayMusic();
    }

    void ButtonsSetUp()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LvlTag");


        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Button>();
            objects[i].AddComponent<EventTrigger>();
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToLevel(string lvlName)
    {
        SceneManager.LoadScene(lvlName);
    }    

    private void Update()
    {

    }
}
