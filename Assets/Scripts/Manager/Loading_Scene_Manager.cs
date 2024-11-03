using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading_Scene_Manager : MonoBehaviour
{
    public static Loading_Scene_Manager Instance;

    public Animator animator;
    public AudioClip loadInAudioClip;
    public AudioClip loadOutAudioClip;

    public int targetSceneIndex; // Scene to load when triggered

    public int maxLevel = 3;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        maxLevel = Game_Manager.instance.maxLevel;
    }

    public void LoadScene(int targetScene)
    {
        if (targetScene > maxLevel)
        {
            Debug.Log("Trying to load an out of index scene");
            return;
        }

        Game_Manager.instance.ChangeCurrLevel(targetScene);

        StartCoroutine(LoadSceneCoroutine(targetScene));
    }

    private IEnumerator LoadSceneCoroutine(int targetScene)
    {
        // Trigger the LoadIn animation
        LoadIn();

        // Wait for the animation to play out (adjust based on animation duration)
        yield return new WaitForSeconds(1.3f); // Adjust if needed


        LoadOut();
        yield return null;
        Time.timeScale = 0f;
        // Load the target scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Wait one frame after the scene loads
        Time.timeScale = 1f;

        //LoadOut();
        // Trigger the LoadOut animation after the scene loads
        //LoadOut();
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameCoroutine());
    }

    private IEnumerator ExitGameCoroutine()
    {
        // Trigger the LoadIn animation
        LoadIn();

        // Wait for the animation to play out (adjust based on animation duration)
        yield return new WaitForSeconds(1.3f); // Adjust if needed

        Application.Quit();
    }

    public void LoadIn()
    {
        if (animator != null)
        {
            animator.SetTrigger("LoadIn");
            Audio_Manager.instance.PlaySFXOneShot(loadInAudioClip);
        }
    }

    public void LoadOut()
    {
        if (animator != null)
        {
            animator.SetTrigger("LoadOut");
            Audio_Manager.instance.PlaySFXOneShot(loadOutAudioClip);
        }
    }
}
