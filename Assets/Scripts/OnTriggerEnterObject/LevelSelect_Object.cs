using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect_Object : MonoBehaviour
{
    public int level_Scene_Index;

    public GameObject unlockedVisualGameObject;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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
        if(timer < 3f)
        {
            return;
        }

        Game_Manager.instance.loading_Scene_Manager.LoadScene(level_Scene_Index);
        timer = 0f;
    }
}
