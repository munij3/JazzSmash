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
using UnityEngine.SceneManagement;
using TMPro;

// Allow the class to be extracted from Unity
// https://stackoverflow.com/questions/40633388/show-members-of-a-class-in-unity3d-inspector
[System.Serializable]
public class User_data
{
    public string user_name;
    public string password;
    public int user_id;
    public string country;
}
[System.Serializable]
public class Attempts
{
    public int user_id;
    public string level_att;
    public int score;
    public double accuracy;
    public int time_elapsed;
    public int result;
}

[System.Serializable]
public class Level_data
{
    public string level_name;
    public int user_id;
}

[System.Serializable]

public class Music_data
{
    public string song_name;
    public int duration;
    public int note_amount;
}

public class APITest : MonoBehaviour
{
    [SerializeField] string url;
    // API routes
    [SerializeField] string checkUsersEP;
    [SerializeField] string postUserEP;
    [SerializeField] string postMusicDataEP;
    [SerializeField] string postAttemptEP;
    [SerializeField] string postLvlDataEP;
    [SerializeField] string getUserIdEP;
    public User_data user; 
    public GameObject userErrorMessage;
    public GameObject countryErrorMessage;

    public void AddUserMethod(string input_name, string input_password, string input_country)
    {
        StartCoroutine(AddUser(input_name, input_password, input_country));
        StartCoroutine(GetId(input_name));
    }

    public void VerifyUserMethod(string input_name, string input_password)
    {
        StartCoroutine(VerifyUser(input_name, input_password));
    }

    public void AddAttemptMethod(string level_attempted, int obtained_score, double obtained_accuracy, int elapsed_time, int obtained_result)
    {
        StartCoroutine(AddAttempt(level_attempted, obtained_score, obtained_accuracy, elapsed_time, obtained_result));
    }

    public void GetUserId(string user_n)
    {
        StartCoroutine(GetId(user_n));
    }

    public void AddMusicDataMethod(string song_name, int duration, int note_amount)
    {
        StartCoroutine(AddLevelData(song_name));
        StartCoroutine(AddMusicData(song_name, duration, note_amount));
    }

    public class Message
    {
        public string message;
    }
    Message newMessage;

   IEnumerator AddLevelData(string song_name)
    {

        Level_data lvlData = new Level_data();
        lvlData.level_name = song_name;
        lvlData.user_id = PlayerPrefs.GetInt("user_id");
        string data = JsonUtility.ToJson(lvlData);
        
        Debug.Log(data);

        using(UnityWebRequest www = UnityWebRequest.Put(url + postLvlDataEP,data))
        {
            // Set the method later, and indicate the encoding is JSON
            www.method="POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) 
            {
                newMessage = new Message();
                newMessage = JsonUtility.FromJson<Message>(www.downloadHandler.text);
                if (newMessage.message == "Data inserted correctly.")
                {
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    countryErrorMessage.GetComponent<TMP_Text>().text = "Error creating user.";
                    countryErrorMessage.SetActive(true);
                }
            }
            else
            {
                countryErrorMessage.GetComponent<TMP_Text>().text = "Error: " + www.error;
                countryErrorMessage.SetActive(true);
            }
        }
    }

    IEnumerator AddUser(string input_name, string input_password, string input_country)
    {
        Debug.Log(input_name);
        Debug.Log(input_password);
        Debug.Log(input_country);

        User_data user = new User_data();
        user.user_name = input_name;
        user.password = input_password;
        user.country = input_country;
        string data = JsonUtility.ToJson(user);
        
        Debug.Log(data);

        using(UnityWebRequest www = UnityWebRequest.Put(url + postUserEP,data))
        {
            // Set the method later, and indicate the encoding is JSON
            www.method="POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) 
            {
                newMessage = new Message();
                newMessage = JsonUtility.FromJson<Message>(www.downloadHandler.text);
                if (newMessage.message == "Data inserted correctly.")
                {
                    PlayerPrefs.SetString("user_name", input_name);
                    PlayerPrefs.SetString("password",  input_password);
                    PlayerPrefs.SetString("country", input_country);
                    yield return new WaitForSeconds(1f);
                    SceneManager.LoadScene(3);
                }
                else
                {
                    countryErrorMessage.GetComponent<TMP_Text>().text = "Error creating user.";
                    countryErrorMessage.SetActive(true);
                }
            }
            else
            {
                countryErrorMessage.GetComponent<TMP_Text>().text = "Error: " + www.error;
                countryErrorMessage.SetActive(true);
            }
        }
    }

