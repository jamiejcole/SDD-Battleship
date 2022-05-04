using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // This Ship class allows individual Ship objects to be created which
    // encapsulate data about any given ship on the board itself.
    [Serializable]
    public class Ship
    {
        public int tileNum;
        public int Length;
        public bool isDefault;
        public Dictionary<int, bool> hitDict;

        public Ship(int prTileNum, int prLength, bool prIsDefault)
        {
            tileNum = prTileNum;
            Length = prLength;
            isDefault = prIsDefault;

            hitDict = new Dictionary<int, bool>();

            for (int i = 0; i < Length; i++)
            {
                hitDict.Add(tileNum + i, false);
            }
        }
    }

    // Each Player object consists of five Ship objects. These ship objects
    // allow the Player object to contain all of the data created by the game
    // once the user has placed their ships down in the pre-game lobby.
    [Serializable]
    public class Player
    {
        public Ship Ship_2_01;
        public Ship Ship_3_01;
        public Ship Ship_3_02;
        public Ship Ship_4_01;
        public Ship Ship_5_01;

        public Player(Ship pr_2_01, Ship pr_3_01, Ship pr_3_02, Ship pr_4_01, Ship pr_5_01)
        {
            Ship_2_01 = pr_2_01;
            Ship_3_01 = pr_3_01;
            Ship_3_02 = pr_3_02;
            Ship_4_01 = pr_4_01;
            Ship_5_01 = pr_5_01;
        }
    }

    public Ship Ship_2_01;
    public Ship Ship_3_01;
    public Ship Ship_3_02;
    public Ship Ship_4_01;
    public Ship Ship_5_01;

    public Player playerOne;
    public Player playerTwo;

    public GameObject messagePopupPrefab;


    // Singleton Implementation of GameManager:
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void GeneratePlayer(Dictionary<string, (int, bool)> ShipStartPositions, bool firstPlayer)
    {
        // Generates Ship objects for each position in the dict

        (int, bool) x;

        ShipStartPositions.TryGetValue("Ship_2_01", out x);
        Ship_2_01 = new Ship(x.Item1, 2, x.Item2);

        ShipStartPositions.TryGetValue("Ship_3_01", out x);
        Ship_3_01 = new Ship(x.Item1, 3, x.Item2);

        ShipStartPositions.TryGetValue("Ship_3_02", out x);
        Ship_3_02 = new Ship(x.Item1, 3, x.Item2);

        ShipStartPositions.TryGetValue("Ship_4_01", out x);
        Ship_4_01 = new Ship(x.Item1, 4, x.Item2);

        ShipStartPositions.TryGetValue("Ship_5_01", out x);
        Ship_5_01 = new Ship(x.Item1, 5, x.Item2);

        if (firstPlayer)
        {
            playerOne = new Player(Ship_2_01, Ship_3_01, Ship_3_02, Ship_4_01, Ship_5_01);
            CreatePopup("Loading Player 2 ship selection...");
            StartCoroutine(LoadSceneAfterSeconds("PlayerTwoSelection", 3f));
        }
        else
        {
            playerTwo = new Player(Ship_2_01, Ship_3_01, Ship_3_02, Ship_4_01, Ship_5_01);
        }
    }

    public IEnumerator DeleteObjectAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }

    IEnumerator LoadSceneAfterSeconds(string scene, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }

    public void CreatePopup(string text)
    {
        GameObject popup = Instantiate(messagePopupPrefab, GameObject.Find("Canvas").transform);
        popup.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = text;
        StartCoroutine(DeleteObjectAfterSeconds(popup, 3f));
    }

    public static void OnHitEvent(int tileNum)
    {
        // look through the player class to find all ship objects
        // for each ship object, find all 
    }

    public int GetLengthOfShip(string name)
    {
        string x;
        x = name.Remove(0, 5).Remove(1, 3);
        return Int32.Parse(x);
    }

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }

    public void DumpDictToConsole(Dictionary<int, bool> dict)
    {
        foreach (KeyValuePair<int, bool> x in dict)
        {
            Debug.Log($"KVP: {x.Key}, {x.Value}");
        }
    }
}
