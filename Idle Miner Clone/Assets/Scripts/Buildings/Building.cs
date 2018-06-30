using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Building : MonoBehaviour
{
    public Button
        m_InfoButton,
        m_ForemanButton;
    public Foreman m_Foreman = null;
    public Text m_DepositText;

    protected int m_Level = 1;
    protected double m_CashDeposit = 0.0;
    protected double m_TransportedCash = 0.0;
    protected int m_MaxLevel;

    public abstract void CreateDetails( List<GameObject> where, GameObject prefab, Transform parent, int upgradeAmount );
    public abstract string GetInfotext();

    private void Start()
    {
        m_DepositText.text = "0";
    }

    public virtual double GetUpgradeCost( int upgradeAmount )
    {
        double baseCost = 100.0;
        double interest = 0.08;
        int period = m_Level + upgradeAmount;
        return CashFormatter.CompoundInterest( baseCost, interest, period );
    }

    public virtual int GetNextBoostLevel()
    {
        int boostLvl = 50;
        int baseAbs = 50;

        while ( m_Level >= boostLvl )
        {
            boostLvl += baseAbs;
        }

        return boostLvl;
    }

    public virtual int GetLastBoostLevel()
    {
        int boostLvl = 50;
        int baseAbs = 50;
        int prevBoostLvl = 0;

        while ( m_Level >= boostLvl )
        {
            prevBoostLvl = boostLvl;
            boostLvl += baseAbs;
        }

        return prevBoostLvl;
    }

    public void AddToDeposit( double amount )
    {
        m_CashDeposit += amount;
        m_DepositText.text = CashFormatter.FormatToString( m_CashDeposit );
    }

    public void RetrieveFromDeposit( double amount )
    {
        m_CashDeposit -= amount;
        m_DepositText.text = CashFormatter.FormatToString( m_CashDeposit );
    }

    public void EmptyDeposit()
    {
        m_CashDeposit = 0.0;
        m_DepositText.text = "0";
    }

    public void LevelUp()
    {
        ++m_Level;
    }

    public void LevelUp( int amount )
    {
        int newLevel = m_Level + amount;
        m_Level = newLevel < m_MaxLevel ? newLevel : m_MaxLevel;
    }

    public bool IsMaxLevel()
    {
        return m_Level >= m_MaxLevel;
    }

    public int Level { get { return m_Level; } }
    public double CashDeposit { get { return m_CashDeposit; } }
    public bool HasForeman { get { return m_Foreman != null; } }
    public int MaxLevel { get { return m_MaxLevel; } }
}