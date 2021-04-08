
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class APIController : MonoBehaviour
{
    [SerializeField]
    string MAIN_URL = "http://worldtimeapi.org/api/ip";

    APITime apiTime;
    Text clock;
    private string ReadJsonFromAPI(string URL)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());

        string jsonResponse = reader.ReadToEnd();
        return jsonResponse;
    }

    private void Awake()
    {
        clock = GetComponent<Text>();
        apiTime = new APITime();
    }

    private void Start()
    {
        SetClock();
    }

    private void Update()
    {
        SetClock();
    }

    private void SetClock()
    {
        JsonUtility.FromJsonOverwrite(ReadJsonFromAPI(MAIN_URL), apiTime);
        System.DateTime dateTime = System.DateTime.Parse(apiTime.datetime);
        
        clock.text = dateTime.ToString("yyyy-MM-dd hh:mm:ss");
    }
}