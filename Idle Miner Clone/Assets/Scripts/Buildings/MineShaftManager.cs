using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShaftManager : MonoBehaviour
{
    private const int m_MaxMineShafts = 17;
    private List<MineShaft> m_MineShafts = new List<MineShaft>();

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

    public double GetNewBuyCost()
    {
        switch ( GetNrMineShafts() )
        {
            case 0:
                return 10.0;
            case 1:
                return 1000.0;
            default:
                return 3000.0 * System.Math.Pow( 20.0, GetNrMineShafts() - 2 );
        }
    }

    public int GetNrMineShafts()
    {
        return m_MineShafts.Count;
    }

    public MineShaft GetMineShaft( int id )
    {
        if ( id > 0 && id <= GetNrMineShafts() )
            return m_MineShafts[id - 1];

        return null;
    }

    public int MaxMineShafts { get { return m_MaxMineShafts; } }
}
