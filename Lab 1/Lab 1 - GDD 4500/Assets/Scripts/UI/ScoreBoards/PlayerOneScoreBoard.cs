using UnityEngine;

public class PlayerOneScoreBoard : ScoreBoardParent
{
    public override string GetPlayerScore()
    {
        int score = (int)GameManager.Instance.PlayerOneScoreBoardTime;

        return score.ToString();
    }
}
