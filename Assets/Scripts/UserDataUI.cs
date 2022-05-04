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
    static Dropdown dropdown;
    APITest api;

    void Start()
    {
        api = apiTest.GetComponent<APITest>();

        dropdown = transform.GetComponent<Dropdown>();
        
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
            dropdown.options.Add(new Dropdown.OptionData() { text = country });
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate 
        { 
            DropdownItemSelected(dropdown);
        });
    }

    bool DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        input_country = dropdown.options[index].text;
        return true;
    }

    bool ReadName()
    {
        input_name = nameInputField.GetComponent<TMP_Text>().text;

        if(input_name.Length < 3 || input_name.Length > 10)
        {
            Debug.Log(input_name.Length);
            Debug.Log("false name");
            return false;
        }
        else
        {
            return true;
        }
    }

    bool ReadPassword()
    {
        input_password = passwordInputField.GetComponent<TMP_Text>().text;
        
        if(input_password.Length < 6 || input_password.Length > 12)
        {
            Debug.Log("false password");
            return false;
        }
        else
        {
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
        Debug.Log(input_country);

        if(DropdownItemSelected(dropdown))
        {
            api.AddUserMethod(PlayerPrefs.GetString("user_name"), PlayerPrefs.GetString("password"), input_country);
        }
        else
        {
            countryErrorMessage.SetActive(true);
        }
    }
    void Update(){}
}
