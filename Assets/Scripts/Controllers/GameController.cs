using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class GameController : MonoBehaviour
{

    public static event Action HandleClicked = delegate { };
    LevelManager levelManager;
    List<Scroller>scrollers;

    ButtonController spinBtn;

    [SerializeField]
    AudioClip stopSpin;
    [SerializeField]
    AudioClip win;

    AudioSource audioSource;

    GameObject winGO;

    void Awake()
    {
        levelManager = LevelManager.Instance;
        Scroller[] unsortedList;
        switch (levelManager.CurrentGameScene)
        {
            case 1:
                Instantiate(Resources.Load("Prefabs/SlotMachine"), new Vector3(0,-1,0), Quaternion.identity);
                unsortedList = GameObject.FindObjectsOfType<Scroller>();
                scrollers = unsortedList.OrderBy(go => go.name).ToList();
                winGO = GameObject.FindGameObjectWithTag("Win");
                winGO.SetActive(false);
                spinBtn = GameObject.FindObjectOfType<ButtonController>();
                break;
            case 2:
                Instantiate(Resources.Load("Prefabs/Prefabgame2"), Vector3.zero, Quaternion.identity);
                break;
            case 3:
                Instantiate(Resources.Load("Prefabs/Prefabgame3"), Vector3.zero, Quaternion.identity);
                break;
            default:
                unsortedList = GameObject.FindObjectsOfType<Scroller>();
                scrollers = unsortedList.OrderBy(go => go.name).ToList();
                break;
        }

        audioSource = GetComponent<AudioSource>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickBackToMenu()
    {
        LevelManager.Instance.LoadMenu();
    }

    public void OnClickSpinBtn(ButtonController go)
    {
        Console.Log("Spin");
        winGO.SetActive(false);
        spinBtn = go;
        spinBtn.isEnabled = false;
        audioSource.clip = stopSpin;
        int count = 0;
        foreach (Scroller scroller in scrollers)
        {
            float time = 0.5f * count;

            count++;

            StartCoroutine(StartSpin(time, scroller));
        }
        StartCoroutine(EnableSpinBtn());
    }
    IEnumerator StartSpin(float time, Scroller scroller)
    {
        yield return new WaitForSeconds(time);

        scroller.Run();

        Console.Log("Time: " + time);
 
        StartCoroutine(StopSpin(3, scroller));
    }

    IEnumerator StopSpin(float time, Scroller scroller)
    {
        yield return new WaitForSeconds(time);
        
        Console.Log("Stop spin");

        audioSource.Play();
          
        scroller.Stop();
    }

    IEnumerator EnableSpinBtn()
    {
        yield return new WaitForSeconds(4.5f);
        spinBtn.isEnabled = true;
        CheckResult();
    }

    private void CheckResult()
    {
        Debug.Log("CheckResult");
        bool isStopped = false;
        foreach (Scroller scroller in scrollers)
        {
            if (scroller.isStopped)
                isStopped = true;
            else
                isStopped = false;
        }
        Console.Log(isStopped+"");

        if(isStopped)
        {
            if(scrollers[0].GetResult() == scrollers[1].GetResult() && scrollers[0].GetResult() == scrollers[2].GetResult())
            {
                audioSource.clip = win;
                audioSource.Play();
                winGO.SetActive(true);
            }
        }
    }

}
