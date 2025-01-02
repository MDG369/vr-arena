using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
public class MovementRecognizer : MonoBehaviour
{
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float activationThreshold = 0.1f;

    public GameObject debugCubePrefab;

    public float newPositionDistanceThreshold = 0.05f;
    private bool isMoving = false;

    public Transform movementSource;
    private List<Vector3> positionsList = new List<Vector3>();
    
    public float yOffset = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    void createDebugCube() {
        if (debugCubePrefab != null) {
            float cylinderHeight = movementSource.localScale.y;
            Vector3 cylinderTip = movementSource.position + (movementSource.up * (cylinderHeight));
            Destroy(Instantiate(debugCubePrefab, cylinderTip, Quaternion.identity), 3);        }
    }
}
