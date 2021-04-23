using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputActionAsset input;
    public Transform target;
    public List<Sprite> sprites;
    public List<GameObject> projectiles;
    public float shootForce;
    public float fireRate;
    private float lastShot;
    public int value;
    public SpriteRenderer spriteRenderer;
    
    public InputAction addAction;
    public Camera camera;

    private void Awake()
    {
        
    }

    public void OnTurn(InputAction.CallbackContext inputValue)
    {
        Vector2 vector = inputValue.ReadValue<Vector2>();
        target.position = vector;
        if (vector.sqrMagnitude > 0.01f)
            transform.LookAt(target.position, Vector3.back);
        
        //transform.rotation = Quaternion.Euler(0f, 0f,  - Mathf.Rad2Deg * Mathf.Asin(vector.x));
        //transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(new Vector3(vector.x, vector.y, 0f), new Vector3(0f, 0f, 1f)));
        //transform.SetPositionAndRotation(new Vector3(vector.x, vector.y, 0f), Quaternion.identity);
    }

    public void HandleShoot(InputAction.CallbackContext ctx)
    {
        if (lastShot + fireRate < Time.time)
        {
            lastShot = Time.time;
            var projectileRigidbody = Instantiate(projectiles[value-1]).GetComponent<Rigidbody2D>();
            projectileRigidbody.AddForce(target.position.normalized * shootForce);
        }
    }

    public void Add()
    {
        value++;
        if (value > 9) value = 1;
        spriteRenderer.sprite = sprites[value-1];
    }
    
    // Start is called before the first frame update
    void Start()
    {
        value = 1;
        spriteRenderer.sprite = sprites[value-1];
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current.position.ReadValue();
        var screen = Screen.currentResolution;
        target.position = 2f * camera.ScreenToViewportPoint(mouse) - new Vector3(1f, 1f, 0f);

    }
}
