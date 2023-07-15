using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(RayBeamer))]
public class LaserPistolController : MonoBehaviour
{
    // VR Input
    public SteamVR_Action_Boolean fireAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Laser", "Fire");

    // Components in Children
    public LinearDrive slideDrive;
    public MagazineWell magazineWell;
    public Text ammoCountDisplay;

    public Vector3 magazineEjectDirection;
    public float magazineEjectionStrength = 10.0f;

    // Component References
    private Interactable interactable;
    private RayBeamer rayBeamer;
    
    private bool isLoaded = false;

    private void Start()
    {
        rayBeamer = GetComponent<RayBeamer>();
        interactable = GetComponent<Interactable>();
        interactable.onAttachedToHand += OnAttachToHand;
        interactable.onDetachedFromHand += OnDetachFromHand;

        if (!slideDrive)
            slideDrive = GetComponentInChildren<LinearDrive>();
        slideDrive.repositionGameObject = false;

        if (!magazineWell)
            magazineWell = GetComponentInChildren<MagazineWell>();
    }

    private void Update()
    {
        if (!interactable.attachedToHand)
            return;

        // Shoot Action
        var hand = interactable.attachedToHand.handType;
        if (fireAction.GetState(hand) && magazineWell.magazine && magazineWell.magazine.currentCapacity > 0)
        {
            Fire();
        }

        if (fireAction.GetLastStateUp(hand))
        {
            rayBeamer.RechargeRay();
        }

        // Eject and load Magazine
        if (isLoaded && Mathf.Approximately(slideDrive.linearMapping.value, 1.0f))
        {
            EjectMagazine();
        }
        else if (!isLoaded && Mathf.Approximately(slideDrive.linearMapping.value, 0.0f) && magazineWell.magazine)
        {
            LoadMagazine();
        }
    }

    public void OnAttachToHand(Hand hand)
    {
        slideDrive.repositionGameObject = true;
    }

    public void OnDetachFromHand(Hand hand)
    {
        slideDrive.repositionGameObject = false;
    }

    private void Fire()
    {
        rayBeamer.ShootRay();

        magazineWell.magazine.currentCapacity -= 1;
        ammoCountDisplay.text = magazineWell.magazine.currentCapacity.ToString();
    }

    private void LoadMagazine()
    {
        Debug.Log("Loaded");

        isLoaded = true;
        ammoCountDisplay.text = magazineWell.magazine.currentCapacity.ToString();

        // Deactivate Collider
        magazineWell.magazine.gameObject.GetComponent<Collider>().enabled = false;
    }

    private void EjectMagazine()
    {
        if (!magazineWell.magazine)
            return;

        Debug.Log("Ejected");

        magazineWell.magazine.gameObject.GetComponent<Collider>().enabled = true;

        // Throw out magazine
        var magRb = magazineWell.magazine.GetComponent<Rigidbody>();
        magazineWell.DetachMagazine();
        magRb.AddForce(magazineEjectDirection * magazineEjectionStrength);

        isLoaded = false;
        ammoCountDisplay.text = "-";
    }
}
