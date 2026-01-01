using UnityEngine;
using SQLite;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Exercise
{
    [PrimaryKey, AutoIncrement]
    public int Workout_id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Time { get; set; }
    public string time_type { get; set; }
}

public class SQLiteNetReader : MonoBehaviour
{
    private string dbPath;
    public string tableName;
    public GameObject exerciseTitlePrefab;
    public GameObject exerciseTitleListParent;
    public GameObject exerciseDescriptionPrefab;
    public GameObject exerciseDescriptionListParent;
    public GameObject startWorkoutPrefab;
    public GameObject startWorkoutParent;

    void Start()
    {
        StartCoroutine(LoadDatabase());
    }

    IEnumerator LoadDatabase()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "HomeWorkout.db");

        // Copy DB from StreamingAssets → persistent on first run
        if (!File.Exists(dbPath))
        {
#if PLATFORM_ANDROID && !UNITY_EDITOR
            // Android requires WWW to read StreamingAssets
            string sourcePath = Path.Combine(Application.streamingAssetsPath, "HomeWorkout.db");
            UnityWebRequest www = UnityWebRequest.Get(sourcePath);
            yield return www.SendWebRequest();
            Debug.Log("Result : " + www.result);
            if (www.result == UnityWebRequest.Result.Success)
            {
                // Write the database to the persistent data path
                File.WriteAllBytes(dbPath, www.downloadHandler.data);
                Debug.Log("Database copied to: " + dbPath);
            }
            else
            {
                Debug.LogError("Failed to load database: " + www.error);
            }

#else
                        Debug.Log("Copy Data To path");
                        string sourcePath = Path.Combine(Application.streamingAssetsPath, "HomeWorkout.db");
                        //File.Copy(sourcePath, dbPath);
                        File.WriteAllBytes(dbPath, File.ReadAllBytes(sourcePath));
                        yield return null;
#endif

            //    string sourcePath = Path.Combine(Application.streamingAssetsPath, "HomeWorkout.db");

            //    // Use File.ReadAllBytes to copy from StreamingAssets
            //    if (File.Exists(sourcePath))
            //    {
            //        File.WriteAllBytes(dbPath, File.ReadAllBytes(sourcePath));
            //        Debug.Log("Database copied to: " + dbPath);  // Log successful copy
            //    }
            //    else
            //    {
            //        Debug.LogError("Source database not found in StreamingAssets.");
            //    }

            //    Debug.Log("Database copied to: " + dbPath);
            //}

            //else
            //{
            //    Debug.Log("Database already exists at: " + dbPath);
            //}

            //if (File.Exists(dbPath))
            //{
            //    Debug.Log("Database exists in persistentDataPath.");
            //}
            //else
            //{
            //    Debug.LogError("Database NOT found in persistentDataPath.");
            //}

            //yield return null;
        }
        ReadData();
    }

    void ReadData()
    {
        var connection = new SQLiteConnection(dbPath);

        var exercises = connection.Query<Exercise>($"SELECT * FROM {tableName}");

        if (exercises.Count == 0)
        {
            Debug.LogWarning($"No data found in table {tableName}.");
        }

        // Automatically maps table to class
        else
        {
            int totalExercises = exercises.Count;
            int i = 0;

            foreach (var e in exercises)
            {
                //Set Exercise List Data
                GameObject exerciseObject = Instantiate(exerciseTitlePrefab);
                exerciseObject.transform.SetParent(gameObject.transform);

                ExerciseTitleData exerciseTitleData = exerciseObject.GetComponent<ExerciseTitleData>();

                exerciseTitleData.SetExerciseTitleData(e.Title, e.Time, e.time_type);

                PanelTransition panelTransition = exerciseObject.GetComponent<PanelTransition>();

                panelTransition.currentPanel = exerciseTitleListParent;
                panelTransition.nextPanel = exerciseDescriptionListParent;

                exerciseObject.GetComponent<Button>().onClick.RemoveAllListeners();
                exerciseObject.GetComponent<Button>().onClick.AddListener(() => panelTransition.ExerciseTransitionUp(e.Workout_id - 1, PreviousPanelType.ExerciseList));

                // Set Exercise Description Data
                GameObject exerciseDescriptionObject = Instantiate(exerciseDescriptionPrefab, exerciseDescriptionListParent.transform);
                //exerciseDescriptionObject.GetComponent<Image>().color = ColorApplicator.instance.ImageColor_light;
                //exerciseDescriptionObject.transform.GetChild(3).GetComponent<TMP_Text>().color = ColorApplicator.instance.TextColor;
                //exerciseDescriptionObject.transform.GetChild(0).GetComponent<Image>().color = ColorApplicator.instance.TextColor;
                //exerciseDescriptionObject.transform.GetChild(1).GetComponent<Image>().color = ColorApplicator.instance.TextColor;
                //exerciseDescriptionObject.transform.GetChild(5).GetChild(2).GetComponent<TMP_Text>().color = ColorApplicator.instance.TextColor;
                exerciseDescriptionObject.SetActive(false);

                ExerciseDescriptionData exerciseDescriptionData = exerciseDescriptionObject.GetComponent<ExerciseDescriptionData>();
                exerciseDescriptionData.SetExerciseDescriptionData(e.Workout_id, e.Title, e.Description, totalExercises, exerciseDescriptionListParent, exerciseTitleListParent, startWorkoutParent);

                //Set Start Workout Data
                GameObject startWorkoutObject = Instantiate(startWorkoutPrefab, startWorkoutParent.transform);
                //startWorkoutObject.transform.GetChild(2).GetComponent<TMP_Text>().color = ColorApplicator.instance.TextColor;
                //startWorkoutObject.transform.GetChild(1).GetComponent<Image>().color = ColorApplicator.instance.TextColor;
                //startWorkoutObject.transform.GetChild(6).GetComponent<TMP_Text>().color = ColorApplicator.instance.TextColor;
                //startWorkoutObject.transform.GetChild(9).GetChild(1).GetComponent<Image>().color = ColorApplicator.instance.ImageColor_dark;
                startWorkoutObject.SetActive(false);

                StartWorkoutData startWorkoutData = startWorkoutObject.GetComponent<StartWorkoutData>();
                startWorkoutData.SetStartWorkoutData(e.Workout_id, e.Title, e.Time, e.time_type, totalExercises, startWorkoutParent, exerciseDescriptionListParent, exerciseTitleListParent);

                i++;
            }
        }

    }
}
