using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserDataUI : MonoBehaviour
{
    public string input_name;
    public string input_country;
    public GameObject nameInputField;
    public GameObject countryInputField;
    public GameObject apiTest;
    APITest api;

    void Start()
    {
        api = apiTest.GetComponent<APITest>();
    }

    public void ReadName()
    {
        input_name = nameInputField.GetComponent<TMP_Text>().text;
    }
    public void ReadCountry()
    {
        input_country = countryInputField.GetComponent<TMP_Text>().text;
    }
    public void SubmitData()
    {
        ReadName();
        ReadCountry();
        api.AddUserMethod(input_name, input_country);
    }
    void Update(){}
}
