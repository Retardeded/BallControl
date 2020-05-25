using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotationPattern : MonoBehaviour
{

    [SerializeField] Transform yVec;
    [SerializeField] Transform zVec;
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

    private float fixedDeltaTime;
    int counter = 0;

    private void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

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
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

        yROT = rb.transform.eulerAngles.y;
        if (throwMode)
        {
            PatternReset();
            TimeRotation();
            yVec.Rotate(0f,rotYSpeed * Time.fixedDeltaTime * Mathf.Cos(yTime * PatternManager.COS_SCALE),0f);
            zVec.Rotate(0f, 0f, rotZSpeed * Time.fixedDeltaTime * Mathf.Cos(zTime * PatternManager.COS_SCALE));
            transform.rotation = Quaternion.Euler(0f, yVec.rotation.y*100, zVec.rotation.z*100);
        }

    }

    private void PatternReset()
    {
        patternTime -= Time.fixedDeltaTime;
        if(patternTime < 0f)
        {
            patternTime = PATTERN_TIME;
            //transform.rotation = Quaternion.Slerp(transform.rotation, basicRotation, Time.fixedDeltaTime * 5f);
            if (transform.rotation == Quaternion.identity)
                print("Reset");
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
        }
    }

    private void TimeRotation()
    {
        zTime -= Time.fixedDeltaTime;
        yTime -= Time.fixedDeltaTime;
        if (yTime < 0f)
        {
            rotYSpeed *= -1;
            yTime = ROT_Y_TIME;
        }
        if (zTime < 0f)
        {
            rotZSpeed *= -1;
            zTime = ROT_Z_TIME;
        }
    }

    public IEnumerator SetUpTiming()
    {
        throwMode = false;
        yield return new WaitForSeconds(ROT_Z_TIME);
        throwMode = true;
        StartCoroutine(hand.BallRelease(hand.initialDelay));
    }
}
