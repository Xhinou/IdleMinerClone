using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public float m_PlayerGold;
    public InfoManager infoManager;
    public Elevator m_Elevator;
    public GameObject m_MineShaftPrefab;
    public RectTransform m_GamePanel;
    public RectTransform m_NewMineRect;
    public Text m_MineShaftCostText;
    private SortedList<int, MineShaft> m_MineShafts;
    public Vector2 v;

	private void Start ()
	{
        m_MineShafts = new SortedList<int, MineShaft>();
        m_MineShaftCostText.text = "New Shaft\n" + CiferConverter.ConvertToString( GetMineShaftCost() );
    }
	
	private void Update ()
	{
		
	}

    public void AddMineShaft()
    {
        int mineId = GetNrMineShafts() + 1;

        GameObject newMine = Instantiate( m_MineShaftPrefab, m_NewMineRect.position, Quaternion.identity, m_GamePanel );
        m_MineShafts.Add( mineId, MineShaft.CreateComponent( newMine, mineId ) );
        Button infoButton = newMine.transform.Find("Button_Info").GetComponent<Button>();
        infoButton.onClick.AddListener( delegate { infoManager.DisplayInfo( m_MineShafts[mineId] ); } );
        Text lvlText = infoButton.transform.GetComponentInChildren<Text>();
        lvlText.text = "Level\n" + 1;
        m_NewMineRect.Translate( new Vector2( 0, -newMine.GetComponent<RectTransform>().rect.height ) );
        //m_Elevator.m_ElevatorArea.sizeDelta = new Vector2( m_Elevator.m_ElevatorArea.sizeDelta.x, m_Elevator.m_ElevatorArea.sizeDelta.y + m_NewMineRect.sizeDelta.y );
        v = m_Elevator.m_ElevatorArea.offsetMin;
        //m_Elevator.m_ElevatorArea.offsetMin = new Vector2( m_Elevator.m_ElevatorArea.offsetMin.x, m_NewMineRect.offsetMin.y );
        // m_NewMineRect.offsetMin;

        const int maxMineShafts = 15;
        if ( mineId >= maxMineShafts )
        {
            m_NewMineRect.gameObject.SetActive( false );
        }
        else
        {
            Debug.Log( "Nr of Shafts: " + GetNrMineShafts() );
            m_MineShaftCostText.text = "New Shaft\n" + CiferConverter.ConvertToString( GetMineShaftCost() );
        }
    }

    public int GetNrMineShafts()
    {
        return m_MineShafts.Count;
    }

    public MineShaft GetMineShaftByID( int id )
    {
        return m_MineShafts[id];
    }

    private float GetMineShaftCost()
    {
        int nrMineShaft = GetNrMineShafts();
        float multiplier = Mathf.Pow(2, nrMineShaft) * Mathf.Pow( 10.0f, nrMineShaft + 2 );
        return multiplier;
    }
}