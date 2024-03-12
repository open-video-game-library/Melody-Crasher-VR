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
                // ���j���[�\���ؑ�
                SetActiveCanvasGroup(startCanvas, true);
                SetActiveCanvasGroup(gameCanvas, false);
                SetActiveCanvasGroup(endCanvas, false);
                // Score�����Z�b�g
                Result.score = 0;
                // ���[�U�[�|�C���^ON�ɂ��鏈��
                razerPointer.SetActive(true);
                // �v���C���Ԃ��Z�b�g
                countDown = Result.playTime;
                break;
            case GameState.Play:
                // ���j���[�\���ؑ�
                SetActiveCanvasGroup(startCanvas, false);
                SetActiveCanvasGroup(gameCanvas, true);
                SetActiveCanvasGroup(endCanvas, false);
                // Score�����Z�b�g
                Result.score = 0;
                // ���[�U�[�|�C���^�؂鏈��
                razerPointer.SetActive(false);
                // �J�E���g�_�E������
                countDown -= Time.deltaTime;
                if (countDown <= 0.0f)
                {
                    Result.score = Result.hits;
                    Result.hits = 0;
                    EndGame();
                }
                break;
            case GameState.End:
                // ���j���[�\���ؑ�
                SetActiveCanvasGroup(startCanvas, false);
                SetActiveCanvasGroup(gameCanvas, false);
                SetActiveCanvasGroup(endCanvas, true);
                // ���[�U�[�|�C���^ON�ɂ��鏈��
                razerPointer.SetActive(true);
                // �v���C���Ԃ��Z�b�g
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

        // �ڂ̍�������
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
