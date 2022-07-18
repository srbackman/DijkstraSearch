using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraData
{
    public Transform _thisNodeTransform;
    public bool _isChecked;
    public float _shortestDistanceValue;
    public Transform _shortestDistanceNodeTransform;
}

public class RoutePlotter : MonoBehaviour
{
    private ClassLibrary lib;

    private void Awake()
    {
        lib = FindObjectOfType<ClassLibrary>();
    }

    public List<Transform> GetRoute(Transform entity, Transform target)
    {
        /*Find closest node available*/
        Transform start = FindClosestNode(entity);

        /*Dijkstra (or a modified version?) greedy search.*/
        DijkstraData[] dijkstraDatas = DijkstraSearch(start);

        foreach (DijkstraData d in dijkstraDatas)
        {
            print(d._thisNodeTransform.name + " node has shortest to: " + d._shortestDistanceNodeTransform.name + ": " + d._shortestDistanceValue);
        }

        /*Validate DijkstraData.*/
        if (!ValidateDijkstraData(dijkstraDatas)) Debug.LogError("Validation failed!");

        /*Get shortest path by looking at Dijkstra search result.*/
        List<Transform> route = PlanRoute(dijkstraDatas, start, target);

        return (route);
    }

    private Transform FindClosestNode(Transform entity)
    {
        Transform closestNode = null;
        float closestDistance = float.MaxValue;
        foreach (Transform t in lib.navigationNodes._nodeTransforms)
        {
            float distance = Vector3.Distance(t.position, entity.position);
            if (closestDistance > distance)
            {
                closestNode = t;
                closestDistance = distance;
            }    
        }
        return (closestNode);
    }

    private bool ValidateDijkstraData(DijkstraData[] dijkstraDatas)
    {
        bool check = true;

        foreach (DijkstraData data in dijkstraDatas)
        {
            if (!data._thisNodeTransform)
            {
                Debug.LogError("Origin not fully complete.");
                check = false;
            }
            if (!data._shortestDistanceNodeTransform)
            {
                Debug.LogError(data._thisNodeTransform.name + ": _shortestDistanceNodeTransform is null.");
                check = false;
            }
            if (data._shortestDistanceValue == float.MaxValue)
            {
                Debug.LogError(data._thisNodeTransform.name + ": was never visited correctly.");
                check = false;
            }
            if (!data._isChecked)
            {
                Debug.LogError(data._thisNodeTransform.name + ": was not checked.");
                check = false;
            }
        }
        return check;
    }

    private DijkstraData[] DijkstraSearch(Transform start)
    {
        /* Get NavigationNodes from: lib.navigationNodes. */
        if (!start) Debug.LogError("start is null.");

        //Transform previousNode = null;
        int nodesChecked = 0;
        Transform currentNode = start;
        NavigationNodes navNodes = lib.navigationNodes;
        DijkstraData[] dijkstraDatas = new DijkstraData[navNodes._nodeTransforms.Length];

        for (int i = 0; i < dijkstraDatas.Length; i++)
        {
            dijkstraDatas[i] = new DijkstraData();
            dijkstraDatas[i]._thisNodeTransform = navNodes._nodeTransforms[i];
            dijkstraDatas[i]._isChecked = false;
            dijkstraDatas[i]._shortestDistanceValue = float.MaxValue;
            dijkstraDatas[i]._shortestDistanceNodeTransform = null;
        }
        /*Set start node min distance to 0 and best node to it self.*/
        DijkstraData starterData = FindDijkstraDataSlot(currentNode, dijkstraDatas);
        starterData._shortestDistanceValue = 0;
        starterData._shortestDistanceNodeTransform = currentNode;

        while (currentNode)
        {
            print("Scanning node: " + currentNode.name);
            Transform closestNodeTransform = null;
            float closestNodeValue = float.MaxValue;
            NodeRoutes nodeRoutes = currentNode.GetComponent<NodeRoutes>();
            DijkstraData currentDijkstraData = FindDijkstraDataSlot(currentNode, dijkstraDatas);

            /*Check all the possible routes.*/
            foreach (Transform t in nodeRoutes._routes)
            {
                DijkstraData dijkstraData = FindDijkstraDataSlot(t, dijkstraDatas);

                //if (t == previousNode) continue;
                float distance = Vector3.Distance(currentNode.position, t.position) + currentDijkstraData._shortestDistanceValue;

                if (distance < closestNodeValue)
                {
                    closestNodeValue = distance;
                    if (!dijkstraData._isChecked)
                        closestNodeTransform = t;
                }

                print(currentNode.name + " distance to: " + t.name + ", is: " + distance + " and current best distance in: " + t.name + ", is: " + dijkstraData._shortestDistanceValue);
                if (dijkstraData._shortestDistanceValue > distance)
                {
                    print("Saved");
                    dijkstraData._shortestDistanceValue = distance;
                    dijkstraData._shortestDistanceNodeTransform = currentNode;
                }
            }
            /*Mark currentNode isChecked as True.*/
            currentDijkstraData._isChecked = true;
            nodesChecked++; /*To skip the last null check that happens right after this line.*/

            /*If closestNodeTransform is null, check if there is still an unexplored node.*/
            if (!closestNodeTransform && nodesChecked < dijkstraDatas.Length)
            {
                foreach (DijkstraData dijData in dijkstraDatas)
                {
                    if (!dijData._isChecked && dijData._shortestDistanceNodeTransform)
                    {
                        closestNodeTransform = dijData._thisNodeTransform;
                        break;
                    }
                }
            }
            //previousNode = currentNode;
            currentNode = closestNodeTransform;
        }
        return (dijkstraDatas);
    }

    private List<Transform> PlanRoute(DijkstraData[] dijkstraDatas, Transform start, Transform target)
    {
        List<Transform> route = new List<Transform>();
        Transform currentNode = target;
        while(currentNode != start)
        {
            route.Insert(0, currentNode);
            DijkstraData dijkstraData = FindDijkstraDataSlot(currentNode, dijkstraDatas);
            currentNode = dijkstraData._shortestDistanceNodeTransform;
        }
        return (route);
    }

    private DijkstraData FindDijkstraDataSlot(Transform target, DijkstraData[] dijkstraDatas)
    {
        for (int i = 0; i < dijkstraDatas.Length; i++)
        {
            if (dijkstraDatas[i]._thisNodeTransform == target)
            {
                return (dijkstraDatas[i]);
            }
        }
        return (null);
    }
}
