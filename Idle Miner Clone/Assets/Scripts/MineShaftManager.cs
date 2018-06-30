using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShaftManager : MonoBehaviour
{
    private const int m_MaxMineShafts = 17;
    private List<MineShaft> m_MineShafts = new List<MineShaft>();
    private double m_Basecost = 10.0;

    public void AddMineShaft( MineShaft mineShaft )
    {
        if ( GetNrMineShafts() <= m_MaxMineShafts )
        {
            m_MineShafts.Add( mineShaft );
            mineShaft.MineId = GetNrMineShafts();
            mineShaft.m_IdText.text = mineShaft.MineId.ToString();
        }
        else
        {
            Debug.LogWarning( "Maximum amount of Mine Shafts reached!" );
        }
    }

    public double GetCurrentBuyCost()
    {
        if ( GetNrMineShafts() <= 0 )
        {
            return m_Basecost;
        }
        return m_MineShafts[GetNrMineShafts() - 1].GetBuyCost();
    }

    public int GetNrMineShafts()
    {
        return m_MineShafts.Count;
    }

    public MineShaft GetMineShaft( int id )
    {
        if (id > 0 && id <= GetNrMineShafts() )
            return m_MineShafts[id - 1];

        //Debug.LogWarning( "GetMineShaft(int id) > Incorrect ID: null was returned." );
        return null;
    }

    public int GetLastMineShaftId()
    {
        int id = GetNrMineShafts();
        if ( id < 1 ) Debug.LogWarning( "GetLastMineShaftId() > m_MineShafts was empty: 0 was returned." );
        return GetNrMineShafts();
    }

    public int MaxMineShafts { get { return m_MaxMineShafts; } }
}
