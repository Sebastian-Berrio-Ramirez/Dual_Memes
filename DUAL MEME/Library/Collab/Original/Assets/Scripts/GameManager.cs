using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.Android;
public class GameManager : MonoBehaviour
{
    public GameObject referenciaUno;
    public GameObject referenciaDos;
    public static GameManager instance;
    public List<TypeCards> Tipos = new List<TypeCards>() { TypeCards.Tipo_Uno, TypeCards.Tipo_Dos, TypeCards.Tipo_Tres, TypeCards.Tipo_Cuatro, TypeCards.Tipo_Cinco, TypeCards.Tipo_Seis, TypeCards.Tipo_Siete, TypeCards.Tipo_Ocho, TypeCards.Tipo_Nueve, TypeCards.Tipo_Diez, TypeCards.Tipo_Once, TypeCards.Tipo_Doce, TypeCards.Tipo_Trece, TypeCards.Tipo_Catorce, TypeCards.Tipo_Quince };
    public List<GameObject> CartasAnimacion = new List<GameObject>() { };
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
    public List<Texture2D> usersImage = new List<Texture2D>();
    public Texture2D algo;
    public Text numeroDeImagen;
    Button impimirUno;
    public Material gallery;
    GameObject vacio;
    public Texture2D holiwis;

    //Holi
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        PlayerPrefs.GetInt("numeroFilasCrear");
        PlayerPrefs.GetInt("numeroColumnasCrear");
    }
    public IEnumerator TiempoParaRevertir(GameObject _referenciaUno, GameObject _referenciaDos)
    {
        Cursor.visible = false;
        yield return new WaitForSeconds(1f);
        _referenciaUno.GetComponent<Card>().Revertir();
        _referenciaDos.GetComponent<Card>().Revertir();
        Cursor.visible = true;
    }
    public IEnumerator InstanciarFuego(GameObject _referenciaUno, GameObject _referenciaDos)
    {
        Audio(_referenciaUno.GetComponent<Card>().mType);
        PlayClip(sonido_del_par);
        GameObject fuegoInstaciadoUno = Instantiate(fuegoPaInstanciar, _referenciaUno.transform.position + new Vector3(0, -0.5f, -1),Quaternion.identity);
        GameObject fuegoInstaciadoDos = Instantiate(fuegoPaInstanciar, _referenciaDos.transform.position + new Vector3(0, -0.5f, -1), Quaternion.identity);
        yield return new WaitForSeconds(2);
        //if (_referenciaUno.GetComponent<Card>().mType == TypeCards.Tipo_Uno)
        //{
        //    pause.gameObject.SetActive(false);
        //    contador = false;
        //    for (int i = 0; i < ICBF.Count; i++)
        //    {
        //        ICBF[i].SetActive(true);
        //        print(ICBF[i].name);

        //        ICBF[i].AddComponent<ICBF>();


        //        yield return new WaitForSeconds(15.5f);
        //        ICBF[i].SetActive(false);
                
        //    }
        //    contador = true;
        //    pause.gameObject.SetActive(true);

        //}
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
        CreateCards();
        int cont = 0;
        contador = true;
        ListaReferencia = CartasAnimacion;
            for (int x = 0; x < PlayerPrefs.GetInt("numeroFilasCrear"); x++)
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
        print(PlayerPrefs.GetString("Dificultad"));
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
    }
    void Update()
    {
        controlDeJuego = (PlayerPrefs.GetInt("numeroColumnasCrear") * PlayerPrefs.GetInt("numeroFilasCrear")) / 2;
        Comparar();
        cronometro.text = System.Convert.ToString(minutos + ":" + System.Convert.ToInt32(segundos));
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
    }

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
    }


    System.Random rng = new System.Random();
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
    }



    // Update is called once per frame
    
    public void Recibir(GameObject _referencia)
    {
        GameObject Referencia = referenciaUno;
        if (contador == true)
        {
            if (referenciaUno == null)
            {
                referenciaUno = _referencia;
                Referencia = referenciaUno;

            }

            else
            {
                if (referenciaDos == null && referenciaDos != Referencia)
                {
                    referenciaDos = _referencia;
                    Comparar();
                }
            }
        }
       
    }
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
            }
        }
        if (ListaReferencia[ListaReferencia.Count-1] == null)
        {
            ganar.SetActive(true);
        }
        

    }
    public void PauseTheGame()
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
    public Material GetSprite(TypeCards type)
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
            default:
                return sprites[15];
        }
    }
    public AudioClip Audio(TypeCards _Tipo)
    {
        switch (_Tipo)
        {
            default:
                return sonido_del_par = sonidoCartasIguales[0];
                break;
        }

    }
    public void PlayClip(AudioClip _audio)
    {
        AudioSource.PlayClipAtPoint(_audio, Camera.main.transform.position, PlayerPrefs.GetFloat("volumenMemes"));
    }
    public void ChooseImage()
    {
        PickImage(512);
    }
    public void ImprimirUNaCosa()
    {
       numeroDeImagen.text = usersImage[usersImage.Count-1].width.ToString();
    }
    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                usersImage.Add(texture);
                
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
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
    Tipo_Quince
}