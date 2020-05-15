using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlingBall : MonoBehaviour
{
    //3 ball
    // properVector(y = 4.5, z = 0.75)
    // initialDelay 1.8, patternTime 1.44
    //

    // properVector(y = 3.65, z = 0.9)
    // initialDelay 1.33, patternTime 1.09
    // (23) - patternCollapses

    public List<GameObject> balls = new List<GameObject>();
    [SerializeField] GameObject ball;
    Rigidbody rb;
    Rigidbody ballRb;
    [SerializeField] float ballSpeedMult = 5f;
    public Vector3 properVector;

    public Vector3 ballVelocity;
    public Vector3 palmVelocity;

    public Vector3 previousPos;


    public float initialDelay = 2f;
    public float patternTime = 1f;

    public bool handMode = true;
    int numOfBalls;
    int ballNum = 0;

    private void OnTriggerEnter(Collider obj)
    {

        if (obj.gameObject != ball && obj.gameObject.tag == "Ball")
        {
            if(handMode)
            {
                //ballRb.velocity = properVector;

                //print(ball.name);
                //print(ballRb.velocity);
            }

            ball = obj.gameObject;
            ballRb = ball.GetComponent<Rigidbody>();
            ball.transform.position = transform.position;
            handMode = true;
            //StartCoroutine(BallRelease(patternTime));
        }
    }

    void Start()
    {
        numOfBalls = balls.Count;
        ball = balls[ballNum];
        rb = GetComponent<Rigidbody>();
        ballRb = ball.GetComponent<Rigidbody>();
        ball.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        ballVelocity = ballRb.velocity;
        palmVelocity = rb.velocity;

        if(handMode)
            ball.transform.position = transform.position;
    }

    public IEnumerator BallRelease(float time)
    {
        yield return new WaitForSeconds(time);
        previousPos = transform.position;
        //yield return new WaitForSeconds(patternTime / 20f);
        //yield return new WaitForSeconds(patternTime / (1.0f / Time.deltaTime));
        //yield return new WaitForSeconds(patternTime / 20);
        //ballRb.velocity = (transform.position - previousPos).normalized * ballSpeedMult;
        ballRb.velocity = properVector;

        print(ball.name);
        print(ballRb.velocity);

        if (balls.Count-1 > ballNum)
        {
            ballNum++;
            ball = balls[ballNum];
            ballRb = ball.GetComponent<Rigidbody>();
            print("ten numer pily tera: " + ball.name);
        }
        else
        {
            handMode = false;
        }

        StartCoroutine(BallRelease(patternTime));
    }
}
