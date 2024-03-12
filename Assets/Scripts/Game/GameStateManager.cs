using UnityEngine;

public enum GameState
{
    Start,
    Play,
    End
}

public class GameStateManager : MonoBehaviour
{
    public static GameState GameState;

    [SerializeField]
    private Transform centerEyeAnchor;
    public static Transform defauldCenterEyeAnchor;

    [SerializeField]
    private CanvasGroup leftParamterCanvas;
    [SerializeField]
    private CanvasGroup rightParamterCanvas;
    private bool isParameterActive;


    [SerializeField]
    private CanvasGroup[] startCanvas = new CanvasGroup[1];
    [SerializeField]
    private CanvasGroup[] gameCanvas = new CanvasGroup[2];
    [SerializeField]
    private CanvasGroup[] endCanvas = new CanvasGroup[1];

    [SerializeField]
    GameObject razerPointer;

    public static float countDown;

    [SerializeField] AudioSource audioSource;

    [SerializeField]
    NotesGenerator ng;

    // Start is called before the first frame update
    void Start()
    {
        defauldCenterEyeAnchor = centerEyeAnchor;
        GameState = GameState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameState)
        {
            case GameState.Start:
                // メニュー表示切替
                SetActiveCanvasGroup(startCanvas, true);
                SetActiveCanvasGroup(gameCanvas, false);
                SetActiveCanvasGroup(endCanvas, false);
                // Scoreをリセット
                Result.score = 0;
                // レーザーポインタONにする処理
                razerPointer.SetActive(true);
                // プレイ時間をセット
                countDown = Result.playTime;
                break;
            case GameState.Play:
                // メニュー表示切替
                SetActiveCanvasGroup(startCanvas, false);
                SetActiveCanvasGroup(gameCanvas, true);
                SetActiveCanvasGroup(endCanvas, false);
                // Scoreをリセット
                Result.score = 0;
                // レーザーポインタ切る処理
                razerPointer.SetActive(false);
                // カウントダウンする
                countDown -= Time.deltaTime;
                if (countDown <= 0.0f)
                {
                    Result.score = Result.hits;
                    Result.hits = 0;
                    EndGame();
                }
                break;
            case GameState.End:
                // メニュー表示切替
                SetActiveCanvasGroup(startCanvas, false);
                SetActiveCanvasGroup(gameCanvas, false);
                SetActiveCanvasGroup(endCanvas, true);
                // レーザーポインタONにする処理
                razerPointer.SetActive(true);
                // プレイ時間をセット
                countDown = Result.playTime;
                break;
        }
    }

    public void SetActiveParameterCanvas()
    {
        isParameterActive = !isParameterActive;

        leftParamterCanvas.alpha = System.Convert.ToInt32(isParameterActive);
        leftParamterCanvas.blocksRaycasts = isParameterActive;

        rightParamterCanvas.alpha = System.Convert.ToInt32(isParameterActive);
        rightParamterCanvas.blocksRaycasts = isParameterActive;
    }

    public void StartGame()
    {
        GameState = GameState.Start;
    }

    public void PlayGame()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        for (int i = 0; i < cubes.Length; i++) { Destroy(cubes[i]); }

        isParameterActive = false;
        leftParamterCanvas.alpha = System.Convert.ToInt32(isParameterActive);
        leftParamterCanvas.blocksRaycasts = isParameterActive;
        rightParamterCanvas.alpha = System.Convert.ToInt32(isParameterActive);
        rightParamterCanvas.blocksRaycasts = isParameterActive;

        // 目の高さを代入
        defauldCenterEyeAnchor = centerEyeAnchor;
        GameState = GameState.Play;

        ng.GameStart();
    }

    public void EndGame()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        for (int i = 0; i < cubes.Length; i++) { Destroy(cubes[i]); }
        GameState = GameState.End;
    }

    private void SetActiveCanvasGroup(CanvasGroup[] canvasGroups, bool value)
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].alpha = System.Convert.ToInt32(value);
            canvasGroups[i].blocksRaycasts = value;
        }
    }

    public void InvokeSound()
    {
        audioSource.Play(0);
    }
}
