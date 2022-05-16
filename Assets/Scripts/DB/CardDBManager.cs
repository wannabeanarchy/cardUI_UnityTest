using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardDBManager   
{
    public static List<CardDB> listCards;  

    private static bool inited = false;
    private static string pathFile = "\\Resources\\cards.json";

    public static void Init()
    {
        if (inited) return; 

        listCards = new List<CardDB>(); 
        String JSONtxt = File.ReadAllText(Application.dataPath+pathFile);
        listCards =  JsonConvert.DeserializeObject<List<CardDB>>(JSONtxt);

        inited = true;
    }

    public static CardDB GetRandomCardData()
    {
        if (!inited)
            Init();

        return listCards[UnityEngine.Random.Range(0, listCards.Count)]; 
    }
}
