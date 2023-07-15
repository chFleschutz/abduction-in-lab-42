using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SampleCharacter : MonoBehaviour
{
    public float speed;

    private CharacterController characterController;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        var move = new Vector3(Input.GetAxis("Vertical"), 0.0f, Input.GetAxis("Horizontal") * -1.0f);
        characterController.Move(move * speed * Time.deltaTime);
    }
}