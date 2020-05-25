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
    Rigidbody ballRb;
    public Vector3 properVector;
    public Vector3 ballVelocity;
    public Vector3 previousPos;
    //1.819422 0.3812808 -0.1167499/0.3608411
    public Vector3 basicPos;
    public float smallEnoughError = 0.1f;

    public float initialDelay = 2f;
    public float patternTime = 1f;

    public bool handMode = true;
    int numOfBalls;
    int ballNum = 0;

    private void OnTriggerEnter(Collider obj)
    {

        if (obj.gameObject != ball && obj.gameObject.tag == "Ball")
        {
            ball = obj.gameObject;
            ballRb = ball.GetComponent<Rigidbody>();
            ball.transform.position = transform.position;
            handMode = true;
        }
    }

    void Start()
    {
        numOfBalls = balls.Count;
        ball = balls[ballNum];
        ballRb = ball.GetComponent<Rigidbody>();
        ball.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        ballVelocity = ballRb.velocity;

        if(handMode)
        {
            ball.transform.position = transform.position;
        }
    }

    public IEnumerator BallRelease(float time)
    {
        yield return new WaitForSeconds(time);
        if (basicPos == Vector3.zero)
            basicPos = ball.transform.position;
        else
            previousPos = ball.transform.position;
        Relase();
    }

    private void Relase()
    {
        float scale = ball.transform.position.y - basicPos.y;
        ballRb.velocity = properVector;

        if (balls.Count - 1 > ballNum)
        {
            ballNum++;
            ball = balls[ballNum];
            ballRb = ball.GetComponent<Rigidbody>();
            print("Now: " + ball.name);
        }
        else
        {
            handMode = false;
        }

        StartCoroutine(BallRelease(patternTime / (1+scale) ));
    }
}
