using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{


    public void OnClick(int index)
    {
        LevelManager.Instance.LoadGame(index);
    }
}
