using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BotHandler : MonoBehaviour
{
    /*This handles selecting all bots and gives orders to them.*/

    [SerializeField] private List<BotSelect> _bots = new List<BotSelect>();
    [HideInInspector] public bool _universalBotButtonState = false;
    [SerializeField] private Button _selectAllButton;
    private Color _selectedColor = Color.green;
    private Color _notSelectedColor = Color.grey;

    private void Awake()
    {
        _selectAllButton.onClick.AddListener(() => SelectAll());
        _selectAllButton.image.color = _universalBotButtonState ? _selectedColor : _notSelectedColor;
    }

    public void RegisterBot(BotSelect botSelect)
    {
        _bots.Add(botSelect);
    }

    public void MoveSelected(Transform target)
    {
        foreach (BotSelect bot in _bots)
        {
            if (!bot._buttonToggleState) continue;
            bot.entityDumbMover.GetNavigationNodes(target);
        }
    }

    public void SelectAll()
    {
        _universalBotButtonState = !_universalBotButtonState;
        foreach (BotSelect bot in _bots)
        {
            bot.SelectToggle(false);
        }
        _selectAllButton.image.color = _universalBotButtonState ? _selectedColor : _notSelectedColor;
    }
}
