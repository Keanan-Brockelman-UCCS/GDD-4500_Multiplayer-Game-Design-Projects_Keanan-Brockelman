using UnityEngine;

public class PlayerFourScoreBoard : ScoreBoardParent
{
    public override string GetPlayerScore()
    {
        int score = (int)GameManager.Instance.PlayerFourScoreBoardTime;
        return score.ToString();
    }
}
