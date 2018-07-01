using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MineShaftManager m_MineShaftManager;
    public BuildingInfoController m_BuildingInfoController;
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
        RefreshUI();
    }

    public void AddCash( double amount )
    {
        m_PlayerCash += amount;
        RefreshUI();
    }

    public void RetrieveCash( double amount )
    {
        m_PlayerCash -= amount;
        RefreshUI();
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
            infoButton.onClick.AddListener( delegate { m_BuildingInfoController.DisplayInfo( mineShaft ); } );
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
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        m_CashText.text = CashUtility.FormatToString( m_PlayerCash );
        double newBuyCost = m_MineShaftManager.GetNewBuyCost();
        m_MineShaftCostText.text = "New Shaft\n" + CashUtility.FormatToString( newBuyCost );
        m_NewMineRect.GetComponent<Button>().interactable = ( m_PlayerCash >= newBuyCost && m_NewMineRect != null );
        m_BuildingInfoController.RefreshInfo();
    }

    public double PlayerCash { get { return m_PlayerCash; } }
}