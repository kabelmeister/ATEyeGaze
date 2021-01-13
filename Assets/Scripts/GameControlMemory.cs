using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameControlMemory : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;

    public List<Button> btns = new List<Button>();

    //private bool isHovering = false;
    public float hoverTime = Configuration.MouseHoverTime;
    float hoverTimer = 0.0f;

    Ray ray;
    RaycastHit2D hit;

    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();

    private bool firstGuess, secondGuess;
    private int cntGuesses, cntCorrectGuesses, gameGuesses;
    private int firstGuessIndex = -1, secondGuessIndex = -1, chosenCardIndex = -1;
    private string firstGuessPuzzle, secondGuessPuzzle;
    private Collider2D lastHitCollider;

    private string cardIndex = null;

    //GameObject endScreen = GameObject.Find("EndScreen");

    void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("foodImages");
#if UNITY_EDITOR
        Configuration.Load();
#endif
    }

    void Start()
    {
        //endScreen.
        GetButtons();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        cntGuesses = 0;
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        EventTrigger.Entry eventtype = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
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

    public void ConfirmChoice()
    {
        if (cardIndex != null)
        {
            PuzzleChosen(cardIndex);
            btns[int.Parse(cardIndex)].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            cardIndex = null;
        }
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, 1e3f);

        if (hit.collider == lastHitCollider)
		{
            hoverTimer -= Time.deltaTime;
            if (hoverTimer <= 0f)
			{
                if (hit.collider != null && hit.collider.CompareTag("PuzzleButton"))
                {

                    if (cardIndex != hit.collider.name && int.Parse(hit.collider.name) != firstGuessIndex && btns[int.Parse(hit.collider.name)].interactable)
                    {
                        if (cardIndex != null)
                            btns[int.Parse(cardIndex)].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        cardIndex = hit.collider.name;
                        btns[int.Parse(cardIndex)].GetComponent<Image>().color = new Color(1, 1, (float)0.6, 1);
                    }
                }
            }
		}
        else
		{
            hoverTimer = Configuration.MouseHoverTime;
		}
        lastHitCollider = hit.collider;

        //hoverTimer += Time.deltaTime;
        //if (hoverTimer >= hoverTime)// && isHovering)
        //{
        //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            
        //    if (hit)
        //    {
        //        if (hit.collider.CompareTag("PuzzleButton"))
        //        {

        //            if (cardIndex != hit.collider.name && int.Parse(hit.collider.name) != firstGuessIndex && btns[int.Parse(hit.collider.name)].interactable)
        //            {
        //                if (cardIndex != null)
        //                    btns[int.Parse(cardIndex)].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        //                cardIndex = hit.collider.name;
        //                btns[int.Parse(cardIndex)].GetComponent<Image>().color = new Color(1, 1, (float)0.6, 1);
        //            }
        //        }
        //    }
        //}
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MemoryStartScreen");
    }

    IEnumerator CheckIfPuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].GetComponent<BoxCollider2D>().enabled = false;
            btns[secondGuessIndex].GetComponent<BoxCollider2D>().enabled = false;

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

        firstGuessIndex = secondGuessIndex = -1;

    }

    void CheckIfGameIsFinished()
    {
        cntCorrectGuesses++;

        if (cntCorrectGuesses == gameGuesses)
        {
            //Debug.Log("game over " + cntGuesses);

            GameObject endScreen = GameObject.Find("EndScreen");
            //endScreen.GetComponent<Image>().color = new Color(25, 64, 82, 150);
            Text endText = GameObject.Find("EndScreenText").GetComponent<Text>();
            endText.text = "game over!\nYou have completed the game in " + cntGuesses + " moves!";

            //SceneManager.LoadScene("MemoryStartScreen");
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
