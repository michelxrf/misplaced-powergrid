using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


/// <summary>
/// Singleton to keep track of the level state like score, game state and win conditions.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public ScoreHud scoreHud;

    public GameObject gameEndScreen;
    public GameObject pauseScreen;
    public GameObject gridBox;
    public GameObject scoreScreen;
    public AudioSource winSfx;

    public List<PowerNode> powerNodes;
    public List<PowerNode> powerSources;
    public List<PowerNode> towns;

    public int setPieces = 0;
    public int level = 0;

    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isGameover = false;
    [HideInInspector] public bool isDragging = false;

    /// <summary>
    /// Counts the score and call hud to update it's visual
    /// </summary>
    public void CountPiece()
    {
        setPieces++;
        scoreHud.UpdateScore(setPieces);
    }

    /// <summary>
    /// Rechecks every powernode connection and then tries to spread the power from the power plants throughout the grid recursively.
    /// Called every time the player set a new piece or rotates an existing one.
    /// </summary>
    public void UpdateConnections()
    {
        // updates all conections ---
        foreach(PowerNode town in towns)
        {
            town.Disconnect();
            town.ConnectAllNeighbors();
        }

        foreach(PowerNode node in powerNodes)
        {
            node.Disconnect();
            node.ConnectAllNeighbors();
        }

        foreach (PowerNode powerSource in powerSources)
        {
            powerSource.Disconnect();
            powerSource.ConnectAllNeighbors();
        }
        // ---

        // recursively tries to spread energy from every powerstation
        foreach (PowerNode powerSource in powerSources)
        {
            FloodEnergy(powerSource);
        }

        // if all towns are powered ends the level
        if(AllTownsPowered())
        {
            EndGame();
        }
    }
    
    /// <summary>
    /// Called when all towns are powered to show the game end screen.
    /// </summary>
    void EndGame()
    {
        isGameover = true;

        winSfx.Play();

        GameManager.Instance.RegisterScore(level, setPieces);
        
        Hud hud = FindFirstObjectByType<Hud>();
        hud.Score(setPieces);

        gridBox.gameObject.SetActive(false);
        scoreScreen.gameObject.SetActive(false);
        gameEndScreen.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isGameover)
            return;

        // pauses the game on ESC pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(!isPaused);
        }
    }

    /// <summary>
    /// Verifies if all towns have power, whinch is the game end condition.
    /// </summary>
    /// <returns>True when all towns have power, False otherwise.</returns>
    bool AllTownsPowered()
    {
        if(towns.Count == 0)
        {
            return false;
        }

        int poweredTownCount = 0;

        foreach (PowerNode town in towns)
        {
            poweredTownCount += town.isPowered ? 1 : 0;
        }

        return poweredTownCount == towns.Count;
    }

    /// <summary>
    /// Recursively walks all pieces connected, powering everything.
    /// </summary>
    /// <param name="node">The starting node.</param>
    /// <param name="visited">Hash passed down to prevent infinite loops.</param>
    public void FloodEnergy(PowerNode node, HashSet<PowerNode> visited = null)
    {
        if (visited == null)
            visited = new HashSet<PowerNode>();

        if (node == null || visited.Contains(node))
            return;

        node.TogglePower(true);
        visited.Add(node);

        // Recursively calls every piece connceted to this node
        foreach (PowerNode neighbor in node.connectedNodes)
        {
            FloodEnergy(neighbor, visited);
        }
    }

    /// <summary>
    /// Registers a new piece to level's list. Called by the pieces themselves, either on level init or when player sets a new one.
    /// </summary>
    /// <param name="newPiece">Reference to the new piece.</param>
    public void RegisterPiece(PowerNode newPiece)
    {
        if (newPiece.isTown)
        {
            towns.Add(newPiece);
        }
        else if (newPiece.isPowerSource)
        {
            powerSources.Add(newPiece);
        }
        else
            powerNodes.Add(newPiece);

        UpdateConnections();
    }

    /// <summary>
    /// Shows the pause menu and reduces player agency.
    /// </summary>
    /// <param name="newState">On or Off</param>
    public void Pause(bool newState)
    {
        scoreScreen.SetActive(!newState);
        gridBox.SetActive(!newState);
        pauseScreen.SetActive(newState);

        isPaused = newState;
    }

    /// <summary>
    /// Loads the tittle screen
    /// </summary>
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Loads the next level. Called by the game end screen.
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
