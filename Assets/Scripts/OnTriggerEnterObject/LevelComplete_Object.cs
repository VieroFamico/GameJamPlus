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
        UnlockNewLevel();

    }
    public void UnlockNewLevel()
    {
        Game_Manager.instance.SaveNewUnlockedLevel(level + 1);
    }
}
