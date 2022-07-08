using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[System.Serializable]
public class RobotData
{
    public GameObject _robotObject;
    //public Button _robotSelectButton;
}

public class NavigationOrderBar : MonoBehaviour
{
    [SerializeField] private GameObject _contentArea;

    private NavigationNodes navigationNodes;
    private bool _oneTimeActivation = true;

    public GameObject _nodeButtonPrefab;
    [Space]
    [SerializeField] private List<RobotData> _robotDatas = new List<RobotData>();

    private void Awake()
    {
        navigationNodes = FindObjectOfType<NavigationNodes>();
    }

    void Update()
    {
        if (_oneTimeActivation) UpdateButtonList();
    }

    public void UpdateButtonList()
    {
        if (navigationNodes._nodeTransforms.Length == 0) return;

        foreach (Transform t in navigationNodes._nodeTransforms)
        {
            if (!t) return;
            GameObject nButton = Instantiate(_nodeButtonPrefab, _contentArea.transform);
            nButton.GetComponent<CallEveryone>()._assignedNode = t.gameObject;
            nButton.GetComponentInChildren<TMP_Text>().text = t.name;
        }
        _oneTimeActivation = false;
    }
}
