using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFinder : MonoBehaviour
{
    public static GameObject GetChildWithTag( Transform parent, string childTag )
    {
        for ( int i = 0; i < parent.childCount; ++i )
        {
            Transform child = parent.GetChild( i );
            if ( child.tag == childTag )
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public static List<GameObject> GetChildsWithTag( Transform parent, string childTag )
    {
        List<GameObject> childs = new List<GameObject>();
        for ( int i = 0; i < parent.childCount; ++i )
        {
            Transform child = parent.GetChild( i );
            if ( child.tag == childTag )
            {
                childs.Add( child.gameObject );
            }
        }
        return childs;
    }
}
