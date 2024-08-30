using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLinks : MonoBehaviour
{
    [SerializeField] Sprite starSpriteLinks;
    [SerializeField] Sprite circleSpriteLinks;
    [SerializeField] Text levelLinksLabel;
    [SerializeField] Button buttonNextLinks;
    [SerializeField] Image[] starsEndLinks;
    [SerializeField] GameObject labelWinLinks;
    void Start()
    {
        int levelLinks = PlayerPrefs.GetInt("levelLinks", 0);
        int countLinks = PlayerPrefs.GetInt("endLinks", 0);
        print(countLinks);
        levelLinksLabel.text = $"Level {levelLinks + 1}";
        for (int ilinks = 0; ilinks < starsEndLinks.Length; ilinks++) starsEndLinks[ilinks].sprite = circleSpriteLinks;
        if(countLinks==0)
        {
            labelWinLinks.SetActive(false);
            buttonNextLinks.interactable = false;
        } else
        {
            int starsLinks = PlayerPrefs.GetInt($"stars{levelLinks}", 0);
            if (starsLinks < countLinks) starsLinks = countLinks;
            PlayerPrefs.SetInt($"stars{levelLinks}", starsLinks);
            PlayerPrefs.Save();
            int currentLinks = PlayerPrefs.GetInt("currentLinks", 0);
            if (currentLinks <= levelLinks) currentLinks++;
            currentLinks = Mathf.Min(11, currentLinks);
            PlayerPrefs.SetInt("currentLinks", currentLinks);
            PlayerPrefs.Save();
            for (int jlinks = 0; jlinks < countLinks; jlinks++) starsEndLinks[jlinks].sprite = starSpriteLinks;
        }
    }



    public void next()
    {
        int levelLinks = PlayerPrefs.GetInt("levelLinks", 0);
        levelLinks++;
        levelLinks %= 12;
        PlayerPrefs.SetInt(nameof(levelLinks), levelLinks);
        PlayerPrefs.Save();
        SceneManager.LoadScene(4);
    }

    
}
