using MagmaLabs.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
namespace MagmaLabs.UI{
    
    public class SceneManagerEnhanced : MonoBehaviour
    {
        public static SceneManagerEnhanced instance;
        public IEnumerator LoadSceneWithLoadingScreenCoroutine(string scene, Canvas fromCanvas, Canvas loadCanvas, Canvas toCanvas, float duration)
        {
            CanvasAnimation.LoadingScreen(fromCanvas, loadCanvas, toCanvas, duration);
            yield return new WaitForSeconds(duration/2);
            SceneManager.LoadScene(scene);
            yield return new WaitForSeconds(duration/2);

        }

        public void LoadScreenWithLoadingScreen(string scene, Canvas fromCanvas, Canvas loadCanvas, Canvas toCanvas, float duration)
        {
            StartCoroutine(LoadSceneWithLoadingScreenCoroutine(scene, fromCanvas, loadCanvas, toCanvas, duration));
        }
    }
}
