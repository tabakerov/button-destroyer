
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloader : MonoBehaviour
{

    
    public     void Reset()
    {
        Invoke("Reload", 2f);
    }

    void Reload()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
