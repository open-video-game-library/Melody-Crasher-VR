using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text resultText;

    // Update is called once per frame
    void Update()
    {
        resultText.text = Result.score.ToString("N0");
    }
}
