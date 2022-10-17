using System;
using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.UI;

public class SplitOrStealBehaviour : MonoBehaviour
{
    public FloatVariable sharedMoney;

    [SerializeField] private List<SplitOrStealPlayer> splitOrStealPlayers;

    private Dictionary<FloatVariable, SplitStealType> playerDecisions;

    private void Awake()
    {
        foreach (var splitOrStealPlayer in splitOrStealPlayers)
        {
            splitOrStealPlayer.stealButton.onClick.AddListener(delegate { RegisterSteal(splitOrStealPlayer); });
            splitOrStealPlayer.splitButton.onClick.AddListener(delegate { RegisterSplit(splitOrStealPlayer); });
        }
        
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (var splitOrStealPlayer in splitOrStealPlayers)
        {
            splitOrStealPlayer.stealButton.onClick.RemoveListener(delegate { RegisterSteal(splitOrStealPlayer); });
            splitOrStealPlayer.splitButton.onClick.RemoveListener(delegate { RegisterSplit(splitOrStealPlayer); });
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        playerDecisions = new Dictionary<FloatVariable, SplitStealType>();

        foreach (var splitOrStealPlayer in splitOrStealPlayers)
        {
            splitOrStealPlayer.playerPanel.SetActive(true);
        }
    }

    private void RegisterSteal(SplitOrStealPlayer splitOrStealPlayer)
    {
        if (playerDecisions.ContainsKey(splitOrStealPlayer.playerMoney))
        {
            Debug.LogWarning("Already registered a decision for this Player.");
            return;
        }
        
        playerDecisions.Add(splitOrStealPlayer.playerMoney, SplitStealType.Steal);
        splitOrStealPlayer.playerPanel.SetActive(false);
        
        EvaluatePlayerDecision();
    }

    private void RegisterSplit(SplitOrStealPlayer splitOrStealPlayer)
    {
        if (playerDecisions.ContainsKey(splitOrStealPlayer.playerMoney))
        {
            Debug.LogWarning("Already registered a decision for this Player.");
            return;
        }
        
        playerDecisions.Add(splitOrStealPlayer.playerMoney, SplitStealType.Split);
        splitOrStealPlayer.playerPanel.SetActive(false);

        EvaluatePlayerDecision();
    }

    private void EvaluatePlayerDecision()
    {
        if (playerDecisions.Count < splitOrStealPlayers.Count) return;

        FloatVariable stealPlayer = null;

        foreach (var playerDecision in playerDecisions)
        {
            if (playerDecision.Value != SplitStealType.Steal) continue;
            
            if (stealPlayer == null)
            {
                stealPlayer = playerDecision.Key;
            }
            else
            {
                sharedMoney.Set(0);
                return;
            }
        }

        if (stealPlayer == null)
        {
            foreach (var playerDecision in playerDecisions)
            {
                playerDecision.Key.Set(playerDecision.Key.Get() + (sharedMoney.Get() / splitOrStealPlayers.Count));
            }
        }
        else
        {
            stealPlayer.Set(stealPlayer.Get() + sharedMoney.Get());
        }
        
        sharedMoney.Set(0);
        
        gameObject.SetActive(false);
    }
}

[Serializable]
public class SplitOrStealPlayer
{
    public FloatVariable playerMoney;
    public GameObject playerPanel;
    public Button stealButton;
    public Button splitButton;
}

public enum SplitStealType
{
    Split, Steal
}
