using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public InputActionAsset input;
    public Transform target;
    public Alphabet alphabet;
    public float shootForce;
    public float fireRate;
    private float lastShot;
    public int value;
    public SpriteRenderer spriteRenderer;
    
    public InputAction addAction;
    public InputAction shootAction;
    public Camera camera;
    public Collider2D selfCollider;
    public Collider2D[] colliders = new Collider2D[1];
    private ContactFilter2D _filter2D = new ContactFilter2D().NoFilter();
    public GameObject lost;
    public GameObject spawner;
    public AudioSource sound;
    public AudioClip addClip;
    public AudioClip shootClip;
    
    
    private void Awake()
    {
        shootAction.performed += HandleShoot;
        addAction.performed += AddAction;
    }

    private void OnEnable()
    {
        shootAction.Enable();
        addAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
        addAction.Disable();
    }

    public void OnTurn(InputAction.CallbackContext inputValue)
    {
        Vector2 vector = inputValue.ReadValue<Vector2>();
        target.position = vector;
        //if (vector.sqrMagnitude > 0.01f)
            
        
        //transform.rotation = Quaternion.Euler(0f, 0f,  - Mathf.Rad2Deg * Mathf.Asin(vector.x));
        //transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(new Vector3(vector.x, vector.y, 0f), new Vector3(0f, 0f, 1f)));
        //transform.SetPositionAndRotation(new Vector3(vector.x, vector.y, 0f), Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        Digit digit;
        if (other.TryGetComponent(out digit))
        {
            Debug.Log("kill me!");
            if (digit.old)
            {
               // Destroy(spawner);
                lost.SetActive(true);
                //Destroy(gameObject);
            }
        }
    }

    public void Kill()
    {
        Destroy(spawner);
        lost.SetActive(true);
        FindObjectOfType<Reloader>().Reset();
        Destroy(gameObject);
    }

    public void HandleShoot(InputAction.CallbackContext ctx)
    {
        
        if (selfCollider.OverlapCollider(_filter2D, colliders) == 0)
        {
            if (lastShot + fireRate < Time.time)
            {
                lastShot = Time.time;
                sound.PlayOneShot(shootClip);
                var projectileRigidbody = Instantiate(alphabet.letters[value-1]).GetComponent<Rigidbody2D>();
                projectileRigidbody.AddForce(target.position.normalized * shootForce);
            }
        }
    }

    public void AddAction(InputAction.CallbackContext ctx)
    {
        sound.PlayOneShot(addClip);
        value++;
        if (value > alphabet.letters.Count) value = 1;
        spriteRenderer.sprite = alphabet.letters[value-1].GetComponent<SpriteRenderer>().sprite;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        value = 1;
        spriteRenderer.sprite = alphabet.letters[value-1].GetComponent<SpriteRenderer>().sprite;
        sound = GetComponent<AudioSource>();
    }



    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current.position.ReadValue();
        target.position = 2f * camera.ScreenToViewportPoint(mouse) - new Vector3(1f, 1f, 0f);
        transform.LookAt(target.position, Vector3.back);

    }
}
