using System.Collections;
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

    public void SetNameText( string name )
    {
        m_NameText.text = name;
    }

    public void SetValueText( double value, string extra = "" )
    {
        m_ValueText.text = CashFormatter.FormatToString( value ) + extra;
    }

    public void SetPredictText( double predict )
    {
        m_PredictText.text = "+" + CashFormatter.FormatToString( predict );
    }
}
