using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDumbMover : MonoBehaviour
{
    private ClassLibrary lib;
    [SerializeField] private float _entitySpeed = 2f;
    [SerializeField] private float _entityNodeDistanceTreshold = 1f;

    [HideInInspector] public List<Transform> _navigationNodeList = new List<Transform>();

    private void Awake()
    {
        lib = FindObjectOfType<ClassLibrary>();
    }

    void Update()
    {
        if (_navigationNodeList.Count > 0)
        {
            NodeManager();
        }
    }

    public void GetNavigationNodes(Transform target)
    {
        _navigationNodeList = lib.routePlotter.GetRoute(transform, target);

        for (int i = 0; i < _navigationNodeList.Count; i++)
        {
            print(transform.name + ", To: " + _navigationNodeList[i]);
        }
        print("moving");
    }

    private void NodeManager() /*Node order and handling.*/
    {
        Move(_navigationNodeList[0]);
        CheckIfAtDestination();
    }

    private void Move(Transform currentTarget)
    {
        Vector3 velocity;
        Vector3 allDirections;
        allDirections = new Vector3((currentTarget.position.x - transform.position.x), 0f, (currentTarget.position.z - transform.position.z)).normalized;
        /*Set velocity.*/
        velocity = allDirections * _entitySpeed;
        transform.position += (velocity * Time.deltaTime); /*Move.*/
    }

    private void CheckIfAtDestination()
    {
        float distance = Vector3.Distance(transform.position, _navigationNodeList[0].position);

        if (_entityNodeDistanceTreshold >= distance)
        {
            print("clear: " + _navigationNodeList[0]);
            _navigationNodeList.RemoveAt(0);
        }
    }
}
