using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationNodes : MonoBehaviour
{
    [SerializeField] bool _nodesHaveChildren = true;
    [SerializeField] int _numberOfChildObjects = 3;
    
    public Transform[] _nodeTransforms;

    private void Awake()
    {
        _nodeTransforms = GetComponentsInChildren<Transform>();
        int originalLenght = _nodeTransforms.Length;

        if (_nodesHaveChildren && _numberOfChildObjects > 0)
        {
            Transform[] withoutParent = new Transform[(originalLenght - 1) / _numberOfChildObjects];
            for (int i = 1; i - 1 < (originalLenght - 1) / 3; i++)
            {
                withoutParent[i - 1] = _nodeTransforms[(i * _numberOfChildObjects) - 2];
            }
            _nodeTransforms = withoutParent;
        }
        else
        {
            Transform[] withoutParent = new Transform[(originalLenght - 1) / _numberOfChildObjects];
            for (int i = 1; i < (originalLenght - 1); i++)
            {
                withoutParent[i - 1] = _nodeTransforms[i];
            }
            _nodeTransforms = withoutParent;
        }
    }
}
