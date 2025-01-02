using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float activationThreshold = 0.1f;

    public GameObject debugCubePrefab;

    public float newPositionDistanceThreshold = 0.05f;
    public float recognitionThreshold = 0.7f;
    private bool isMoving = false;

    public Transform movementSource;
    private List<Vector3> positionsList = new List<Vector3>();

    public float yOffset = 0.1f;

    public bool creationMode = true;

    public string newGestureName = "NewGesture";

    public List<Gesture> trainingSet = new List<Gesture>();


    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnGestureRecognized;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, activationThreshold);

        // Start the movement
        if (isPressed && !isMoving)
        {
            startMovement();
        }
        // Stop the movement
        else if (!isPressed && isMoving)
        {
            stopMovement();
        }
        // Moving 
        else if (isPressed && isMoving)
        {
            updateMovement();
            Debug.Log("Moving");
        }
    }

    void startMovement()
    {
        isMoving = true;
        Debug.Log("Movement started");
        positionsList.Clear();
        var pos = movementSource.position + Vector3.up * yOffset;
        positionsList.Add(pos);

        createDebugCube();
    }

    void stopMovement()
    {
        isMoving = false;
        Debug.Log("Movement stopped");
        Point[] points = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector3 pos = positionsList[i];
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(pos);
            points[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(points);
        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);
            string filename = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(points, newGestureName, filename);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass + " " + result.Score);
            if (result.Score > recognitionThreshold)
            {
                OnGestureRecognized.Invoke(result.GestureClass);
            }
        }
    }

    void updateMovement()
    {
        Debug.Log("Moving");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        var pos = movementSource.position + Vector3.up * yOffset;
        if (Vector3.Distance(lastPosition, pos) > newPositionDistanceThreshold)
        {
            positionsList.Add(pos);
        }
        // positionsList.Add(pos);
        createDebugCube();
    }

    void createDebugCube()
    {
        if (debugCubePrefab != null)
        {
            float cylinderHeight = movementSource.localScale.y;
            Vector3 cylinderTip = movementSource.position + (movementSource.up * (cylinderHeight));
            Destroy(Instantiate(debugCubePrefab, cylinderTip, Quaternion.identity), 3);
        }
    }
}
