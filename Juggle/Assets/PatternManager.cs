using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// speed 0.92* 0.96 1.02 1.07 1.12 1.193 1.25825 1.3165 1.4333 1.5498 1.645 1.76 1.94 2.15 2.4 stabilne

// sprawdzic jak bardzo stabilne w "granicach" konkretnych przedzialow

public class PatternManager : MonoBehaviour
{
    [SerializeField] float cosScale = 0.1f;
    public static float COS_SCALE;

    public int ballsInPattern = 3;
    [SerializeField] float patternSpeed = 5f / 3f;
    float fixedPatternSpeed;

    // basicVector = new Vector3(0f, 4.9f, 0.65f);
    // basicInitialDelay  = 1.8f;
    // basicPatternTime  = 1.41f;
    [SerializeField] Vector3 basicVector = new Vector3(0f, 4.5f, 0.75f);
    [SerializeField] float basicInitialDelay = 1.8f;
    [SerializeField] float basicPatternTime = 1.44f;

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
        //basicInitialDelay /= ballsInPattern / 3.0f;
        //basicPatternTime /= ballsInPattern / 3.0f;
        //basicVector = new Vector3(basicVector.x, basicVector.y * (ballsInPattern/3.0f),basicVector.z * (3.0f / ballsInPattern) );

        fixedPatternSpeed = patternSpeed * ballsInPattern / 3.0f;

        float yVector, zVector;
        CalculateCorrectSpeed(out yVector, out zVector);
        SetCorrectSpeed(yVector, zVector);

        SetPatternRotation();
        SetUpPatternStats();
        DistributeBalls();
    }

    private void SetCorrectSpeed(float yVector, float zVector)
    {
        basicVector = new Vector3(basicVector.x, yVector, zVector);

        basicRotYSpeed *= fixedPatternSpeed;
        basicRotZSpeed *= fixedPatternSpeed;

        basicROT_Y_TIME /= fixedPatternSpeed;
        basicROT_Z_TIME /= fixedPatternSpeed;

        // dotąd legit wszystko

        basicInitialDelay /= fixedPatternSpeed;
        basicPatternTime /= fixedPatternSpeed;

        cosScale = (cosScale / basicROT_Y_TIME); //* Mathf.Abs(basicRotYSpeed);
        COS_SCALE = cosScale;
    }

    private void CalculateCorrectSpeed(out float yVector, out float zVector)
    {
        float s = (basicVector.y / 2) * (basicVector.y / Physics.gravity.y);
        print(s);
        float t = Mathf.Sqrt(2 * s / Physics.gravity.y);
        print(t);
        float desiredT = t / patternSpeed * ballsInPattern / 3.0f;
        print(desiredT);
        float desiredS = Physics.gravity.y * Mathf.Pow(desiredT, 2f) / 2;
        print("Wysokosc: " + desiredS);
        yVector = Mathf.Abs((desiredS + (Physics.gravity.y / 2) * Mathf.Pow(desiredT, 2f)) / desiredT);
        print(yVector);
        zVector = basicVector.z * t / desiredT;
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
        leftHand.properVector = basicVector;
        rightHand.properVector = new Vector3(basicVector.x, basicVector.y, -basicVector.z);

        leftHand.initialDelay = basicInitialDelay;
        rightHand.initialDelay = basicInitialDelay;
        leftHand.patternTime = basicPatternTime;
        rightHand.patternTime = basicPatternTime;
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
