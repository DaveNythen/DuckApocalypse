using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPHandler : MonoBehaviour
{
    public int hp = 5;

    private bool isAlive = true;

    UIManager uiMan;
    GameManager gameMan;
    TouchHandler gun;

    private void Awake()
    {
        uiMan = FindObjectOfType<UIManager>();
        gameMan = FindObjectOfType<GameManager>();
        gun = GetComponent<TouchHandler>();
    }

    private void Start()
    {
        /*if (PlayerPrefs.GetString("WatchedAd").Equals("N"))
            TakeDamage();
        else
            PlayerPrefs.SetString("WatchedAd", "N");*/
    }

    public void TakeDamage()
    {
        if (isAlive)
        {
            hp--;

            uiMan.UpdateHP(hp);

            if (hp == 0)
                Die();
        }
    }

    private void Die()
    {
        isAlive = false;

        StartCoroutine(Dying());
    }

    IEnumerator Dying()
    {
        gun.enabled = false;

        uiMan.ShowGameOver();
        yield return new WaitForSeconds(4f);

        gameMan.FinishGame();
    }
}
