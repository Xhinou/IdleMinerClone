using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineShaft : Building
{
    public Button m_MinerButton;
    public Text m_IdText;

    private int m_MineId;

    #region States
    public void StartMining()
    {
        StartCoroutine( "Mine" );
        m_MinerButton.interactable = !m_MinerButton.interactable;
    }

    private IEnumerator Mine()
    {
        double miningSpeed = GetMiningSpeed();
        double duration = GetMinerCapacity() / miningSpeed;

        while ( duration > 0.0 )
        {
            m_TransportedGold += miningSpeed * Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }
        
        yield return StartCoroutine( "Deposit" );
    }

    private IEnumerator Deposit()
    {
        double duration = GetWalkingTime( m_Level );

        while ( duration > 0.0 )
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        m_GoldDeposit += m_TransportedGold;
        m_TransportedGold = 0.0;
        m_DepositText.text = CashFormatter.FormatToString( m_GoldDeposit );
        m_MinerButton.interactable = !m_MinerButton.interactable;
        yield return null;
    }
    #endregion States

    #region Details
    public override void CreateDetails( List<GameObject> where, GameObject prefab, Transform parent, int upgradeAmount )
    {
        GameObject newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Total Extraction", GetTotalExtraction(), GetTotalExtraction(  upgradeAmount ) - GetTotalExtraction(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Miners", GetNrMiners(), GetNrMiners( upgradeAmount ) - GetNrMiners() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Walking Speed", GetWalkingSpeed(), GetWalkingSpeed(  upgradeAmount ) - GetWalkingSpeed() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Mining Speed", GetMiningSpeed(), GetMiningSpeed(  upgradeAmount ) - GetMiningSpeed(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Worker Capacity", GetMinerCapacity(), GetMinerCapacity(  upgradeAmount ) - GetMinerCapacity() );
        where.Add( newDetail );
    }

    public override string GetInfotext()
    {
        return "Mine Shaft " + m_MineId + " Level " + m_Level;
    }

    public double GetTotalExtraction( int upgradeAmount = 0 )
    {
        int nrMiners = GetNrMiners( upgradeAmount );
        double walkTime = GetWalkingTime( upgradeAmount );
        double mineSpeed = GetMiningSpeed( upgradeAmount );
        double capacity = GetMinerCapacity( upgradeAmount );

        double formula = nrMiners * ( capacity / ( walkTime +  capacity / mineSpeed ) );
        return formula;
    }

    public int GetNrMiners( int upgradeAmount = 0 )
    {
        int predictNr = 1;
        int lvlLimit = 10;
        int limitMultiplier = 2;

        while ( m_Level + upgradeAmount >= lvlLimit )
        {
            ++predictNr;
            if (lvlLimit <= 10 )
                lvlLimit = 50;
            else
                lvlLimit *= limitMultiplier;
        }
        return predictNr;
    }

    public double GetWalkingSpeed( int upgradeAmount = 0 )
    {
        double predictSpeed = 2.0;
        int lvlLimit = 82;

        if ( m_Level + upgradeAmount < lvlLimit )
        {
            return predictSpeed;
        }

        int powBase = 3;
        int multiplier = 9;

        for ( lvlLimit = 82; m_Level + upgradeAmount >= lvlLimit; lvlLimit = (int)System.Math.Pow( powBase, predictSpeed ) * multiplier )
        {
            if ( m_Level + upgradeAmount < lvlLimit )
            {
                return predictSpeed;
            }
            ++predictSpeed;
        }

        return predictSpeed;
    }

    public double GetWalkingTime( int upgradeAmount = 0 )
    {
        double timeBase = 4.0;
        return timeBase / GetWalkingSpeed( upgradeAmount );
    }

    public double GetMiningSpeed( int upgradeAmount = 0 )
    {
        int exp = 2;
        double multiplier = 50.0;

        return System.Math.Pow( m_Level + upgradeAmount, exp ) * System.Math.Pow( multiplier, m_MineId );
    }

    public double GetMinerCapacity( int upgradeAmount = 0 )
    {
        double multiplier = 4.0;
        return GetMiningSpeed( upgradeAmount ) * multiplier;
    }

    public double GetNextBuyCost( double baseCost = 500.0 )
    {
        int idspec = 2;
        if ( m_MineId <= 0 )
            return 10.0;
        else if ( m_MineId <= 1 )
            return 1000.0;
        else
            return 3000.0 * System.Math.Pow( 20.0, m_MineId - idspec );
    }

    public double GetBuyCost()
    {
        switch ( m_MineId - 1 )
        {
            case 0:
                return 10.0;
            case 1:
                return 1000.0;
            default:
                return 3000.0 * System.Math.Pow( 20.0, m_MineId - 2 );
        }
    }

    public double CalculateBuyCost( double arg, float id )
    {
        if ( id >= 0 )
        {
            arg = CalculateBuyCost( arg, id - 1 ) * System.Math.Pow( 20.0, 1 / ( 1 + id / 10 ) );
        }
        return arg;
    }

    public override double GetUpgradeCost( int amount )
    {
        int period = m_Level + amount - 1;
        double baseInterest = 0.16;
        double baseInterestDecrease = -0.00086;
        double interest = CashFormatter.CompoundInterest( baseInterest, baseInterestDecrease, period );

        return CashFormatter.CompoundInterest( GetBuyCost(), interest, period );
    }

    public override int GetNextBoostLevel()
    {
        int boostLvl = 10;
        int baseAbs = 15;
        int maxAbs = 100;

        while ( m_Level >= boostLvl )
        {
            int prevBoostLvl = boostLvl;
            boostLvl += baseAbs;
            baseAbs += prevBoostLvl;
            if ( baseAbs > maxAbs )
                baseAbs = maxAbs;
        }

        return boostLvl;
    }

    public override int GetLastBoostLevel()
    {
        int boostLvl = 10;
        int baseAbs = 15;
        int maxAbs = 100;
        int prevBoostLvl = 0;

        while( m_Level >= boostLvl )
        {
            prevBoostLvl = boostLvl;
            boostLvl += baseAbs;
            baseAbs += prevBoostLvl;
            if ( baseAbs > maxAbs )
                baseAbs = maxAbs;
        }
        return prevBoostLvl;
    }

    public void PrintBoostLevels()
    {
        int last = GetLastBoostLevel();
        int next = GetNextBoostLevel();
        string message = "For level " + m_Level + ": Last=" + last + ", Next=" + next;
        Debug.Log(message);
    }
    #endregion Details

    #region Properties
    public int MineId { get { return m_MineId; } set { m_MineId = value; } }
    #endregion Properties
}
