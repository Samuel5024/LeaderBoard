using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor.PackageManager;

public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;

    //create an instance of the script
    public static Leaderboard instance;
    private void Awake()
    {
        instance = this;
    }

    // OnLoggedIn gets caled when we log into our PlayFab account
    public void OnLoggedIn ()
    {
        leaderboardCanvas.SetActive(false);
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard ()
    {
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "FastestTime",
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
            result => UpdateLeaderboardUI(result.Leaderboard),
            error => Debug.Log(error.ErrorMessage)
         );
    }

    // UpdateLeaderboardUI gets called when we get the API request back with a list of players in the leaderboard
    // This function will display the leaderboard on-screen
    void UpdateLeaderboardUI (List<PlayerLeaderboardEntry> leaderboard)
    {
        for(int x = 0; x < leaderboardEntries.Length; x++)
        {
            leaderboardEntries[x].SetActive(x < leaderboard.Count);
            if (x >= leaderboard.Count)
            {
                continue;
            }

            leaderboardEntries[x].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            leaderboardEntries[x].transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = (-(float)leaderboard[x].StatValue * 0.001f).ToString("F2");
        }
    }

    public void SetLeaderboardEntry (int newScore)
    {
        bool useLegacyMethod = false;
        
        if (useLegacyMethod)
        {
            ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
            {
                FunctionName = "UpdateHighScore",
                FunctionParameter = new
                {
                    score = newScore
                }
            };

            PlayFabClientAPI.ExecuteCloudScript(request,
                result =>
                {
                    Debug.Log(result);
                    //Debug.Log("SUCCESS");
                    //Debug.Log(result.FunctionName);
                    //Debug.Log(result.FunctionResult);
                    //Debug.Log(result.FunctionResultTooLarge);
                    //Debug.Log(result.Error);
                    DisplayLeaderboard();
                    Debug.Log(result.ToJson());
                },
                error =>
                {
                    Debug.Log(error.ErrorMessage);
                    Debug.Log("ERROR");
                    
                }    
             );
        }
        else
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate 
                        { 
                            StatisticName = "FastestTime",
                            Value = newScore 
                        },
                    }
                },
                result =>
                {
                    Debug.Log("User statistics updated");
                },
                error =>
                {
                    Debug.LogError(error.GenerateErrorReport());
                }
            );
        }
    }
}
