using Final_Survivors.Enemies;
using Final_Survivors.Observer;
using UnityEngine;

namespace Final_Survivors.Core
{
    public class ScoreManager : Subject
    {
        [SerializeField] private int normalEnemyPoints = 5;
        [SerializeField] private int eliteEnemyPoints = 25;
        [SerializeField] private int bossEnemyPoints = 75;

        public int score = 0;
        public int normalCounter = 0;
        public int eliteCounter = 0;
        public int bossCounter = 0;

        public int GetScore()
        {
            return score;
        }

        public void AddScore(EnemyLevel type)
        {
            if (type == EnemyLevel.NORMAL)
            {
                ++normalCounter;
                score += normalEnemyPoints;
            }
            else if (type == EnemyLevel.ELITE)
            {
                ++eliteCounter;
                score += eliteEnemyPoints;
            }
            else if (type == EnemyLevel.BOSS)
            {
                ++bossCounter;
                score += bossEnemyPoints;
            }

            NotifyObservers(Events.SCORE);
        }
    }
}
