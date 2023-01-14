using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public  GameObject Map;
    public  GameObject Score;
    public  GameObject Health;
    public  GameObject DeathMenu;
    public GameObject Rompetrol;
    public GameObject Background;
    public static bool value = false;

    void Update()
    {
        if (PlayerHealth.PlayerIsDead)
        {
            Map.SetActive(value);
            Score.SetActive(value);
            Health.SetActive(value);
            Rompetrol.SetActive(value);
            DeathMenu.SetActive(!value);
            if(ScoreManager.score > 10000)
                Background.SetActive(!value);
	    else
		Background.SetActive(value);
        }
        else
        {
            Map.SetActive(!value);
            Score.SetActive(!value);
            Health.SetActive(!value);
            Rompetrol.SetActive(!value);
            DeathMenu.SetActive(value);
            Background.SetActive(value);
        }
    }
}
