using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Card: MonoBehaviour
{
    //Number,Suit
    [SerializeField]
    public int number;
    [SerializeField]
    public string num,color;
    [SerializeField]
    public string Suit;
    [SerializeField]
    public Sprite FrontImg, BackImg;

    private void Start()
    {
        GetComponent<Image>().sprite = FrontImg;
    }
}
