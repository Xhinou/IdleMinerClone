using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineShaftManager : MonoBehaviour
{
    SortedList<int, GameObject> mines = new SortedList<int, GameObject>();
    public RectTransform gamePanel;
    public RectTransform newMineButton;
    public GameObject mine;
    public InfoManager infoManager;

    public void AddMineShaft()
    {
        int mineId = mines.Count + 1;

      //  GameObject newMine = Instantiate( mine, newMineButton.position, Quaternion.identity, gamePanel );
      //  mines.Add( mineId, newMine );
       // Button infoButton = newMine.transform.Find("Button_Info").GetComponent<Button>();
      //  infoButton.onClick.AddListener( delegate { infoManager.DisplayInfo( mines[mineId] ); } );
      //  newMineButton.Translate( new Vector2( 0, -newMine.GetComponent<RectTransform>().rect.height ) );

        const int maxMineShafts = 15;
        if ( mineId >= maxMineShafts )
        {
            newMineButton.gameObject.SetActive( false );
        }
    }

    private int GetNrMineShafts()
    {
        int nrMineShafts = mines.Count;
        Debug.Log( "Nr of Mines:" + nrMineShafts );
        return nrMineShafts;
    }

    public void DisplayInfo()
    {
        Debug.Log( "Display info" );
    }

    public void DisplayInfo( int id )
    {
        Debug.Log( "Display info: " + id );
    }
}
