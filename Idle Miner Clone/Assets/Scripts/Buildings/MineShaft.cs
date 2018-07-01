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

    private void Start()
    {
        m_MaxLevel = 800;
    }

    #region States
    public void StartMining()
    {
        StartCoroutine( "Mine" );
        m_MinerButton.interactable = !m_MinerButton.interactable;
    }

    private IEnumerator Mine()
    {
        double miningSpeed = GetCollectingSpeed();
        double duration = GetMaxLoad() / miningSpeed;

        while ( duration > 0.0 )
        {
            m_TransportedCash += miningSpeed * Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }
        
        yield return StartCoroutine( "Deposit" );
    }

    private IEnumerator Deposit()
    {
        double duration = GetWalkingTime();

        while ( duration > 0.0 )
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        m_CashDeposit += m_TransportedCash;
        m_TransportedCash = 0.0;
        m_DepositText.text = CashUtility.FormatToString( m_CashDeposit );
        m_MinerButton.interactable = !m_MinerButton.interactable;
        yield return null;
    }
    #endregion States

    #region Details
    public override void CreateDetails( List<GameObject> where, GameObject prefab, Transform parent, int upgradeAmount )
    {
        GameObject newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Total Extraction", GetTotalCollecting(), GetTotalCollecting(  upgradeAmount ) - GetTotalCollecting(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Miners", GetNrWorkers(), GetNrWorkers( upgradeAmount ) - GetNrWorkers() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Walking Speed", GetMovementSpeed(), GetMovementSpeed(  upgradeAmount ) - GetMovementSpeed() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Mining Speed", GetCollectingSpeed(), GetCollectingSpeed(  upgradeAmount ) - GetCollectingSpeed(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Worker Capacity", GetMaxLoad(), GetMaxLoad(  upgradeAmount ) - GetMaxLoad() );
        where.Add( newDetail );
    }

    public override string GetInfotext()
    {
        return "Mine Shaft " + m_MineId + " Level " + m_Level;
    }

    public override double GetTotalCollecting( int upgradeAmount = 0 )
    {
        int nrMiners = GetNrWorkers( upgradeAmount );
        double walkTime = GetWalkingTime( upgradeAmount );
        double mineSpeed = GetCollectingSpeed( upgradeAmount );
        double capacity = GetMaxLoad( upgradeAmount );

        double formula = nrMiners * ( capacity / ( walkTime +  capacity / mineSpeed ) );
        return formula;
    }

    public int GetNrWorkers( int upgradeAmount = 0 )
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

    public override double GetMovementSpeed( int upgradeAmount = 0 )
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
        return timeBase / GetMovementSpeed( upgradeAmount );
    }

    public override double GetCollectingSpeed( int upgradeAmount = 0 )
    {
        int exp = 2;
        double multiplier = 50.0;

        return System.Math.Pow( m_Level + upgradeAmount, exp ) * System.Math.Pow( multiplier, m_MineId );
    }

    public double GetNextBuyCost()
    {
        switch ( m_MineId )
        {
            case 0:
                return 10.0;
            case 1:
                return 1000.0;
            default:
                return 3000.0 * System.Math.Pow( 20.0, m_MineId - 2 );
        }
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

    public override double GetUpgradeCost( int upgradeAmount )
    {
        int period = m_Level + upgradeAmount - 1;
        double baseInterest = 0.16;
        double baseInterestDecrease = -0.00086;
        double interest = CashUtility.CompoundInterest( baseInterest, baseInterestDecrease, period );

        return CashUtility.CompoundInterest( GetBuyCost(), interest, period );
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
    #endregion Details

    #region Properties
    public int MineId { get { return m_MineId; } set { m_MineId = value; } }
    #endregion Properties
}
