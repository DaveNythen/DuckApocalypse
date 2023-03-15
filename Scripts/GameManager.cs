using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int _maxScore;

    Animator transition;
    UIManager uiMan;
    AddHighscore addHighscore;
    AudioHandler audioHandler;

    void Awake()
    {
        uiMan = FindObjectOfType<UIManager>();
        addHighscore = GetComponent<AddHighscore>();
        audioHandler = FindObjectOfType<AudioHandler>();
    }

    IEnumerator TransitionToMenu()
    {
        //Refresh the reference
        uiMan = FindObjectOfType<UIManager>();
        //Save score
        _maxScore = uiMan.GetScore();
        addHighscore.AddHighscoreEntry(_maxScore);

        transition = GameObject.Find("Transition").GetComponent<Animator>();

        transition.SetTrigger("TransitionOut");
        float secondsToWait = transition.GetCurrentAnimatorClipInfo(0).Length / 2;

        yield return new WaitForSeconds(secondsToWait);

        //Change music

        //Go back to menu
        SceneManager.LoadScene(0);

        Destroy(gameObject, 0.5f);
    }

    public void FinishGame()
    {
        StartCoroutine(TransitionToMenu());
    }

    public int GetScore()
    {
        return _maxScore;
    }
}
