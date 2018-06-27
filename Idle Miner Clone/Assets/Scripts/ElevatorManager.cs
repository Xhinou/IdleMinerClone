using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorManager : MonoBehaviour
{
    public Elevator elevator;
    public BuildingInfo infoManager;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener( delegate { infoManager.DisplayInfo( elevator ); } );
       // button.onClick.AddListener( delegate( info))
    }
}