using System.Collections.Generic;

public static class ScoreCalculator
{
    

    public static void DistributeScore(int score, List<Ball> balls)
    {
        if (balls.Count == 0) return;

        int scoreForBall = score / balls.Count;
        int scoreForLastBall = scoreForBall + score % balls.Count;


        for (int i = 0; i < balls.Count; i++)
        {
            if (i != balls.Count - 1) balls[i].SetScoreForBall(scoreForBall);
            else balls[i].SetScoreForBall(scoreForLastBall);
        }

    }



}
