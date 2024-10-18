using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoader : MonoBehaviour
    {
    
        public void ShowScene(string scene)
        {
            Scene sceneInHistory = SceneManager.GetSceneByName(scene);
            if (sceneInHistory.isLoaded)
            {
                SceneManager.SetActiveScene(sceneInHistory);
                print("Scene count 1: " + SceneManager.sceneCount);
            }
            else
            {
                StartCoroutine(LoadSceneWithTransition(scene));
            }
        }
        
        private IEnumerator LoadSceneWithTransition(string scene)
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(scene);
            print("Scene count 2: " + SceneManager.sceneCount);
        }
        
    }
}