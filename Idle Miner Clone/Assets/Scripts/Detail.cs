﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detail : MonoBehaviour
{
    public Text m_NameText;
    public Text m_ValueText;
    public Text m_PredictText;

    public void SetDetailTexts( string name, double value, double predict, string extra = "" )
    {
        SetNameText( name );
        SetValueText( value, extra );
        SetPredictText( predict );
    }

    private void SetNameText( string name )
    {
        m_NameText.text = name;
    }

    private void SetValueText( double value, string extra = "" )
    {
        m_ValueText.text = CashUtility.FormatToString( value ) + extra;
    }

    private void SetPredictText( double predict )
    {
        m_PredictText.text = "+" + CashUtility.FormatToString( predict );
    }
}
