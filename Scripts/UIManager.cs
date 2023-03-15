using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup gameOver;

    [Header("HP")]
    [SerializeField] GameObject heartContainer;
    [SerializeField] Sprite chicken;
    private List<Image> _hearts = new List<Image>();

    [Header ("Score")]
    [SerializeField] Text scoreText;
    private int _score;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        foreach (Image heart in heartContainer.GetComponentsInChildren<Image>())
            _hearts.Add(heart);

        _score = gameManager.GetScore();
        scoreText.text = _score.ToString();
    }

    public void UpdateHP(int currentHP)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            if (i <= currentHP - 1)
                _hearts[i].sprite = chicken;
            else
                _hearts[i].color = new Color(1,1,1,0);
        }
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
        scoreText.text = _score.ToString();
    }

    public void ShowGameOver()
    {
        gameOver.gameObject.SetActive(true);
        StartCoroutine(FadeIn(gameOver));
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float speedFade = 3f;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += (0.01f * speedFade);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public int GetScore()
    {
        return _score;
    }
}
