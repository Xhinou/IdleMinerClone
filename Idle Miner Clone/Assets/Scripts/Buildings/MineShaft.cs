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

    public int GetNrMiners()
    {
        int predictNr = 1;
        int lvlLimit = 10;
        int limitMultiplier = 2;

        if ( m_Lvl < lvlLimit )
            return predictNr;
        ++predictNr;
        
        for (lvlLimit = 50; m_Lvl < lvlLimit; lvlLimit *= limitMultiplier )
        {
            ++predictNr;
        }
        return predictNr;
    }

    public override float GetUpgradeCost()
    {
        return 0.0f;
    }

    private float GetMinerCapacity()
    {
        return 0.0f;
    }

    private float GetMiningSpeed()
    {
        return 0.0f;
    }

    public int MineId { get { return m_MineId; } }
}
