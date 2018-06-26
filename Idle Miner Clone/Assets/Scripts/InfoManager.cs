using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public GameObject m_InfoPanel;
    public Text m_HeadText;
    public Slider m_CurLvlSlider;
    public Slider m_NextLvlSlider;
    public ToggleGroup m_MultToggle;

    public void DisplayInfo( Building building )
    {
        m_InfoPanel.SetActive( true );
        if ( building is MineShaft )
        {
            MineShaft mineShaft = (MineShaft)building;
            m_HeadText.text = "Mine Shaft " + mineShaft.MineId + " Level " + mineShaft.Level;
            Debug.Log( "Mineshaft info: " + mineShaft.MineId );
        }
        else if ( building is Elevator )
        {
            Elevator elevator = (Elevator)building;
            m_HeadText.text = "Elevator Level " + elevator.Level;
            Debug.Log( "Elevator info" );
        }
    }

    public void HideInfo()
    {
        m_InfoPanel.SetActive( false );
    }

    public void OnToggleChange( float amount )
    {
        if ( amount > m_NextLvlSlider.maxValue )
            amount = m_NextLvlSlider.maxValue;
        m_NextLvlSlider.value = amount;
    }
}
