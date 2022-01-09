using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform Reroll_Cup;
    public Transform Reroll_Cup_Ready_Position;
    public Transform Reroll_Cup_Wait_Position;
    public Transform Reroll_Dice_Target_Position;
    public Animator Reroll_Cup_AN;

    public bool Reroll_Ready;
    public bool isRerolling;

    public float cup_move_speed = 15;

    public List<GameObject> Kepp_Dices;
    public List<GameObject> Reroll_Dices;
    System.Random random = new System.Random();

    void Start()
    {
        Reroll_Ready = false;
        isRerolling = false;
    }

    void Update()
    {
        Reroll_Cup_Position_Update();
    }

    public void Reroll_Cup_Position_Update()
    {
        if (Reroll_Ready)
        {
            Reroll_Cup.position = Vector3.Lerp(Reroll_Cup.position, Reroll_Cup_Ready_Position.position, Time.deltaTime * cup_move_speed);

            if (Reroll_Cup.position == Reroll_Cup_Ready_Position.position && !isRerolling)
            {
                Reroll_Cup_AN.enabled = true;
                StartCoroutine(reroll_anim());
                isRerolling = true;
            }
        }

        else
        {
            Reroll_Cup.position = Vector3.Lerp(Reroll_Cup.position, Reroll_Cup_Wait_Position.position, Time.deltaTime * cup_move_speed);
        }
    }

    IEnumerator reroll_anim()
    {
        foreach(GameObject go in Reroll_Dices)
        {
            go.transform.position = go.GetComponent<Dice>().Reroll_Position.position;
            go.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        Reroll_Cup_AN.SetTrigger("Reroll");

        for(int i = 0; i <Reroll_Dices.Count; i++)
        {
            int randomRotate = random.Next(10, 20);
            Reroll_Dices[i].GetComponent<Rigidbody>().AddTorque(Vector3.up * randomRotate, ForceMode.Impulse);
        }


        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < Reroll_Dices.Count; i++)
        {
            int randomPower = random.Next(3, 6);
            int randomRotate = random.Next(20, 30);
            Debug.Log("added force : " + Reroll_Dices[i].name + " , " + randomPower.ToString());
            Reroll_Dices[i].GetComponent<Rigidbody>().AddForce((Reroll_Dice_Target_Position.position - Reroll_Dices[i].transform.position) * randomPower, ForceMode.Impulse);
            Reroll_Dices[i].GetComponent<Rigidbody>().AddTorque(Vector3.left * randomRotate, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.4f);
        Reroll_Cup.transform.rotation = Quaternion.Euler(0, 0, 0);

        Reroll_Ready = false;
        isRerolling = false;
        Reroll_Cup_AN.enabled = false;
    }


    public void Reroll_Button()
    {
        int offset = 25;

        foreach(GameObject go in Reroll_Dices)
        {
            go.transform.position = new Vector3(0, 0, offset);
            go.GetComponent<Rigidbody>().velocity = Vector3.zero;
            offset += 5;
        }

        Reroll_Ready = !Reroll_Ready;
    }


}
