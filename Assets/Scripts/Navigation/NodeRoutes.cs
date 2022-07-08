using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRoutes : MonoBehaviour
{
    public List<Transform> _routes = new List<Transform>();

    private void OnDrawGizmos()
    {
        foreach(Transform t in _routes)
        {
            if (!t) continue;

            Vector3 direction = transform.position + ((t.position - transform.position).normalized * 5);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, direction);
        }
    }
}
