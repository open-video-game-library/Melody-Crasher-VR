using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
//https://github.com/findix/litjson-unity


public class Data
{

    public string name;
    public int maxBlock;
    public int bpm;
    public int offset;
    public Notes[] notes;

}

[Serializable]
public class Notes
{
    public Note[] notes;
}

public class Note
{
    public int type;
    public int num;
    public int block;
    public int LPB;
}

public class NotesManager : MonoBehaviour
{
    public int noteNum;
    private string songName;

    public List<int> LaneNum = new List<int>();
    public List<int> NoteType = new List<int>();
    public List<float> NotesTime = new List<float>();
    public List<GameObject> NotesObj = new List<GameObject>();

    [SerializeField] private float NotesSpeed;
    [SerializeField] GameObject noteObj;

    void OnEnable()
    {
        noteNum = 0;
        songName = "m1";
        Load(songName);
    }

    private void Load(string SongName)
    {
        string inputString = Resources.Load<TextAsset>(SongName).ToString();
        JsonData jsondata = JsonMapper.ToObject(inputString);

        //Debug.Log("test" + jsondata["notes"][0][0]["num"].ToString());

        
        //LaneNum.Add(note.num);
        //noteNum = inputJson.notes.Length;

        for (int i = 0; i < 50; i++)
        {
            /*
            float kankaku = 60 / (inputJson.BPM * (float)inputJson.notes[0][i].LPB);
            float beatSec = kankaku * (float)inputJson.notes[0][i].LPB;
            float time = (beatSec * inputJson.notes[0][i].num / (float)inputJson.notes[0][i].LPB) + inputJson.offset + 0.01f;
            NotesTime.Add(time);
            LaneNum.Add(inputJson.notes[0][i].block);
            NoteType.Add(inputJson.notes[0][i].type);

            float z = NotesTime[i] * NotesSpeed;
            //NotesObj.Add(Instantiate(noteObj, new Vector3(inputJson.notes[i].block - 1.5f, 0.55f, z), Quaternion.identity));
            */
        }
    }
}