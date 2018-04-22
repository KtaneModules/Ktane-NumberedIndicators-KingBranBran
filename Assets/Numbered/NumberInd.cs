using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NumberInd : MonoBehaviour {

    public TextMesh NumberText;
    public Material White;
    public Material Black;
    public GameObject LightColor;
    int number1;
    int number2;
    int number3;
    int letter1;
    int letter2;
    int letter3;
    string display;
    string actual;
    string on;
    bool lit = false;

    void Awake()
    {
        GetComponent<KMWidget>().OnQueryRequest += GetQueryResponse;
        GetComponent<KMWidget>().OnWidgetActivate += Activate;

        number1 = Random.Range(0, 10);
        number2 = Random.Range(0, 10);
        number3 = Random.Range(0, 10);
        letter1 = (number1 % 3 * 10 + number2) % 26;
        letter1 += letter1 == 0 ? 26 : 0;
        letter2 = (number2 % 3 * 10 + number3) % 26;
        letter2 += letter2 == 0 ? 26 : 0;
        letter3 = (number3 % 3 * 10 + number1) % 26;
        letter3 += letter3 == 0 ? 26 : 0;
        lit = 0 == Random.Range(0, 2);

        display = "" + number1 + number2 + number3;
        actual = Number2String(letter1, true) + Number2String(letter2, true) + Number2String(letter3, true);
        on = lit ? "true" : "false";
    }

    private string Number2String(int number, bool isCaps)
    {
        char c = (char)((isCaps ? 65 : 97) + (number - 1));

        return c.ToString();
    }

    //This happens when the bomb turns on, don't turn on any lights or unlit shaders until activate
    public void Activate()
    {
        NumberText.text = display;
        LightColor.GetComponent<Renderer>().material = lit ? White : Black;
        Debug.LogFormat("{0} {1}", lit ? "lit" : "unlit", actual);
    }

    public string GetQueryResponse(string queryKey, string queryInfo)
    {
        if (queryKey == KMBombInfo.QUERYKEY_GET_INDICATOR)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add("display", display);
            response.Add("label", actual);
            response.Add("on", on);
            string responseStr = JsonConvert.SerializeObject(response);
            return responseStr;
        }

        return "";
    }
}
