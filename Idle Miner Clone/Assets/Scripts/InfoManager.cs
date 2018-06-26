using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
   // public Text panelText;
   // public int buildingLvl;

    public void DisplayInfo( Building building )
    {
        //MineShaft ms = info as MineShaft;
        if ( building is MineShaft )
        {
            MineShaft mineShaft = (MineShaft)building;
            Debug.Log( "Mineshaft info: " + mineShaft.MineId );
        }
        else if ( building is Elevator )
        {
            Debug.Log( "Elevator info" );
        }
    }

    public void HideInfo()
    {

    }
}
