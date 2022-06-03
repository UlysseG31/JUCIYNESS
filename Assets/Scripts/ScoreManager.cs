using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    public float currentScore;
    public ParticleSystem hundred;
    
    void Start()
    {
        currentScore = 0;
    }

    public void AddScore()
    {
        currentScore = currentScore + 100 ;
        hundred.Play();
    }
    
    void Update()
    {
        scoreText.text = currentScore.ToString("0");
    }
}
