/*
Test for the connection to an API
Able to use the Get method to read data and "Post" to send data
NOTE: Using Put instead of Post. See the links around line 86
Gilberto Echeverria
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

// Allow the class to be extracted from Unity
// https://stackoverflow.com/questions/40633388/show-members-of-a-class-in-unity3d-inspector
[System.Serializable]
public class User_data
{
    // public int user_id;
    public string user_name;
    public string country;
}
[System.Serializable]
public class Attempts
{
    public int user_name;
    public string level_att;
    public int score;
    public double accuracy;
    public int time_elapsed;
    public int result;
}

// Allow the class to be extracted from Unity
[System.Serializable]
public class User_data_list
{
    public List<User_data> userData;
}

public class APITest : MonoBehaviour
{
    [SerializeField] string url;
    // API routes
    [SerializeField] string getUsersEP;
    [SerializeField] string putUsersEP;
    [SerializeField] string putAttemptsEP;

    // This is where the information from the api will be extracted
    // public User_data_list allUserData;

    // Update is called once per frame
    void Update(){}

    public void AddUserMethod(string input_name, string input_country)
    {
        StartCoroutine(AddUser(input_name, input_country));
    }

    public void AddAttemptMethod(string level_attempted, int obtained_score, double obtained_accuracy, int elapsed_time, int obtained_result)
    {
        StartCoroutine(AddAttempt(level_attempted, obtained_score, obtained_accuracy, elapsed_time, obtained_result));
    }

    IEnumerator GetUsers(string user_name)
    {
        UnityWebRequest www = UnityWebRequest.Get(url + getUsersEP);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            //Debug.Log("Response: " + www.downloadHandler.text);
            // Compose the response to look like the object we want to extract
            // https://answers.unity.com/questions/1503047/json-must-represent-an-object-type.html
            string jsonString = "{\"users\":" + www.downloadHandler.text + "}";
            // user_name = JsonUtility.FromJson<User_data_list>(jsonString)
        } else {
            Debug.Log("Error: " + www.error);
        }
    }

    IEnumerator AddUser(string input_name, string input_country)
    {
        // Create the object to be sent as json
        User_data newUser = new User_data();
        newUser.user_name = input_name;
        newUser.country = input_country;
        Debug.Log("USER: " + newUser);
        Debug.Log(newUser.user_name);
        Debug.Log(newUser.country);

        string jsonData = JsonUtility.ToJson(newUser);
        //Debug.Log("BODY: " + jsonData);

        // Send using the Put method:
        // https://stackoverflow.com/questions/68156230/unitywebrequest-post-not-sending-body
        using(UnityWebRequest www = UnityWebRequest.Put(url + putUsersEP, jsonData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                Debug.Log("Response: " + www.downloadHandler.text);
            } else {
                Debug.Log("Error: " + www.error);
            }
        }
        // Set the method later, and indicate the encoding is JSON
    }

    IEnumerator AddAttempt(string level_attempted, int obtained_score, double obtained_accuracy, int elapsed_time, int obtained_result)
    {
        Attempts newAttempt = new Attempts();
        newAttempt.level_att = level_attempted;
        newAttempt.score = obtained_score;
        newAttempt.accuracy = obtained_accuracy;
        newAttempt.time_elapsed = elapsed_time;
        newAttempt.result = obtained_result;
        Debug.Log("ATTEMPT: " + newAttempt);

        string jsonData = JsonUtility.ToJson(newAttempt);
        //Debug.Log("BODY: " + jsonData);

        // Send using the Put method:
        // https://stackoverflow.com/questions/68156230/unitywebrequest-post-not-sending-body
        UnityWebRequest www = UnityWebRequest.Put(url + putAttemptsEP, jsonData);
        // Set the method later, and indicate the encoding is JSON
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            Debug.Log("Response: " + www.downloadHandler.text);
        } else {
            Debug.Log("Error: " + www.error);
        }
    }
}