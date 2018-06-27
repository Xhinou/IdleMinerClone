using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float m_PlayerCash;
    public Text m_CashText;
    public Text m_IdleCashText;
    public BuildingInfo infoBuilding;
    public Elevator m_Elevator;
    public GameObject m_MineShaftPrefab;
    public Canvas m_Viewport;
    public RectTransform m_GamePanel;
    public RectTransform m_NewMineRect;
    public Text m_MineShaftCostText;
    private SortedList<int, MineShaft> m_MineShafts;

	private void Start ()
	{
        float playerInitCash = 100.0f;
        m_PlayerCash = playerInitCash;
        m_MineShafts = new SortedList<int, MineShaft>();
        m_CashText.text = CashFormatter.FormatToString( m_PlayerCash );
        m_MineShaftCostText.text = "New Shaft\n" + CashFormatter.FormatToString( GetMineShaftCost() );
    }
    private string big = "aa";
	private void Update ()
	{
		if ( Input.GetKeyDown( KeyCode.C ) == true )
        {
            m_PlayerCash *= 2;
            m_CashText.text = CashFormatter.FormatToString( m_PlayerCash );
        }
        if ( Input.GetKeyDown( KeyCode.T ) == true )
        {
            Debug.Log( big );
            char nextLetter = (char)(big[big.Length - 1] + 1);
            big = big.Remove( big.Length - 1 );
            if ( !System.Char.IsLetter( nextLetter ) )
            {
                char firstLetter = (char)( big[big.Length - 1] + 1 );
                nextLetter = 'a';
                big = "";
                big += firstLetter;
                big += nextLetter;
            }
            else
            {
                big += nextLetter;
            }
        }
    }

    public void AddMineShaft()
    {
        int mineId = GetNrMineShafts() + 1;

        GameObject newMine = Instantiate( m_MineShaftPrefab, m_NewMineRect.position, Quaternion.identity, m_GamePanel );
        m_MineShafts.Add( mineId, MineShaft.CreateComponent( newMine, mineId ) );
        Button infoButton = GameObjectFinder.GetChildWithTag(newMine.transform, "infoButton" ).GetComponent<Button>();
        infoButton.onClick.AddListener( delegate { infoBuilding.DisplayInfo( m_MineShafts[mineId] ); } );
        Text lvlText = infoButton.transform.GetComponentInChildren<Text>();
        lvlText.text = "Level\n" + 1;
        float yOffset = newMine.GetComponent<RectTransform>().rect.height;
        m_NewMineRect.transform.Translate( new Vector2( 0, -yOffset * m_Viewport.scaleFactor ) );
        RectTransform elevatorRect = m_Elevator.m_ElevatorArea.GetComponent<RectTransform>();
        Debug.Log( elevatorRect.offsetMin.y );
        elevatorRect.offsetMin = new Vector2( elevatorRect.offsetMin.x, elevatorRect.offsetMin.y - yOffset );

        const int maxMineShafts = 17;
        if ( mineId >= maxMineShafts )
        {
            m_NewMineRect.gameObject.SetActive( false );
        }
        else
        {
            m_MineShaftCostText.text = "New Shaft\n" + CashFormatter.FormatToString( GetMineShaftCost() );
            if (m_PlayerCash < GetMineShaftCost() )
            {
                m_NewMineRect.GetComponent<Button>().interactable = false;
            }
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
        int powBase = 2;

        return Mathf.Pow( powBase, nrMineShaft) * Mathf.Pow( 10.0f, nrMineShaft + powBase );
    }
}