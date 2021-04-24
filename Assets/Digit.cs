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
    public AudioSource sound;
    public bool master;
    public GameObject collapseVfxPrefab;

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
        sound = GetComponent<AudioSource>();
        master = true;
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
        sound.PlayOneShot(sound.clip);
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
                if (master)
                {
                    otherDigit.master = false;
                    score.AddScore();
                    var vfx  = Instantiate(collapseVfxPrefab, other.GetContact(0).point, Quaternion.identity);
                    Destroy(vfx, vfx.GetComponent<AudioSource>().clip.length);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
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
