using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string playerName;

    public int playerScore;
    public int money;

    public bool hasPattedPet;
    public bool hasAnsweredQuestion;
    public bool hasPlayedMiniGame;

    public List<String> hatsObtained = new();

    public ShopItem currentHat;

    public float petQuestionChance;
    public float petFailedQuestionChanceIncrement;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void IncrementPetQuestionChance(float increment)
    {
        GameManager.Instance.petQuestionChance += increment;
    }

    public bool CheckIfHasHat(string hatID)
    {
        return hatsObtained.Contains(hatID);
    }
}
