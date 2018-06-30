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
        m_InitPos = m_ElevatorButton.transform.position;
        Debug.Log( m_InitPos );
    }

    #region States
    public void StartCollecting()
    {
        StartCoroutine( "Collect" );
        m_ElevatorButton.interactable = !m_ElevatorButton.interactable;
    }

    private IEnumerator Collect()
    {
        Debug.Log( "in" );
        double loadingSpeed = GetLoadingSpeed();
        double maxLoad = GetMaxLoad();
        int id = 1;
        MineShaft targetMineShaft = m_MineShaftManager.GetMineShaft( id );
        Vector2 initPos = m_ElevatorButton.transform.position;
        SetVerticalPos( targetMineShaft.m_IdText.transform.position.y );
        Debug.Log( "Currently Loading" );
        yield return new WaitForSeconds(1.0f);

        while ( true )
        {
            if (m_TransportedGold >= maxLoad )
            {
                yield return new WaitForSeconds( 1.0f );
                Debug.Log( "Max Load reached" );
                ToIdleState( initPos );
                break;
            }
            else
            {
                if ( targetMineShaft.GoldDeposit > loadingSpeed )
                {
                    Debug.Log( "Loading from MineShaft" );
                    m_TransportedGold += loadingSpeed;
                    targetMineShaft.RetrieveFromDeposit( loadingSpeed );
                    yield return new WaitForSeconds( 1.0f );
                }
                else
                {
                    Debug.Log( "MineShaft empty" );
                    m_TransportedGold += targetMineShaft.GoldDeposit;
                    targetMineShaft.EmptyDeposit();
                    if ( targetMineShaft = m_MineShaftManager.GetMineShaft( ++id ) )
                    {
                        yield return new WaitForSeconds( 1.0f );
                        Debug.Log( "Moving to next MineShaft" );
                        SetVerticalPos( targetMineShaft.m_IdText.transform.position.y );
                        yield return null;
                    }
                    else
                    {
                        Debug.Log( "No more MineShaft, back to base" );
                        ToIdleState( initPos );
                        break;
                    }
                }
            }
        }
        Debug.Log( "Loading ended" );
        yield return null;
    }

    private void SetVerticalPos( float yPos )
    {
        m_ElevatorButton.transform.position = new Vector2( m_ElevatorButton.transform.position.x, yPos );
    }

    private void ToIdleState( Vector2 initPos )
    {
        m_ElevatorButton.transform.position = initPos;
       // Debug.Log( m_ElevatorButton.transform.position );
        AddToDeposit( m_TransportedGold );
        m_TransportedGold = 0.0;
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

    public override double GetUpgradeCost( int upgradeAmount )
    {
        double baseCost = 100.0;
        double interest = 0.08;
        int period = m_Level + upgradeAmount;
        return CashFormatter.CompoundInterest( baseCost, interest, period );
    }
    #endregion Details

    public double TransportedGold { get { return m_TransportedGold; } }
    public bool IsBottom { get { return m_IsBottom; } }
}
