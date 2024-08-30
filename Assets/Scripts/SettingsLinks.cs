using UnityEngine;
using UnityEngine.UI;

public class SettingsLinks : MonoBehaviour
{

    [SerializeField] Image soundsLinks;
    [SerializeField] Image musicLinks;

    void Start()
    {
        soundsLinks.color = PlayerPrefs.GetInt("soundsLinks", 1) == 1 ? Color.white : Color.gray;
        musicLinks.color = PlayerPrefs.GetInt("musicLinks", 1) == 1 ? Color.white : Color.gray;

    }

    public void toggleSoundsLinks()
    {
        int soundLinks = PlayerPrefs.GetInt("soundsLinks", 1);
        soundLinks += 1;
        soundLinks %= 2;
        PlayerPrefs.SetInt("soundsLinks", soundLinks);
        PlayerPrefs.Save();
        soundsLinks.color = PlayerPrefs.GetInt("soundsLinks", 1) == 1 ? Color.white : Color.gray;

    }


    public void toggleMusicLinks()
    {
        int musicLinks1 = PlayerPrefs.GetInt("musicLinks", 1);
        musicLinks1 += 1;
        musicLinks1 %= 2;
        PlayerPrefs.SetInt("musicLinks", musicLinks1);
        PlayerPrefs.Save();
        musicLinks.color = PlayerPrefs.GetInt("musicLinks", 1) == 1 ? Color.white : Color.gray;

    }


}
