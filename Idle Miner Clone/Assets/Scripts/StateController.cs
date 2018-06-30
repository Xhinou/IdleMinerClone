using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public delegate void State();
    private State m_State;

	private void Start ()
	{
        m_State = null;
	}
	
	public void UpdateState()
	{
        m_State();
	}

    public void SetState( State state )
    {
        m_State = state;
    }
}
