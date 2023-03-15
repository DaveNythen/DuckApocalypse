using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] InputField nameInputfield;
    [SerializeField] GameObject namePanel;

    private void Start()
    {
        ShowNamePanel();
        //RewardedAds.OnRewardAdWatched += OnAdWatched;
    }

    private void OnDestroy()
    {
        //RewardedAds.OnRewardAdWatched -= OnAdWatched;
    }

    public void StartGame()
    {
        ButtonClick();
        StartCoroutine(TransitionToNextLevel());
    }

    IEnumerator TransitionToNextLevel()
    {
        transition.SetTrigger("TransitionOut");
        float secondsToWait = transition.GetCurrentAnimatorClipInfo(0).Length / 2;

        yield return new WaitForSeconds(secondsToWait);

        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        ButtonClick();
        Application.Quit();
    }

    public void ButtonClick()
    {
        SoundManager.PlaySound(SoundManager.soundList.button);
    }

    public void SubmitName()
    {
        ButtonClick();
        if (!nameInputfield.text.Equals(""))
        {
            PlayerPrefs.SetString("UserName", nameInputfield.text);
            PlayerPrefs.Save();
            namePanel.SetActive(false);
        }
    }

    public void WatchRewarderAd()
    {
        ButtonClick();
        //FindObjectOfType<RewardedAds>().LoadAd();
    }

    private void OnAdWatched()
    {
        StartGame();
    }

    public void DeclineAd()
    {
        PlayerPrefs.SetString("WatchedAd", "N");
        StartGame();
    }

    private void ShowNamePanel()
    {
        if(!PlayerPrefs.GetString("UserName").Equals(""))
            namePanel.SetActive(false);
    }
}
