using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : Building
{
    public float GetTotalTransportation()
    {
        return 0.0f;
    }

    public int GetNrTransporters()
    {
        return 0;
    }

    public float GetLoadPerTransporter()
    {
        return 0.0f;
    }

    public float GetLoadingspeed()
    {
        return 0.0f;
    }

    public int GetWalkSpeed()
    {
        return 0;
    }

    public override float GetUpgradeCost()
    {
        return 0.0f;
    }
}
