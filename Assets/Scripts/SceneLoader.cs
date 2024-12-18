using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void ShowScene(string scene)
    {
        var sceneInHistory = SceneManager.GetSceneByName(scene);
        if (sceneInHistory.isLoaded)
        {
            SceneManager.SetActiveScene(sceneInHistory);
        }
        else
        {
            if (scene == Constants.Scenes.Result)
            {
                if (!NetworkManager.Singleton.IsHost) return;
                NetworkManager.Singleton.SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }
            else
            {
                StartCoroutine(LoadSceneWithTransition(scene));
            }
        }
    }
    
    private IEnumerator LoadSceneWithTransition(string scene)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(scene);
    }
    
}