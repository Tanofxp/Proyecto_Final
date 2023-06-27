using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject LapTrigger;
    [SerializeField] private int Total_lap;
    [SerializeField] private int lpc;
    [SerializeField] private float lp1T;
    [SerializeField] private float lp2T;
    [SerializeField] private float lp3T;
    [SerializeField] private TextMeshProUGUI lap_1;
    [SerializeField] private TextMeshProUGUI lap_2;
    [SerializeField] private TextMeshProUGUI lap_3;
    [SerializeField] private TextMeshProUGUI ST;
    [SerializeField] private TextMeshProUGUI TLP;
    private int minutes, seconds, cents;
    private float start = 3;

    [SerializeField] private bool lap1, lap2, lap3, go;

    public static GameManager Instance;

    private void Awake()
    {


        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(lpc);
        if (go == true) { 
        StartTime();
        }
        if(lpc == 1)
        {
            lap2 = false;
            lap3 = false;
            lap1 = true;
        if( lap1 == true) {

            lp1T += Time.deltaTime;
            Cronometro(lp1T);
        lap_1.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);

        }
        }
        if(lpc == 2) {

            lap1 = false;
            lap3 = false;
            lap2 = true;
        if (lap2 == true)
        {

            lp2T += Time.deltaTime;
            Cronometro(lp2T);
            lap_2.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);

        }
        }
        if (lpc == 3)
        {  
            lap1 = false;
            lap2 = false;
            lap3 = true;
        if (lap3 == true)
        {
            lp3T += Time.deltaTime;
            Cronometro(lp3T);
            lap_3.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
        }
        }


    }

    public void Cronometro(float a)
    {
        minutes = (int)(a / 60f);
        seconds = (int)(a - minutes * 60f);
        cents = (int)((a - (int)a) * 100f);
    }

    public void StartTime()
    {
        start -= Time.deltaTime;
        ST.SetText(start.ToString("f0"));
        if (start < 0) {
            startPanel.gameObject.SetActive(false);
            go = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        lpc += 1;
        if (lpc <= Total_lap)
        {
            TLP.SetText(lpc + " / " + Total_lap);
        }
        LapTrigger.SetActive(false);

    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("salir");
    }

}
   