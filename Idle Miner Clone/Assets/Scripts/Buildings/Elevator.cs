using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : Building
{
    public MineShaftManager m_MineShaftManager;
    public RectTransform m_ElevatorArea;
    public Button m_ElevatorButton;

    private bool m_IsBottom = false;
    private Vector3 m_InitPos;

    private void Start()
    {
        m_MaxLevel = 2000;
        m_InitPos = m_ElevatorButton.transform.position;
        Debug.Log( m_InitPos );
    }

    #region States
    public void StartCollecting()
    {
        if (m_MineShaftManager.GetNrMineShafts() > 0 )
        {
            StartCoroutine( "Collect" );
            m_ElevatorButton.interactable = !m_ElevatorButton.interactable;
        }
    }

    private IEnumerator Collect()
    {
        double loadingSpeed = GetLoadingSpeed();
        double maxLoad = GetMaxLoad();
        int id = 1;
        MineShaft targetMineShaft = m_MineShaftManager.GetMineShaft( id );
        Vector2 initPos = m_ElevatorButton.transform.position;
        SetVerticalPos( targetMineShaft.m_IdText.transform.position.y );

        while ( true )
        {
            if (m_TransportedCash >= maxLoad )
            {
                ToIdleState( initPos );
                break;
            }
            else
            {
                if ( targetMineShaft.CashDeposit > loadingSpeed )
                {
                    m_TransportedCash += loadingSpeed;
                    targetMineShaft.RetrieveFromDeposit( loadingSpeed );
                    yield return new WaitForSeconds( 1 );
                }
                else
                {
                    m_TransportedCash += targetMineShaft.CashDeposit;
                    targetMineShaft.EmptyDeposit();
                    yield return new WaitForSeconds( 1 );
                    if ( targetMineShaft = m_MineShaftManager.GetMineShaft( ++id ) )
                    {
                        SetVerticalPos( targetMineShaft.m_IdText.transform.position.y );
                        yield return null;
                    }
                    else
                    {
                        ToIdleState( initPos );
                        break;
                    }
                }
            }
        }
        yield return null;
    }

    private void SetVerticalPos( float yPos )
    {
        m_ElevatorButton.transform.position = new Vector2( m_ElevatorButton.transform.position.x, yPos );
    }

    private void ToIdleState( Vector2 initPos )
    {
        m_ElevatorButton.transform.position = initPos;
        AddToDeposit( m_TransportedCash );
        m_TransportedCash = 0.0;
        m_ElevatorButton.interactable = !m_ElevatorButton.interactable;
    }

    #endregion States

    #region Details
    public override void CreateDetails( List<GameObject> where, GameObject prefab, Transform parent, int upgradeAmount )
    {
        GameObject newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Total Transportation", GetTotalTransportation(), GetTotalTransportation( upgradeAmount ) - GetTotalTransportation(), "/s" );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Load", GetMaxLoad(), GetMaxLoad( upgradeAmount ) - GetMaxLoad() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Movement Speed", GetMovementSpeed(), GetMovementSpeed( upgradeAmount ) - GetMovementSpeed() );
        where.Add( newDetail );

        newDetail = Instantiate( prefab, parent );
        newDetail.GetComponent<Detail>().SetDetailTexts( "Loading Speed", GetLoadingSpeed(), GetLoadingSpeed( upgradeAmount ) - GetLoadingSpeed(), "/s" );
        where.Add( newDetail );
    }

    public override string GetInfotext()
    {
        return "Elevator Level " + m_Level;
    }

    public double GetTotalTransportation( int upgradeAmount = 0 )
    {
        double loadSpeed = GetLoadingSpeed( upgradeAmount );
        double moveSpeed = GetMovementSpeed( upgradeAmount );
        double maxLoad = GetMaxLoad( upgradeAmount );

        double formula = ( System.Math.Pow( loadSpeed, 2.0 ) / maxLoad ) - ( 1.0 / moveSpeed );
        return formula;
    }

    public double GetMaxLoad( int upgradeAmount = 0 )
    {
        double multiplier = 4.0;
        return GetLoadingSpeed( upgradeAmount ) * multiplier;
    }

    public double GetMovementSpeed( int upgradeAmount = 0 )
    {
        double baseSpeed = 1.0;
        double increase = 0.01;

        return baseSpeed + 3.0 * ( ( m_Level + upgradeAmount ) / 5.0 ) * increase;
    }

    public double GetLoadingSpeed( int upgradeAmount = 0 )
    {
        double baseSpeed = 10.0;
        double increase = 0.07;
        return CashFormatter.CompoundInterest( baseSpeed, increase, m_Level + upgradeAmount );
    }
    #endregion Details

    public double TransportedGold { get { return m_TransportedCash; } }
    public bool IsBottom { get { return m_IsBottom; } }
}
