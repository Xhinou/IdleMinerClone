using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
   // public NameList firstNames;
    //public NameList surnames;

    public new string name;

    public Manager()
    {
        name = GenerateName();
    }

    private string GenerateName()
    {
        return "";
    }
}
