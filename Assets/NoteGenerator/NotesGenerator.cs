using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
//https://github.com/findix/litjson-unity

public class NotesGenerator : MonoBehaviour
{
    //public string musicName;


    [Serializable]
    public class InputJson
    {
        public string name;
        public int maxBlock;
        public int bpm;
        public int offset;
        public List<Note> notes;
    }

    [Serializable]
    public class Note
    {
        public int lpb;
        public int num;
        public int block;
        public int type;
        public List<Note> notes;
    }



    //[System.NonSerialized]
    public static List<int> scoreNum = new List<int>(); // �m�[�c�̔ԍ������ɓ����

    //[System.NonSerialized]
    public List<int> scoreBlock = new List<int>(); // �m�[�c�̎�ނ����ɓ����

    //[System.NonSerialized]
    public List<int> scoreLPB = new List<int>(); // �m�[�c��LPB�����ɓ����

    //[System.NonSerialized]
    public List<int> scoreType = new List<int>(); // �m�[�c��LPB�����ɓ����


    private int BPM;

    [System.NonSerialized]
    public int LPB;

    [SerializeField]
    CubeGenerator cg;

    [SerializeField]
    TextAsset musicJson;

    [SerializeField]
    AudioClip musicClip;

    [SerializeField]
    AudioSource audioSource;


    //[SerializeField]
    //NotesManager notesManager;

    public int difficult = 0;

    void Awake()
    {
        MusicReading();
    }

    enum Difficult
    {
        veryeasy,
        easy,
        normal,
        hard,
        veryhard
    }

    [SerializeField]
    Difficult Priset = Difficult.veryeasy;

    /// <summary>
    /// ���ʂ̓ǂݍ���
    /// </summary>
    
    public void MusicLoad(JsonData jsondata)
    {
        switch (Priset)
        {
            case Difficult.veryeasy:
                difficult = 0;
                break;
            case Difficult.easy:
                difficult = 1;
                break;
            case Difficult.normal:
                difficult = 2;
                break;
            case Difficult.hard:
                difficult = 3;
                break;
            case Difficult.veryhard:
                difficult = 4;
                break;
        }

        for (int i = 0; i < jsondata["notes"][difficult].Count; i++)
        {
            scoreNum.Add(int.Parse(jsondata["notes"][difficult][i]["num"].ToString()));
            scoreBlock.Add(int.Parse(jsondata["notes"][difficult][i]["block"].ToString()));
            scoreLPB.Add(int.Parse(jsondata["notes"][difficult][i]["lpb"].ToString()));
            scoreType.Add(int.Parse(jsondata["notes"][difficult][i]["type"].ToString()));
        }
    }


    void MusicReading()
    {
        string inputString = musicJson.ToString();
            //Resources.Load<TextAsset>(musicName).ToString();
        JsonData jsondata = JsonMapper.ToObject(inputString);

        MusicLoad(jsondata);

        int num = 0;
        //scoreNum = notesManager.NoteNum;

        BPM = int.Parse(jsondata["bpm"].ToString());
        LPB = int.Parse(jsondata["notes"][difficult][0]["lpb"].ToString());
    }

    private void Start()
    {
        audioSource.clip = musicClip;
    }

    private void Update()
    {

        if (GameStateManager.GameState != GameState.Play)
        {
            audioSource.time = 0f;
            beatCount = 0;
            audioSource.Pause();
            GameStateManager.countDown = audioSource.clip.length;
            nowTime = -CubeParameters.startPointZ / CubeParameters.speed ;
            beatBorder = 0;
            beatTorus = 0;

            //Debug.Log("nowTime" + nowTime + ":BPM" + BPM + "CubeParameters.startPointZ" + CubeParameters.startPointZ + "CubeParameters.speed" + CubeParameters.speed);
            return;
        }
        else
        {
            nowTime += Time.deltaTime;
            if (!audioSource.isPlaying && nowTime >= 0f && PauseScript.pauseCount %2 == 0)
            {
                audioSource.Play();
            }
            else
            {
                //audioSource.Pause();
            }
            NotesIns();
        }
    }

    public void GameStart()
    {
        
        /*
        audioSource.Play();
        nowTime = 0f;
        //InvokeRepeating("NotesIns", 0f, moveSpan);
        */
    }

    //[SerializeField]
    //private GameObject notesPre;

    private float moveSpan = 0.01f;
    private float nowTime;// ���y�̍Đ�����Ă��鎞��
    private int beatNum;// ���̔���
    private int beatCount;// json�z��p(����)�̃J�E���g
    private bool isBeat;// �r�[�g��ł��Ă��邩(�����̃^�C�~���O)

    [SerializeField] GameObject torusPrefab;
    [SerializeField] GameObject borderPrefab;
    [SerializeField] GameObject borderPrefab2;

    int beatTorus;
    int beatBorder;

    /// <summary>
    /// ���ʏ�̎��ԂƃQ�[���̎��Ԃ̃J�E���g�Ɛ���
    /// </summary>
    /// 

    void GetScoreTime()
    {
        //���̉��y�̎��Ԃ̎擾
            //moveSpan; //(1)

        //�m�[�c�������Ȃ����珈���I��
        if (beatCount > scoreNum.Count) return;

        //�y����łǂ����̎擾
        float instTime = -CubeParameters.startPointZ / CubeParameters.speed ;
        beatNum = (int)((nowTime - instTime) * BPM / 60 * LPB); //(2)
        if((beatNum) >= (BPM) / 60f * beatBorder)
        {
            GameObject torus3 = Instantiate(borderPrefab2);
            CubeController cubeController3 = torus3.GetComponent<CubeController>();
            cubeController3.startPoint = new Vector3(0f, GameStateManager.defauldCenterEyeAnchor.position.y * 3.0f / 9.0f, CubeParameters.startPointZ);
            beatBorder++;
        }

        if ((beatNum) >= (BPM) * LPB / 60f * beatTorus)
        {   
            GameObject torus = Instantiate(torusPrefab);
            CubeController cubeController = torus.GetComponent<CubeController>();
            cubeController.startPoint = new Vector3(0f, GameStateManager.defauldCenterEyeAnchor.position.y * 3.0f / 9.0f, CubeParameters.startPointZ);

            GameObject torus2 = Instantiate(borderPrefab);
            CubeController cubeController2 = torus2.GetComponent<CubeController>();
            cubeController2.startPoint = new Vector3(0f, GameStateManager.defauldCenterEyeAnchor.position.y * 3.0f / 9.0f, CubeParameters.startPointZ);
            beatTorus++;
        }

    }

    /// <summary>
    /// �m�[�c�𐶐�����
    /// </summary>
    void NotesIns()
    {
        GetScoreTime();

        //json��ł̃J�E���g�Ɗy����ł̃J�E���g�̈�v
        if (beatCount < scoreNum.Count)
        {
            Debug.Log("scoreNum" + scoreNum[beatCount] +"/ "+  beatNum);
            isBeat = (scoreNum[beatCount] == beatNum); //(3)
        }

        Debug.Log("isBeat:" + isBeat);

        //�����̃^�C�~���O�Ȃ�
        if (isBeat)
        {
            cg.GenerateCube(scoreBlock[beatCount] - 3, 0f);
            /*
            switch (scoreBlock[beatCount])
            {
                cg.GenerateCube(scoreBlock[beatCount] - 3 * 0.75f, 0f);
            }
            */

            beatCount++; //(5)
            isBeat = false;

        }
    }

}
