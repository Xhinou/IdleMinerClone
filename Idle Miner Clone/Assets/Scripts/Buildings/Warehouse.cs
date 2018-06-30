using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : Building
{
    public override void CreateDetails( List<GameObject> where, GameObject prefab, Transform parent, int upgradeAmount )
    {
        GameObject newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Total Transportation", GetTotalTransportation( m_Level ), GetTotalTransportation( m_Level + upgradeAmount ) - GetTotalTransportation( m_Level ), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Transporters", GetNrTransporters( m_Level ), GetNrTransporters( m_Level + upgradeAmount ) - GetNrTransporters( m_Level ) );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Load per Transporter", GetLoadPerTransporter( m_Level ), GetLoadPerTransporter( m_Level + upgradeAmount ) - GetLoadPerTransporter( m_Level ) );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Loading Speed", GetLoadingSpeed( m_Level ), GetLoadingSpeed( m_Level + upgradeAmount ) - GetLoadingSpeed( m_Level ), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Walking Speed", GetWalkingSpeed( m_Level ), GetWalkingSpeed( m_Level + upgradeAmount ) - GetWalkingSpeed( m_Level ), "/s" );
        where.Add( newDetail );
    }

    public override string GetInfotext()
    {
        return "Warehouse Level " + m_Level;
    }

    public double GetTotalTransportation( int lvl )
    {
        return 0.0;
    }

    public int GetNrTransporters( int lvl )
    {
        return 0;
    }

    public double GetLoadPerTransporter( int lvl )
    {
        return 0.0;
    }

    public double GetLoadingSpeed( int lvl )
    {
        return 0.0;
    }

    public double GetWalkingSpeed( int lvl )
    {
        return 0.0;
    }

    public override double GetUpgradeCost( int lvl )
    {
        return 0.0;
    }
}
