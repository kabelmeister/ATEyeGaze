using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddButtons : MonoBehaviour
{
    [SerializeField] private Transform field;

    [SerializeField] private GameObject btn;

    public int btn_cnt;

    void Awake ()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName[sceneName.Length - 1] == '1')
        {
            btn_cnt = 8;
            btn.GetComponent<BoxCollider2D>().size = new Vector2(200, 200);
        }
        else if (sceneName[sceneName.Length - 1] == '2')
        {
            btn_cnt = 16;
            btn.GetComponent<BoxCollider2D>().size = new Vector2(180, 180);
        }
        else if (sceneName[sceneName.Length - 1] == '3')
        {
            btn_cnt = 25;
            btn.GetComponent<BoxCollider2D>().size = new Vector2(150, 150);
        }
        else if (sceneName[sceneName.Length - 1] == '4')
        {
            btn_cnt = 32;
            btn.GetComponent<BoxCollider2D>().size = new Vector2(155, 155);
        }

        for (int i = 0; i < btn_cnt; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(field, false);
        }            
    }
}
