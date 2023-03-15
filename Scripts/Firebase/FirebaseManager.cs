using UnityEngine;
using Firebase;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    public delegate void OnFirebaseReadyEvent();
    public static event OnFirebaseReadyEvent OnFirebaseReady;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseApp app;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        //Singleton
        if (instance == null)
            instance = this;
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object -> " + name);
            Destroy(instance.gameObject);
            instance = this;
        }

        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(checkDepencencyTask =>
        {
            dependencyStatus = checkDepencencyTask.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                app = FirebaseApp.DefaultInstance;

                //flag
                OnFirebaseReady?.Invoke();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });

    }
}
