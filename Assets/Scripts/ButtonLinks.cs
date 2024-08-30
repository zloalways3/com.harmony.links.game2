using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLinks : MonoBehaviour
{

    [SerializeField] Sprite blockedLinks;
    [SerializeField] Sprite StarLinks;
    [SerializeField] Sprite circleLinks;
    [SerializeField] int levelLinks;
    [SerializeField] Image[] starsButtonLinks;

    void Start()
    {
        int currentLinks = PlayerPrefs.GetInt("currentLinks", 0);
        if(levelLinks<=currentLinks)
        {
            Text labelButtonLinks = GetComponentInChildren<Text>();
            labelButtonLinks.text = (levelLinks+1).ToString();
            int starsLinks = PlayerPrefs.GetInt($"stars{levelLinks}", 0);
            for (int ilinks = 0; ilinks < starsButtonLinks.Length; ilinks++)
            {
                starsButtonLinks[ilinks].sprite = circleLinks;
            }
            for (int ilinks = 0; ilinks < starsLinks; ilinks++)
            {
                starsButtonLinks[ilinks].sprite = StarLinks;
            }
        } else
        {
            for (int ilinks = 0; ilinks < starsButtonLinks.Length; ilinks++)
            {
                starsButtonLinks[ilinks].gameObject.SetActive(false);
            }
            Text labelButtonLinks = GetComponentInChildren<Text>();
            labelButtonLinks.gameObject.SetActive(false);
            Image imageLinks = GetComponent<Image>();
            imageLinks.sprite = blockedLinks;
            Button buttonLinks = GetComponent<Button>();
            buttonLinks.interactable = false;
        }

    }

    public void clickLinks()
    {
        PlayerPrefs.SetInt(nameof(levelLinks), levelLinks);
        PlayerPrefs.Save();
        int tmpLinks = levelLinks * 2;
        SceneManager.LoadScene(4);
    }
}
