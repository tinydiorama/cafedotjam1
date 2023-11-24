using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    private int mostRecentScene;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject oldEventSystem;
    [SerializeField] RectTransform fader;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public static UnityAction<int> OnNewLevelLoaded;

    private void Awake()
    {
        instance = this;
        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
        mostRecentScene = (int)SceneIndexes.TITLE_SCREEN;
        oldEventSystem.SetActive(false);
    }

    private void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }

    public void LoadLevel(int level)
    {
        fader.gameObject.SetActive(true);
        if ( mostRecentScene == (int)SceneIndexes.TITLE_SCREEN )
        {
            AudioManager.GetInstance().FadeOutMusic();
        }
        LeanTween.alpha(fader, 1, 1f).setOnComplete(() =>
        {
            loadingScreen.SetActive(true);
            fader.gameObject.SetActive(false);
            scenesLoading.Add(SceneManager.UnloadSceneAsync(mostRecentScene));
            scenesLoading.Add(SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive));
            mostRecentScene = level;

            StartCoroutine(GetSceneLoadProgress());
        });
    }

    public void LoadLevel(int level, Vector2 playerCoords)
    {
        fader.gameObject.SetActive(true);
        if (mostRecentScene == (int)SceneIndexes.TITLE_SCREEN)
        {
            AudioManager.GetInstance().FadeOutMusic();
        }
        LeanTween.alpha(fader, 1, 1f).setOnComplete(() =>
        {
            loadingScreen.SetActive(true);
            fader.gameObject.SetActive(false);
            scenesLoading.Add(SceneManager.UnloadSceneAsync(mostRecentScene));
            scenesLoading.Add(SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive));
            mostRecentScene = level;

            StartCoroutine(GetSceneLoadProgress(playerCoords));
        });
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }
        fader.gameObject.SetActive(true);
        loadingScreen.SetActive(false);
        OnNewLevelLoaded?.Invoke(mostRecentScene);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
            if (mostRecentScene == (int)SceneIndexes.MAIN_WORLD)
            {
                AudioManager.GetInstance().StartPlaylist();
            }
        });
    }
    public IEnumerator GetSceneLoadProgress(Vector2 playerCoords)
    {

        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if ( playerObj != null )
        {
            playerObj.transform.position = playerCoords;
            Cinemachine.CinemachineVirtualCamera vcam = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
            vcam.ForceCameraPosition(new Vector3(playerCoords.x, playerCoords.y, 0), vcam.transform.rotation);
        }
        fader.gameObject.SetActive(true);
        loadingScreen.SetActive(false);
        OnNewLevelLoaded?.Invoke(mostRecentScene);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
            if (mostRecentScene == (int)SceneIndexes.MAIN_WORLD)
            {
                AudioManager.GetInstance().StartPlaylist();
            }
        });
    }
}
