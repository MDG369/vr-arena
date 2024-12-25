using UnityEngine;
using UnityEngine.InputSystem;
public class CustomActionScript : MonoBehaviour
{
    public InputActionReference customButton;
    public SceneTransitionManager sceneTransition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customButton.action.performed += ButtonWasPressed;
        customButton.action.canceled += ButtonWasReleased;
    }

    void ButtonWasPressed(InputAction.CallbackContext context)
    {
        sceneTransition.GoToScene(0);
        Debug.Log("Button was pressed");
    }


    void ButtonWasReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Button was released");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
