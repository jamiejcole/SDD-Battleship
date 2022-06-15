using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;
using System.Linq;

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
        public bool hasSunk;
        public Dictionary<int, bool> hitDict;

        public Ship(int prTileNum, int prLength, bool prIsDefault)
        {
            tileNum = prTileNum;
            Length = prLength;
            isDefault = prIsDefault;
            hasSunk = false;

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
    public GameObject missileObjPrefab;
    public GameObject hitPrefab;
    public GameObject missPrefab;

    GameObject[] playerOneObjs;
    GameObject[] playerTwoObjs;
    public string currentViewer;
    public string publicCurrentViewer;

    public ComponentManager componentManager;
    public SelectionManager selectionManager;

    public bool radarButtonEnabled = false;
    public bool setupMenuItemsEnabled = true;

    public bool inRadarMode = false;
    public bool inSetupMode = true;
    public GameObject bombSelectorPref;

    List<GameObject> playerOneShotObjects = new List<GameObject>();
    List<GameObject> playerTwoShotObjects = new List<GameObject>();

    public GameObject whiteObjectPrefab;
    List<GameObject> playerOneWhiteObjects = new List<GameObject>();
    List<GameObject> playerTwoWhiteObjects = new List<GameObject>();

    public Material greyedOutMaterial;

    public string playerOneUsername;
    public string playerTwoUsername;

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
        publicCurrentViewer = "PlayerOne";
        DontDestroyOnLoad(this.gameObject);
        componentManager = GameObject.Find("ComponentManager").GetComponent<ComponentManager>();
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        componentManager = GameObject.Find("ComponentManager").GetComponent<ComponentManager>();
        callCompManReloadSb();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetUsernames(string one, string two)
    {
        playerOneUsername = one;
        playerTwoUsername = two;
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

            playerOneObjs = GameObject.FindGameObjectsWithTag("PlayerOneVisible");

            StartCoroutine(LoadSceneAfterSeconds("PlayerTwoSelection", 0.1f));
            SwapVisibility("PlayerTwo");
        }
        else
        {
            // Checking to see whether the playerTwo Player instance has been created or not
            if (playerTwo.Ship_2_01.Length == 0)
            {
                playerTwo = new Player(Ship_2_01, Ship_3_01, Ship_3_02, Ship_4_01, Ship_5_01);
                CreatePopup("Player Positions saved! Loading Game...");
                playerTwoObjs = GameObject.FindGameObjectsWithTag("PlayerTwoVisible");
                StartCoroutine(LoadSceneAfterSeconds("PlayerOneSelection", 1f));
                StartCoroutine(SwapVisibilityAfterSeconds("PlayerOne", 1f));

                // TODO: Need to fix the issue with ships being disclosed before switching scenes.

                // Changing menu items visiblity
                setupMenuItemsEnabled = false;
                radarButtonEnabled = true;
                inSetupMode = false;
                //componentManager.ToggleSetupMenuItems();
                //componentManager.ToggleRadar(true);
            }
            else
            {
                // TODO: deactivate the confirm button! + same for P1!
            }
        }
    }

    public void LaunchMissile(int tileNum)
    {
        // instantiate the missile obj
        Vector3 tilePos = selectionManager.p2tiles[tileNum].transform.position;
        Vector3 missileSpawnPos = new Vector3(tilePos.x + 0.5f, tilePos.y + 13, tilePos.z + 0.5f);
        GameObject missileObj = GameObject.Instantiate(missileObjPrefab, missileSpawnPos, Quaternion.identity);

        StartCoroutine(DeleteObjectAfterSeconds(missileObj, 2f));

        // Determine whether it's a hit or miss
        string currentPlayer = GetCurrentPlayer();
        string lCurrentPlayer;
        if (currentPlayer == "PlayerOne") { lCurrentPlayer = "playerOne"; }
        else if (currentPlayer == "PlayerTwo") { lCurrentPlayer = "playerTwo"; }

        string lOtherPlayer;
        if (currentPlayer == "PlayerOne") { lOtherPlayer = "playerTwo"; }
        else { lOtherPlayer = "playerOne"; }

        Player currentPlayerObj = GetCurrentPlayerObj();
        Player otherPlayerObj = GetOtherPlayerObj();

        bool hit = false;

        // Loop through each ship in playerTwo Object, and then loop through each occupied 
        // tile within each Ship object to see if the tile matches the tileNum of the missile
        foreach (FieldInfo prop in otherPlayerObj.GetType().GetFields())
        {
            string shipName = prop.Name;

            foreach (int x in GetOccupiedTiles(otherPlayerObj, shipName))
            {
                if (x == tileNum)
                {
                    hit = true;
                }
            }
        }

        // Spawns the hit or miss prefab at the tile position
        Vector3 hitSpawnPos = new Vector3(tilePos.x + 0.5f, tilePos.y + 1f, tilePos.z + 0.5f);

        // Toggling UI components
        componentManager.ToggleButtonInteractable(componentManager.bombButton);
        StartCoroutine(ToggleButtonInteractablesAfterSeconds(1.6f));

        if (hit)
        {
            StartCoroutine(InstantiateAfterSeconds(hitPrefab, hitSpawnPos, Quaternion.identity, 1.6f, lOtherPlayer));
            UpdateHitDict(otherPlayerObj, tileNum);
            StartCoroutine(CreatePopupAfterSeconds("Hit!", 1.6f, 2f));

            // white object logic
            CreateWhiteObject(lOtherPlayer, tileNum);
        }
        else
        {
            StartCoroutine(InstantiateAfterSeconds(missPrefab, hitSpawnPos, Quaternion.identity, 1.6f, lOtherPlayer));
            StartCoroutine(CreatePopupAfterSeconds("Miss!", 1.6f, 2f));
        }

        CheckForSink(otherPlayerObj);
    }

    private void CheckForSink(Player player)
    {
        string playerName = "";
        if (player == playerOne) { playerName = "playerOne"; }
        else { playerName = "playerTwo"; }
        List<Ship> playerShips = new List<Ship>();
        Ship[] playerInput = {
            player.Ship_2_01, player.Ship_3_01, player.Ship_3_02, player.Ship_4_01, player.Ship_5_01
        };
        playerShips.AddRange(new List<Ship>(playerInput));

        foreach (Ship ship in playerShips)
        {
            bool sunk = ship.hitDict.Values.All(value => value);
            if (sunk)
            {
                GameObject shipObj = GetShipFromPlayerType(GetPlayerNameLowercaseFromObj(player), GetShipNameLowercaseFromObj(ship));
                shipObj.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = greyedOutMaterial;
                componentManager.UpdateScoreboard(playerName, shipObj.name);

                ship.hasSunk = true;
            }
        }
        callCompManReloadSb();
    }

    private void callCompManReloadSb()
    {
        Dictionary<string, bool> p1Sinks = new Dictionary<string, bool>();
        Dictionary<string, bool> p2Sinks = new Dictionary<string, bool>();

        List<Ship> playerOneShips = new List<Ship>();
        Ship[] playerOneInput = {
            playerOne.Ship_2_01, playerOne.Ship_3_01, playerOne.Ship_3_02, playerOne.Ship_4_01, playerOne.Ship_5_01
        };
        playerOneShips.AddRange(new List<Ship>(playerOneInput));

        List<Ship> playerTwoShips = new List<Ship>();
        Ship[] playerTwoInput = {
            playerTwo.Ship_2_01, playerTwo.Ship_3_01, playerTwo.Ship_3_02, playerTwo.Ship_4_01, playerTwo.Ship_5_01
        };
        playerTwoShips.AddRange(new List<Ship>(playerTwoInput));

        int x = 0;
        foreach (Ship ship in playerOneShips)
        {
            if (ship.hasSunk == true)
            {
                if (x == 0)
                {
                    p1Sinks["0"] = true;
                }
                else if (x == 1)
                {
                    p1Sinks["1"] = true;
                }
                else if (x == 2)
                {
                    p1Sinks["2"] = true;
                }
                else if (x == 3)
                {
                    p1Sinks["3"] = true;
                }
                else if (x == 4)
                {
                    p1Sinks["4"] = true;
                }
            }
            x = x + 1;
        }

        x = 0;
        foreach (Ship ship in playerTwoShips)
        {
            if (ship.hasSunk == true)
            {
                if (x == 0)
                {
                    p2Sinks["0"] = true;
                }
                else if (x == 1)
                {
                    p2Sinks["1"] = true;
                }
                else if (x == 2)
                {
                    p2Sinks["2"] = true;
                }
                else if (x == 3)
                {
                    p2Sinks["3"] = true;
                }
                else if (x == 4)
                {
                    p2Sinks["4"] = true;
                }
            }
            x = x + 1;
        }

        componentManager.ReloadScoreboards(p1Sinks, p2Sinks);
        CheckForWin(p1Sinks, p2Sinks);
    }

    public void PlayAgain()
    {
        componentManager.DestroyAllDontDestroyOnLoadObjects();
        SceneManager.LoadScene(0);
    }

    public void CheckForWin(Dictionary<string, bool> p1Sinks, Dictionary<string, bool> p2Sinks)
    {
        int p1Amt = 0;
        int p2Amt = 0;
        foreach (KeyValuePair<string, bool> kvp in p1Sinks)
        {
            if (kvp.Value == true)
            {
                p1Amt += 1;
            }
        }
        Debug.Log($"p1Amt: {p1Amt}");
        if (p1Amt >= 5)
        {
            //player one wins
            AnnounceWinner("playerOne");
            return;
        }
        foreach (KeyValuePair<string, bool> kvp in p2Sinks)
        {
            if (kvp.Value == true)
            {
                p2Amt += 1;
            }
        }
        Debug.Log($"p2Amt: {p2Amt}");
        if (p2Amt >= 5)
        {
            //player two wins
            AnnounceWinner("playerOne");
            return;
        }
    }

    public void AnnounceWinner(string playerName)
    {
        CreatePopup($"Player {playerName} wins!");
        componentManager.winnerMenu.SetActive(true);
    }

    private string GetPlayerNameLowercaseFromObj(Player player)
    { 
        if (player == playerOne) { return "playerOne"; }
        else if (player == playerTwo) { return "playerTwo"; }
        else { return null; }
    }

    private string GetShipNameLowercaseFromObj(Ship ship)
    {
        DumpToConsole(ship);
        if (ship == playerOne.Ship_2_01 || ship == playerTwo.Ship_2_01) { return "Ship_2_01"; }
        else if (ship == playerOne.Ship_3_01 || ship == playerTwo.Ship_3_01) { return "Ship_3_01"; }
        else if (ship == playerOne.Ship_3_02 || ship == playerTwo.Ship_3_02) { return "Ship_3_02"; }
        else if (ship == playerOne.Ship_4_01 || ship == playerTwo.Ship_4_01) { return "Ship_4_01"; }
        else if (ship == playerOne.Ship_5_01 || ship == playerTwo.Ship_5_01) { return "Ship_5_01"; }
        else { return null; }
    }

    private GameObject GetShipFromPlayerType(string player, string ship)
    {
        if (player == "playerOne")
        {
            foreach (GameObject obj in playerOneObjs)
            {
                if (obj.name.Contains(ship)) { return obj; }
            }
        }
        else if (player == "playerTwo")
        {
            foreach (GameObject obj in playerTwoObjs)
            {
                if (obj.name.Contains(ship)) { return obj; }
            }
        }
        return null;
    }

    private void CreateWhiteObject(string player, int tileNum)
    {
        // tileNum of corresponding thing in actual tilemap
        Vector3 tilePosition = selectionManager.tiles[tileNum].transform.position;
        Vector3 spawnPosition = new Vector3(tilePosition.x + 0.5f, tilePosition.y + 1.25f, tilePosition.z + 0.5f);
        GameObject obj = Instantiate(whiteObjectPrefab, spawnPosition, Quaternion.identity);
        if (player == "playerOne") { 
            playerOneWhiteObjects.Add(obj);
            obj.tag = "PlayerOneVisible";
        }
        else if (player == "playerTwo") { 
            playerTwoWhiteObjects.Add(obj);
            obj.tag = "PlayerTwoVisible";
        }
        obj.SetActive(false);
        
    }

    IEnumerator ToggleButtonInteractablesAfterSeconds(float seconds = 0f)
    {
        yield return new WaitForSeconds(seconds);
        componentManager.ToggleButtonInteractable(componentManager.nextPlayerButton);
    }
 
    private void UpdateShotObjectDict(string player, GameObject item)
    {
        
        if (player == "playerOne")
        {
            playerTwoShotObjects.Add(item);
        }
        else if (player == "playerTwo")
        {
            playerOneShotObjects.Add(item);
        }
    }

    private void UpdateHitDict(Player player, int tileNum)
    {
        List<Ship> playerShips = new List<Ship>();
        Ship[] playerInput = {
            player.Ship_2_01, player.Ship_3_01, player.Ship_3_02, player.Ship_4_01, player.Ship_5_01
        };
        playerShips.AddRange(new List<Ship>(playerInput));

        foreach (Ship ship in playerShips)
        {
            foreach (KeyValuePair<int, bool> x in ship.hitDict)
            {
                if (x.Key == tileNum)
                {
                    ship.hitDict[tileNum] = true;
                    //Debug.Log($"Hit dict updated. {tileNum} is now true!");
                    return;
                }
            }
        }
    }

    private List<int> GetOccupiedTiles(Player player, string shipName)
    {
        List<int> occupiedTiles = new List<int>();

        List<Ship> playerShips = new List<Ship>();
        Ship[] playerInput = {
            player.Ship_2_01, player.Ship_3_01, player.Ship_3_02, player.Ship_4_01, player.Ship_5_01
        };
        playerShips.AddRange(new List<Ship>(playerInput));

        foreach (Ship ship in playerShips)
        {
            foreach (KeyValuePair<int, bool> x in ship.hitDict)
            {
                occupiedTiles.Add(x.Key);
            }
        }
        return occupiedTiles;
    }

    public void NextButton()
    {
        componentManager.ToggleButtonInteractable(componentManager.nextPlayerButton);
        CreatePopup("Switching players! Look away!", 2f);
        StartCoroutine(SwapScenesAfterSeconds(2f));
    }

    IEnumerator InstantiateAfterSeconds(GameObject prefab, Vector3 origin, Quaternion quat, float seconds, string player)
    {
        yield return new WaitForSeconds(seconds);
        GameObject spawnObject = Instantiate(prefab, origin, quat);
        if (player == "playerOne") { spawnObject.tag = "PlayerTwoVisible"; }
        else if (player == "playerTwo") { spawnObject.tag = "PlayerOneVisible"; }
        UpdateShotObjectDict(player, spawnObject);
    }

    public void BombSelection()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        GameObject bombSelector = Instantiate(bombSelectorPref, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        selectionManager.isHighlighting = true;
        selectionManager.selectedLength = 1;
    }

    private IEnumerator SwapScenesAfterSeconds(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
        SwapScenes();
    }

    public void SwapScenes()
    {
        string curScene = SceneManager.GetActiveScene().name;
        if (curScene == "PlayerOneSelection")
        {
            SwapVisibility("PlayerTwo");
            SceneManager.LoadScene("PlayerTwoSelection");
        }
        else if (curScene == "PlayerTwoSelection")
        {
            SwapVisibility("PlayerOne");
            SceneManager.LoadScene("PlayerOneSelection");
        }
    }

    public string GetCurrentPlayer()
    {
        if (SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            return "PlayerOne";
        }
        else if (SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            return "PlayerTwo";
        }
        else
        {
            return null;
        }
    }

    public Player GetCurrentPlayerObj()
    {
        if (SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            return playerOne;
        }
        else if (SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            return playerTwo;
        }
        else
        {
            return null;
        }
    }

    public Player GetOtherPlayerObj()
    {
        if (SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            return playerTwo;
        }
        else if (SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            return playerOne;
        }
        else
        {
            return null;
        }
    }

    public void SwapVisibility(string currentViewer)
    {        
        if (currentViewer == "PlayerOne")
        {
            publicCurrentViewer = "PlayerOne";
            foreach (GameObject Obj in playerTwoObjs)
            {
                Obj.SetActive(false);
            }
            foreach (GameObject Obj in playerTwoShotObjects)
            {
                Obj.SetActive(false);
            }
            foreach (GameObject Obj in playerTwoWhiteObjects)
            {
                Obj.SetActive(false);
            }
            try
            {
                foreach (GameObject Obj in playerOneObjs)
                {
                    Obj.SetActive(true);
                }
                foreach (GameObject Obj in playerOneShotObjects)
                {
                    Obj.SetActive(true);
                }
                foreach (GameObject Obj in playerOneWhiteObjects)
                {
                    Obj.SetActive(true);
                }
            }
            catch {
                // Do nothing
            }
        }
        else if (currentViewer == "PlayerTwo")
        {
            publicCurrentViewer = "PlayerTwo";
            foreach (GameObject Obj in playerOneObjs)
            {
                Obj.SetActive(false);
            }
            foreach (GameObject Obj in playerOneShotObjects)
            {
                Obj.SetActive(false);
            }
            foreach (GameObject Obj in playerOneWhiteObjects)
            {
                Obj.SetActive(false);
            }
            try
            {
                foreach (GameObject Obj in playerTwoObjs)
                {
                    Obj.SetActive(true);
                }
                foreach (GameObject Obj in playerTwoShotObjects)
                {
                    Obj.SetActive(true);
                }
                foreach (GameObject Obj in playerTwoWhiteObjects)
                {
                    Obj.SetActive(true);
                }
            }

            catch
            {
                // Do nothing
            }
            
        }
    }

    IEnumerator SwapVisibilityAfterSeconds(string str, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SwapVisibility(str);
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

    public void CreatePopup(string text, float deleteSeconds = 3f)
    {
        GameObject popup = Instantiate(messagePopupPrefab, GameObject.Find("Canvas").transform);
        popup.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = text;
        StartCoroutine(DeleteObjectAfterSeconds(popup, deleteSeconds));
    }

    IEnumerator CreatePopupAfterSeconds(string text, float seconds = 0f, float deleteSeconds = 3f)
    {
        yield return new WaitForSeconds(seconds);
        GameObject popup = Instantiate(messagePopupPrefab, GameObject.Find("Canvas").transform);
        popup.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = text;
        StartCoroutine(DeleteObjectAfterSeconds(popup, deleteSeconds));
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

    public void DumpDictToConsole(Dictionary<string, bool> dict)
    {
        foreach (KeyValuePair<string, bool> x in dict)
        {
            Debug.Log($"KVP: {x.Key}, {x.Value}");
        }
    }

    public int GetTileNumFromName(string unmodif)
    {
        string modifString = unmodif.Substring(unmodif.Length - 3, 2);
        int modifInt = Int32.Parse(modifString);
        return modifInt;
    }
}
