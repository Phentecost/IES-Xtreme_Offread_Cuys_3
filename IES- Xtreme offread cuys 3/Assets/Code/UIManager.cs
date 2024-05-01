using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    [SerializeField] private TextMeshProUGUI[] leaderBoardTXTs;
    [SerializeField] private TextMeshProUGUI winnerPanelTXT;
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private GameObject mainMenuPanel;

    bool ending=false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Organize_LeaderBoard(List<Cuy> cuys)
    {
        if (ending) return;
        for (int i = 0; i < cuys.Count; i++) 
        {
            leaderBoardTXTs[i].text = cuys[i].name;
        }
    }

    public void Reset_LiderBoard() 
    {
        foreach(TextMeshProUGUI txt in leaderBoardTXTs) 
        {
            txt.color = Color.white;
        }

        ending = false;
    }

    public void On_Final_Leaderboard(Cuy finalistCuy, int i) 
    {
        ending=true;
        leaderBoardTXTs[i].color = Color.green;
        leaderBoardTXTs[i].text = finalistCuy.name;
    }

    public void On_Ending_Race() 
    {
        winnerPanelTXT.text = leaderBoardTXTs[0].text;
        Winner_Panel_Activation();
    }

    public void Main_Menu_Panel_Activation() 
    {
        if (mainMenuPanel.activeInHierarchy) 
        {
            mainMenuPanel.SetActive(false);
        }
        else 
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void Winner_Panel_Activation() 
    {
        if(winnerPanel.activeInHierarchy) 
        {
            winnerPanel.SetActive(false);
        }
        else 
        {
            winnerPanel.SetActive(true);
        }
    }

    public void Exit_Game() 
    {
        ApplicationManager.Exit();
    }
}
