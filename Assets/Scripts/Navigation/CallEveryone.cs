using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEveryone : MonoBehaviour
{
    public GameObject _assignedNode;
    private ClassLibrary lib;

    private void Awake()
    {
        lib = FindObjectOfType<ClassLibrary>();
    }
    
    public void Activate()
    {
        print(_assignedNode.name + ": was pressed.");
        NavigationNodes navNodes = lib.navigationNodes;
        lib.botHandler.MoveSelected(_assignedNode.transform);
    }
}
