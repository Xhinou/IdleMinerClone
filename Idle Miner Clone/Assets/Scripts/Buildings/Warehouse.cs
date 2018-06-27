using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : Building
{
    public float GetTotalTransportation( int lvl )
    {
        return 0.0f;
    }

    public int GetNrTransporters( int lvl )
    {
        return 0;
    }

    public float GetLoadPerTransporter( int lvl )
    {
        return 0.0f;
    }

    public float GetLoadingspeed( int lvl )
    {
        return 0.0f;
    }

    public float GetWalkingSpeed( int lvl )
    {
        return 0.0f;
    }

    public override float GetUpgradeCost( int lvl )
    {
        return 0.0f;
    }
}
