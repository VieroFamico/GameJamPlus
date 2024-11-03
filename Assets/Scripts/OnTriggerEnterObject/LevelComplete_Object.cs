using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete_Object : MonoBehaviour
{
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player_Entity>())
        {
            UnlockNewLevel();

            if(Game_Manager.instance.CurrentLevel() < Game_Manager.instance.maxLevel)
            {
                Game_Manager.instance.level_Select_Manager.LoadScene(Game_Manager.instance.CurrentLevel() + 1);
                return;
            }
            else
            {
                Game_Manager.instance.level_Select_Manager.LoadScene(0);
            }
        }
    }

    public void UnlockNewLevel()
    {
        Game_Manager.instance.SaveNewUnlockedLevel(level + 1);
    }
}
