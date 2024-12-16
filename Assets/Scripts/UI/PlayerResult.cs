using Objects;
using Unity.Netcode;
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
        
        public void Init(PlayerResultData currentPlayer, PlayerResultData otherPlayerResult)
        {
            // Set name (client id starts from 0)
            var isMe = NetworkManager.Singleton.LocalClientId == currentPlayer.clientId;
            playerTitle.text += " " + (currentPlayer.clientId + 1) + " " + (isMe ? " (vous)" : "");
            // Compute player's result
            clearedMines.text += currentPlayer.clearedMines + " (" + currentPlayer.ClearedMinesScore + " pts)";
            explodedMines.text += currentPlayer.explodedMines + " (" + currentPlayer.ExplodedMinesScore + " pts)";
            // Set total score
            totalScore.text += currentPlayer.TotalScore + " pts";
            // Set result
            var resultText = "Égalité";
            var color = Color.white;
            Debug.Log("Player " + isMe + " current=" + currentPlayer.TotalScore + " other=" + otherPlayerResult.TotalScore);
            if (currentPlayer.TotalScore > otherPlayerResult.TotalScore)
            {
                resultText = "Gagnant !";
                color = Color.green;
            }
            else if (currentPlayer.TotalScore < otherPlayerResult.TotalScore)
            {
                resultText = "Perdant.";
                color = Color.red;
            }
            result.text = resultText;
            result.color = color;
        }
    }
}