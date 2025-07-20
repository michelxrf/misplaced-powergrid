using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerNode : MonoBehaviour
{
    public bool isPowered = false;
    public bool isPowerSource = false;
    public bool isTown = false;

    public List<PowerNode> connectedNodes = new List<PowerNode>();
    [SerializeField] List<GameObject> conectionPoints = new List<GameObject>();
    [SerializeField] GameObject poweredLight;
    [SerializeField] Image poweredIcon;

    private void Awake()
    {
        TogglePower(isPowerSource);
    }

    private void Start()
    {
        LevelManager.Instance.RegisterPiece(this);
    }

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
                        hitNode.Handshake(this);
                    }
                }
            }
        }
    }

    private void Handshake(PowerNode otherNode)
    {
        foreach (var conectionPoint in conectionPoints)
        {
            Collider[] neighbors = Physics.OverlapBox(conectionPoint.transform.position, new Vector3(.05f, .05f, .05f), Quaternion.identity);

            foreach (var hit in neighbors)
            {
                if (hit.gameObject.GetComponent<PowerNode>() == otherNode)
                {
                    otherNode.ConnectNode(this);
                    ConnectNode(otherNode);
                }
            }
        }
    }

    void ConnectNode(PowerNode otherNode)
    {
        if (!connectedNodes.Contains(otherNode))
            connectedNodes.Add(otherNode);
    }

    public void Disconnect()
    {
        TogglePower(isPowerSource);
        connectedNodes.Clear();
    }

    public void TogglePower(bool newState)
    {
        isPowered = newState;

        if (poweredLight != null)
            poweredLight.SetActive(newState);
        
        if( poweredIcon != null)
             poweredIcon.color = newState ? Color.green : Color.red;
    }
}
