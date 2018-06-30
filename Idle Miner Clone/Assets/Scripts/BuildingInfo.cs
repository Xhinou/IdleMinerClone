using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfo : MonoBehaviour
{
    public GameManager m_GameManager;
    public MineShaftManager m_MineShaftManager;
    public Canvas m_Viewport;
    public GameObject m_InfoPanel;
    public GameObject m_HeadPanel;
    public GameObject m_DetailPanelPrefab;
    public Text m_HeadText;
    public Text m_CostText;
    public Slider m_CurLvlSlider;
    public Slider m_NextLvlSlider;
    public ToggleGroup m_MultToggle;
    public Button m_UpgradeButton;

    private List<GameObject> m_Details = new List<GameObject>();
    private Building m_TargetBuilding;
    private int m_CurUpgradeAmount = 1;

    public void OnToggleChange( int amount )
    {
        if ( !m_TargetBuilding.IsMaxLevel() )
        {
            m_CurUpgradeAmount = amount;
            if ( amount > m_NextLvlSlider.maxValue )
                amount = (int)m_NextLvlSlider.maxValue;
            m_NextLvlSlider.value = m_CurLvlSlider.value + amount;
        }
        RefreshInfo();
    }

    public void DisplayInfo( Building building )
    {
        m_InfoPanel.SetActive( true );
        m_TargetBuilding = building;
        RefreshInfo();
        //double upgradeCost = m_TargetBuilding.GetUpgradeCost( m_CurUpgradeAmount );
        //CheckCash();
        //m_InfoPanel.SetActive( true );
        //m_HeadText.text = building.GetInfotext();
        //RefreshDetails( building );
        //m_CostText.text = CashFormatter.FormatToString( m_TargetBuilding.GetUpgradeCost( m_CurUpgradeAmount ) );
        //RefreshSliders();
        //m_GameManager.RefreshUI();
    }
    public void Upgrade()
    {
        m_TargetBuilding.LevelUp( m_CurUpgradeAmount );
        m_TargetBuilding.m_InfoButton.transform.GetComponentInChildren<Text>().text = "Level\n" + m_TargetBuilding.Level;
        m_GameManager.RetrieveCash( m_TargetBuilding.GetUpgradeCost( m_CurUpgradeAmount ) );

        if ( m_TargetBuilding.Level >= m_TargetBuilding.GetLastBoostLevel() )
        {
            RefreshSliders();
        }
        else
        {
            m_CurLvlSlider.value += m_CurUpgradeAmount;
            m_NextLvlSlider.value += m_CurUpgradeAmount;
        }
        //DisplayInfo( m_TargetBuilding );
        m_GameManager.E();
    }

    public void HideInfo()
    {
        ClearDetails();
        m_InfoPanel.SetActive( false );
    }

    public void RefreshInfo()
    {
        if ( m_InfoPanel.activeInHierarchy )
        {
            CheckCash();
            m_HeadText.text = m_TargetBuilding.GetInfotext();
            m_CostText.text = CashFormatter.FormatToString( m_TargetBuilding.GetUpgradeCost( m_CurUpgradeAmount ) );
            RefreshDetails( m_TargetBuilding );
            RefreshSliders();
        }
    }

    private void RefreshDetails( Building building )
    {
        if (m_Details.Count > 0 )
        {
            ClearDetails();
        }

        building.CreateDetails( m_Details, m_DetailPanelPrefab, m_InfoPanel.transform, m_CurUpgradeAmount );

        for (int i = 0; i < m_Details.Count; ++i )
        {
            RectTransform detailRect = m_Details[i].GetComponent<RectTransform>();
            detailRect.localPosition = new Vector2( detailRect.localPosition.x, detailRect.localPosition.y - detailRect.rect.height * m_Viewport.scaleFactor * i );
        }
    }

    private void RefreshSliders( int lastBoostLevel = 0 )
    {
        if ( !m_TargetBuilding.IsMaxLevel() )
        {
            m_CurLvlSlider.minValue = m_TargetBuilding.Level >= 10 ? 0 : 1;
            m_NextLvlSlider.minValue = m_CurLvlSlider.minValue;
            m_CurLvlSlider.maxValue = m_TargetBuilding.GetNextBoostLevel() - m_TargetBuilding.GetLastBoostLevel();
            m_NextLvlSlider.maxValue = m_CurLvlSlider.maxValue;
            m_CurLvlSlider.value = m_TargetBuilding.Level - m_TargetBuilding.GetLastBoostLevel();
            m_NextLvlSlider.value = m_CurLvlSlider.value + m_CurUpgradeAmount;
        }
        else
        {
            m_CurLvlSlider.value = m_NextLvlSlider.maxValue;
            m_NextLvlSlider.value = m_NextLvlSlider.maxValue;
        }
    }

    private void CheckCash()
    {
        m_UpgradeButton.interactable = ( !m_TargetBuilding.IsMaxLevel() && m_GameManager.PlayerCash >= m_TargetBuilding.GetUpgradeCost( m_CurUpgradeAmount ) );
    }

    private void ClearDetails()
    {
        foreach ( GameObject detail in m_Details )
        {
            Destroy( detail );
        }
        m_Details.Clear();
    }
}