using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Options : MonoBehaviour
{
    //variables
    public static Options instance;
    public Text numeroDeFilas;
    public Text numeroDeColumnas;
    public int numeroRef;
    public Button filaIzquierda;
    public Button filaDerecha;
    public Button columnaIzquierda;
    public Button columnaDerecha;
    private int numeroActualDeFilas;
    private int numeroActualDeColumnas;
    public int opcionalFilas;
    public int opcionalColumnas;
    public GameObject escogerCartas;
    public GameObject advertencia;
    public Slider memes;
    public Slider musica;

    public int NumeroFilasDeCartas//se obtiene un dato y se establece su valor, debido a que es un texto en pantalla.
    {
        get
        {
            return numeroActualDeFilas;
        }
        set
        {
            numeroDeFilas.text = value.ToString();
            numeroActualDeFilas = value;
        }
    }
    public int NumeroColumnasDeCartas//se obtiene un dato y se establece su valor, debido a que es un texto en pantalla.
    {
        get
        {
            return numeroActualDeColumnas;
        }
        set
        {
            numeroDeColumnas.text = value.ToString();
            numeroActualDeColumnas = value;
        }
    }
    private void Awake()//se crea una instancia unica "singletone"
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    } 
    void Start()
    {
        memes.value = 1;//nivel de sonido de los efectos
        musica.value = 1;//nivel de sonido de la musica
    }
    void Update()//se convierten y envian datos para ser usados en la otra escena
    {
        int opcionalFilas = System.Convert.ToInt32(numeroDeFilas.text);
        int opcionalColumnas = System.Convert.ToInt32(numeroDeColumnas.text);
        PlayerPrefs.SetInt("numeroColumnasCrear", System.Convert.ToInt32(numeroActualDeColumnas));
        PlayerPrefs.SetInt("numeroFilasCrear", System.Convert.ToInt32(numeroActualDeFilas));
        PlayerPrefs.SetFloat("volumenMemes", memes.value);
    }

    public void ObtenerF()
    {
        int numerito =  System.Convert.ToInt32(numeroDeFilas);
    }//funcion que obtiene numero de filas
    public void ObtenerC()
    {
        int numerito = System.Convert.ToInt32(numeroDeColumnas);

    }//funcion que obtiene numero de columnas
    public void AumentarC()
    {
        if (numeroActualDeFilas >= 0 && numeroActualDeFilas <= 4)
        {
            NumeroFilasDeCartas += 1;
        }
        if (numeroActualDeFilas > 4)
        {
            NumeroFilasDeCartas = 0;
        }
    }//funcion que se asignan a un boton para cambiar valores
    public void ReducirC()
    {
        if (numeroActualDeFilas > 0)
        {
            NumeroFilasDeCartas -= 1;
        }
        if (numeroActualDeFilas == 0)
        {
            NumeroFilasDeCartas = 4;
        }


    }//funcion que se asignan a un boton para cambiar valores
    public void AumentarF()
    {
        if (numeroActualDeColumnas >=0 && numeroActualDeColumnas <= 6)
        {
            NumeroColumnasDeCartas += 1;
        }
        if (numeroActualDeColumnas > 6)
        {
            NumeroColumnasDeCartas = 0;
        }

    }//funcion que se asignan a un boton para cambiar valores
    public void ReducirF()
    {
        if (numeroActualDeColumnas > 0)
        {
            NumeroColumnasDeCartas -= 1;
        }
        if (numeroActualDeColumnas == 0)
        {
            NumeroColumnasDeCartas = 6;
        }
    }//funcion que se asignan a un boton para cambiar valores
    public void GuardarDificultadFacil()//funcion que se asignan a un boton para definir la dificultad
    {
        PlayerPrefs.SetString("Dificultad", "Easy");
    }
    public void GuardarDificultadNormal()
    {
        PlayerPrefs.SetString("Dificultad", "Normal");
        print("entraste normal");
    }//funcion que se asignan a un boton para definir la dificultad
    public void GuardarDificultadDificil()
    {
            PlayerPrefs.SetString("Dificultad", "Hard");
            print("entraste dificil");
    }//funcion que se asignan a un boton para definir la dificultad
}
