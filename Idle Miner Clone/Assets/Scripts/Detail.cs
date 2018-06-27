using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detail : MonoBehaviour
{
    public Text m_NameText;
    public Text m_ValueText;
    public Text m_PredictText;
    public delegate float GetCurValue( int lvl );
    public delegate float GetPredictValue( int predict );
    public GetCurValue CurValueHandler;
    public GetPredictValue PredictValueHandler;
    public float m_CurValue;
    public float m_PredictValue;

    //public void SetCurValue( int lvl )
    //{
    //    CurValueHandler( lvl );
    //}

    //public void SetPredictValue( int predict )
    //{
    //    PredictValueHandler( predict );
    //}
}
