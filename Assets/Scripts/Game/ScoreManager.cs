using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score" + "\n" + Result.hits.ToString() +  "/" +  NotesGenerator.scoreNum.Count.ToString();
    }
}
