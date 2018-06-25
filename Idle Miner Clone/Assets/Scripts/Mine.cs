using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public int mineId;
    public int lvl;
    public int nrMiners;
    public float minerCapacity;
    public float totalExtract;
    public int walkSpeed;
    public float miningSpeed;
    public float goldDeposit;
    public bool hasManager;

    public Mine( int id )
    {
        mineId = id;
        lvl = 1;
        nrMiners = 1;
        walkSpeed = 1;
        goldDeposit = 0;
        minerCapacity = CalculateMinerCapacity();
        miningSpeed = CalculateMiningSpeed();
        hasManager = false;
    }

    private float CalculateUpgradeCost()
    {
        return 0.0f;
    }

    private float CalculateMinerCapacity()
    {
        return 0.0f;
    }

    private float CalculateMiningSpeed()
    {
        return 0.0f;
    }
}
