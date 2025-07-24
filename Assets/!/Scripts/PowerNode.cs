using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
/// <summary>
/// Controls the behavior of a piece of the power grid
/// </summary>
public class PowerNode : MonoBehaviour
{
    public bool isPowered = false;
    public bool isPowerSource = false;
    public bool isTown = false;

    [HideInInspector] public List<PowerNode> connectedNodes = new List<PowerNode>();
    [SerializeField] List<GameObject> conectionPoints = new List<GameObject>();
    [SerializeField] GameObject poweredLight;
    [SerializeField] Image poweredIcon;

    private LevelManager levelManager;

    private void Awake()
    {
        TogglePower(isPowerSource);
    }

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        // calls the level manager to register itself on the list of pieces
        levelManager.RegisterPiece(this);
    }

    /// <summary>
    /// Goes through every conection point to verify if another piece is on it's side
    /// </summary>
    public void ConnectAllNeighbors()
    {
        foreach(var conectionPoint  in conectionPoints)
        {
            Collider[] neighbors = Physics.OverlapBox(conectionPoint.transform.position, new Vector3(.05f, .05f, .05f), Quaternion.identity);

            foreach (var hit in neighbors)
            {
                if (hit.gameObject != gameObject)
                {
                    if (hit.gameObject.TryGetComponent<PowerNode>(out PowerNode hitNode))
                    {
                        // tries to connect with the found neihgboring piece
                        hitNode.Handshake(this);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called by another piece to attempt to connect. It will connect if both are mutual neigbors.
    /// </summary>
    /// <param name="otherNode">The piece that called to attempt a connection.</param>
    private void Handshake(PowerNode otherNode)
    {
        foreach (var conectionPoint in conectionPoints)
        {
            Collider[] neighbors = Physics.OverlapBox(conectionPoint.transform.position, new Vector3(.05f, .05f, .05f), Quaternion.identity);

            foreach (var hit in neighbors)
            {
                if (hit.gameObject.GetComponent<PowerNode>() == otherNode)
                {
                    // Connect both
                    otherNode.ConnectNode(this);
                    ConnectNode(otherNode);
                }
            }
        }
    }

    /// <summary>
    /// Adds the node to the list of connected nodes
    /// </summary>
    /// <param name="otherNode">The node it connects to.</param>
    void ConnectNode(PowerNode otherNode)
    {
        if (!connectedNodes.Contains(otherNode))
            connectedNodes.Add(otherNode);
    }

    /// <summary>
    /// Remove the node from it's list of connection and turns off if not a power plant.
    /// </summary>
    public void Disconnect()
    {
        TogglePower(isPowerSource);
        connectedNodes.Clear();
    }

    /// <summary>
    /// Changes it's powered state and toogles visual effects.
    /// </summary>
    /// <param name="newState"></param>
    public void TogglePower(bool newState)
    {
        isPowered = newState;

        if (poweredLight != null)
            poweredLight.SetActive(newState);
        
        if( poweredIcon != null)
             poweredIcon.color = newState ? Color.green : Color.red;
    }
}
