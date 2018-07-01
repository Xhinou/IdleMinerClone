using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : Building
{
    public GameManager m_GameManager;
    public Elevator m_Elevator;
    public Button m_WarehouseButton;

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
        double maxLoad = GetMaxLoad();
        double loadSpeed = GetCollectingSpeed();

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
        newDetail.GetComponent<Detail>().SetDetailTexts( "Total Transportation", GetTotalCollecting(), GetTotalCollecting(  upgradeAmount ) - GetTotalCollecting(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Transporters", GetNrWorkers(), GetNrWorkers( upgradeAmount ) - GetNrWorkers() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Load per Transporter", GetMaxLoad(), GetMaxLoad( upgradeAmount ) - GetMaxLoad() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Loading Speed", GetCollectingSpeed(), GetCollectingSpeed( upgradeAmount ) - GetCollectingSpeed(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Walking Speed", GetMovementSpeed(), GetMovementSpeed( upgradeAmount ) - GetMovementSpeed() );
        where.Add( newDetail );
    }

    public override string GetInfotext()
    {
        return "Warehouse Level " + m_Level;
    }

    public override double GetTotalCollecting( int upgradeAmount = 0 )
    {
        int nrTransporters = GetNrWorkers( upgradeAmount );
        double walkTime = GetWalkingTime( upgradeAmount );
        double loadSpeed = GetCollectingSpeed( upgradeAmount );
        double capacity = GetMaxLoad( upgradeAmount );

        double formula = nrTransporters * ( capacity / ( walkTime + capacity / loadSpeed ) );
        return formula;
    }

    public int GetNrWorkers( int upgradeAmount = 0 )
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

    public override double GetCollectingSpeed( int upgradeAmount = 0 )
    {
        double baseSpeed = 50.0;
        double interest = 0.16;
        return CashUtility.CompoundInterest( baseSpeed, interest, m_Level + upgradeAmount );
    }

    public override double GetMovementSpeed( int upgradeAmount = 0 )
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
        return timeBase / GetMovementSpeed( upgradeAmount );
    }
    #endregion Details
}
