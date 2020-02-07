using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanges : MonoBehaviour
{//script para cambios de escena
    public void CargarEscena(string _nombreEscena)
    {
        if (PlayerPrefs.GetInt("numeroFilasCrear") * PlayerPrefs.GetInt("numeroColumnasCrear") != 9 && PlayerPrefs.GetInt("numeroFilasCrear") * PlayerPrefs.GetInt("numeroColumnasCrear") != 3 && PlayerPrefs.GetInt("numeroFilasCrear") * PlayerPrefs.GetInt("numeroColumnasCrear") != 1 && PlayerPrefs.GetInt("numeroFilasCrear") * PlayerPrefs.GetInt("numeroColumnasCrear") != 0 && PlayerPrefs.GetInt("numeroFilasCrear") * PlayerPrefs.GetInt("numeroColumnasCrear") != 15 && PlayerPrefs.GetInt("numeroFilasCrear") * PlayerPrefs.GetInt("numeroColumnasCrear") !=5)
        {
            SceneManager.LoadScene(_nombreEscena);
        }
        else
        {
            Options.instance.advertencia.SetActive(true);
            Options.instance.escogerCartas.SetActive(false);
        }
            
    }//se verifica que no haya numero impares de cartas y se llama el cambio de escena
    public void CloseGame()
    {
        Application.Quit();
    }//funcion para cerrar el juego
    public void VolverAMenu()
    {
        SceneManager.LoadScene("Pantalla Inicial");
    }//funcion para volver al menú
    public void ResetLevel()
    {
        SceneManager.LoadScene("Pantalla juego");
    }//funcion para volver a empezar el juego
}
