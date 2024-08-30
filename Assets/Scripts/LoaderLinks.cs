using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderLinks : MonoBehaviour
{

    void Update()
    {
        if(Time.timeSinceLevelLoad>1.5f)
        {
            SceneManager.LoadScene(1);
        }
    }
}
