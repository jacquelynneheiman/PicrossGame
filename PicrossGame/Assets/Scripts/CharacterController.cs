using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterInteraction))]
public class CharacterController : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private CharacterInteraction characterInteraction;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterInteraction = GetComponent<CharacterInteraction>();

    }

    public Vector2 GetFacingDirection()
    {
        return characterMovement.GetMoveDirection();
    }
}
