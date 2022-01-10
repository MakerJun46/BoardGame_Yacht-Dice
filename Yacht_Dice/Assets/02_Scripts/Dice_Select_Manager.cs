using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Select_Manager : MonoBehaviour
{
    public static Dice_Select_Manager instance;

    public GameObject Dice_Keep_Positions;
    public GameObject Dice_Select_Positions;
    public GameObject Dices;

    public Dice moving_min_Dice;
    public List<GameObject> rerolled_Dices;

    public bool isDiceMoving;
    public int sortCount;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sortCount = -1;
        isDiceMoving = false;
    }

    void Update()
    {
        if (GameManager.GetInstance().isSelecting)
        {
            mouseClick();
            if(rerolled_Dices.Count > 0)
                sort_Dice();
        }
    }


    public void mouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit && hitInfo.transform.gameObject.tag == "Dice")
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (!hitInfo.transform.gameObject.GetComponent<Dice>().isKeep)
                    KeepDice(hitInfo.transform.gameObject);
                else
                {
                    backDice(hitInfo.transform.gameObject);
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }

    public void sort_Dice()
    {
        Debug.Log("검사1");

        foreach (GameObject go in rerolled_Dices)
        {
            if (!go.GetComponent<Dice>().isDiceStop)
                return;
        }

        if (!isDiceMoving)
        {
            Debug.Log("검사2");

            moving_min_Dice = rerolled_Dices[0].GetComponent<Dice>();

            foreach (GameObject go in rerolled_Dices)
            {
                if (moving_min_Dice.thisTurn_Number > go.GetComponent<Dice>().thisTurn_Number)
                    moving_min_Dice = go.GetComponent<Dice>();
            }

            moving_min_Dice.GetComponent<Rigidbody>().isKinematic = true;

            sortCount++;
            isDiceMoving = true;
        }
        else
        {
            GameObject target = Dice_Select_Positions.transform.GetChild(sortCount).gameObject;
            moving_min_Dice.transform.position = Vector3.Lerp(Dice_Select_Positions.transform.GetChild(sortCount).position,
                                                                moving_min_Dice.transform.position, Time.deltaTime * 10);


            Quaternion currentRotation = moving_min_Dice.transform.rotation;
            Quaternion targetRotation;

            if (moving_min_Dice.thisTurn_Number == 6 || moving_min_Dice.thisTurn_Number == 1)
            {
                targetRotation = Quaternion.Euler(moving_min_Dice.transform.rotation.x, 0, 0);
            }
            else
            {
                targetRotation = Quaternion.Euler(moving_min_Dice.transform.rotation.x, 0, moving_min_Dice.transform.rotation.z);
            }

            Debug.Log("rotate to : " + targetRotation);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10);

            Debug.Log("moving dice : " + moving_min_Dice.gameObject.name);

            if (moving_min_Dice.transform.position == Dice_Select_Positions.transform.GetChild(sortCount).position
                && moving_min_Dice.transform.rotation == targetRotation)
            {
                rerolled_Dices.Remove(moving_min_Dice.gameObject);
                isDiceMoving = false;
            }
        }


    }

    public void KeepDice(GameObject dice)
    {
        dice.GetComponent<Rigidbody>().isKinematic = true;
        dice.GetComponent<RectTransform>().localPosition = new Vector3(dice.transform.position.x, dice.transform.position.y, 0);
        dice.GetComponent<Dice>().isKeep = true;

    }

    public void backDice(GameObject dice)
    {
        dice.transform.position = Dices.transform.position;
        dice.GetComponent<Rigidbody>().isKinematic = false;
        dice.GetComponent<Dice>().isKeep = false;
    }
}
