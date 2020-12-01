using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> myDeck = new List<GameObject>();
    private List<GameObject> tempDeck;
    [SerializeField]
    private List<GameObject> discardDeck = new List<GameObject>();
    private bool Lose = false;
    [SerializeField]
    private Transform ShowCard, DiscardPile;
    [SerializeField]
    private GameObject ExtraButtons, WinPanel, LosePanel;
    [SerializeField]
    private TextMeshProUGUI Questions;
    [SerializeField]
    private Button ButtonA, ButtonB,ButtonC,ButtonD;
    private int round = 1;
    private string[] questions = {"Red or Black","High or Low","Outside or In-Between","Pick a suit"};
    // Start is called before the first frame update
    void Start()
    {
        tempDeck = myDeck;
        Questions.text = questions[0];
        Shuffle();
        RoundStart();
    }

    // Update is called once per frame
    void Update()
    {
        Questions.text = questions[round - 1];
        if (Lose)
            LosePanel.SetActive(true);

        if (myDeck.Count == 0)
        {
            myDeck = tempDeck;
            discardDeck.Clear();
            foreach (Transform obj in DiscardPile)
            {
                Destroy(obj.gameObject);
            }
            Shuffle();
        }
    }

    public void Shuffle()
    {
        List<GameObject> tempList = new List<GameObject>();

        while(myDeck.Count != 0)
        {
            int rand = Random.Range(0, myDeck.Count);
            tempList.Add(myDeck[rand]);
            myDeck.RemoveAt(rand);
        }

        myDeck = tempList;
    }

    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    //pulls card from main deck
    private void PullCard()
    {
        if(ShowCard.childCount != 0)
        {
            foreach(Transform obj in ShowCard)
            {
                obj.transform.parent = DiscardPile.transform;
                obj.transform.localPosition = new Vector2(0f, 0f);
            }
        }

        //If the main deck is empty reshuffle the cards from the discard into the main deck
        if(myDeck.Count == 0)
        {
            foreach (GameObject obj in discardDeck)
            {
                myDeck.Add(obj);
                discardDeck.Remove(obj);
            }
            Shuffle();
        }

        Instantiate(myDeck[0], ShowCard.transform);
        discardDeck.Add(myDeck[0]);
        myDeck.RemoveAt(0);
    }

    private void CheckSolution(string myChoice)
    {

        if(round == 1)
        {
            PullCard();
            Card thisCard = ShowCard.GetChild(0).GetComponent<Card>();
            if (myChoice == thisCard.color.ToLower())
            {
                round++;
                
            }
            else
            {
                Lose = true;
            }
        }
        else if (round == 2)
        {
            PullCard();
            Card thisCard = ShowCard.GetChild(0).GetComponent<Card>();
            Card prevCard = DiscardPile.GetChild(DiscardPile.childCount - 1).GetComponent<Card>();
            string answer = (thisCard.number > prevCard.number) ? "high" : "low";
            if (myChoice == answer)
            {
                round++;
            }
            else
            {
                Lose = true;
            }
        }
        else if (round == 3)
        {
            PullCard();
            Card thisCard = ShowCard.GetChild(0).GetComponent<Card>();
            Card HighCard = (DiscardPile.GetChild(DiscardPile.childCount - 1).GetComponent<Card>().number > DiscardPile.GetChild(DiscardPile.childCount - 2).GetComponent<Card>().number) ? DiscardPile.GetChild(DiscardPile.childCount - 1).GetComponent<Card>() : DiscardPile.GetChild(DiscardPile.childCount - 2).GetComponent<Card>();
            Card LowCard = (DiscardPile.GetChild(DiscardPile.childCount - 1).GetComponent<Card>() == HighCard) ? DiscardPile.GetChild(DiscardPile.childCount - 2).GetComponent<Card>() : DiscardPile.GetChild(DiscardPile.childCount - 1).GetComponent<Card>();
            string answer = (thisCard.number <= HighCard.number && thisCard.number >= LowCard.number) ? "inbetween" : "outside";
            if (myChoice == answer)
            {
                round++;
            }
            else
            {
                Lose = true;
            }
        }
        else if(round == 4)
        {
            PullCard();
            Card thisCard = ShowCard.GetChild(0).GetComponent<Card>();
            if(myChoice == thisCard.Suit.ToLower())
            {
                WinPanel.SetActive(true);
            }
            else
            {
                Lose = true;
            }
        }
        Wait(5f);
        if(round <=4)
            RoundStart();
    }

    private void RoundStart()
    {
        ExtraButtons.SetActive(false);
        if (round == 1)
        {
            ButtonA.GetComponentInChildren<TextMeshProUGUI>().text = "Red";
            ButtonB.GetComponentInChildren<TextMeshProUGUI>().text = "Black";
        }
        else if (round == 2)
        {
            ButtonA.GetComponentInChildren<TextMeshProUGUI>().text = "High";
            ButtonB.GetComponentInChildren<TextMeshProUGUI>().text = "Low";
        }
        else if (round == 3)
        {
            ButtonA.GetComponentInChildren<TextMeshProUGUI>().text = "Outside";
            ButtonB.GetComponentInChildren<TextMeshProUGUI>().text = "Inbetween";
        }
        else if (round == 4)
        {
            ExtraButtons.SetActive(true);
            ButtonA.GetComponentInChildren<TextMeshProUGUI>().text = "Clover";
            ButtonB.GetComponentInChildren<TextMeshProUGUI>().text = "Diamond";
            ButtonC.GetComponentInChildren<TextMeshProUGUI>().text = "Spade";
            ButtonD.GetComponentInChildren<TextMeshProUGUI>().text = "Heart";
        }
    }

    public void Button1()
    {
        CheckSolution(RedButton());
    }

    public void Button2()
    {
        CheckSolution(BlackButton());
    }

    public void Button3()
    {
        CheckSolution(Spade());
    }

    public void Button4()
    {
        CheckSolution(Heart());
    }

    public string RedButton()
    {
        if (round == 1)
            return "red";
        else if (round == 2)
            return "high";
        else if (round == 3)
            return "outside";
        else
            return "clover";
    }

    public string BlackButton()
    {
        if (round == 1)
            return "black";
        else if (round == 2)
            return "low";
        else if (round == 3)
            return "inbetween";
        else
            return "diamond";
    }

    public string Spade()
    {
        return "spade";
    }
    public string Heart()
    {
        return "heart";
    }

    //Starts the game over from round 1
    public void StartOver()
    {
        LosePanel.SetActive(false);
        foreach (Transform obj in ShowCard)
        {
            obj.transform.parent = DiscardPile.transform;
            obj.transform.localPosition = new Vector2(0f, 0f);
        }
        Lose = false;
        round = 1;
        RoundStart();
    }

    //Starts a new Game
    public void NewGame()
    {
        WinPanel.SetActive(false);
        foreach (Transform obj in DiscardPile)
        {
            Destroy(obj.gameObject);
        }
        Destroy(ShowCard.GetChild(0).gameObject);
        myDeck = tempDeck;
        Shuffle();
        discardDeck.Clear();
        
        round = 1;
        Shuffle();
        RoundStart();
    }

    public void Home()
    {
        SceneManager.LoadScene("Main");
    }
}
