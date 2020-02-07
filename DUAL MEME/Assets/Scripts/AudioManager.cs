using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip clip; //definir las variables

    private void Update() // se llama cada frame
    {
        this.gameObject.GetComponent<AudioSource>().volume = Options.instance.musica.value;   // se accede al componente "AudioSource" de este objeto
    }                                                                                         // y el volumen se iguala al valor de una variable de la instacia de su clase
}
