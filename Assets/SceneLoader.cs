using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] GameObject loaderPanel;
    [SerializeField] Slider loadingSlider; 
    [SerializeField] public bool sceneloaded;
    [SerializeField] float loadingTime = 5f; 
    private float elapsedTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //StartCoroutine(DisableIfSceneLoaded());
    }

    public async void LoadScene()
    {
        var scene = SceneManager.LoadSceneAsync(1);
        loaderPanel.SetActive(true);

        /*loadingSlider.value = 0f;

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            loadingSlider.value = Mathf.Clamp01(elapsedTime / loadingTime);
            await Task.Yield(); // Yield to let the game loop continue
        }

        // Once the slider reaches the end, allow scene activation
        sceneloaded = true;
        scene.allowSceneActivation = true;*/
    }

   /* public IEnumerator DisableIfSceneLoaded()
    {
        while (!sceneloaded)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        loaderPanel.SetActive(false);
    }*/
}
