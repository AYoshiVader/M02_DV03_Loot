using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

using CustomExtensions;

public class GameBehaviour : MonoBehaviour, IManager
{
    private string _state;
    public Stack<string> lootStack = new Stack<string>();

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public string labelText = "Collect all 4 items and win your freedom!";
    public int maxItems = 4;
    public bool showWinScreen = false;
    public bool showLossScreen = false;

    private int _itemsCollected = 0;
    public int Items
    {
        get { return _itemsCollected; }

        set
        {
            _itemsCollected = value;
            if (_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                endGame(true);
            }
            else
            {
                labelText = "Item found, only " + (maxItems - _itemsCollected) + " more to go!";
            }
            UnityEngine.Debug.LogFormat("Items: {0}", _itemsCollected);
        }
    }

    private int _playerHP = 10;
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            UnityEngine.Debug.LogFormat("Lives: {0}", _playerHP);
            if (_playerHP <= 0)
            {
                labelText = "You want another life with that?";
                endGame(false);
            }
            else
            {
                labelText = "Ouch... that's got hurt.";
            }
        }
    }

    void endGame(bool winLose)
    {
        if (winLose)
        {
            showWinScreen = true;
        } else
        {
            showLossScreen = true;
        }
        Time.timeScale = 0f;
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _state = "Manager initialized..";
        _state.FancyDebug();
        UnityEngine.Debug.Log(_state);
        lootStack.Push("Sword of Doom");
        lootStack.Push("HP+");
        lootStack.Push("Golden Key");
        lootStack.Push("Winged Boot");
        lootStack.Push("Mythril Bracers");
    }

    void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health:" + _playerHP);
        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected: " + _itemsCollected);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);
        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU WON!"))
            {
                Utilities.RestartLevel(0);
            }
        }
        if (showLossScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "You lose..."))
            {
                Utilities.RestartLevel();
            }
        }
    }

    public void PrintLootReport()
    {
        var currentItem;
        if (lootStack.TryPop(out currentItem))
        {
            UnityEngine.Debug.LogFormat("No items in Stack");
            break;
        }
        var nextItem;
        if (lootStack.TryPeek())
        {
            UnityEngine.Debug.LogFormat("You got a {0}! You've got a good chance of finding a {1} next!", currentItem, nextItem);
        }
        else
        {
            UnityEngine.Debug.LogFormat("You got a {0}!", currentItem);
        }
        UnityEngine.Debug.LogFormat("There are {0} random loot items waiting for you!", lootStack.Count);
    }
}
