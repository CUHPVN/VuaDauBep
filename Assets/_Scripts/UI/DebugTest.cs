using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DebugTest : MonoBehaviour
{
    [SerializeField] private Transform RightTop;
    [SerializeField] private TimingManager timingManager;
    [SerializeField] private List<UnityEngine.UI.Button> buttons = new();
    void Start()
    {
        LoadButton();
    }

    void Update()
    {
        
    }
    void LoadButton()
    {
        foreach(Transform tran in RightTop)
        {
            buttons.Add(tran.GetComponent<UnityEngine.UI.Button>());
        }
        int i = 1;
        foreach(UnityEngine.UI.Button tran in buttons)
        {
            int inx = i;
            i++;
            tran.onClick.AddListener(()=>PlayLevel(inx));
        }
    }

    void PlayLevel(int level)
    {
        TimingManager.Instance.TurnOn(level);
    }
}
