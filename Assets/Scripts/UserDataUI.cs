using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UserDataUI : MonoBehaviour
{
    public string input_name;
    public string input_password;
    public string input_country;
    public GameObject nameInputField;
    public GameObject passwordInputField;
    public GameObject apiTest;
    public GameObject userErrorMessage;
    public GameObject countryErrorMessage;
    APITest api;
    public bool countrySelected = false;

    void Start()
    {
        api = apiTest.GetComponent<APITest>();
        var dropdown = transform.GetComponent<Dropdown>();
        
        dropdown.options.Clear();

        List<string> countries = new List<string>()
        {
            "Argentina",
            "Bahamas",
            "Belize",
            "Bolivia",
            "Brazil",
            "Canada",
            "Chile",
            "Colombia",
            "Costa Rica",
            "Cuba",
            "Ecuador",
            "El Salvador",
            "Guatemala",
            "Guyana",
            "Honduras",
            "Mexico",
            "Nicaragua",
            "Panama",
            "Paraguay",
            "Peru",
            "Puerto rico",
            "United States",
            "Uruguay",
            "Venezuela"
        };

        // Fill dropdown list with countries
        foreach(var country in countries)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = country});
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate 
        { 
            DropdownItemSelected(dropdown);
        });
    }
    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        input_country = dropdown.options[index].text;
        countrySelected = true;
    }
    bool ReadName()
    {
        if(input_name.Length < 3 || input_name.Length > 10)
        {
            return false;
        }
        else
        {
            input_name = nameInputField.GetComponent<TMP_Text>().text;
            return true;
        }
    }
    bool ReadPassword()
    {
        if(input_password.Length < 6 || input_password.Length > 16)
        {
            return false;
        }
        else
        {
            input_password = passwordInputField.GetComponent<TMP_Text>().text;
            return true;
        }
    }
    public void SubmitUserData()
    {
        if(ReadName() && ReadPassword())
        {
            api.VerifyUserMethod(input_name, input_password);
        }
        else
        {
            userErrorMessage.SetActive(true);
        }
    }
    public void SubmitCountry()
    {
        if(countrySelected)
        {
            api.AddUserMethod(input_name, input_password, input_country);
        }
        else
        {
            countryErrorMessage.SetActive(true);
        }
    }
    void Update(){}
}
