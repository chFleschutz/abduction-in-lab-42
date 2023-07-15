using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR.InteractionSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Rigidbody))]
public class Hologram : MonoBehaviour
{
    public GameObject clonePrefab;

    // Snapping Variables
    public float snapVelocityThreshold = 0.1f;
    public float snapToGridVelocity = 0.2f;

    // Components
    private GameObject cloneObject;
    private Interactable interactable;
    private Rigidbody rb;

    private HoloTable holoTable;

    private bool isInsideTable = false;
    private bool isAttachedToHand = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        interactable.onDetachedFromHand += OnDetachFromHand;
        interactable.onAttachedToHand += OnAttachedToHand;

        cloneObject = Instantiate(clonePrefab, Vector3.down * 10.0f, Quaternion.identity);
        cloneObject.GetComponent<HoloClone>().Hide(); // Its assumed to have the component
    }

    private void Update()
    {
        if (!isInsideTable)
            return;

        // Update transform of clone object
        var relativePos = transform.position - holoTable.holoTableOrigin.position;
        cloneObject.transform.position = holoTable.cloneAreaOrigin.position + (relativePos * holoTable.scaleValue);
        cloneObject.transform.rotation = transform.rotation;
        cloneObject.transform.localScale = transform.localScale * holoTable.scaleValue;
    }

    private void FixedUpdate()
    {
        if (!isInsideTable || isAttachedToHand || !(rb.velocity.magnitude < snapVelocityThreshold)) 
            return;

        // Grid snapping (In FixedUpdate to ensure smooth animation without using delta time)
        var gridPosition = holoTable.SnapToGridPos(gameObject.transform.position);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, gridPosition, snapToGridVelocity);
        var gridRotation = holoTable.SnapRotation(gameObject.transform.eulerAngles);
        gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, gridRotation, snapToGridVelocity);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var table = other.gameObject.GetComponentInParent<HoloTable>();
        if (!table)
            return;

        OnHoloTableEnter(table);
    }

    private void OnTriggerExit(Collider other)
    {
        var table = other.gameObject.GetComponentInParent<HoloTable>();
        if (!table)
            return;

        OnHoloTableExit();
    }

    public void OnHoloTableEnter(HoloTable table)
    {
        holoTable = table;

        isInsideTable = true;
        rb.useGravity = false;

        ShowClone();
    }

    public void OnHoloTableExit()
    {
        holoTable = null;

        isInsideTable = false;
        rb.useGravity = true;

        HideClone();
    }

    public void OnAttachedToHand(Hand hand)
    {
        isAttachedToHand = true;
    }

    public void OnDetachFromHand(Hand hand)
    {
        isAttachedToHand = false;
    }

    public void HideClone()
    {
        cloneObject.GetComponent<HoloClone>().Hide();
    }

    public void ShowClone()
    {
        cloneObject.GetComponent<HoloClone>().Show();
    }
}
