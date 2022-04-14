using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundLinear : MonoBehaviour
{
    Vector3 rotationPointPos;
    public float currentAngle = 0f;
    public float targetAngle;

    Vector3 initOriginVector;
    Vector3 initLocalPosition;
    Quaternion initRotation;

    Vector3 lastOffset;

    public bool activated;

    [SerializeField] GameObject rotationPoint;
    [SerializeField] public float angle;
    [SerializeField] Vector3 axis;
    [SerializeField] public float speed;

    [SerializeField] float angleMargin = .1f;

    //[SerializeField] bool keepX;
    //[SerializeField] bool keepY;
    //[SerializeField] bool keepZ;

    // Start is called before the first frame update

    private void Start()
    {
        initLocalPosition = transform.localPosition;
        var transformParent = transform.parent;
        var rotationPointBaseParent = transformParent ?
            transform.parent
            .InverseTransformPoint(rotationPoint.transform.position) :
            rotationPoint.transform.position
        ;
        initOriginVector =
            (transform.localPosition -
            rotationPointBaseParent)
        ;
        initRotation = transform.localRotation;
    }

    void SetTargetAngle()
    {
        if (activated)
        {
            targetAngle = angle;
        }
        else
        {
            targetAngle = 0;
        }
    }

    void SetRotation(float angle)
    {
        var rotation = Quaternion.AngleAxis(angle, axis);
        var rotatedOriginVector =
             rotation * initOriginVector;
        var currentOffset = rotatedOriginVector - initOriginVector;

        transform.localRotation = rotation * initRotation;
        transform.localPosition -= lastOffset - currentOffset;

        lastOffset = currentOffset;
    }

    void FixedUpdate()
    {
        SetTargetAngle();

        float dTargetCurrentAngle = targetAngle - currentAngle;
        float dSign = Mathf.Sign(dTargetCurrentAngle);

        if (Mathf.Abs(dTargetCurrentAngle) < angleMargin)
        {
            return;
        }

        Debug.Log("Rotated");

        float step = speed * Time.deltaTime;
        float toRotate =
            dSign * Mathf.Min(Mathf.Abs(dTargetCurrentAngle), step);

        currentAngle += toRotate;
        //currentAngle =
        //    Mathf.Abs(currentAngle) > Mathf.Abs(targetAngle) ?
        //    targetAngle :
        //    currentAngle
        //;

        SetRotation(currentAngle);
    }
}
