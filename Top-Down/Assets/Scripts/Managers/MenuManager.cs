using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   public void changeScene(int scene)
   {
       if (scene == 1)
       {
           PauseMenu.GameIsPaused = false;
           Time.timeScale = 1f;
           ScoreManager.score = 0;

       }else
	PlayerHealth.PlayerIsDead = false;
       SceneManager.LoadScene(scene);
   }

   public void quit()
   {
       Application.Quit();
   }
}
