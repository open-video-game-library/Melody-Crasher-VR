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



    [System.NonSerialized]
    public static List<int> scoreNum = new List<int>(); // ノーツの番号を順に入れる

    [System.NonSerialized]
    public List<int> scoreBlock = new List<int>(); // ノーツの種類を順に入れる

    [System.NonSerialized]
    public List<int> scoreLPB = new List<int>(); // ノーツのLPBを順に入れる
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


    [SerializeField]
    NotesManager notesManager;

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
    /// 譜面の読み込み
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
    private float nowTime;// 音楽の再生されている時間
    private int beatNum;// 今の拍数
    private int beatCount;// json配列用(拍数)のカウント
    private bool isBeat;// ビートを打っているか(生成のタイミング)


    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        //今の音楽の時間の取得
            //moveSpan; //(1)

        //ノーツが無くなったら処理終了
        if (beatCount > scoreNum.Count) return;

        //楽譜上でどこかの取得
        float instTime = -CubeParameters.startPointZ / CubeParameters.speed ;
        beatNum = (int)((nowTime - instTime) * BPM / 60 * LPB); //(2)
    }

    /// <summary>
    /// ノーツを生成する
    /// </summary>
    void NotesIns()
    {
        GetScoreTime();

        //json上でのカウントと楽譜上でのカウントの一致
        if (beatCount < scoreNum.Count)
        {
            Debug.Log("scoreNum" + scoreNum[beatCount] +"/ "+  beatNum);
            isBeat = (scoreNum[beatCount] == beatNum); //(3)
        }

        Debug.Log("isBeat:" + isBeat);

        //生成のタイミングなら
        if (isBeat)
        {
            /*
            //ノーツ0の生成
            if (scoreBlock[beatCount] == 0)
            {
                cg.GenerateCube(0f, 0f);
                Debug.Log("Generated0");
            }

            //ノーツ1の生成
            if (scoreBlock[beatCount] == 1)
            {
                cg.GenerateCube(0f, 0f);
                Debug.Log("Generated1");
            }
            */
            cg.GenerateCube(0f, 0f);

            beatCount++; //(5)
            isBeat = false;

        }
    }

}
