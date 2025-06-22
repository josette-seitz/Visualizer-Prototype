using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] 
    private InputActionAsset inputActions;

    private const string ActionMap = "MoveMenu";
    private const string ActionButton = "ButtonAOrXPressed";

    private VisualizerController visualizerController;
    private InputAction moveMenuAction;
    private Vector3 offset = new Vector3(0, 2.3f, 2.15f);
    public Vector3 MenuToPlayerOffset => offset;


    private void Start()
    {
        visualizerController = GetComponentInParent<VisualizerController>();
    }

    private void OnEnable()
    {
        // Get Input Actions
        moveMenuAction = inputActions.FindActionMap(ActionMap).FindAction(ActionButton);

        moveMenuAction.performed += MoveMenu;
        moveMenuAction.Enable();
    }

    private void OnDisable()
    {
        moveMenuAction.performed -= MoveMenu;
        moveMenuAction.Disable();
    }

    private void MoveMenu(InputAction.CallbackContext context)
    {
        // Pos: apply offset in player's local space
        transform.position = visualizerController.CurrentPlayer.TransformPoint(MenuToPlayerOffset);
        // Rot: have menu face the same direction as player
        transform.rotation = Quaternion.Euler(0, visualizerController.CurrentPlayer.eulerAngles.y, 0);
    }
}
