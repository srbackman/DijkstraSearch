using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BotSelect : MonoBehaviour
{
    /*Put this to the bot.*/
    private ClassLibrary lib;
    [HideInInspector] public EntityDumbMover entityDumbMover;
    [SerializeField] private Button _botButton;

    private Color _selectedColor = Color.green;
    private Color _notSelectedColor = Color.grey;
    [HideInInspector] public bool _buttonToggleState = false;

    private void Awake()
    {
        lib = FindObjectOfType<ClassLibrary>();
        entityDumbMover = GetComponent<EntityDumbMover>();
        _botButton.onClick.AddListener(() => SelectToggle(true));
        _botButton.image.color = _buttonToggleState ? _selectedColor : _notSelectedColor;

        lib.botHandler.RegisterBot(this);
    }

    public void SelectToggle(bool toggleCall)
    {
        if (toggleCall) _buttonToggleState = !_buttonToggleState;
        else _buttonToggleState = lib.botHandler._universalBotButtonState;
        _botButton.image.color = _buttonToggleState ? _selectedColor : _notSelectedColor;
    }
}
