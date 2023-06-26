using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    public CinemachineVirtualCamera[] cameras;
    private int currentCameraIndex = 0;
    private bool firstCam = true;
    public Dictionary<string, GameObject> carsDictionary;
    public List<string> carNames;
    
    public GameObject carGameObject1;
    public GameObject carGameObject2;
    public GameObject carGameObject3;
    public GameObject carGameObject4;
    public GameObject carGameObject5;
    public GameObject carGameObject6;

    private void Start()
    {
        _camera.SetActive(true);

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = (i == 0) ? 10 : 0;
        }

        carsDictionary = new Dictionary<string, GameObject>();
        carNames = new List<string>();


        AddCar("Camaro", carGameObject1);
        AddCar("Fordd RS 200", carGameObject2);
        AddCar("Lanncia Stratoss Group 4", carGameObject3);
        AddCar("Moris Mini Cooper", carGameObject4);
        AddCar("Subaruu Impreeza WRC", carGameObject5);
        AddCar("Toyotta Celicca GT", carGameObject6);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            next();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            prev();
        }
    }

    public void next()
    {
        if (firstCam)
        {
            firstCam = false;
            currentCameraIndex = 0;
        }
        else
        {
            cameras[currentCameraIndex].Priority = 0;
            currentCameraIndex++;
            if (currentCameraIndex >= cameras.Length)
            {
                currentCameraIndex = 0;
            }
        }

        cameras[currentCameraIndex].Priority = 10;

        SwitchCar(carNames[currentCameraIndex]);
    }
    public void prev()
    {
        if (firstCam)
        {
            firstCam = false;
            currentCameraIndex = cameras.Length - 1;
        }
        else
        {
            cameras[currentCameraIndex].Priority = 0;
            currentCameraIndex--;
            if (currentCameraIndex < 0)
            {
                currentCameraIndex = cameras.Length - 1;
            }
        }

        cameras[currentCameraIndex].Priority = 10;

        SwitchCar(carNames[currentCameraIndex]);
    }
    private void AddCar(string carName, GameObject carGameObject)
    {
        carsDictionary.Add(carName, carGameObject);
        carNames.Add(carName);
    }

    private void SwitchCar(string carName)
    {

        foreach (var carGameObject in carsDictionary.Values)
        {
            carGameObject.SetActive(false);
        }


        if (carsDictionary.TryGetValue(carName, out GameObject selectedCar))
        {
            selectedCar.SetActive(true);
            Debug.Log("Selected Car: " + carName);
        }
        else
        {
            Debug.LogWarning($"Car with name {carName} does not exist.");
        }
    }
}