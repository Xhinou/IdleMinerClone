using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : Building
{
    public GameManager gameManager;

    public Text m_LvlText;
    public Text m_DepositText;
    public RectTransform m_ElevatorArea;

    private float m_TransportedGold;
    private bool m_IsBottom = false;

    private void Update()
    {
        
    }

    private void LoadGoldFromShaft( MineShaft from )
    {
        m_TransportedGold += from.GoldDeposit;
        from.EmptyDeposit();
    }

    public float GetTotalTransportation( int lvl )
    {
        return 0.0f;
    }

    public float GetMaxLoad( int lvl )
    {
        return 0.0f;
    }

    public float GetMovementSpeed( int lvl )
    {
        return 0.0f;
    }

    public float GetLoadingSpeed( int lvl )
    {
        return 0.0f;
    }

    override public float GetUpgradeCost( int lvl )
    {
        return 0.0f;
    }

    public float TransportedGold { get { return m_TransportedGold; } }
    public bool IsBottom { get { return m_IsBottom; } }
}
