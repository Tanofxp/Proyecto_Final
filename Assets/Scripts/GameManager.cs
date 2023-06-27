using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityCar;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject LapTrigger;
    [SerializeField] private GameObject CarControls;
    [SerializeField] private GameObject AIControls;
    private LapComplete LapComplete;
    private int lpc;
    [SerializeField] private float lp1T;
    [SerializeField] private float lp2T;
    [SerializeField] private float lp3T;
    [SerializeField] private TextMeshProUGUI lap_1;
    [SerializeField] private TextMeshProUGUI lap_2;
    [SerializeField] private TextMeshProUGUI lap_3;
    [SerializeField] private TextMeshProUGUI ST;
    [SerializeField] private TextMeshProUGUI TLP;
    private int minutes, seconds, cents;
    public AudioSource GetReady;
    public AudioSource GoAudio;

    [SerializeField] private bool lap1, lap2, lap3, go;

    public static GameManager Instance;

    private void Awake()
    {
        LapComplete = LapTrigger.GetComponent<LapComplete>();

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
    
    void Start()
    {
        if (go == true)
        {
            StartCoroutine(CountStart());
        }
    }

    void Update()
    {
        TLP = LapComplete.TLP;
        lpc = LapComplete.lpc;
        Debug.Log(lpc);
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

    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.5f);
        ST.SetText("3");
        GetReady.Play();
        startPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        startPanel.gameObject.SetActive(false);
        ST.SetText("2");
        GetReady.Play();
        startPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        startPanel.gameObject.SetActive(false);
        ST.SetText("1");
        GetReady.Play();
        startPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        startPanel.gameObject.SetActive(false);
        GoAudio.Play();
        go = false;
        AIControls.GetComponent<CarAIControl>().enabled = true;
        CarControls.GetComponent<UnityCar.CarController>().enabled = true;
    }

}
   