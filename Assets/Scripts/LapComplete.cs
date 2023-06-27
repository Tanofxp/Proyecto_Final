using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LapComplete : MonoBehaviour
{
    public GameObject LapCompleteTrigger;
    public GameObject HalfLapTrigger;
    public int Total_lap;
    public int lpc;
    public TextMeshProUGUI TLP;
    private void OnTriggerEnter( Collider other)
    {
        if(other.gameObject.tag == "Player") {
        lpc += 1;
        if (lpc <= Total_lap)
        {
            TLP.SetText(lpc + " / " + Total_lap);
        }
        LapCompleteTrigger.SetActive(false);
        HalfLapTrigger.SetActive(true);
        if(lpc > 3 ) 
        {
            lpc = 0;
            SceneManager.LoadScene("Fin");
        }
        }
    }
}
