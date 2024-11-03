using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Select_Manager : MonoBehaviour
{
    public int maxLevel = 3;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        maxLevel = Game_Manager.instance.maxLevel;
    }

    public void LoadScene(int targetScene)
    {
        if (targetScene >= maxLevel)
        {
            Debug.Log("Trying to load an out of index scene");
            return;
        }

        Game_Manager.instance.ChangeCurrLevel(targetScene);
    }
}
