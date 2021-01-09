using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameControlMemory : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;

    public List<Button> btns = new List<Button>();

    //private bool isHovering = false;
    public const float hoverTime = 1.5f;
    float hoverTimer = 0.0f;

    Ray ray;
    RaycastHit2D hit;

    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();

    private bool firstGuess, secondGuess;
    private int cntGuesses, cntCorrectGuesses, gameGuesses;
    private int firstGuessIndex, secondGuessIndex, chosenCardIndex = -1;
    private string firstGuessPuzzle, secondGuessPuzzle;

    void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("foodImages");
    }

    void Start()
    {
        GetButtons();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        cntGuesses = 0;
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

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }

            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    private void PickAPuzzle()
    {
        hoverTimer = 0.0f;
        //isHovering = true;
    }

    public void PuzzleChosen(string buttonName)
    {
        hoverTimer = 0.0f;
        //isHovering = false;

        if (chosenCardIndex != int.Parse(buttonName))
            chosenCardIndex = int.Parse(buttonName);
        else
            return;

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(buttonName);
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(buttonName);
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            cntGuesses++;
            StartCoroutine(CheckIfPuzzlesMatch());
        }
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
                PuzzleChosen(hit.collider.name);
            }
        }
    }

    IEnumerator CheckIfPuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckIfGameIsFinished();
        }
        else
        {
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }

        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
        chosenCardIndex = -1;

    }

    void CheckIfGameIsFinished()
    {
        cntCorrectGuesses++;

        if (cntCorrectGuesses == gameGuesses)
        {
            Debug.Log("game over " + cntGuesses);
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite tmp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = tmp;
        }
    }

}
