using UnityEngine;
using UnityEngine.SceneManagement;


public class UtilsLinks : MonoBehaviour
{
    private int counterLinks = 1;

    public void openSettingsLinks()
    {
        counterLinks = PlayerPrefs.GetInt("links", 0) * 3;
        SceneManager.LoadScene(2);
    }

    public void openMenuLinks()
    {
        counterLinks = "links".Length;
        SceneManager.LoadScene(1);
    }

}
