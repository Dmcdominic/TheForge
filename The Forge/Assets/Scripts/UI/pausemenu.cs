using System.Collections; using System.Collections.Generic; using UnityEditor; using UnityEngine; using UnityEngine.EventSystems;  public class pausemenu : MonoBehaviour {     public static bool isPaused = false;     public GameObject pauseMenuUI;     private float prev_timeScale = 1f;     private int length;       // Start is called before the first frame update     void Start()     {         pauseMenuUI.SetActive(false);         length = 4;
    }       // Update is called once per frame      void Update()       {         for (int p = 0; p < length; p++)         {
            if (input.p[p].start)
            {
                if (isPaused)
                {
                    Resume();
                 }
                 else
                 {
                    Pause();
                 }
            }         }              }       public void Resume()       {          pauseMenuUI.SetActive(false);
         logical_resume();      }       private void logical_resume()       {         Time.timeScale = prev_timeScale;         isPaused = false;      }       void Pause()       {          pauseMenuUI.SetActive(true);          prev_timeScale = Time.timeScale;          Time.timeScale = 0f;          isPaused = true;      }  }