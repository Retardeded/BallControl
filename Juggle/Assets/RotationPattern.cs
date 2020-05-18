using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPattern : MonoBehaviour
{


    [SerializeField] HandlingBall hand;
    public bool leftSide;
    public bool throwMode = true;

    public float rotZSpeed = 30f;
    public float ROT_Z_TIME = 2f;
    float zTime;

    public float rotYSpeed = 10f;
    public float ROT_Y_TIME = 2f;
    float yTime;

    public float yROT;

    Rigidbody rb;
    Quaternion basicRotation;

    float PATTERN_TIME;
    float patternTime;

    void Start()
    {
        PATTERN_TIME = ROT_Y_TIME * 2;
        patternTime = PATTERN_TIME;
        basicRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        zTime = ROT_Z_TIME/2;
        yTime = ROT_Y_TIME;

        if (leftSide)
            StartCoroutine(hand.BallRelease(hand.initialDelay));
        else
            StartCoroutine(SetUpTiming());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        yROT = rb.transform.eulerAngles.y;
        if (throwMode)
        {
            PatternReset();
            zRotation();
            yRotation();
            rb.transform.Rotate(0f, rotYSpeed * Time.fixedDeltaTime * Mathf.Cos(yTime* PatternManager.COS_SCALE), rotZSpeed * Time.fixedDeltaTime * Mathf.Cos(zTime * PatternManager.COS_SCALE));
        }

    }

    private void PatternReset()
    {
        patternTime -= Time.fixedDeltaTime;
        if(patternTime < 0f)
        {
            patternTime = PATTERN_TIME;
            transform.rotation = basicRotation;
        }
    }

    private void yRotation()
    {
        if (yTime < 0f)
        {
            rotYSpeed *= -1;
            yTime = ROT_Y_TIME;
        }
        yTime -= Time.fixedDeltaTime;
    }

    private void zRotation()
    {
        if (zTime < 0f)
        {
            rotZSpeed *= -1;
            zTime = ROT_Z_TIME;
        }
        zTime -= Time.fixedDeltaTime;
    }

    public IEnumerator SetUpTiming()
    {
        throwMode = false;
        yield return new WaitForSeconds(ROT_Z_TIME);
        throwMode = true;
        StartCoroutine(hand.BallRelease(hand.initialDelay));
    }
}
