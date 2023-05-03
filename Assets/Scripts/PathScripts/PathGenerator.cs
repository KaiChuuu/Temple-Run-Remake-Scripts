using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    GameObject startingPath;

    Queue<GameObject> activePaths = new Queue<GameObject>();
    GameObject activePathContainer;

    GameObject lastPath;

    [SerializeField]
    List<PathSettings> paths = new List<PathSettings>();

    [SerializeField]
    List<ConnectorSettings> connectors = new List<ConnectorSettings>();

    //Remove object overlap flickering
    float offset = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        startingPath = GameObject.Find("Path Start");
        activePathContainer = GameObject.Find("Path Generator/Active Paths");

        InstantiateFirstPath();
    }

    GameObject getNextPath(List<PathSettings> path)
    {
        int randomPath = Random.Range(0, path.Count);
        GameObject nextPath = Instantiate<GameObject>(path[randomPath].pathPrefab);
        nextPath.transform.SetParent(activePathContainer.transform);

        //Generate Coin path
        if (path[randomPath].coinGenerations.Count != 0 && path[randomPath].coinGenerations != null) {
            int randomCoinGen = Random.Range(-1, path[randomPath].coinGenerations.Count);

            if (randomCoinGen != -1) //Odds that no coin path is generated
            {
                GameObject coinPath = Instantiate<GameObject>(path[randomPath].coinGenerations[randomCoinGen]);

                coinPath.transform.SetParent(nextPath.transform.Find("Path Coins"));
            }
        }
        return nextPath;
    }

    GameObject connectNextPath(List<ConnectorSettings> connector, GameObject path)
    {
        //Picks random connection path
        int randomPath = Random.Range(0, connector.Count);
        GameObject nextPath = Instantiate<GameObject>(connector[randomPath].connectorPrefab);
        nextPath.transform.SetParent(activePathContainer.transform);

        //Updates next connector & path to lastpath (position & rotation)
        nextPath.transform.position = lastPath.transform.position + 
                                      lastPath.transform.TransformDirection(getPathLength(lastPath));

        nextPath.transform.rotation = lastPath.transform.rotation;
        path.transform.rotation = lastPath.transform.rotation;

        //Updates based on left or right route
        ConnectorSettings.CornerType newDirection = connector[randomPath].cornerType;
        Vector3 connectorWidth = getPathWidth(nextPath);
        Vector3 connectorLength = getPathLength(nextPath);
        if (newDirection == ConnectorSettings.CornerType.Right)
        {
            //Updates connector rotation and position
            nextPath.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
            Vector3 fitCorner = new Vector3(connectorWidth.x / 2, 0, connectorLength.z / 2);
            nextPath.transform.position -= nextPath.transform.TransformDirection(fitCorner);

            //Updates path rotation and position
            path.transform.rotation *= Quaternion.AngleAxis(90, Vector3.up);
            path.transform.position = nextPath.transform.position +
                                      nextPath.transform.TransformDirection(new Vector3(0, 0, connectorLength.z));
        }
        else if (newDirection == ConnectorSettings.CornerType.Left)
        {
            nextPath.transform.rotation *= Quaternion.AngleAxis(90, Vector3.down);
            Vector3 fitCorner = new Vector3(-connectorWidth.x / 2, 0, connectorLength.z / 2);
            nextPath.transform.position -= nextPath.transform.TransformDirection(fitCorner);

            path.transform.rotation *= Quaternion.AngleAxis(90, Vector3.down);
            path.transform.position = nextPath.transform.position + 
                                      nextPath.transform.TransformDirection(new Vector3(0,0, connectorLength.z));
        }

        return nextPath;
    }
    
    Vector3 getPathWidth(GameObject path)
    {
        Mesh planeMesh = path.GetComponentInChildren<MeshFilter>().mesh;
        Bounds planeBounds = planeMesh.bounds;

        Vector3 planeLength = new Vector3(planeBounds.size.x * path.transform.localScale.x + offset, 0, 0);
        return planeLength;
    }

    Vector3 getPathLength(GameObject path)
    {
        Mesh planeMesh = path.GetComponentInChildren<MeshFilter>().mesh;
        Bounds planeBounds = planeMesh.bounds;

        Vector3 planeLength = new Vector3(0, 0, planeBounds.size.z * path.transform.localScale.z + offset);
        return planeLength;
    }

    void InstantiateFirstPath()
    {
        //Grab first path in paths List.
        GameObject nextPath = Instantiate<GameObject>(paths[0].pathPrefab);
        nextPath.transform.SetParent(activePathContainer.transform);

        nextPath.transform.SetLocalPositionAndRotation(startingPath.transform.position + getPathLength(startingPath), 
                                                       startingPath.transform.rotation);
        lastPath = nextPath;

        activePaths.Enqueue(startingPath);
        activePaths.Enqueue(nextPath);


        GameObject path = getNextPath(paths);
        GameObject connector = connectNextPath(connectors, path);

        lastPath = path;

        activePaths.Enqueue(connector);
        activePaths.Enqueue(path);
    }

    public void GenerateNextConnectorAndPath()
    {
        //Remove unused paths
        if(activePaths.Count > 4)
        {
            Destroy(activePaths.Dequeue());
            Destroy(activePaths.Dequeue());
        }

        GameObject path = getNextPath(paths);
        GameObject connector = connectNextPath(connectors, path);

        lastPath = path;

        activePaths.Enqueue(connector);
        activePaths.Enqueue(path);


    }
}
