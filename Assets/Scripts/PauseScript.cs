using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public static int pauseCount;

    [SerializeField] AudioSource audiosource;

    [SerializeField]
    GameStateManager gameStateManager;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            pauseCount++;
            PauseEdit();
            // Aボタンが押された時の処理
        }

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            gameStateManager.EndGame();
            // Bボタン。強制停止
        }
    }

    void PauseEdit()
    {
        if (pauseCount %2 == 0)
        {
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            Time.timeScale = 1;
        }
        else  //1回目
        {
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            Time.timeScale = 0;
            audiosource.Pause();
        }
    }
}
