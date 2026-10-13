using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class VideoSceneController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Tiempos")]
    [SerializeField] private float delayBeforeVideo = 1.5f;
    [SerializeField] private float delayAfterVideo = 1f;

    [Header("Escena")]
    [SerializeField] private int nextSceneIndex;

    private void Awake()
    {
        if (transitionManager == null)
            transitionManager = FindAnyObjectByType<TransitionManager>();
    }

    private void Start()
    {
        StartCoroutine(SceneSequence());
    }

    private IEnumerator SceneSequence()
    {
        transitionManager.transitionAnimator.Rebind();
        transitionManager.transitionAnimator.Update(0f);

        yield return new WaitForSecondsRealtime(delayBeforeVideo);

        videoPlayer.Play();

        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        yield return new WaitForSecondsRealtime(delayAfterVideo);

        transitionManager.PlayFadeOut();

        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene(nextSceneIndex);
    }
}

