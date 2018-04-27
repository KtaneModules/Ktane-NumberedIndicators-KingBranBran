using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NumberInd : MonoBehaviour {

    public TextMesh NumberText;
    public Material White;
    public Material Black;
    public Material[] BackColors;
    public GameObject LightColor;
    public GameObject Back;
    static List<string> used = new List<string>();
    string[] common = {"CLR", "IND", "TRN", "FRK", "CAR", "FRQ", "NSA", "SIG", "MSA", "SND", "BOB"};
    string color;
    int number1;
    int number2;
    int number3;
    int letter1;
    int letter2;
    int letter3;
    int colorNum;
    string display;
    string actual;
    string on;
    bool lit = false;

    void Awake()
    {
        GetComponent<KMWidget>().OnQueryRequest += GetQueryResponse;
        GetComponent<KMWidget>().OnWidgetActivate += Activate;
        GetComponent<KMBombInfo>().OnBombExploded += ClearList;
        GetComponent<KMBombInfo>().OnBombSolved += ClearList;

        ChooseIndicator();
    }

    void ChooseIndicator()
    {
        if (Random.Range(0, 2) == 5)
        {
            number1 = Random.Range(0, 10);
            number2 = Random.Range(0, 10);
            number3 = Random.Range(0, 10);
            letter1 = (number1 + 20) % 26;
            letter1 += letter1 == 0 ? 26 : 0;
            letter2 = (number2 + 10);
            letter2 += letter2 == 0 ? 26 : 0;
            letter3 = (number3);
            letter3 += letter3 == 0 ? 26 : 0;
            display = "" + number1 + number2 + number3;
            if (used.Contains(display))
            {
                ChooseIndicator();
                return;
            }
            actual = Number2String(letter1, true) + Number2String(letter2, true) + Number2String(letter3, true);
            used.Add(display);
        }
        else
        {
            int c = Random.Range(0, common.Length);
            if (used.Contains(common[c]))
            {
                ChooseIndicator();
                return;
            }
            actual = common[c];
            display = common[c];
            used.Add(actual);
        }

        colorNum = Random.Range(0, BackColors.Length);
        lit = 0 == Random.Range(0, 2);
        on = lit ? "True" : "False";

        Back.GetComponent<Renderer>().material = BackColors[colorNum];
        Debug.LogFormat("[NumberedIndicator] Added {0} {1}, display is {2}, color is {3}", lit ? "LIT" : "UNLIT", actual, display, BackColors[colorNum].name.ToUpperInvariant());
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
    }

    public string GetQueryResponse(string queryKey, string queryInfo)
    {
        if (queryKey == KMBombInfo.QUERYKEY_GET_INDICATOR)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            response.Add("display", display);
            response.Add("label", actual);
            response.Add("on", on);
            response.Add("color", BackColors[colorNum].name.ToLowerInvariant());
            string responseStr = JsonConvert.SerializeObject(response);
            return responseStr;
           
        }

        return "";
    }

    public void ClearList()
    {
        used.Clear();
    }
}
