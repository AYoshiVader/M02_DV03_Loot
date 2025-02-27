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

    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;

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
        InventoryList<string> inventoryList = new InventoryList<string>();
        inventoryList.SetItem("Potion");
        UnityEngine.Debug.Log(inventoryList.item);
    }

    public void Initialize()
    {
        _state = "Manager initialized..";
        _state.FancyDebug();
        debug(_state);
        LogWithDelegate(debug);
        GameObject player = GameObject.Find("Player");
        PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
        playerBehaviour.playerJump += HandlePlayerJump;
        lootStack.Push("Sword of Doom");
        lootStack.Push("HP+");
        lootStack.Push("Golden Key");
        lootStack.Push("Winged Boot");
        lootStack.Push("Mythril Bracers");
    }

    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }

    public static void Print(string newText)
    {
        UnityEngine.Debug.Log(newText);
    }

    public void LogWithDelegate(DebugDelegate del)
    {
        del("Delegating the debug task...");
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
                try
                {
                    Utilities.RestartLevel(-1);
                    debug("Level restarted successfully...");
                }
                catch (System.ArgumentException e)
                {
                    Utilities.RestartLevel(0);
                    debug("Reverting to scene 0: " + e.ToString());
                }
                finally
                {
                    debug("Restart handled...");
                }
            }
        }
    }

    public void PrintLootReport()
    {
        string currentItem;
        if (!(lootStack.TryPop(out currentItem)))
        {
            UnityEngine.Debug.LogFormat("No items in Stack");
            return;
        }
        string nextItem;
        if (lootStack.TryPeek(out nextItem))
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
