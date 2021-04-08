using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    LevelManager levelManager;

    void Awake()
    {
        levelManager = LevelManager.Instance;
        
        switch(levelManager.CurrentGameScene)
        {
            case 1:
                break;
            case 2:
                Instantiate(Resources.Load("Prefabs/Prefabgame2"), Vector3.zero, Quaternion.identity);
                break;
            case 3:
                Instantiate(Resources.Load("Prefabs/Prefabgame3"), Vector3.zero, Quaternion.identity);
                break;
            default:
                break;
        }

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

    public void OnClickSpinBtn(GameObject button)
    {
        button.GetComponent<AudioSource>().Play();        
    }
}
