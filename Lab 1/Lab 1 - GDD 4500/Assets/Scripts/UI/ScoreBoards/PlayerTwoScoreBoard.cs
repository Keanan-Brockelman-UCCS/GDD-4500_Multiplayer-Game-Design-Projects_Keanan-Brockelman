using UnityEngine;

public class PlayerTwoScoreBoard : ScoreBoardParent
{
    public override string GetPlayerScore()
    {
        int score = (int)GameManager.Instance.PlayerTwoScoreBoardTime;
        return score.ToString();
    }
}
