using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWorldMovingNPC : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private MovingCharacter model;

    [SerializeField] private MovingCharacterObject interactivePart;


    public void Initialize(string characterName)
    {
        gameObject.name = "MovingCharacter (" + characterName + ")";
        model = GameManager.movingCharacters.GetMovingCharacter(characterName);
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Characters/Animators/" + characterName);
        interactivePart.Init(model);
    }

    void Update()
    {
        if (model.map != GameManager.map)
        {
            transform.position = new Vector3(-9999, -9999, 0);
            return;
        }
        transform.position = model.placement;
        animator.SetFloat("Horizontal", model.faceX);
        animator.SetFloat("Vertical", model.faceY);
        animator.SetFloat("Speed", model.moving ? 1 : 0);
    }
}
