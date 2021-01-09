using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    [SerializeField] private Transform field;

    [SerializeField] private GameObject btn;

    public int btn_cnt = 8;

    void Awake ()
    {
        for (int i = 0; i < btn_cnt; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(field, false);
        }            
    }
}
