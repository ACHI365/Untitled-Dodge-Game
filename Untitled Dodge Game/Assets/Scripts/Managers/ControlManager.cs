using UnityEngine;
using UnityEngine.UI;
namespace Managers
{
   public class ControlManager : MonoBehaviour
   {

      public GameObject toggleStart; // toggle gyroscope on/off
      public GameObject toggleInGame;
      public GameObject toggleDeath;

      private void Start()
      {
         toggleStart.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("controller") == 60; // toggle gyroscope according to pref
         toggleInGame.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("controller") == 60;
         toggleDeath.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("controller") == 60;
      }

      private void Update()
      {
         toggleStart.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("controller") == 60; // update everytime to make sure it stays on all settings menus
         toggleInGame.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("controller") == 60;
         toggleDeath.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("controller") == 60;
      }

      public void UserToggle(bool status)
      {
         PlayerPrefs.SetInt("controller", status ? 60 : 59); // check if toggle is on and set control
      }
   }
}
