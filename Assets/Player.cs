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
    public InputAction shootAction;
    public Camera camera;
    public Collider2D selfCollider;
    public Collider2D[] colliders = new Collider2D[1];
    private ContactFilter2D _filter2D = new ContactFilter2D().NoFilter();
    
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

    public void HandleShoot(InputAction.CallbackContext ctx)
    {
        if (selfCollider.OverlapCollider(_filter2D, colliders) == 0)
        {
            var projectileRigidbody = Instantiate(projectiles[value-1]).GetComponent<Rigidbody2D>();
            projectileRigidbody.AddForce(target.position.normalized * shootForce);
        }
    }

    public void AddAction(InputAction.CallbackContext ctx)
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
        transform.LookAt(target.position, Vector3.back);

    }
}
