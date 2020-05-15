using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPattern : MonoBehaviour
{
    // 3 ball
    // rotZSpeed -30, ROT_Z_TIME = 0.7
    // rotYSpeed -30, ROT_Y_TIME = 0.7
    //

    // rotZSpeed -45, ROT_Z_TIME = 0.525
    // rotYSpeed -45, ROT_Y_TIME = 0.525
    // 


    [SerializeField] HandlingBall hand;
    public bool leftSide;
    public bool throwMode = true;
    float cosScale = 4f;

    //[SerializeField] float rotZMax = 10f;
    //[SerializeField] float rotZMin = 340f;
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
        cosScale /= yTime;

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
            rb.transform.Rotate(0f, rotYSpeed * Time.fixedDeltaTime * Mathf.Cos(yTime*cosScale), rotZSpeed * Time.fixedDeltaTime * Mathf.Cos(zTime*cosScale));
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
