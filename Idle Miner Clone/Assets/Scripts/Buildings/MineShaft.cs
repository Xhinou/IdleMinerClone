using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShaft : Building
{
    private int m_MineId;

    public MineShaft( int id )
    {
        m_MineId = id;
        m_Lvl = 1;
        m_GoldDeposit = 0;
        m_Foreman = null;
    }

    public MineShaft( int mineId, int lvl, float goldDeposit, Foreman foreman )
    {
        m_MineId = mineId;
        m_Lvl = lvl;
        m_GoldDeposit = goldDeposit;
        m_Foreman = (foreman != null) ? foreman : null;
    }

    public static MineShaft CreateComponent( GameObject target, int id )
    {
        MineShaft mineShaft = target.AddComponent<MineShaft>();

        mineShaft.m_MineId = id;
        mineShaft.m_Lvl = 1;
        mineShaft.m_GoldDeposit = 0.0f;
        mineShaft.m_Foreman = null;

        return mineShaft;
    }

    public void StartMining()
    {
        StartCoroutine( "Mine" );
       // Time
    }

    public IEnumerator Mine()
    {
        yield return null;
    }

    #region Details
    public float GetTotalExtraction( int lvl )
    {
        return 0.0f;
    }

    public int GetNrMiners( int lvl )
    {
        int predictNr = 1;
        int lvlLimit = 10;
        int limitMultiplier = 2;

        if ( lvl < lvlLimit )
            return predictNr;
        ++predictNr;

        for ( lvlLimit = 50; lvl >= lvlLimit; lvlLimit *= limitMultiplier )
        {
            if ( lvl < lvlLimit )
            {
                return predictNr;
            }
            ++predictNr;
        }
        return predictNr;
    }

    public float GetWalkingSpeed( int lvl )
    {
        float predictSpeed = 2.0f;
        int lvlLimit = 82;

        if ( lvl < lvlLimit )
        {
            return predictSpeed;
        }

        int powBase = 3;
        int multiplier = 9;

        for ( lvlLimit = 82; lvl >= lvlLimit; lvlLimit = (int)Mathf.Pow( powBase, predictSpeed) * multiplier )
        {
            if ( lvl < lvlLimit )
            {
                return predictSpeed;
            }
            ++predictSpeed;
        }

        return predictSpeed;
    }

    public float GetMiningSpeed( int lvl )
    {
        int powBase = 2;
        float multiplier = 50.0f;

        return Mathf.Pow( powBase, lvl ) * multiplier;
    }

    public float GetMinerCapacity( int lvl )
    {
        float multiplier = 4;

        return GetMiningSpeed( lvl ) * multiplier;
    }

    public override float GetUpgradeCost( int lvl )
    {
        int powBase = 2;

        return Mathf.Pow( powBase, m_Lvl - 1 ) * Mathf.Pow( 10.0f, lvl - 1 + powBase );
    }

    public override int GetNextBoostLvl()
    {
        int lvlLimit = 10;
        if ( m_Lvl < lvlLimit )
            return lvlLimit;

        lvlLimit = 25;
        if ( m_Lvl < lvlLimit )
            return lvlLimit;

        lvlLimit = 50;
        if ( m_Lvl < lvlLimit )
            return lvlLimit;

        for ( lvlLimit = 50; m_Lvl >= lvlLimit; lvlLimit += 100 )
        {
            if (m_Lvl < lvlLimit )
                return lvlLimit;
        }
        return lvlLimit;
    }
    #endregion Details

    #region Properties
    public int MineId { get { return m_MineId; } }
    #endregion Properties
}
