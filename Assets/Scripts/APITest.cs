/*
Test for the connection to an API
Able to use the Get method to read data and "Post" to send data
NOTE: Using Put instead of Post. See the links around line 86
Gilberto Echeverria
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// using TMPro_Test;

// Allow the class to be extracted from Unity
// https://stackoverflow.com/questions/40633388/show-members-of-a-class-in-unity3d-inspector
[System.Serializable]
public class User_data
{
    public int user_id;
    public string user_name;
    public string country;
}
[System.Serializable]
public class Attempts
{
    public int user_id;
    public string name;
    public string surname;
}
[System.Serializable]
public class Level_data_user
{
    public string level_name;
    public int user_id;
    public int lvl_1_highscore;
    public int lvl_2_highscore;
    public int lvl_1_rating;
    public int lvl_2_rating;
    public int lvl_1_attempts;
    public int lvl_2_attempts;
}
[System.Serializable]
public class Music_data
{
    public string song_name;
    public int duration;
    public int note_amount;
}

// Allow the class to be extracted from Unity
[System.Serializable]
public class User_data_list
{
    public List<User_data> userData;
}
[System.Serializable]
public class Attempts_list
{
    public List<Attempts> attempts;
}
[System.Serializable]
public class Level_data_user_list
{
    public List<Level_data_user> levelDataUser;
}
[System.Serializable]
public class Music_data_list
{
    public List<Music_data> musicData;
}

public class APITest : MonoBehaviour
{
    [SerializeField] string url;
    [SerializeField] string getUsersEP;

    // This is where the information from the api will be extracted
    public User_data_list allUserData;
    public Attempts_list allAttemptsData;
    public Level_data_user_list allLevelData;
    public Music_data_list allMusicData;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(GetUsers());
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            StartCoroutine(AddUser());
        }
    }

    IEnumerator GetUsers()
    {
        UnityWebRequest www = UnityWebRequest.Get(url + getUsersEP);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            //Debug.Log("Response: " + www.downloadHandler.text);
            // Compose the response to look like the object we want to extract
            // https://answers.unity.com/questions/1503047/json-must-represent-an-object-type.html
            string jsonString = "{\"users\":" + www.downloadHandler.text + "}";
            allUserData = JsonUtility.FromJson<User_data_list>(jsonString);
            // DisplayUsers();
        } else {
            Debug.Log("Error: " + www.error);
        }
    }

    IEnumerator AddUser()
    {
        /*
        // This should work with an API that does not expect JSON
        WWWForm form = new WWWForm();
        form.AddField("name", "newGuy" + Random.Range(1000, 9000).ToString());
        form.AddField("surname", "Tester" + Random.Range(1000, 9000).ToString());
        Debug.Log(form);
        */

        // Create the object to be sent as json
        User_data testUser = new User_data();
        testUser.user_name = "newGuy" + Random.Range(1000, 9000).ToString();
        testUser.country = "Mexico";

        //Debug.Log("USER: " + testUser);
        string jsonData = JsonUtility.ToJson(testUser);
        //Debug.Log("BODY: " + jsonData);
        // Send using the Put method:
        // https://stackoverflow.com/questions/68156230/unitywebrequest-post-not-sending-body
        UnityWebRequest www = UnityWebRequest.Put(url + getUsersEP, jsonData);
        //UnityWebRequest www = UnityWebRequest.Post(url + getUsersEP, form);
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

    // void DisplayUsers()
    // {
    //     TMPro_Test texter = GetComponent<TMPro_Test>();
    //     texter.LoadNames(allUserData);
    // }
}