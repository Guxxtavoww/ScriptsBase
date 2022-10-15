using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int score;
    public Text txtScore;


    public AudioSource fxGame;


    public GameObject hitPrefab;

    public Sprite[] spriteVida;
    public Image barraVida; 
    public void Pontuacao(int qtdPontos)
    {
        score += qtdPontos;
        txtScore.text = score.ToString();

        
    }

    public void BarraVida(int healthvida)
    {
        barraVida.sprite = spriteVida[healthvida];
    }
}
