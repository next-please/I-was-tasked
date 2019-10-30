using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameLogicManager : MonoBehaviour
{
    public GameLogicData Data;
    public Text VersionText;
    public Text PathText;
    public Text DebugText;
    public GameObject OtherCanvas;
    public GameObject VersionCanvas;
    private bool isOpen = false;

    static GameLogicManager instanceInternal = null;
    public static GameLogicManager Inst
    {
        get
        {
            if (instanceInternal == null)
            {
                Debug.Log("should be an error");
            }

            return instanceInternal;
        }
    }

    void Awake()
    {
        if (instanceInternal != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instanceInternal = this;
            VersionText.text = Data.Version;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OtherCanvas.SetActive(isOpen);
            isOpen = !isOpen;
            VersionCanvas.SetActive(isOpen);
        }
    }

    public void WriteCurrentToPath()
    {
        try
        {
            StreamWriter writer = new StreamWriter(PathText.text, true);
            string json = JsonUtility.ToJson(Data);
            writer.WriteLine(json);
            writer.Close();
        }
        catch
        {
            DebugText.text = "Failed to write to " + PathText.text;
            return;
        }
        DebugText.text = "Wrote data to " + PathText.text;
    }

    public void ReadAndOverwriteData()
    {
        try
        {
            StreamReader reader = new StreamReader(PathText.text);
            string json = reader.ReadToEnd();
            JsonUtility.FromJsonOverwrite(json, Data);
            reader.Close();
        }
        catch
        {
            DebugText.text = "Failed to Read " + PathText.text;
            return;
        }
        DebugText.text = "Read " + PathText.text + " into Game";
        VersionText.text = Data.Version;
    }
}
