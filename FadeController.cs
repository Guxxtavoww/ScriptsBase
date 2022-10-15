using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FadeController : MonoBehaviour
{
    public static FadeController instanciaFade;
    public Image imgFade;
    public Color corInicial;
    public Color corFinal;
    public float duracaoFade;
    
    public bool isFade;

    private float tempo;

   void Awake()
    {
        instanciaFade = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(inicioFade());
    }

    IEnumerator inicioFade()
    {
        isFade = false;
        tempo = 0f;

        while (tempo <= duracaoFade)
        {
            imgFade.color = Color.Lerp(corInicial, corFinal, tempo / duracaoFade);
            tempo += Time.deltaTime;
            yield return null;
        }
        isFade = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Vai corno");
    }
}
