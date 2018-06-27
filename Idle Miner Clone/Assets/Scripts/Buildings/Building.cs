using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public Foreman m_Foreman = null;

    protected int m_Lvl = 1;
    protected float m_GoldDeposit = 0.0f;

    abstract public float GetUpgradeCost( int lvl );
    virtual public int GetNextBoostLvl()
    {
        int predict = 50;
        for (int lvlLimit = predict; m_Lvl > lvlLimit; lvlLimit += 50 )
        {
            if( m_Lvl < lvlLimit )
            {
                return lvlLimit;
            }
        }
        return predict;
    }

    public void EmptyDeposit()
    {
        m_GoldDeposit = 0.0f;
    }

    public void LevelUp()
    {
        ++m_Lvl;
    }

    public void LevelUp( int amount )
    {
        m_Lvl += amount;
    }

    public int Level { get { return m_Lvl; } }
    public float GoldDeposit { get { return m_GoldDeposit; } }
    public bool HasForeman { get { return m_Foreman != null; } }
}