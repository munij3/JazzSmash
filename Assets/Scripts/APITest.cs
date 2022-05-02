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
    // public int user_id;
    public string user_name;
    public string password;
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
    [SerializeField] string checkUsersEP;
    [SerializeField] string putUsersEP;
    [SerializeField] string putAttemptsEP;
    [SerializeField] string getUserIdEP;
    public User_data user; 
    public GameObject userErrorMessage;
    public GameObject countryErrorMessage;
    
    // Update is called once per frame
    void Update(){}

    public void AddUserMethod(string input_name, string input_password, string input_country)
    {
        StartCoroutine(AddUser(input_name, input_password, input_country));
    }
    public void VerifyUserMethod(string input_name, string input_password)
    {
        StartCoroutine(Verify_User(input_name, input_password));
    }

    public void AddAttemptMethod(string level_attempted, int obtained_score, double obtained_accuracy, int elapsed_time, int obtained_result)
    {
        StartCoroutine(AddAttempt(level_attempted, obtained_score, obtained_accuracy, elapsed_time, obtained_result));
    }
    public int GetUserId(string user_name)
    {
        StartCoroutine(GetId(user_name));
    }
    public class Message
    {
        public string message;
    }
    Message msg;

    IEnumerator AddUser(string input_name, string input_password, string input_country)
    {   
        User_data user = new User_data();
        user.user_name=input_name;
        user.password=input_password;
        string data=JsonUtility.ToJson(user);
        using(UnityWebRequest www= UnityWebRequest.Put(url + putUsersEP,data))
        {
            www.method="POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) 
            {
                msg= new Message();
                msg= JsonUtility.FromJson<Message>(www.downloadHandler.text);
                if (msg.message=="User Created Succesfully!")
                {
                    yield return new WaitForSeconds(1.5f);
                    SceneManager.LoadScene(3);;
                }
            }
        }
    }

    IEnumerator Verify_User(string input_name, string input_password)
    {   
        User_data user = new User_data();
        user.user_name = input_name;
        user.password = input_password;
        string data = JsonUtility.ToJson(user);
        using(UnityWebRequest www = UnityWebRequest.Put(url + checkUsersEP, data))
        {
        //using
            www.method="POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) 
            {
                if(www.downloadHandler.text == "1")
                {
                    PlayerPrefs.SetString("user_name", user.user_name);
                    PlayerPrefs.SetString("user_password", user.password);
                    yield return new WaitForSeconds(.8f);
                    SceneManager.LoadScene(2);
                }
                else
                {
                    userErrorMessage.SetActive(true);
                }
            } 
            else 
            {
                userErrorMessage.GetComponent<TMP_Text>().text = "Error: " + www.error;
                userErrorMessage.SetActive(true);
            }
        }
    }
    IEnumerator GetId(string user_name)
    {
        User_data user = new User_data();
        user.user_name = user_name;
        string data = JsonUtility.ToJson(user);
        using(UnityWebRequest www = UnityWebRequest.Put(url + getUserIdEP, data))
        {
        //using
            www.method="POST";
            www.SetRequestHeader("Content-Type", "Application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) 
            {
                if(www.downloadHandler.text == "1")
                {
                    PlayerPrefs.SetString("user_name", user.user_name);
                    PlayerPrefs.SetString("user_password", user.password);
                    yield return new WaitForSeconds(.8f);
                    SceneManager.LoadScene(2);
                }
                else
                {
                    userErrorMessage.SetActive(true);
                }
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
        newAttempt.user_id = GetUserId(PlayerPrefs.GetString("user_name"));
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