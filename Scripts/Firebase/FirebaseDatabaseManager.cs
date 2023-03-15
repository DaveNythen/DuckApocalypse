using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FirebaseDatabaseManager : MonoBehaviour
{
    public static FirebaseDatabaseManager instance;

    public DatabaseReference DBReference;

    public List<HighscoreEntry> highscoreEntryList;

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        FirebaseManager.OnFirebaseReady += FirebaseManager_OnFirebaseReady;
    }

    private void OnDisable()
    {
        FirebaseManager.OnFirebaseReady -= FirebaseManager_OnFirebaseReady;
    }

    private void FirebaseManager_OnFirebaseReady()
    {
        DBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task<List<HighscoreEntry>> GetHighscoresAsync()
    {
        highscoreEntryList = new List<HighscoreEntry>();

        await DBReference.Child("Highscores").GetValueAsync()
            .ContinueWith(LoadDBTask =>
            {
                if (LoadDBTask.IsFaulted)
                {
                    Debug.LogWarning($"Failed to register task with {LoadDBTask.Exception}");
                    return;
                }
                else if (LoadDBTask.Result.Value == null)
                {
                    Debug.Log("DB is empty");
                    return;
                }
                else if (LoadDBTask.IsCompleted)
                {
                    //Data has been retrieved
                    DataSnapshot snapshot = LoadDBTask.Result;

                    Dictionary<string, object> loadedData = snapshot.Value as Dictionary<string, object>;

                    foreach (var entry in loadedData)
                    {
                        Dictionary<string, object> Entry = entry.Value as Dictionary<string, object>;

                        highscoreEntryList.Add(new HighscoreEntry
                        {
                            userName = Entry["userName"].ToString(),
                            points = int.Parse(Entry["points"].ToString()),
                            id = Entry["id"].ToString()
                        });
                    }
                }
            });

        return highscoreEntryList;
    }

    public void SaveHighcore(string userName, int points, string id)
    {
        DBReference.Child("Highscores").Child("HighscoresEntry" + "-" + userName + "-" + id).Child("userName").SetValueAsync(userName)
            .ContinueWith(DBTask =>
            {
                if (DBTask.IsFaulted)
                {
                    Debug.LogWarning($"Failed to register task with {DBTask.Exception}");
                    return;
                }
                else if (DBTask.IsCompleted)
                {
                    Debug.Log("UserName Updated");
                }
            });

        DBReference.Child("Highscores").Child("HighscoresEntry" + "-" + userName + "-" + id).Child("points").SetValueAsync(points)
           .ContinueWith(DBTask =>
           {
               if (DBTask.IsFaulted)
               {
                   Debug.LogWarning($"Failed to register task with {DBTask.Exception}");
                   return;
               }
               else if (DBTask.IsCompleted)
               {
                   Debug.Log("Points Updated");
               }
           });

        DBReference.Child("Highscores").Child("HighscoresEntry" + "-" + userName + "-" + id).Child("id").SetValueAsync(id)
           .ContinueWith(DBTask =>
           {
               if (DBTask.IsFaulted)
               {
                   Debug.LogWarning($"Failed to register task with {DBTask.Exception}");
                   return;
               }
               else if (DBTask.IsCompleted)
               {
                   Debug.Log("ID Updated");
               }
           });
    }
}
