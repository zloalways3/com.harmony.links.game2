using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuLinks : MonoBehaviour
{
    private float linksFloat = 0f;
    private string linksString = "";

    public void openLevelsLinks()
    {
        linksString += linksFloat.ToString();
        SceneManager.LoadScene(3);
    }

    public void QuitLinks()
    {
        linksFloat += linksString.Length;
        Application.Quit();
    }

}
