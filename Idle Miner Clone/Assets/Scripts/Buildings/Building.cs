using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public Foreman m_Foreman = null;

    protected int m_Lvl = 1;
    protected float m_GoldDeposit = 0.0f;

    abstract public float GetUpgradeCost();

    public void EmptyDeposit()
    {
        m_GoldDeposit = 0.0f;
    }

    public void LevelUp()
    {
        ++m_Lvl;
    }

    public int Level { get { return m_Lvl; } }
    public float GoldDeposit { get { return m_GoldDeposit; } }
}