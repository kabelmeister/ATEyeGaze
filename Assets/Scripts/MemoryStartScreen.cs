﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemoryStartScreen : MonoBehaviour
{
    private List<Button> lvlBtns = new List<Button>();

    public const float hoverTime = 1.5f;
    float hoverTimer = 0.0f;

    Ray ray;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        ButtonsSetUp();
    }

    void ButtonsSetUp()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LvlTag");

        EventTrigger.Entry eventtype = new EventTrigger.Entry();
        eventtype.eventID = EventTriggerType.PointerEnter;
        eventtype.callback.AddListener((eventData) => { PickALevel(); });

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Button>();
            objects[i].AddComponent<EventTrigger>();
            objects[i].GetComponent<EventTrigger>().triggers.Add(eventtype);
        }
    }

    private void PickALevel()
    {
        hoverTimer = 0.0f;
        //isHovering = true;
    }

    public void LevelChosen(string buttonName)
    {
        hoverTimer = 0.0f;
        //isHovering = false;

        //Debug.Log(buttonName[3]);

        SceneManager.LoadScene("MemoryLvl" + buttonName[3]);
    }

    private void Update()
    {
        hoverTimer += Time.deltaTime;
        if (hoverTimer >= hoverTime)// && isHovering)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit)
            {
                LevelChosen(hit.collider.name);
            }
        }
    }
}
