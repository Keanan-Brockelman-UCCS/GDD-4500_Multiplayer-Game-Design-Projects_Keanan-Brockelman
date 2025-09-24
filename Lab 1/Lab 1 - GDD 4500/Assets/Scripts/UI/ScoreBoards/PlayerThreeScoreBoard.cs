using UnityEngine;

public class PlayerThreeScoreBoard : ScoreBoardParent
{
    public override string GetPlayerScore()
    {
        int score = (int)GameManager.Instance.PlayerThreeScoreBoardTime;
        return score.ToString();
    }
}
