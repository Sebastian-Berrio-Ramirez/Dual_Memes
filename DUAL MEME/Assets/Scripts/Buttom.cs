
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Buttom : MonoBehaviour
{
    public AudioClip clip;  // se definen las variables
    public AudioClip buttomSelect;

    public void ReproducirSonido() // se define una funcion que accede al componente "AudioSource" de este objeto y el volumen se iguala al valor de una variable de la instacia de su clase
    {                              // y se llama a otra funcion con un parametro
        this.gameObject.GetComponent<AudioSource>().volume = Options.instance.musica.value;
        PlayClip(clip);
    }
    public void PlayClip(AudioClip _audio) // se define una funcion con un parametro de tipo "AudioClip" y la funcion se encarga de reproducir un audio y controlar su volumen
    {
        AudioSource.PlayClipAtPoint(_audio, Camera.main.transform.position, PlayerPrefs.GetFloat("volumenMemes"));
    }
    public void ReproducirSonidoSeleccion () // se define una funcion que accede al componente "AudioSource" de este objeto y el volumen se iguala al valor de una variable de la instacia de su clase
    {                                        // y se llama a otra funcion con un parametro
        this.gameObject.GetComponent<AudioSource>().volume = Options.instance.musica.value;
        PlayClip(buttomSelect);
    }
}
