using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{//script que contiene cada carta
    public Animator animator;
    public bool Player1;
    public bool CambioColores;
    Material sfh;
    public TypeCards mType;
    void Start()
    {
       animator = GetComponent<Animator>();
    }
    void OnMouseDown()//funcion que permite dar click a un objeto y que se acceda a el
    {
        if (GameManager.instance.contador == true)
        {
            GameManager.instance.Recibir(this.gameObject);
            animator.Play("Show");
        }
        
    }
    public void Revertir()//funcion que en caso de se llamada, revierte la posicion de la carta
    {
        animator.Play("Retorno");
    }
    public void Encoger()//funcion que en caso de se llamada, encoge la carta
    {
        animator.Play("Desaparecer");
    }
    
}

