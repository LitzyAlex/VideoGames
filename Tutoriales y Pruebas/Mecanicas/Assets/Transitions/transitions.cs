using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public Animator transitionAnimator;



    public void LoadNextScene(int sceneIndex, bool Activar)
    {
        
        StartCoroutine(SceneLoad(sceneIndex,Activar));
    }

    IEnumerator SceneLoad(int sceneIndex, bool Activar)
    {
        Time.timeScale = 1;
        transitionAnimator.SetBool("Button", Activar);
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(sceneIndex);
    }
}
