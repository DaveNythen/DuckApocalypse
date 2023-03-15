using System;
using UnityEngine;

public class AddHighscore : MonoBehaviour
{
    public void AddHighscoreEntry(int points)
    {
        if (points == 0)
            return;

        string userName = PlayerPrefs.GetString("UserName");
        string id = Guid.NewGuid().ToString();
        
        //Save updated Highscores
        FirebaseDatabaseManager.instance.SaveHighcore(userName, points, id);

        //Save it to compare
        PlayerPrefs.SetString("lasthighscoreid", id);
    }
}
