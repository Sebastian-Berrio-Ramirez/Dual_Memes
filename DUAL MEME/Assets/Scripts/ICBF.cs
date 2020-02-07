using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICBF : MonoBehaviour
{//script opcional, que ejecuta la animacion de los planos con las caras de "icbf"
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        IniciarAnimacion();//unica funcion, ejecuta la animacion
    }

    public void IniciarAnimacion()//unica funcion, ejecuta la animacion
    {
        this.animator.Play("Tristes");
    }
}
