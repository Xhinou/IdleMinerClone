using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    SortedList<int, MineShaft> mines = new SortedList<int, MineShaft>();
    public RectTransform gamePanel;
    public RectTransform newMineButton;
    public GameObject mine;

    public void AddMineShaft()
    {
        int nrMineShafts = mines.Count;
        mines.Add( nrMineShafts + 1, new MineShaft( nrMineShafts ) );
        Instantiate( mine, newMineButton.position, Quaternion.identity, gamePanel );
        newMineButton.Translate( new Vector2( 0, -newMineButton.rect.height ) );

        const int maxMineShafts = 15;
        if ( nrMineShafts + 1 >= maxMineShafts )
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
}
