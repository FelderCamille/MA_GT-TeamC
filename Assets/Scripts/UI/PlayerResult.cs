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
        
        public void Init(ulong clientId, bool isCurrent, ResourcesManager resourcesManager, ResourcesManager otherResourcesManager)
        {
            // Set name (client id starts from 0)
            playerTitle.text += " " + (clientId + 1) + " " + (isCurrent ? " (vous)" : "");
            // Compute player's result
            clearedMines.text += resourcesManager.ClearedMines + " (score : " + resourcesManager.ClearedMinesScore + ")";
            explodedMines.text += resourcesManager.ExplodedMines + " (score : " + resourcesManager.ExplodedMinesScore + ")";
            // Set total score
            totalScore.text += resourcesManager.TotalScore;
            // Set result
            if (otherResourcesManager != null)
            {
                result.text = resourcesManager.TotalScore > otherResourcesManager.TotalScore ? "Gagnant !" : "Perdant.";
            }
            else
            {
                result.gameObject.SetActive(false);
            }
        }
    }
}