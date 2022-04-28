using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    static string input_name;
    static string input_country;
    public InputField nameInputField;
    public InputField countryInputField;
    public APITest apiTest;

    void Start()
    {
        APITest apiTest = GetComponent<APITest>();
    }

    void Update()
    {
        input_name = nameInputField.text;
        input_country = countryInputField.text;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            apiTest.AddUserMethod(input_name, input_country);
        }
    }
}
