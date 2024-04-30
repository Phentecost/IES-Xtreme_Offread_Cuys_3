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

    public static GameManager instance;

    private int winer;

    private int index = 0; // 0 = Start  1 = Medium  2 = Ending 

    private Cuy[] finalist = new Cuy[3];

    private void Awake()
    {
        instance = this;
    }

    public void On_Cuy_Reach_Goal(Cuy cuy)
    {
        if (finalist[0] == null) 
        {
            finalist[0] = cuy;
            Debug.Log(cuy.name + " Gano La carrera");
        }
        else if (finalist[1] == null) 
        {
            finalist[1] = cuy;
            Debug.Log(cuy.name + " Quedo de segundo");
        }
        else 
        {
            finalist[2] = cuy;
            Debug.Log(cuy.name + " Quedo de ultimo");
        }
    }

    void Start()
    {
        //winer = Generate_Random_Winer();
        winer = 2;
        Debug.Log(winer);
        Change_Cuys_Speed();
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
            Cuy winerCuy = cuys.FirstOrDefault(x => x.CUY_ID == winer);
            if (winerCuy == cuys[0]) return;

            Cuy cuyToBeat = cuys[0];
            Cuy cuyToBeat2 = cuys[1] == winerCuy? cuys[2] : cuys[1];

            while (true) 
            {
                
                cuyToBeat.Speed -= speedThreshold / 2;
                cuyToBeat2.Speed -= speedThreshold/2;
                winerCuy.Speed += speedThreshold;

                float finishTimeCuy = (1 - cuyToBeat.DistancePorcentage) / cuyToBeat.Speed;
                float finishTimeCuy2 = (1 - cuyToBeat2.DistancePorcentage) / cuyToBeat2.Speed;
                float finishTimeWinnerCuy = (1 - winerCuy.DistancePorcentage) / winerCuy.Speed;

                Debug.Log(finishTimeCuy);

                if (finishTimeCuy > finishTimeWinnerCuy+0.05 && finishTimeCuy2 > finishTimeWinnerCuy+0.05)
                {
                    break;
                }
            }
        }
        else 
        {
            foreach(Cuy cuyOBJ in cuys) 
            {
                cuyOBJ.Speed = UnityEngine.Random.Range(mediaSpeed - speedThreshold, mediaSpeed + speedThreshold);
            }

            index++;
        }
    }

    private void Update()
    {
        cuys = cuys.OrderByDescending(x => x.DistancePorcentage).ToList();

        if (cuys[0].DistancePorcentage > 0.2 && index == 1)
        {
            Change_Cuys_Speed();
        }
        else if(cuys[0].DistancePorcentage > 0.6 && index == 2) 
        {
            Change_Cuys_Speed();
        }
    }


}
