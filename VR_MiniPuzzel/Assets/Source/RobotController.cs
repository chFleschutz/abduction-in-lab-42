using UnityEngine;
using System.Collections;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RobotController : MonoBehaviour
{
    public Transform Joystick;
    public float joyMove = 0.1f;

    public Transform Trigger;
    public float trigMove = 0.1f;

    public SteamVR_Action_Vector2 moveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("platformer", "AddMovementInput");
    public SteamVR_Action_Boolean jumpAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("platformer", "Jump");
    public SteamVR_Action_Single grabAction = SteamVR_Input.GetAction<SteamVR_Action_Single>("platformer", "Grab");

    public Robot character;
    private bool grabbing = false;
    
    private SteamVR_Input_Sources hand;
    private Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        Vector3 movement;
        float triggerAmount = 0;

        if (interactable.attachedToHand)
        {
            hand = interactable.attachedToHand.handType;
            var input = moveAction[hand].axis;
            movement = new Vector3(input.x, 0, input.y);

            triggerAmount = grabAction[hand].axis;

            if (jumpAction[hand].stateDown)
                character.Jump();

         

            if (triggerAmount > 0.5 && !grabbing)
            {
                character.ToggleGrab();
                grabbing = true;
            }
            else if (triggerAmount < 0.5 && grabbing)
            {
                character.ToggleGrab();
                grabbing = false;
            }
        }
        else
        {
            //Debug Input
            movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (Input.GetKeyDown(KeyCode.E))
                character.ToggleGrab();
            if (Input.GetKeyDown(KeyCode.Space))
                character.Jump();
        }

        // Align input to controller rotation (local space of controller)
        var rot = transform.eulerAngles.y;
        movement = Quaternion.AngleAxis(rot, Vector3.up) * movement;
        // Update virtual joystick
        Joystick.eulerAngles = new Vector3( 20-movement.x * joyMove, Trigger.eulerAngles.y, movement.y * joyMove);
        //Update virtual Trigger
        Trigger.eulerAngles = new Vector3( 20-triggerAmount * trigMove, Trigger.eulerAngles.y, Trigger.eulerAngles.z);


        character.AddMovementInput(movement);
    }
}
