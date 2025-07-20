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
    /// Counts the score and updates it's visual
    /// </summary>
    public void CountPiece()
    {
        setPieces++;
        scoreHud.UpdateScore(setPieces);
    }

    public void SpreadEnergy()
    {
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

        foreach (PowerNode powerSource in powerSources)
        {
            FloodEnergy(powerSource);
        }

        if(AllTownsPowered())
        {
            EndGame();
        }
    }
    
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(!isPaused);
        }
    }

    bool AllTownsPowered()
    {
        if(towns.Count == 0)
            return false;

        int townCount = 0;

        foreach (PowerNode town in towns)
        {
            townCount += town.isPowered ? 1 : 0;
        }

        return townCount == towns.Count;
    }

    public void FloodEnergy(PowerNode node, HashSet<PowerNode> visited = null)
    {
        if (visited == null)
            visited = new HashSet<PowerNode>();

        if (node == null || visited.Contains(node))
            return;

        visited.Add(node);
        node.TogglePower(true);

        foreach (PowerNode neighbor in node.connectedNodes)
        {
            FloodEnergy(neighbor, visited);
        }
    }

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

        SpreadEnergy();
    }

    public void Pause(bool newState)
    {
        scoreScreen.SetActive(!newState);
        gridBox.SetActive(!newState);
        pauseScreen.SetActive(newState);

        isPaused = newState;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
