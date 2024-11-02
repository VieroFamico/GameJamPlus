using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity_Manager : MonoBehaviour
{
    public static Entity_Manager instance;

    public Player_Entity player;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangedActiveScene(Scene scene, Scene scene1)
    {
        
    }

}
