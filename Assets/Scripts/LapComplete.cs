using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapComplete : MonoBehaviour
{
    public GameObject LapCompleteTrigger;
    public GameObject HalfLapTrigger;
    public int Total_lap;
    public int lpc;
    public TextMeshProUGUI TLP;
    private void OnTriggerEnter()
    {
        lpc += 1;
        if (lpc <= Total_lap)
        {
            TLP.SetText(lpc + " / " + Total_lap);
        }
        LapCompleteTrigger.SetActive(false);
        HalfLapTrigger.SetActive(true);

    }
}
