using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect_Object : MonoBehaviour
{
    public int level_Scene_Index;

    public GameObject unlockedVisualGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlocked(bool state)
    {
        if(state)
        {
            unlockedVisualGameObject.SetActive(true);
        }
        else
        {
            unlockedVisualGameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Game_Manager.instance.level_Select_Manager.LoadScene(level_Scene_Index);
    }
}
