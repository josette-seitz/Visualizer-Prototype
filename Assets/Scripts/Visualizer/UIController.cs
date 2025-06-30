using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Visualizer
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;

        private const string ActionMap = "MoveMenu";
        private const string ActionButton = "ButtonAOrXPressed";

        private VisualizerController _visualizerController;
        private InputAction _moveMenuAction;
        private Vector3 _offset = new Vector3(0, 2.3f, 2.15f);
        public Vector3 MenuToPlayerOffset => _offset;

        private void Start()
        {
            _visualizerController = GetComponentInParent<VisualizerController>();
        }

        private void OnEnable()
        {
            // Get Input Actions
            _moveMenuAction = inputActions.FindActionMap(ActionMap).FindAction(ActionButton);

            _moveMenuAction.performed += MoveMenu;
            _moveMenuAction.Enable();
        }

        private void OnDisable()
        {
            _moveMenuAction.performed -= MoveMenu;
            _moveMenuAction.Disable();
        }

        private void MoveMenu(InputAction.CallbackContext context)
        {
            // Pos: apply offset in player's local space
            transform.position = _visualizerController.CurrentPlayer.TransformPoint(MenuToPlayerOffset);
            // Rot: have menu face the same direction as player
            transform.rotation = Quaternion.Euler(0, _visualizerController.CurrentPlayer.eulerAngles.y, 0);
        }
    }
}
