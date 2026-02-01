using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MagmaLabs.Economy;
using UnityEngine.SceneManagement;

namespace MagmaLabs.UI{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]private ProgressionNode levelData;

        public void OnClick()
        {
            SceneManager.LoadScene(levelData.name);
        }
    }
}
