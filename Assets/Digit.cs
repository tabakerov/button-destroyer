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

    // Update is called once per frame
    void Update()
    {
        thisRigidbody.AddForce(- speed * Time.deltaTime * transform.position.normalized);
        transform.Translate(- Time.deltaTime * transform.position.normalized);
       
        
        
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
