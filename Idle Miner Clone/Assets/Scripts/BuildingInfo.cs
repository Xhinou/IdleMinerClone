using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfo : MonoBehaviour
{
    public Canvas m_Viewport;
    public GameObject m_InfoPanel;
    public GameObject m_DetailPanelPrefab;
    public Text m_HeadText;
    public Slider m_CurLvlSlider;
    public Slider m_NextLvlSlider;
    public ToggleGroup m_MultToggle;

    private List<Detail> m_Details = new List<Detail>();
    private Building m_TargetBuilding;
    private int m_CurUpgradeAmount = 1;

    public void DisplayInfo( Building building )
    {
        m_TargetBuilding = building;
        m_InfoPanel.SetActive( true );
        if ( m_TargetBuilding is MineShaft )
        {
            MineShaft mineShaft = (MineShaft)m_TargetBuilding;
            m_HeadText.text = "Mine Shaft " + mineShaft.MineId + " Level " + mineShaft.Level;
            LoadDetails( mineShaft );
        }
        else if ( m_TargetBuilding is Elevator )
        {
            Elevator elevator = (Elevator)m_TargetBuilding;
            m_HeadText.text = "Elevator Level " + elevator.Level;
            LoadDetails( elevator );
        }
        else if ( m_TargetBuilding is Warehouse )
        {
            Warehouse warehouse = (Warehouse)m_TargetBuilding;
            m_HeadText.text = "Warehouse Level " + warehouse.Level;
            LoadDetails( warehouse );
        }
    }

    private void LoadDetails( MineShaft mineShaft )
    {
        if (m_Details.Count > 0 )
        {
            ClearDetails();
        }

        Detail detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        RectTransform detailRect = detail.gameObject.GetComponent<RectTransform>();
        float xPos = detailRect.localPosition.x;
        float yPos = detailRect.localPosition.y;
        detail.m_NameText.text = "Total Extraction";
        detail.m_ValueText.text = CashFormatter.FormatToString( mineShaft.GetTotalExtraction( mineShaft.Level ) ) + "/s";
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( mineShaft.GetTotalExtraction( mineShaft.Level + m_CurUpgradeAmount ) - mineShaft.GetTotalExtraction( mineShaft.Level ) );
        m_Details.Add( detail );
        float rectHeight = detail.GetComponent<RectTransform>().rect.height * m_Viewport.scaleFactor;
        yPos -= rectHeight;

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Miners";
        detail.m_ValueText.text = CashFormatter.FormatToString( mineShaft.GetNrMiners( mineShaft.Level ) );
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( mineShaft.GetNrMiners( mineShaft.Level + m_CurUpgradeAmount ) - mineShaft.GetNrMiners( mineShaft.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Walking Speed";
        detail.m_ValueText.text = CashFormatter.FormatToString( mineShaft.GetWalkingSpeed( mineShaft.Level ) );
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( mineShaft.GetWalkingSpeed( mineShaft.Level + m_CurUpgradeAmount ) - mineShaft.GetWalkingSpeed( mineShaft.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Mining Speed";
        detail.m_ValueText.text = CashFormatter.FormatToString( mineShaft.GetMiningSpeed( mineShaft.Level ) ) + "/s";
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( mineShaft.GetMiningSpeed( mineShaft.Level + m_CurUpgradeAmount ) - mineShaft.GetMiningSpeed( mineShaft.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        detail.m_NameText.text = "Worker Capacity";
        detail.m_ValueText.text = CashFormatter.FormatToString( mineShaft.GetMinerCapacity( mineShaft.Level ) );
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( mineShaft.GetMinerCapacity( mineShaft.Level + m_CurUpgradeAmount ) - mineShaft.GetMinerCapacity( mineShaft.Level ) );
        m_Details.Add( detail );
    }

    private void LoadDetails( Elevator elevator )
    {
        if ( m_Details.Count > 0 )
        {
            ClearDetails();
        }

        Detail detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        RectTransform detailRect = detail.gameObject.GetComponent<RectTransform>();
        float xPos = detailRect.localPosition.x;
        float yPos = detailRect.localPosition.y;
        detail.m_NameText.text = "Total Transportation";
        detail.m_ValueText.text = CashFormatter.FormatToString( elevator.GetTotalTransportation( elevator.Level ) ) + "/s";
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( elevator.GetTotalTransportation( elevator.Level + m_CurUpgradeAmount ) - elevator.GetTotalTransportation( elevator.Level ) );
        m_Details.Add( detail );
        float rectHeight = detail.GetComponent<RectTransform>().rect.height * m_Viewport.scaleFactor;
        yPos -= rectHeight;

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Load";
        detail.m_ValueText.text = CashFormatter.FormatToString( elevator.GetMaxLoad( elevator.Level ) );
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( elevator.GetMaxLoad( elevator.Level + m_CurUpgradeAmount ) - elevator.GetMaxLoad( elevator.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Movement Speed";
        detail.m_ValueText.text = CashFormatter.FormatToString( elevator.GetMovementSpeed( elevator.Level ) );
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( elevator.GetMovementSpeed( elevator.Level + m_CurUpgradeAmount ) - elevator.GetMovementSpeed( elevator.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        detail.m_NameText.text = "Loading Speed";
        detail.m_ValueText.text = CashFormatter.FormatToString( elevator.GetLoadingSpeed( elevator.Level ) ) + "/s";
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( elevator.GetLoadingSpeed( elevator.Level + m_CurUpgradeAmount ) - elevator.GetLoadingSpeed( elevator.Level ) );
        m_Details.Add( detail );
    }

    private void LoadDetails( Warehouse warehouse )
    {
        if ( m_Details.Count > 0 )
        {
            ClearDetails();
        }

        Detail detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        RectTransform detailRect = detail.gameObject.GetComponent<RectTransform>();
        float xPos = detailRect.localPosition.x;
        float yPos = detailRect.localPosition.y;
        detail.m_NameText.text = "Total Transportation";
        detail.m_ValueText.text = CashFormatter.FormatToString( warehouse.GetTotalTransportation( warehouse.Level ) ) + "/s";
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( warehouse.GetTotalTransportation( warehouse.Level + m_CurUpgradeAmount ) - warehouse.GetTotalTransportation( warehouse.Level ) );
        m_Details.Add( detail );
        float rectHeight = detail.GetComponent<RectTransform>().rect.height * m_Viewport.scaleFactor;
        yPos -= rectHeight;

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Transporters";
        detail.m_ValueText.text = CashFormatter.FormatToString( warehouse.GetNrTransporters( warehouse.Level ) );
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( warehouse.GetNrTransporters( warehouse.Level + m_CurUpgradeAmount ) - warehouse.GetNrTransporters( warehouse.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Load per Transporter";
        detail.m_ValueText.text = CashFormatter.FormatToString( warehouse.GetLoadPerTransporter( warehouse.Level ) );
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( warehouse.GetLoadPerTransporter( warehouse.Level + m_CurUpgradeAmount ) - warehouse.GetLoadPerTransporter( warehouse.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        yPos -= rectHeight;
        detail.m_NameText.text = "Loading Speed";
        detail.m_ValueText.text = CashFormatter.FormatToString( warehouse.GetLoadingspeed( warehouse.Level ) ) + "/s";
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( warehouse.GetLoadingspeed( warehouse.Level + m_CurUpgradeAmount ) - warehouse.GetLoadingspeed( warehouse.Level ) );
        m_Details.Add( detail );

        detail = Instantiate( m_DetailPanelPrefab, m_InfoPanel.transform ).GetComponent<Detail>();
        detail.GetComponent<RectTransform>().localPosition = new Vector2( xPos, yPos );
        detail.m_NameText.text = "Walking Speed";
        detail.m_ValueText.text = CashFormatter.FormatToString( warehouse.GetWalkingSpeed( warehouse.Level ) ) + "/s";
        detail.m_PredictText.text = "+" + CashFormatter.FormatToString( warehouse.GetWalkingSpeed( warehouse.Level + m_CurUpgradeAmount ) - warehouse.GetWalkingSpeed( warehouse.Level ) );
        m_Details.Add( detail );
    }

    public void HideInfo()
    {
        ClearDetails();
        m_InfoPanel.SetActive( false );
    }

    private void ClearDetails()
    {
        foreach ( Detail detail in m_Details )
        {
            Destroy( detail.gameObject );
        }
        m_Details.Clear();
    }

    public void Upgrade()
    {
        m_TargetBuilding.LevelUp( m_CurUpgradeAmount );

        if (m_CurLvlSlider.value + m_CurUpgradeAmount > m_TargetBuilding.GetNextBoostLvl() )
        {
            m_CurLvlSlider.maxValue = m_TargetBuilding.GetNextBoostLvl();
            m_CurLvlSlider.value = m_TargetBuilding.Level - m_CurLvlSlider.maxValue;
        }
        else
        {
            m_CurLvlSlider.value += m_CurUpgradeAmount;
        }
        DisplayInfo( m_TargetBuilding );
    }

    private void UpdateLevelSlider()
    {
       m_CurLvlSlider.maxValue = m_TargetBuilding.GetNextBoostLvl();
    }

    public void OnToggleChange( int amount )
    {
        m_CurUpgradeAmount = amount;
        if ( amount > m_NextLvlSlider.maxValue )
            amount = (int)m_NextLvlSlider.maxValue;
        m_NextLvlSlider.value = amount;
        LoadDetails( (MineShaft)m_TargetBuilding );
       // UpdateInfo( amount );
    }
}
