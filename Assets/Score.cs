using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score;
    public SpriteRenderer firstDigit;
    public SpriteRenderer secondDigit;
    public SpriteRenderer thirdDigit;

    public List<Sprite> sprites;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddScore()
    {
        score++;
        firstDigit.sprite = sprites[score % 10];
        secondDigit.sprite = sprites[(score / 10) % 10];
    }
}
