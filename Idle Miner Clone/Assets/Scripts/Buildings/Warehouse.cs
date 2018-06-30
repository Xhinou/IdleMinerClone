using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : Building
{
    public GameManager m_GameManager;
    public Elevator m_Elevator;
    public Button m_WarehouseButton;
    //public new Text m_DepositText = 0;

    private void Start()
    {
        m_MaxLevel = 2000;
    }

    public void StartCollecting()
    {
        StartCoroutine( "Collect" );
        m_WarehouseButton.interactable = !m_WarehouseButton.interactable;
    }

    #region States
    private IEnumerator Collect()
    {
        double maxLoad = GetLoadPerTransporter();
        double loadSpeed = GetLoadingSpeed();

        while ( m_TransportedCash < maxLoad )
        {
            if (loadSpeed <= m_Elevator.CashDeposit )
            {
                m_TransportedCash += loadSpeed;
                m_Elevator.RetrieveFromDeposit( loadSpeed );
                yield return new WaitForSeconds( 1 );
            }
            else
            {
                m_TransportedCash += m_Elevator.CashDeposit;
                m_Elevator.EmptyDeposit();
                break;
            }
        }

        yield return StartCoroutine( "Deposit" );
    }

    private IEnumerator Deposit()
    {
        double duration = GetWalkingTime();

        while ( duration > 0.0 )
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        m_GameManager.AddCash( m_TransportedCash );
        m_TransportedCash = 0.0;
        m_WarehouseButton.interactable = !m_WarehouseButton.interactable;
        yield return null;
    }
    #endregion States

    #region Details
    public override void CreateDetails( List<GameObject> where, GameObject prefab, Transform parent, int upgradeAmount )
    {
        GameObject newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Total Transportation", GetTotalTransportation(), GetTotalTransportation(  upgradeAmount ) - GetTotalTransportation(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Transporters", GetNrTransporters(), GetNrTransporters( upgradeAmount ) - GetNrTransporters() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Load per Transporter", GetLoadPerTransporter(), GetLoadPerTransporter( upgradeAmount ) - GetLoadPerTransporter() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Loading Speed", GetLoadingSpeed(), GetLoadingSpeed( upgradeAmount ) - GetLoadingSpeed(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Walking Speed", GetWalkingSpeed(), GetWalkingSpeed( upgradeAmount ) - GetWalkingSpeed(), "/s" );
        where.Add( newDetail );
    }

    public override string GetInfotext()
    {
        return "Warehouse Level " + m_Level;
    }

    public double GetTotalTransportation( int upgradeAmount = 0 )
    {
        int nrTransporters = GetNrTransporters( upgradeAmount );
        double walkTime = GetWalkingTime( upgradeAmount );
        double loadSpeed = GetLoadingSpeed( upgradeAmount );
        double capacity = GetLoadPerTransporter( upgradeAmount );

        double formula = nrTransporters * ( capacity / ( walkTime + capacity / loadSpeed ) );
        return formula;
    }

    public int GetNrTransporters( int upgradeAmount = 0 )
    {
        int predictNr = 1;
        int lvlLimit = 100;
        int limitIncrease = 100;

        while ( m_Level + upgradeAmount >= lvlLimit )
        {
            ++predictNr;
            lvlLimit += limitIncrease;
        }
        return predictNr;
    }

    public double GetLoadPerTransporter( int upgradeAmount = 0 )
    {
        double multiplier = 4.0;
        return GetLoadingSpeed( upgradeAmount ) * multiplier;
    }

    public double GetLoadingSpeed( int upgradeAmount = 0 )
    {
        double baseSpeed = 10.0;
        double interest = 0.16;
        return CashFormatter.CompoundInterest( baseSpeed, interest, m_Level + upgradeAmount );
    }

    public double GetWalkingSpeed( int upgradeAmount = 0 )
    {
        double predictSpeed = 2.0;
        int lvlLimit = 400;
        int limitIncrease = 400;

        while (m_Level + upgradeAmount >= lvlLimit )
        {
            ++predictSpeed;
            lvlLimit += limitIncrease;
        }
        return predictSpeed;
    }

    public double GetWalkingTime( int upgradeAmount = 0 )
    {
        double timeBase = 4.0;
        return timeBase / GetWalkingSpeed( upgradeAmount );
    }
    #endregion Details
}
