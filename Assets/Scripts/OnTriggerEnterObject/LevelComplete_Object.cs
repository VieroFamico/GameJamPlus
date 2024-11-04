using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete_Object : MonoBehaviour
{
    public int level;
    public AudioClip nextLevelAudioClip;
    public GameObject nextLevelVFX;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player_Entity>())
        {
            if (timer < 3f)
            {
                return;
            }

            timer = 0;

            UnlockNewLevel();

            Instantiate(nextLevelVFX, other.transform.position, Quaternion.identity);
            Audio_Manager.instance.PlaySFXOneShot(nextLevelAudioClip);

            if(Game_Manager.instance.CurrentLevel() < Game_Manager.instance.maxLevel)
            {
                Game_Manager.instance.loading_Scene_Manager.LoadScene(Game_Manager.instance.CurrentLevel() + 1);
                return;
            }
            else
            {
                Game_Manager.instance.loading_Scene_Manager.LoadScene(0);
            }
        }
    }

    public void UnlockNewLevel()
    {
        Game_Manager.instance.SaveNewUnlockedLevel(level + 1);
    }
}
