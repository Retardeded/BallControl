using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// speed 0.92* 0.96 1.02 1.07 1.12 1.193 1.25825 1.3165 1.4333 1.5498 1.645 1.76 1.94 2.15 2.4 stabilne

// sprawdzic jak bardzo stabilne w "granicach" konkretnych przedzialow

public class PatternManager : MonoBehaviour
{
    float correctCycle = 2.5f;
    float handPosDiff = 0.65f;
    float handRange = 0.11f;

    [SerializeField] string patternType;
    [SerializeField] float cosScale = 0.1f;
    [SerializeField] float offset = 0.05f;
    public static float COS_SCALE;

    public int ballsInPattern = 3;
    [SerializeField] float patternSpeed = 5f / 3f;
    float fixedPatternSpeed;

    // basicVector = new Vector3(0f, 4.9f, 0.65f);
    // basicInitialDelay  = 1.8f;
    // basicPatternTime  = 1.41f;
    //[SerializeField] Vector3 basicVector = new Vector3(0f, 3.4335f, 0.9285f);
    [SerializeField] Vector3 basicVector;
    [SerializeField] Vector3 basicAsyncVector = new Vector3(0f, 6.867f, 0.4642857f);
    [SerializeField] Vector3 basicSyncVector = new Vector3(0f, 5.15025f, -(0.11f / 1.05f));
    [SerializeField] float maxErrorX = 0.05f;
    [SerializeField] float maxErrorY = 0.05f;
    [SerializeField] float basicHandTime = 0.7f;
    [SerializeField] float basicAirTime = 0.7f;

    [SerializeField] float basicRotYSpeed = -30f;
    [SerializeField] float basicRotZSpeed = -30f;

    [SerializeField] float basicROT_Y_TIME = 0.7f;
    [SerializeField] float basicROT_Z_TIME = 0.7f;

    [SerializeField] float timeScale = 0.5f;

    [SerializeField] GameObject[] balls;

    [SerializeField] RotationPattern leftSide;
    [SerializeField] RotationPattern rightSide;

    [SerializeField] HandlingBall leftHand;
    [SerializeField] HandlingBall rightHand;

    private void Awake()
    {
        if (ballsInPattern % 2 == 0)
            basicVector = basicSyncVector;
        else
            basicVector = basicAsyncVector;

        float yVector, zVector;
        CalculateCorrectSpeed(out yVector, out zVector);
        SetCorrectSpeed(yVector, zVector);
        SetPatternRotation();
        SetUpPatternStats();
        DistributeBalls();
    }

    private void SetCorrectSpeed(float yVector, float zVector)
    {
        float ballMaths;

        if (ballsInPattern % 2 == 1)
            ballMaths = (ballsInPattern-1) / 2;
        else
            ballMaths = (ballsInPattern-1f) / 3f;


        basicVector = new Vector3(basicVector.x, yVector, zVector);

        basicRotYSpeed *= patternSpeed * ballMaths;
        basicRotZSpeed *= patternSpeed * ballMaths;

        basicROT_Y_TIME /= (patternSpeed * ballMaths);
        basicROT_Z_TIME /= (patternSpeed * ballMaths);

        basicHandTime /= (patternSpeed * ballMaths);
        basicAirTime /= (patternSpeed * ballMaths);

        cosScale = (cosScale / basicROT_Y_TIME); //* Mathf.Abs(basicRotYSpeed);
        COS_SCALE = cosScale;
    }

    private void CalculateCorrectSpeed(out float yVector, out float zVector)
    {
      
        yVector = basicVector.y / patternSpeed;
        print(yVector);
        zVector = basicVector.z * basicVector.y / yVector;
        print(zVector);
        

    }

    private void SetPatternRotation()
    {
        leftSide.rotZSpeed = basicRotZSpeed;
        leftSide.rotYSpeed = basicRotYSpeed;
        leftSide.ROT_Z_TIME = basicROT_Z_TIME;
        leftSide.ROT_Y_TIME = basicROT_Y_TIME;

        rightSide.rotZSpeed = basicRotZSpeed;
        rightSide.rotYSpeed = -basicRotYSpeed;
        rightSide.ROT_Z_TIME = basicROT_Z_TIME;
        rightSide.ROT_Y_TIME = basicROT_Y_TIME;

        if(patternType == "Reverse Cascade")
        {
            correctCycle = 1.5f;
            leftSide.rotZSpeed *= -1;
            //leftSide.rotYSpeed *= -1;

            rightSide.rotZSpeed *= -1;
            //rightSide.rotYSpeed *= -1;
        }
    }

    private void DistributeBalls()
    {
        for (int i = 0; i < ballsInPattern; i++)
        {
            balls[i].gameObject.SetActive(true);

            if (i % 2 == 0)
                leftHand.balls.Add(balls[i]);
            else
                rightHand.balls.Add(balls[i]);
        }
    }

    private void SetUpPatternStats()
    {
        leftHand.maxErrorX = maxErrorX;
        leftHand.maxErrorY = maxErrorY;
        rightHand.maxErrorX = maxErrorX;
        rightHand.maxErrorY = maxErrorY;

        leftHand.properVector = basicVector;
        rightHand.properVector = new Vector3(basicVector.x, basicVector.y, -basicVector.z);

        leftHand.initialDelay = basicHandTime * correctCycle + offset*2;
        rightHand.initialDelay = basicHandTime * correctCycle + offset*2;

        if(ballsInPattern % 2 == 1)
        {
            leftHand.patternTime = basicAirTime * 2 + offset;
            rightHand.patternTime = basicAirTime * 2 + offset;
        }
        else
        {
            leftHand.patternTime = basicAirTime * 2f + offset;
            rightHand.patternTime = basicAirTime * 2f + offset;
        }
    }

    void Start()
    {
        Time.timeScale = timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
