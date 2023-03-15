using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField] Transform rankingContainer;
    [SerializeField] Transform entryTemplate;

    [SerializeField] protected ScrollRect scrollRect;

    private List<Transform> highscoreEntryTransformList;

    private Transform lastScoreEntryTransform;

    private void Start()
    {
        DisplayHighscoreTable();
    }

    private async void DisplayHighscoreTable()
    {
        entryTemplate.gameObject.SetActive(false);

        Highscores highscores = new Highscores
        {
            highscoreEntryList = await FirebaseDatabaseManager.instance.GetHighscoresAsync()
        };

        //Sort entry list by points
        highscores = SortByPoints(highscores);

        //make sure the last entry is the only one highlighted
        highscores = HighlightLastEntry(highscores);

        //Display entries
        highscoreEntryTransformList = new List<Transform>();

        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            if (highscoreEntry.points > 0)
                CreateScoreEntryTransform(highscoreEntry, rankingContainer, highscoreEntryTransformList);
        }

        //Move the scroll to the last entry
        if(lastScoreEntryTransform != null)
            SnapTo(lastScoreEntryTransform.GetComponent<RectTransform>());
    }

    private Highscores SortByPoints(Highscores highscores)
    {
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = 0; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[i].points > highscores.highscoreEntryList[j].points)
                {
                    //Swap if greater
                    HighscoreEntry aux = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = aux;
                }
            }
        }

        return highscores;
    }

    private Highscores HighlightLastEntry(Highscores highscores)
    {
        string lastEntryID = PlayerPrefs.GetString("lasthighscoreid", "0");

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            if (highscores.highscoreEntryList[i].id.Equals("0"))
                break;

            if (highscores.highscoreEntryList[i].id != lastEntryID)
                highscores.highscoreEntryList[i].lastEntry = false;
            else
                highscores.highscoreEntryList[i].lastEntry = true;
        }

        return highscores;
    }

    private void CreateScoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 50f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;

            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
        }

        entryTransform.Find("PosText").GetComponent<Text>().text = rankString;

        int points = highscoreEntry.points;
        entryTransform.Find("ScoreText").GetComponent<Text>().text = points.ToString();

        entryTransform.Find("LastRecordMark").gameObject.SetActive(highscoreEntry.lastEntry);
        if (highscoreEntry.lastEntry)
            lastScoreEntryTransform = entryTransform;

        string userName = highscoreEntry.userName;
        entryTransform.Find("NameText").GetComponent<Text>().text = userName;

        transformList.Add(entryTransform);
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        RectTransform contentPanel = rankingContainer.GetComponent<RectTransform>();

        contentPanel.anchoredPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(new Vector3(0, contentPanel.position.y, 0))
            - (Vector2)scrollRect.transform.InverseTransformPoint(new Vector3(0, target.position.y, 0));
    }

    public void ResetHighscoreTable()
    {
        List<HighscoreEntry> emptyList = new List<HighscoreEntry>();

        Highscores highscores = new Highscores {highscoreEntryList = emptyList};

        //Save empty Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

        //PlayerPrefs.DeleteKey("highscoreTable");
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }
}
