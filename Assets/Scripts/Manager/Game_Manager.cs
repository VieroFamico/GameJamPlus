using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    public Level_Select_Manager level_Select_Manager;
    public Loading_Scene_Manager loading_Scene_Manager;
    public Setting_Manager setting_Manager;
    public Audio_Manager audio_Manager;

    public string levelUnlockedParameter = "LevelUnlocked";

    public int maxLevel = 3;

    private int currentLevel;
    private int unlockedLevel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        unlockedLevel = PlayerPrefs.GetInt(levelUnlockedParameter, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0; // 0 = Main Menu Stage

        Cursor.visible = false;

        Debug.Log(unlockedLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int CurrentLevel()
    {
        return currentLevel;
    }

    public void ChangeCurrLevel(int level)
    {
        currentLevel = level;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void SaveNewUnlockedLevel(int newLevel)
    {
        if(newLevel <= unlockedLevel)
        {
            return;
        }

        PlayerPrefs.SetInt(levelUnlockedParameter, newLevel);
        PlayerPrefs.Save();

        unlockedLevel = PlayerPrefs.GetInt(levelUnlockedParameter, newLevel);
    }

    #region Unlocked Level

    public int UnlockedLevel()
    {
        Debug.Log(unlockedLevel);
        return unlockedLevel;
    }

    #endregion
}
