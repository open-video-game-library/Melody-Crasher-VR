using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeText;

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Time" + "\n" + GameStateManager.countDown.ToString("N0");
    }
}
