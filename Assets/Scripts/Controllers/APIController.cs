
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class APIController : MonoBehaviour
{
    [SerializeField]
    string MAIN_URL = "http://worldtimeapi.org/api/ip";

    APITime apiTime;
    Text clock;

    float timer;
    float waitTime = 1f;
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
        StartCoroutine(RunSetClock());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            StartCoroutine(RunSetClock());
            timer = 0;
        }
    }
    IEnumerator RunSetClock()
    {
        yield return SetClock();
    }

    private async Task SetClock()
    {

        JsonUtility.FromJsonOverwrite(ReadJsonFromAPI(MAIN_URL), apiTime);
        System.DateTime dateTime = System.DateTime.Parse(apiTime.datetime);
        clock.text = dateTime.ToString("yyyy-MM-dd hh:mm:ss");        
    }
}