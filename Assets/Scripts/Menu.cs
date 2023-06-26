using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{ 
    public void SelectCar()
    {
        SceneManager.LoadScene("SelectCar");
    }
    public void EmpezarNivel()
    {
        SceneManager.LoadScene("trackl");
    }

    public void Inicio()
    {
        SceneManager.LoadScene("Menu");
    }

}
