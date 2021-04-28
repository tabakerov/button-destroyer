using UnityEngine;

public class Digit : MonoBehaviour
{
    public float speed;
    public int value;
    public Rigidbody2D thisRigidbody;
    public bool old;
    public GameObject digit;
    public Score score;
    public bool master;
    public GameObject collapseVfxPrefab;
    public int targetSum;
    public GameObject collisionVfx;
    public Player player;
    public bool hit;
    public float gravityConstant;
    public float gravityDecayExponent;
    public float vPrev;
    
    void GrowUp()
    {
        old = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        targetSum = FindObjectOfType<Alphabet>().letters.Count + 1;
        score = FindObjectOfType<Score>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        Invoke("GrowUp", 1f);
        master = true;
        player = FindObjectOfType<Player>();
    }


    Vector2 CalculateGravity()
    {
        var digits = FindObjectsOfType<Digit>();
        var force = Vector2.zero;

        force -= (Vector2)transform.position.normalized * gravityConstant * value * player.value / (
                        Mathf.Pow((transform.position.magnitude),
                                        gravityDecayExponent));
        foreach (var d in digits)
        {
            if (d.Equals(this)) continue;
            var dir = (Vector2)d.transform.position - (Vector2)transform.position;
            force += dir.normalized * gravityConstant * value * d.value / (
                            Mathf.Pow((dir.magnitude),
                                            gravityDecayExponent)
            );
        }
        
        return force;
    }

    void UpdateDrag()
    {
        Debug.Log($"deltaV = {vPrev - thisRigidbody.velocity.magnitude}");
        if (thisRigidbody.velocity.magnitude < 1f)
        {
            thisRigidbody.drag -= 0.1f;
        }
        else if (thisRigidbody.velocity.magnitude > 7f)
        {
            thisRigidbody.drag += 0.1f;
        }

        vPrev = thisRigidbody.velocity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDrag();
        thisRigidbody.AddForce(CalculateGravity());
        //transform.Translate(- Time.deltaTime * transform.position.normalized);
       
        
        
        if (old && transform.position.sqrMagnitude < 0.5f)
        {
            //Debug.Log($"{value} + {player.value} = {targetSum}");
            if (player.value + value == targetSum)
            {
                score.AddScore();
                var vfx  = Instantiate(collapseVfxPrefab, transform.position, Quaternion.identity);
                Destroy(vfx, vfx.GetComponent<AudioSource>().clip.length);
                Destroy(gameObject);
            }
            else
            {
                player.Kill();    
            }
            
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Digit otherDigit;

        if (other.gameObject.TryGetComponent(out otherDigit))
        {
            if (hit == false)
            {
                hit = true;
                if (otherDigit.value + value == targetSum)
                {
                    if (master)
                    {
                        Debug.Log($"Im {gameObject.name} hitting {otherDigit.gameObject.name}");
                        otherDigit.master = false;
                        score.AddScore();
                        var thisVfx = Instantiate(collapseVfxPrefab, other.transform.position, Quaternion.identity);
                        var otherVfx = Instantiate(collapseVfxPrefab, transform.position, Quaternion.identity);
                        Destroy(thisVfx, thisVfx.GetComponent<AudioSource>().clip.length);
                        Destroy(otherVfx, otherVfx.GetComponent<AudioSource>().clip.length);
                        Destroy(other.gameObject);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (collisionVfx != null)
                    {
                        var vfx = Instantiate(collisionVfx, other.GetContact(0).point, Quaternion.identity);
                        Destroy(vfx, vfx.GetComponent<AudioSource>().clip.length);
                    }

                }
            }
        }
    }
}