    IEnumerator VerifyUser(string input_name, string input_password)
    {
        Debug.Log(input_name);
        Debug.Log(input_password);

        User_data user = new User_data();
        user.user_name = input_name;
        user.password = input_password;
        string data = JsonUtility.ToJson(user);
        using(UnityWebRequest www = UnityWebRequest.Put(url + checkUsersEP, data))
        {
            // Set the method later, and indicate the encoding is JSON
            www.method="POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) 
            {
                // If the user exists
                if(www.downloadHandler.text == "1")
                {
                    PlayerPrefs.SetString("user_name", input_name);
                    PlayerPrefs.SetString("password",  input_password);
                    yield return new WaitForSeconds(1f);
                    SceneManager.LoadScene(3); // Send to level selection
                }
                // If the user does not exist
                else
                {
                    PlayerPrefs.SetString("user_name", input_name);
                    PlayerPrefs.SetString("password",  input_password);
                    yield return new WaitForSeconds(1f);
                    SceneManager.LoadScene(2); // Send to country selection, which will add the user after submitting country
                }
            } 
            else 
            {
                userErrorMessage.GetComponent<TMP_Text>().text = "Error: " + www.error;
                userErrorMessage.SetActive(true);
            }
        }
    }
    IEnumerator GetId(string user_n)
    {
        User_data user = new User_data();
        user.user_name = user_n;
        string data = JsonUtility.ToJson(user);
        using(UnityWebRequest www = UnityWebRequest.Put(url + getUserIdEP, data))
        {
            // Set the method later, and indicate the encoding is JSON
            www.method="POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) 
            {
                Debug.Log(int.Parse(www.downloadHandler.text));

                user.user_id = int.Parse(www.downloadHandler.text);

                Debug.Log($"Get user id obtained: {user.user_id}");

                PlayerPrefs.SetInt("user_id", user.user_id);

                Debug.Log($"Prefs id after setting int: {PlayerPrefs.GetInt("user_id")}");
                
            } 
            else 
            {
                userErrorMessage.GetComponent<TMP_Text>().text = "Error: " + www.error;
                userErrorMessage.SetActive(true);
            }
        }
    }

    IEnumerator AddAttempt(string level_attempted, int obtained_score, double obtained_accuracy, int elapsed_time, int obtained_result)
    {
        Attempts newAttempt = new Attempts();

        Debug.Log($"Prefs user name: {PlayerPrefs.GetString("user_name")}");
        Debug.Log($"Prefs user id: {PlayerPrefs.GetString("user_id")}");

        newAttempt.user_id = PlayerPrefs.GetInt("user_id");

        Debug.Log($"New attempt user id: {newAttempt.user_id}");

        newAttempt.level_att = level_attempted;
        newAttempt.score = obtained_score;
        newAttempt.accuracy = obtained_accuracy;
        newAttempt.time_elapsed = elapsed_time;
        newAttempt.result = obtained_result;

        Debug.Log("ATTEMPT: " + newAttempt);

        string jsonData = JsonUtility.ToJson(newAttempt);
        
        Debug.Log("BODY: " + jsonData);

        // Send using the Put method:
        // https://stackoverflow.com/questions/68156230/unitywebrequest-post-not-sending-body
        UnityWebRequest www = UnityWebRequest.Put(url + postAttemptEP, jsonData);
        // Set the method later, and indicate the encoding is JSON
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) 
        {
            Debug.Log("Response: " + www.downloadHandler.text);
        } 
        else 
        {
            Debug.Log("Error: " + www.error);
        }
    }

    IEnumerator AddMusicData(string name, int dur, int amount)
    {
        Music_data newData = new Music_data();
        newData.song_name = name;
        newData.duration = dur;
        newData.note_amount = amount;

        Debug.Log("ATTEMPT: " + newData);

        string jsonData = JsonUtility.ToJson(newData);
        
        Debug.Log("BODY: " + jsonData);

        // Send using the Put method:
        // https://stackoverflow.com/questions/68156230/unitywebrequest-post-not-sending-body
        UnityWebRequest www = UnityWebRequest.Put(url + postMusicDataEP, jsonData);
        // Set the method later, and indicate the encoding is JSON
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) 
        {
            Debug.Log("Response: " + www.downloadHandler.text);
        } 
        else 
        {
            Debug.Log("Error: " + www.error);
        }
    }
}