using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movable : InteractableGeneral
{
    [Header("Movable Object Settings")]
    public UnityEvent onDrop;
    public Transform attachPoint;
    [Tooltip("是否启用地面贴合放置")]
    public bool groundPlace = true;
    public Vector3 groundPlaceOffset = new Vector3(0, 0.01f, 0);
    [Tooltip("松开按键是否自动放下")]
    public bool dropOnKeyLift = false;
    [Tooltip("是否在拿起时关闭碰撞体")]
    public bool noCollideOnHold = true;
    public bool freezeRotation = false;
    public float snapSpeed = 20;

    [Header("System Stuff (Usually Don't Touch)")]
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Rigidbody myRbody;
    public Collider myCollider;
    public MovableMagnetSnapper myMagnetSnapper;
    public RaycastInteractor myRayManipulator;
    public bool originalUseGravity = true;
    public bool moving = false;
    public Collider[] subCollliders;

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        myCollider = GetComponent<Collider>();
        myRbody = GetComponent<Rigidbody>();

        if (myRbody != null)
            originalUseGravity = myRbody.useGravity;

        if (subCollliders == null || subCollliders.Length == 0)
            subCollliders = transform.GetComponentsInChildren<Collider>();

        if (attachPoint == null)
        {
            string[] names = { "attachPoint", "AttachPoint", "Attach Point", "attach point" };
            foreach (string n in names)
            {
                Transform found = transform.Find(n);
                if (found != null)
                {
                    attachPoint = found;
                    break;
                }
            }
        }

        if (attachPoint != null)
        {
            if (transform.localScale != Vector3.one)
            {
                Debug.LogWarning($"[Movable] {name} 的缩放不是 (1,1,1)，可能导致拾取定位错误。");
            }
        }
    }

    private void Update()
    {
        if (freezeRotation)
            transform.rotation = startingRotation;

        if (myMagnetSnapper != null && myMagnetSnapper.subject != this)
            ForceDrop();
    }

    public void RotateY(float angle)
    {
        transform.Rotate(0, angle, 0);
    }

    public void ResetOrientation()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;

        if (myRbody != null)
        {
            myRbody.linearVelocity = Vector3.zero;
            myRbody.angularVelocity = Vector3.zero;
            myRbody.useGravity = true;
        }
    }

    public void ResetOrientationAll()
    {
        Movable[] allMovables = Object.FindObjectsByType<Movable>(FindObjectsSortMode.None);
        foreach (Movable m in allMovables)
        {
            m.ResetOrientation();
            m.transform.parent = null;
        }

        MovableMagnetSnapper[] allMagnets = Object.FindObjectsByType<MovableMagnetSnapper>(FindObjectsSortMode.None);
        foreach (MovableMagnetSnapper m in allMagnets)
            m.ReleaseSubject();

        Debug.Log("MOVABLE - RESET ALL");
    }

    public void Grab(RaycastInteractor newManipulator)
    {
        myRayManipulator = newManipulator;
    }

    public void Drop()
    {
        onDrop.Invoke();
    }

    public void ForceDrop()
    {
        moving = false;
        transform.parent = null;

        if (myRayManipulator != null)
        {
            myRayManipulator.previousMoveParent = null;
            myRayManipulator.moveSubject = null;
            myRayManipulator = null;
        }

        if (myMagnetSnapper != null)
        {
            myMagnetSnapper.subject = null;
            myMagnetSnapper = null;
        }

        SetColliderIsTrigger(this, false);

        if (myRbody != null)
        {
            myRbody.isKinematic = false;
            myRbody.useGravity = originalUseGravity;

            myRbody.linearVelocity = Vector3.zero;
            myRbody.angularVelocity = Vector3.zero;
        }

        if (groundPlace)
        {
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f))
            {
                transform.position = hit.point + groundPlaceOffset;
            }
        }

        Drop(); 
    }


    public static void SetColliderIsTrigger(Movable m, bool setting)
    {
        Collider c = m.GetComponent<Collider>();
        if (c != null)
            c.isTrigger = setting;

        if (m.subCollliders != null && m.subCollliders.Length > 0)
        {
            foreach (Collider subC in m.subCollliders)
            {
                if (subC == null) continue;

                // 跳过特殊组件
                if (subC.GetComponent<CharacterController>() != null) continue;
                if (subC.GetComponent<InteractableTrigger>() != null) continue;
                if (subC.GetComponent<MovableMagnetSnapper>() != null) continue;

                subC.isTrigger = setting;
            }
        }
    }
}
