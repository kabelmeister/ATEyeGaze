using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameControlMemory : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;

    public List<Button> btns = new List<Button>();

    private bool isHovering = false;
    public const float hoverTime = 1.5f;
    float hoverTimer = 0.0f;

    Ray ray;
    RaycastHit2D hit;

    void Start()
    {
        GetButtons();
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        EventTrigger.Entry eventtype = new EventTrigger.Entry();
        eventtype.eventID = EventTriggerType.PointerEnter;
        eventtype.callback.AddListener((eventData) => { PickAPuzzle(); });

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;

            objects[i].AddComponent<EventTrigger>();
            objects[i].GetComponent<EventTrigger>().triggers.Add(eventtype);
        }
    }

    private void PickAPuzzle()
    {
        hoverTimer = 0.0f;
        isHovering = true;
    }

    public void PuzzleChosen(string buttonName)
    {
        hoverTimer = 0.0f;
        isHovering = false;

        Debug.Log("btn: " + buttonName);

    }

    /*public void onHover(Ray ray, RaycastHit2D hit)
    {
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider == null)
        {
            Debug.Log("nothing hit");

        }
        else
        {
            print(hit.collider.name);
        }
    }*/

    private void Update()
    {
        hoverTimer += Time.deltaTime;
        if (hoverTimer >= hoverTime && isHovering)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            
            PuzzleChosen(hit.collider.name);
        }
    }

}
