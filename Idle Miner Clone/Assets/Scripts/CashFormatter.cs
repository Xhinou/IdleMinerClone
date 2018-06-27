﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashFormatter : MonoBehaviour
{
    public static string FormatToString( float value )
    {

        int scale = 0;
        float f = value;
        float floor = 1000.0f;
        while ( f >= floor )
        {
            f /= floor;
            scale++;
        }

        return f.ToString( "0.#" ) + GetSuffix( scale );
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
                suffix = CreateDoubleAlphaSuffix( scale );
                break;
        }
        return suffix;
    }

    private static string CreateDoubleAlphaSuffix( int scale )
    {
        int baseScale = 5;
        int maxScale = 50;

        scale -= baseScale;
        if ( scale > maxScale )
        {
            return "BIG";
        }

        string doubleAlpha = "aa";
        for (int i = 0; i < scale; ++i )
        {
            char nextLetter = (char)( doubleAlpha[1] + 1 );
            doubleAlpha = doubleAlpha.Remove( 1 );
            if ( !System.Char.IsLetter( nextLetter ) )
            {
                char firstLetter = (char)( doubleAlpha[1] + 1 );
                nextLetter = 'a';
                doubleAlpha = "" + firstLetter + nextLetter;
            }
            else
            {
                doubleAlpha += nextLetter;
            }
        }
        return doubleAlpha;
    }
}