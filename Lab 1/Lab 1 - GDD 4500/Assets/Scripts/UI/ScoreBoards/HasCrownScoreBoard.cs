using UnityEngine;
using UnityEngine.InputSystem;

public class HasCrownScoreBoard : ScoreBoardParent
{
    public string tank;
    public string color;

    public override void UpdateScoreBoard()
    {
        playerScore = GetPlayerScore();

        scoreText.text = templateText.Replace("{holder}", playerScore);
    }

    public override string GetPlayerScore()
    {
        PlayerInput holder = GameManager.Instance.CrownedPlayer;
        string score = "";

        if (holder != null)
        {
            switch (holder.playerIndex)
            {
                case 0: 
                    tank = "Red";
                    color = "#FF0000";
                    break;
                case 1:
                    tank = "Blue";
                    color = "#0000FF";
                    break;
                case 2:
                    tank = "Green";
                    color = "#00FF00";
                    break;
                case 3:
                    tank = "Blue";
                    color = "#FFFF00";
                    break;
                default: 
                    tank = ""; 
                    color = "";
                    break;
            }
        }

        if (tank != "" && color != "")
        {
            score = $"<color={color}>{tank}</color>";
        }

        return score;
    }
}
