using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Select_Manager : MonoBehaviour
{
    public int targetSceneIndex; // Scene to load when triggered

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene(int targetScene)
    {
        StartCoroutine(LoadSceneCoroutine(targetScene));
    }

    private IEnumerator LoadSceneCoroutine(int targetScene)
    {
        // Trigger the LoadIn animation
        Loading_Scene_Manager.Instance.LoadIn();

        // Wait for the animation to play out (adjust based on animation duration)
        yield return new WaitForSeconds(1f); // Adjust if needed

        // Load the target scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Trigger the LoadOut animation after the scene loads
        Loading_Scene_Manager.Instance.LoadOut();
    }

}
