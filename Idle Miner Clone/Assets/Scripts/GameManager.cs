using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MineShaftManager m_MineShaftManager;
    public BuildingInfo m_BuildingInfoManager;
    public Elevator m_Elevator;
    public Text m_CashText;
    public Text m_IdleCashText;
    public GameObject m_MineShaftPrefab;
    public Canvas m_Viewport;
    public RectTransform m_GamePanel;
    public RectTransform m_NewMineRect;

    private Text m_MineShaftCostText;
    private double m_PlayerCash;

	private void Start ()
	{
        double playerInitCash = 100.0;
        m_PlayerCash = playerInitCash;
        m_MineShaftCostText = m_NewMineRect.GetComponentInChildren<Text>();
        //m_CashText.text = CashFormatter.FormatToString( m_PlayerCash );
        //m_MineShaftCostText.text = "New Shaft\nCost: " + CashFormatter.FormatToString( m_MineShaftManager.GetCurrentBuyCost() );
        E();
    }

	private void Update ()
	{
		if ( Input.GetKeyDown( KeyCode.C ) == true )
        {
            m_PlayerCash *= 2.0;
            if ( m_PlayerCash > m_MineShaftManager.GetNewBuyCost() && m_NewMineRect != null )
            {
                m_NewMineRect.GetComponent<Button>().interactable = true;
            }
            E();
        }
    }

    public void AddCash( double amount )
    {
        m_PlayerCash += amount;
        E();
    }

    public void RetrieveCash( double amount )
    {
        m_PlayerCash -= amount;
        E();
    }

    public void CreateMineShaft()
    {
        if (m_PlayerCash >= m_MineShaftManager.GetNewBuyCost() )
        {
            m_PlayerCash -= m_MineShaftManager.GetNewBuyCost();

            GameObject newObject = Instantiate( m_MineShaftPrefab, m_NewMineRect.position, Quaternion.identity, m_GamePanel );
            MineShaft mineShaft = newObject.GetComponent<MineShaft>();
            m_MineShaftManager.AddMineShaft( mineShaft );

            Button infoButton = mineShaft.m_InfoButton;
            infoButton.onClick.AddListener( delegate { m_BuildingInfoManager.DisplayInfo( mineShaft ); } );
            Text lvlText = infoButton.transform.GetComponentInChildren<Text>();
            lvlText.text = "Level\n" + 1;

            float yOffset = mineShaft.GetComponent<RectTransform>().rect.height;
            m_NewMineRect.transform.Translate( new Vector2( 0, -yOffset * m_Viewport.scaleFactor ) );
            RectTransform elevatorRect = m_Elevator.m_ElevatorArea.GetComponent<RectTransform>();
            elevatorRect.offsetMin = new Vector2( elevatorRect.offsetMin.x, elevatorRect.offsetMin.y - yOffset );

            if ( m_MineShaftManager.GetNrMineShafts() >= m_MineShaftManager.MaxMineShafts )
            {
                Destroy( m_NewMineRect.gameObject );
                m_NewMineRect = null;
            }
            E();
        }
        //else
        //{
        //    //double newBuyCost = m_MineShaftManager.GetCurrentBuyCost();
        //    //m_MineShaftCostText.text = "New Shaft\n" + CashFormatter.FormatToString( newBuyCost );
        //    //if ( m_PlayerCash < newBuyCost && m_NewMineRect != null )
        //    //{
        //    //    m_NewMineRect.GetComponent<Button>().interactable = false;
        //    //}
        //}
    }

    //public void RefreshUI()
    //{
    //    double newBuyCost = m_MineShaftManager.GetCurrentBuyCost();
    //    m_MineShaftCostText.text = "New Shaft\n" + CashFormatter.FormatToString( newBuyCost );
    //    if ( m_PlayerCash < newBuyCost && m_NewMineRect != null )
    //    {
    //        m_NewMineRect.GetComponent<Button>().interactable = false;
    //    }
    //    m_CashText.text = CashFormatter.FormatToString( m_PlayerCash );
    //}

    public void E()
    {
        m_CashText.text = CashFormatter.FormatToString( m_PlayerCash );
        double newBuyCost = m_MineShaftManager.GetNewBuyCost();
        m_MineShaftCostText.text = "New Shaft\n" + CashFormatter.FormatToString( newBuyCost );
         m_NewMineRect.GetComponent<Button>().interactable = ( m_PlayerCash >= newBuyCost && m_NewMineRect != null );
        m_BuildingInfoManager.RefreshInfo();
    }

    public double PlayerCash { get { return m_PlayerCash; } }
}