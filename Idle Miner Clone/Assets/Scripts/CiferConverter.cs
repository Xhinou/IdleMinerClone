using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiferConverter : MonoBehaviour
{
    public static string ConvertToString( float value )
    {

        int scale = 0;
        float f = value;
        float floor = 1000.0f;
        while ( f >= floor )
        {
            f /= floor;
            scale++;
        }

        return f.ToString( "#.0" ) + GetSuffix( scale );
    }

    private static string GetSuffix( int scale )
    {
        string suffix = "";
        switch( scale )
        {
            case 0:
                suffix = "";
                break;
            case 1:
                suffix = "K";
                break;
            case 2:
                suffix = "M";
                break;
            case 3:
                suffix = "B";
                break;
            case 4:
                suffix = "T";
                break;
            default:
                suffix = "big";
                break;
        }
        return suffix;
    }
}