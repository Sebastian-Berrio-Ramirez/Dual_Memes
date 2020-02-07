using System.Collections;//Librerias
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.Android;
public class GameManager : MonoBehaviour
{
    public GameObject referenciaUno;//creacion y definicion de algunas variables
    public GameObject referenciaDos;
    public static GameManager instance;
    public List<TypeCards> Tipos = new List<TypeCards>() { TypeCards.Tipo_Uno, TypeCards.Tipo_Dos, TypeCards.Tipo_Tres, TypeCards.Tipo_Cuatro, TypeCards.Tipo_Cinco, TypeCards.Tipo_Seis, TypeCards.Tipo_Siete, TypeCards.Tipo_Ocho, TypeCards.Tipo_Nueve, TypeCards.Tipo_Diez, TypeCards.Tipo_Once, TypeCards.Tipo_Doce, TypeCards.Tipo_Trece, TypeCards.Tipo_Catorce, TypeCards.Tipo_Quince };
    public List<GameObject> CartasAnimacion = new List<GameObject>() { };
    GameObject plano;
    public AnimationClip voltear;
    public AnimationClip retorno;
    public List<Card> allCards = new List<Card>();
    public GameObject fuegoPaInstanciar;
    public int numeroObtenido;
    public GameObject cardPrefab;
    public List<Material> sprites = new List<Material>();
    public AudioClip sonido_del_par;
    public List<AudioClip> sonidoCartasIguales = new List<AudioClip>();
    public List<GameObject> ICBF = new List<GameObject>();
    public int i;
    public Text cronometro;
    public float segundos;
    public float minutos;
    public int controlDeJuego;
    public bool contador;
    public List<GameObject> ListaReferencia = new List<GameObject>();
    int indiceBorradoUno = 0;
    int indiceBorradoDos = 1;
    public GameObject ganar;
    public GameObject perder;
    private GameObject cartica;
    public Button pause;
    
    
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
        PlayerPrefs.GetInt("numeroFilasCrear");//se obtienen unos datos de otra escena y otro script para su uso en este
        PlayerPrefs.GetInt("numeroColumnasCrear");
    }
    public IEnumerator TiempoParaRevertir(GameObject _referenciaUno, GameObject _referenciaDos)//se crea una corrutina para revertir las cartas que no son iguales
    {
        Cursor.visible = false;
        yield return new WaitForSeconds(1f);
        _referenciaUno.GetComponent<Card>().Revertir();
        _referenciaDos.GetComponent<Card>().Revertir();
        Destroy(plano);
        Cursor.visible = true;
    }
    public IEnumerator InstanciarFuego(GameObject _referenciaUno, GameObject _referenciaDos)//se crea una corrutina para eliminar las cartas iguales, hacer animaciones y lo que sea necesario
    {
        Audio(_referenciaUno.GetComponent<Card>().mType);
        PlayClip(sonido_del_par);
        GameObject fuegoInstaciadoUno = Instantiate(fuegoPaInstanciar, _referenciaUno.transform.position + new Vector3(0, -0.5f, -1),Quaternion.identity);
        GameObject fuegoInstaciadoDos = Instantiate(fuegoPaInstanciar, _referenciaDos.transform.position + new Vector3(0, -0.5f, -1), Quaternion.identity);
        yield return new WaitForSeconds(2);
        _referenciaUno.GetComponent<Card>().Encoger();
        _referenciaDos.GetComponent<Card>().Encoger();
        yield return new WaitForSeconds(1);
        Destroy(fuegoInstaciadoUno);
        Destroy(fuegoInstaciadoDos);
        ListaReferencia[indiceBorradoDos] = null;
        ListaReferencia[indiceBorradoUno] = null;
        indiceBorradoUno += 2;
        indiceBorradoDos += 2;
    }
    void Start()
    {

        CreateCards();//se llama funcion que hace que las cartas se desordenen
        int cont = 0;
        contador = true;
        ListaReferencia = CartasAnimacion;
            for (int x = 0; x < PlayerPrefs.GetInt("numeroFilasCrear"); x++)//se crea una malla para la creacion de las cartas, donde se les asigna posicion, tipo, materiales y lo necesario
            {
                for (int y = 0; y < PlayerPrefs.GetInt("numeroColumnasCrear"); y++)
                {
                    GameObject Card = GameObject.Instantiate(cardPrefab) as GameObject;
                    Vector3 position = new Vector3(x, y * 1.5f, 0);
                    Card.transform.position = position;
                    Card.AddComponent<Card>().mType = allCards[cont].mType;
                    Card.transform.GetChild(0).GetComponent<Renderer>().material = GetSprite(allCards[cont].mType);
                    cont++;
                    CartasAnimacion.Add(Card);
                }
            }
        Dificultades();//se escoge el tiempo dependiendo de la dificultad
    }
    void Update()
    {
        controlDeJuego = (PlayerPrefs.GetInt("numeroColumnasCrear") * PlayerPrefs.GetInt("numeroFilasCrear")) / 2;//numero de cartas que vas a usar
        cronometro.text = System.Convert.ToString(minutos + ":" + System.Convert.ToInt32(segundos));//transformacion de texto a entero para poder tener un cronometro en pantalla
        Cronometro();//llamada de funcion para el cronometro
        Comparar();//llamada de funcion para comparar cartas
    }
    public void Cronometro()
    {
        if (contador == true)
        {
            segundos -= 1 * Time.deltaTime;
            if (segundos <= 0)
            {
                if (minutos > 0)
                {
                    minutos -= 1;
                    segundos = 59;
                }
            }
            if (minutos == 0 && segundos <= 0)
            {
                print("perdiste");
                cronometro.text = "";
                perder.SetActive(true);
            }
        }
    }//se define que pasará con el cronometro cuando se de una dificultad 
    public void Dificultades()
    {
        if (PlayerPrefs.GetString("Dificultad") == "Easy")
        {
            minutos = 5;
        }
        if (PlayerPrefs.GetString("Dificultad") == "Normal")
        {
            minutos = 3;
        }
        if (PlayerPrefs.GetString("Dificultad") == "Hard")
        {
            minutos = 1;
        }
    }//define el tiempo maximo del juego

    public void CreateCards()
    {
        Shuffle(Tipos);

        int cont = 0;
        int t = 0;

        for (int i = 0; i < PlayerPrefs.GetInt("numeroColumnasCrear") * PlayerPrefs.GetInt("numeroFilasCrear"); i++)
        {
            if (cont >= 2)
            {
                cont = 0;
                t++;
            }
            Card carta = new Card();
            carta.mType = Tipos[t];
            allCards.Add(carta);
            cont++;
        }
        Shuffle(allCards);
    }//funcion que revuelve las posiciones en listas para desordenar ordenes de las cartas, además Asigna un tipo a las cartas


    System.Random rng = new System.Random();//otra manera de hacer el orden aleatorio
    public void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }//otra manera de hacer el orden aleatorio

    
    public void Recibir(GameObject _referencia)
    {
        GameObject Referencia = referenciaUno;
        if (contador == true)
        {
            if (referenciaUno == null)
            {
                referenciaUno = _referencia;
                Referencia = referenciaUno;
                plano = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plano.transform.position = referenciaUno.transform.position+new Vector3(0,0,-0.5f);
                plano.transform.localScale = new Vector3(0.07f, 0.25f,0.125f);
                plano.transform.rotation = Quaternion.Euler(-90,0,0);
                plano.GetComponent<MeshRenderer>().enabled = false;

            }

            else
            {
                if (referenciaDos == null && referenciaDos != Referencia && referenciaDos != referenciaUno)
                {
                    referenciaDos = _referencia;
                    Comparar();
                }
            }
        }
       
    }//funcion que recibe la referencia de las cartas a comparar
    public void Comparar()
    {
        if (referenciaDos != null)
        {
            if (referenciaUno.GetComponent<Card>().mType == referenciaDos.GetComponent<Card>().mType)
            {
                StartCoroutine(InstanciarFuego(referenciaUno, referenciaDos));
                referenciaUno = null;
                referenciaDos = null;
            }
            else
            {
                    StartCoroutine(TiempoParaRevertir(referenciaUno, referenciaDos));
                    referenciaUno = null;
                    referenciaDos = null;
                    Destroy(plano);
            }
        }
        if (ListaReferencia[ListaReferencia.Count-1] == null)
        {
            ganar.SetActive(true);
            contador = false;
        }
    }//funcion que compara las cartas y llama a la corrutina, ya sea para eliminar o devolver las cartas a su posicion
    public void PauseTheGame()//funcion que pausa el cronometro del juego
    {
        if (contador == true)
        {
            contador = false;
        }
        else
        {
            contador = true;
        }
    }
    public Material GetSprite(TypeCards type)//funcion que asigna material dependiendo del tipo de carta
    {
        switch (type)
        {
           case TypeCards.Tipo_Uno:
                return sprites[0];
                break;
            case TypeCards.Tipo_Dos:
                return sprites[1];
                break;
            case TypeCards.Tipo_Tres:
                return sprites[2];
                break;
            case TypeCards.Tipo_Cuatro:
                return sprites[3];
                break;
            case TypeCards.Tipo_Cinco:
                return sprites[4];
                break;
            case TypeCards.Tipo_Seis:
                return sprites[5];
                break;
            case TypeCards.Tipo_Siete:
                return sprites[6];
                break;
            case TypeCards.Tipo_Ocho:
                return sprites[7];
                break;
            case TypeCards.Tipo_Nueve:
                return sprites[8];
                break;
            case TypeCards.Tipo_Diez:
                return sprites[9];
                break;

            case TypeCards.Tipo_Once:
                return sprites[10];
                break;

            case TypeCards.Tipo_Doce:
                return sprites[11];
                break;

            case TypeCards.Tipo_Trece:
                return sprites[12];
                break;

            case TypeCards.Tipo_Catorce:
                return sprites[13];
                break;

            case TypeCards.Tipo_Quince:
                return sprites[14];
                break;
            case TypeCards.Tipo_Dieciseis:
                return sprites[15];
                break;
            default:
                return sprites[16];
        }
    }
    public AudioClip Audio(TypeCards _Tipo)//Se escoge un sonido dependiendo del tipo de la carta
    {
        switch (_Tipo)
        {
            default:
                return sonido_del_par = sonidoCartasIguales[0];
                break;
        }
    }
    public void PlayClip(AudioClip _audio)//funcion para controlar el nivel de sonido
    {
        AudioSource.PlayClipAtPoint(_audio, Camera.main.transform.position, PlayerPrefs.GetFloat("volumenMemes"));
    }
}
public enum TypeCards
{
    Tipo_Uno,
    Tipo_Dos,
    Tipo_Tres,
    Tipo_Cuatro,
    Tipo_Cinco,
    Tipo_Seis,
    Tipo_Siete,
    Tipo_Ocho,
    Tipo_Nueve,
    Tipo_Diez,
    Tipo_Once,
    Tipo_Doce,
    Tipo_Trece,
    Tipo_Catorce,
    Tipo_Quince,
    Tipo_Dieciseis
}//enum que contiene los tipos de cartas