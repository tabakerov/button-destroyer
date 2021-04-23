using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Digit : MonoBehaviour
{
    public float speed;
    public int value;
    public Rigidbody2D thisRigidbody;
    public bool old;
    public GameObject digit;
    public Score score;

    void GrowUp()
    {
        old = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        score = FindObjectOfType<Score>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        Invoke("GrowUp", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        thisRigidbody.AddForce(- speed * Time.deltaTime * transform.position.normalized);
        transform.Translate(- Time.deltaTime * transform.position.normalized);
        if (old && transform.position.sqrMagnitude < 0.5f)
        {
            FindObjectOfType<Player>()?.Kill();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Digit otherDigit;
        //Debug.Log("collision");
        Player player;
        if (other.gameObject.TryGetComponent(out player) && old)
        {
            player.Kill();
        }
        if (other.gameObject.TryGetComponent(out otherDigit))
        {
            //Debug.Log("other digit");

            if (otherDigit.value + value == 10)
            {
               // Debug.Log("sum ok");
                score.AddScore();
                Destroy(other.gameObject);
                Destroy(gameObject);

            }
            else
            {
                if (Random.value > 0.7f)
                {
                    //Debug.Log("bad luck");

                    Instantiate(digit, other.GetContact(0).point, Quaternion.identity);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
