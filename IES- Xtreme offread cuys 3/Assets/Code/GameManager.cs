using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Cuy> cuys;
    [SerializeField] float mediaSpeed;
    [SerializeField] float speedThreshold;

    public static GameManager instance { get; private set; } = null;

    private int winner;

    private int index = 0; // 0 = Start  1 = Medium  2 = Ending 

    private Cuy[] finalist = new Cuy[3];

    private void Awake()
    {
        if(instance != null) 
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void On_Cuy_Reach_Goal(Cuy cuy)
    {
        if (finalist[0] == null) 
        {
            finalist[0] = cuy;
            UIManager.Instance.On_Final_Leaderboard(cuy, 0);
        }
        else if (finalist[1] == null) 
        {
            finalist[1] = cuy;
            UIManager.Instance.On_Final_Leaderboard(cuy, 1);
        }
        else 
        {
            finalist[2] = cuy;
            UIManager.Instance.On_Final_Leaderboard(cuy, 2);
            UIManager.Instance.On_Ending_Race();
        }
    }

    private int Generate_Random_Winer() 
    {
        System.Random rand = new System.Random(DateTime.Now.Millisecond);

        return rand.Next(0, 3); // 0 = Red   1 = Purple  2 = Brown
    }

    private void Change_Cuys_Speed() 
    {

        if(index == 2) 
        {
            index++;
            Cuy winnerCuy = cuys.FirstOrDefault(x => x.CUY_ID == winner);
            if (winnerCuy == cuys[0]) { winnerCuy.TargetSpeed = winnerCuy.Speed + 0.3f; return; }

            Cuy cuyToBeat = cuys[0];
            Cuy cuyToBeat2 = cuys[1] == winnerCuy ? cuys[2] : cuys[1];

            float cuyToBeatAcumulatedSpeed = 0;
            float cuyToBeat2AcumulatedSpeed = 0;
            float winnerCuyAcumulatedSpeed = 0;

            while (true) 
            {
                cuyToBeatAcumulatedSpeed -= (speedThreshold / 2);
                cuyToBeat2AcumulatedSpeed -= (speedThreshold / 2);
                winnerCuyAcumulatedSpeed += speedThreshold;

                float finishTimeCuy = (1 - cuyToBeat.DistancePorcentage) / (cuyToBeat.Speed + cuyToBeatAcumulatedSpeed);
                float finishTimeCuy2 = (1 - cuyToBeat2.DistancePorcentage) / (cuyToBeat2.Speed + cuyToBeat2AcumulatedSpeed);
                float finishTimeWinnerCuy = (1 - winnerCuy.DistancePorcentage) / (winnerCuy.Speed + winnerCuyAcumulatedSpeed);

                if (finishTimeCuy > finishTimeWinnerCuy + 0.06 && finishTimeCuy2 > finishTimeWinnerCuy + 0.06)
                {
                    cuyToBeat.TargetSpeed = cuyToBeatAcumulatedSpeed + cuyToBeat.Speed;
                    cuyToBeat2.TargetSpeed = cuyToBeat2AcumulatedSpeed + cuyToBeat2.Speed;
                    winnerCuy.TargetSpeed = winnerCuyAcumulatedSpeed + winnerCuy.Speed;
                    break;
                }
            }
        }
        else 
        {
            foreach (Cuy cuyOBJ in cuys)
            {
                cuyOBJ.TargetSpeed = UnityEngine.Random.Range(mediaSpeed - speedThreshold, mediaSpeed + speedThreshold);
            }

            index++;
        } 
    }

    private void Update()
    {
        if (cuys[2].DistancePorcentage > cuys[1].DistancePorcentage || cuys[1].DistancePorcentage > cuys[0].DistancePorcentage)
        {
            cuys = cuys.OrderByDescending(x => x.DistancePorcentage).ToList();
            UIManager.Instance.Organize_LeaderBoard(cuys);
        }

        if (cuys[0].DistancePorcentage > 0.2 && index == 1)
        {
            Change_Cuys_Speed();
        }
        else if(cuys[0].DistancePorcentage > 0.6 && index == 2) 
        {
            Change_Cuys_Speed();
            
        }
    }

    public void Restart_Race() 
    {
        for (int i = 0; i < cuys.Count; i++) 
        {
            cuys[i].Reset_Position();
            finalist[i] = null;
        }

        index = 0;

        UIManager.Instance.Reset_LiderBoard();

        Start_Race();
    }

    public void Start_Race() 
    {
        winner = Generate_Random_Winer();
        cuys = cuys.OrderByDescending(x => x.DistancePorcentage).ToList();
        UIManager.Instance.Organize_LeaderBoard(cuys);

#if UNITY_EDITOR

        if (winner == 0)
        {
            Debug.Log("El cuy rojo va a ganar");
        }
        else if (winner == 1)
        {
            Debug.Log("El cuy morado va a ganar");
        }
        else if (winner == 2)
        {
            Debug.Log("El cuy cafe va a ganar");
        }
#endif

        Change_Cuys_Speed();
    }

}
