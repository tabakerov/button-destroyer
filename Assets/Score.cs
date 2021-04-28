using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;
    public SpriteRenderer firstDigit;
    public SpriteRenderer secondDigit;
    public SpriteRenderer thirdDigit;

    public List<Sprite> sprites;
    
    
    public void AddScore()
    {
        Debug.Log("Score!");
        score++;
        firstDigit.sprite = sprites[score % 10];
        secondDigit.sprite = sprites[(score / 10) % 10];
        thirdDigit.sprite = sprites[(score / 100)];
    }
}
