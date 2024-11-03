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
    }

    // Start is called before the first frame update
    void Start()
    {
        unlockedLevel = PlayerPrefs.GetInt(levelUnlockedParameter, 1);

        currentLevel = 0; // 0 = Main Menu Stage

        Debug.Log(unlockedLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeCurrLevel(int level)
    {
        currentLevel = level;
    }

    private void OnApplicationQuit()
    {
        
    }

    public void SaveNewUnlockedLevel(int newLevel)
    {
        if(unlockedLevel <= newLevel)
        {
            return;
        }

        PlayerPrefs.SetInt(levelUnlockedParameter, newLevel);
    }

    #region Unlocked Level

    public int UnlockedLevel()
    {
        return unlockedLevel;
    }

    #endregion
}
