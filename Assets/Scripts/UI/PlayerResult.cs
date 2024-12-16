using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerResult : MonoBehaviour
    {
        [SerializeField] private Text playerTitle;
        [SerializeField] private Text clearedMines;
        [SerializeField] private Text explodedMines;
        [SerializeField] private Text totalScore;
        [SerializeField] private Text result;
        
        public void Init(ulong clientId, ResourcesManager resourcesManager, ResourcesManager otherResourcesManager)
        {
            // Set name (client id starts from 0)
            playerTitle.text += (clientId + 1) + ". " + (clientId == 0 ? " (hébergeur)" : "");
            // Compute player's result
            clearedMines.text += resourcesManager.ClearedMines + ". Score : " + resourcesManager.ClearedMinesScore;
            explodedMines.text += resourcesManager.ExplodedMines + ". Score : " + resourcesManager.ExplodedMinesScore;
            // Set total score
            totalScore.text += resourcesManager.TotalScore;
            // Set result
            result.text = resourcesManager.TotalScore > otherResourcesManager.TotalScore ? "Gagnant !" : "Perdu...";
        }
    }
}