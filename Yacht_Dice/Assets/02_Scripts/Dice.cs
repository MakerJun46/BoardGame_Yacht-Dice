using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public int thisTurn_Number;
    Rigidbody RB;

    public List<Transform> sides;

    public bool isDiceStop;
    public bool isRerolled;
    public bool isKeep;

    public Transform Reroll_Position;

    float noMovementThreshold = 0.0001f;
    const int moMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[moMovementFrames];

    private void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();

        isDiceStop = false;
        isRerolled = false;
        isKeep = false;
    }

    private void Update()
    {
        isStop();
    }


    public void isStop()
    {
        if(!isMoving() && !isRerolled)
        {
            Debug.Log("stop");

            StartCoroutine(waitSecond());

            double maxY = 0;
            int number = 0;

            for(int i = 0; i < sides.Count; i++)
            {
                if(maxY < sides[i].position.y)
                {
                    maxY = sides[i].position.y;
                    number = i + 1;
                }
            }

            thisTurn_Number = number;
            isRerolled = true;
        }
    }

    public bool isMoving()
    {
        for(int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = gameObject.transform.position;

        for(int i = 0; i < previousLocations.Length - 1; i++)
        {
            if(Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
            {
                isDiceStop = false;
                return true;
            }
            else
            {
                isDiceStop = true;
                return false;
            }
        }

        return true;
    }

    IEnumerator waitSecond()
    {
        yield return new WaitForSeconds(1.0f);
        isDiceStop = true;
    }


    public void reroll_reset()
    {
        isDiceStop = false;
        isRerolled = false;
    }
}
