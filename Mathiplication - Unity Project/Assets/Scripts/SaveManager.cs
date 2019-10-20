using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    public SaveState ThisSaveState;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();

        Debug.Log(SaveHelper.Serialize<SaveState>(ThisSaveState));
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", SaveHelper.Serialize<SaveState>(ThisSaveState));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            ThisSaveState = SaveHelper.DeSerialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            ThisSaveState = new SaveState();
            Save();
            Debug.Log("No save detected, made a new one.");
        }
    }
}
