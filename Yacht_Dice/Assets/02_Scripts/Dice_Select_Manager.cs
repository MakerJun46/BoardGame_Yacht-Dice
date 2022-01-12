using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Dice_Select_Manager : MonoBehaviour
{
    public static Dice_Select_Manager instance;

    public GameObject Dice_Keep_Positions;
    public GameObject Dice_Select_Positions;
    public GameObject Dices;

    public Dice moving_min_Dice;
    public List<GameObject> rerolled_Dices;

    public bool isDiceMoving;
    public bool isDiceAllStop;
    public int sortCount;
    public int KeepDiceCount;

    public AudioSource Dice_Keep_Sound;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Reset_Values();
    }

    void Update()
    {
        if (GameManager.GetInstance().isSelecting)
        {
            if (rerolled_Dices.Count > 0)
            {
                sort_Dice();    // 소팅이 다 끝난 다음에
            }
            else
            {
                SelectDice(); // 선택 가능
            }
        }
        else
        {
            sortCount = -1;
        }
        
    }

    public void Reset_Values()
    {
        rerolled_Dices.Clear();
        sortCount = -1;
        KeepDiceCount = GameManager.GetInstance().Keep_Dices.Count;
        isDiceAllStop = false;
        isDiceMoving = false;

        for(int i = 0; i < GameManager.GetInstance().Reroll_Dices.Count; i++)
        {
            Dice_Select_Positions.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SelectDice()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit && hitInfo.transform.gameObject.tag == "Dice")
            {
                if (!hitInfo.transform.gameObject.GetComponent<Dice>().isKeep)
                    KeepDice(hitInfo.transform.gameObject);
                else
                {
                    backDice(hitInfo.transform.gameObject);
                }
            }
        }
    }

    public void sort_Dice()
    {
        foreach (GameObject go in rerolled_Dices)
        {
            if (!go.GetComponent<Dice>().isDiceStop)
                return;
        }

        isDiceAllStop = true;

        if (!isDiceMoving)
        {
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
            moving_min_Dice.transform.position = Vector3.Lerp(target.transform.position, moving_min_Dice.transform.position, Time.deltaTime * 15);

            Vector3 targetRotation;
            if (moving_min_Dice.thisTurn_Number == 6 || moving_min_Dice.thisTurn_Number == 1)
            {
                targetRotation = new Vector3(moving_min_Dice.transform.eulerAngles.x, 0, 0);
            }
            else
            {
                targetRotation = new Vector3(moving_min_Dice.transform.eulerAngles.x, 0, moving_min_Dice.transform.eulerAngles.z);
            }

            moving_min_Dice.transform.eulerAngles = Vector3.Lerp(targetRotation, moving_min_Dice.transform.eulerAngles, Time.deltaTime * 15);

            if (moving_min_Dice.transform.position == Dice_Select_Positions.transform.GetChild(sortCount).position
                && moving_min_Dice.transform.eulerAngles == targetRotation)
            {
                rerolled_Dices.Remove(moving_min_Dice.gameObject);
                isDiceMoving = false;
            }
        }
    }

    public int Dice_Number_cmp(GameObject a, GameObject b)
    {
        if (a.GetComponent<Dice>().thisTurn_Number > b.GetComponent<Dice>().thisTurn_Number)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void KeepingDice_Sort()
    {
        GameManager.GetInstance().Keep_Dices.Sort(Dice_Number_cmp);

        for(int i = 0; i < KeepDiceCount; i++)
        {
            GameManager.GetInstance().Keep_Dices[i].transform.position = Dice_Keep_Positions.transform.GetChild(i).position;
        }
    }

    public void KeepDice(GameObject dice)
    {
        Transform position = Dice_Keep_Positions.transform.GetChild(KeepDiceCount);
        dice.transform.position = position.position;

        GameManager.GetInstance().Keep_Dices.Add(dice);
        dice.GetComponent<Dice>().isKeep = true;

        GameManager.GetInstance().Reroll_Dices.Remove(dice);
        GameManager.GetInstance().getReroll_Dices();

        KeepDiceCount++;
        Dice_Select_Positions.transform.GetChild(rerolled_Dices.Count).gameObject.SetActive(false);
        sortCount = -1;

        Dice_Keep_Sound.Play();

        KeepingDice_Sort();
    }

    public void backDice(GameObject dice)
    {
        GameManager.GetInstance().Keep_Dices.Remove(dice);
        dice.GetComponent<Dice>().isKeep = false;

        GameManager.GetInstance().Reroll_Dices.Add(dice);
        GameManager.GetInstance().getReroll_Dices();

        KeepDiceCount--;
        Dice_Select_Positions.transform.GetChild(rerolled_Dices.Count - 1).gameObject.SetActive(true);
        sortCount = -1;

        Dice_Keep_Sound.Play();
    }

    public void setKeepPositionAllDices(List<GameObject> Dices)
    {
        int count = 0;
        GameManager.GetInstance().Keep_Dices.Clear();
        GameManager.GetInstance().Reroll_Dices.Clear();

        foreach(GameObject Dice in Dices)
        {
            Dice.transform.position = Dice_Keep_Positions.transform.GetChild(count).position;
            Dice.GetComponent<Rigidbody>().isKinematic = true;
            Dice.GetComponent<Dice>().isKeep = true;
            GameManager.GetInstance().Keep_Dices.Add(Dice);
            count++;
        }

        KeepDiceCount = 5;
        sortCount = -1;
        KeepingDice_Sort();
    }
}
