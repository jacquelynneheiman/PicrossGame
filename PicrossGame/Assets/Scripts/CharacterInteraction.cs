using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField]
    private float interactionDistance;

    [SerializeField]
    private LayerMask interactionMask;

    private CharacterController characterController;
    private PlayerInput playerInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 position = transform.position;
            Vector2 direction = characterController.GetFacingDirection();

            RaycastHit2D hit = Physics2D.Raycast(position, direction, interactionDistance, interactionMask);

            if (hit.collider != null)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}
