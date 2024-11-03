using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIndex_Assigner : MonoBehaviour
{
    public List<LevelSelect_Object> levelSelect_Objects;
    // Start is called before the first frame update
    void Start()
    {
        int i = 1;
        foreach (var levelSelect in levelSelect_Objects)
        {
            levelSelect.level_Scene_Index = i;

            if(i <= Game_Manager.instance.UnlockedLevel())
            {
                levelSelect.Unlocked(true);
            }
            else
            {
                levelSelect.Unlocked(false);
            }
            i += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}