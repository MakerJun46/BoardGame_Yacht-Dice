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
        if(RB.velocity == Vector3.zero && !isRerolled)
        {
            isDiceStop = true;

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
}
