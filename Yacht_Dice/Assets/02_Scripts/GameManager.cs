using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    public Button RerollButton;
    public GameObject GameOverObjects;
    public TextMeshProUGUI GameOverText;

    public Image Induce_Select_Rect_upper;
    public Image Induce_Select_Rect_lower;

    public Transform Reroll_Cup;
    public Transform Reroll_Cup_Ready_Position;
    public Transform Reroll_Cup_Wait_Position;
    public Transform Reroll_Dice_Target_Position;
    public Transform Turn_Cross;
    public Transform Turn_Cross_Player1_Position;
    public Transform Turn_Cross_Player2_Position;
    public Animator Reroll_Cup_AN;

    public Text CanRerollCount_Text;

    public bool Reroll_Ready;
    public bool isRerolling;
    public bool isSelecting;

    public float cup_move_speed = 15;

    public List<GameObject> Dices;
    public List<GameObject> Keep_Dices;
    public List<GameObject> Reroll_Dices;
    System.Random random = new System.Random();

    public int CanRerollCount;
    public float color_a;
    public bool color_a_goingUp;

    public int Turn_player;
    public int Turn_Game;
    public TextMeshProUGUI Turn_Text;
    public TextMeshProUGUI Reroll_Count_Text;

    public bool isGameEnd;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Turn_Game = 1;
        Turn_player = 1;
        isGameEnd = false;
        Reset_Values();
    }

    void Update()
    {
        if (isGameEnd)
        {
            GameOverScene();
        }
        else
        {
            Reroll_Cup_Position_Update();
            CanRerollDetect();
            Induce_Select();
            Turn_Detect();
        }
    }

    public void ReStart_Game()
    {
        Turn_Game = 1;
        Turn_player = 1;
        isGameEnd = false;

        Reset_Values();
        Dice_Select_Manager.instance.Reset_Values();
        ScoreBoard.instance.Reset_Values();

        foreach(GameObject go in Dices)
        {
            go.GetComponent<Dice>().Reset_Values();
        }

        GameOverObjects.SetActive(false);
    }

    public void GameOverScene()
    {
        GameOverObjects.SetActive(true);

        ScoreBoard.instance.UpdateScore();

        string winner = ScoreBoard.instance.player1_Total > ScoreBoard.instance.player2_Total ? "Player A" : "Player B";

        GameOverText.text = winner + " Win!";
    }

    public void Reset_Values()
    {
        color_a = 0;
        CanRerollCount = 3;
        Reroll_Ready = false;
        isRerolling = false;
        isSelecting = false;
        color_a_goingUp = true;
    }

    public void Induce_Select()
    {
        if(CanRerollCount == 0 && isSelecting && Dice_Select_Manager.instance.rerolled_Dices.Count == 0)
        {
            Induce_Select_Rect_upper.color = new Color(1, 1, 1, color_a);
            Induce_Select_Rect_lower.color = new Color(1, 1, 1, color_a);
            color_a += color_a_goingUp ? 0.01f : -0.01f;

            if (color_a >= 1)
                color_a_goingUp = false;
            if (color_a <= 0)
                color_a_goingUp = true;
        }
        else
        {
            Induce_Select_Rect_upper.color = new Color(1, 1, 1, 0);
            Induce_Select_Rect_lower.color = new Color(1, 1, 1, 0);
        }
    }

    public void CanRerollDetect()
    {
        CanRerollCount_Text.text = CanRerollCount.ToString();

        if (CanRerollCount == 0 && RerollButton.enabled == true && isSelecting && Dice_Select_Manager.instance.rerolled_Dices.Count == 0)
        {
            Dice_Select_Manager.instance.setKeepPositionAllDices(Dices);
            RerollButton.enabled = false;
        }
        else if(CanRerollCount > 0 && RerollButton.enabled == false)
        {
            RerollButton.enabled = true;
        }
    }

    public void Turn_Detect()
    {
        if(ScoreBoard.instance.WriteScore)
        {
            Turn_player = Turn_player == 1 ? 2 : 1;
            if(Turn_player == 1)
            {
                Turn_Game++;
                if(Turn_Game == 13)
                {
                    isGameEnd = true;
                    return;
                }
            }
            ScoreBoard.instance.WriteScore = false;
            Turn_Change();
        }

        if(Turn_player == 1)
        {
            Turn_Cross.localPosition = Vector3.Lerp(Turn_Cross.localPosition, Turn_Cross_Player1_Position.localPosition, Time.deltaTime * 10);
            Induce_Select_Rect_upper.gameObject.transform.localPosition = new Vector3(84, Induce_Select_Rect_upper.transform.localPosition.y);
            Induce_Select_Rect_lower.gameObject.transform.localPosition = new Vector3(84, Induce_Select_Rect_lower.transform.localPosition.y);
        }
        else
        {
            Turn_Cross.localPosition = Vector3.Lerp(Turn_Cross.localPosition, Turn_Cross_Player2_Position.localPosition, Time.deltaTime * 10);
            Induce_Select_Rect_upper.gameObject.transform.localPosition = new Vector3(324, Induce_Select_Rect_upper.transform.localPosition.y);
            Induce_Select_Rect_lower.gameObject.transform.localPosition = new Vector3(324, Induce_Select_Rect_lower.transform.localPosition.y);
        }

        Turn_Text.text = Turn_Game + " /12";
    }

    public void Turn_Change()
    {
        Reroll_Dices.Clear();
        Keep_Dices.Clear();
        Dice_Select_Manager.instance.Reset_Values();
        foreach(GameObject go in Dices)
        {
            go.GetComponent<Dice>().Reset_Values();
            Reroll_Dices.Add(go);
        }

        Reset_Values();
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
            go.GetComponent<Dice>().isRerolled = false;
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
            Reroll_Dices[i].GetComponent<Rigidbody>().AddForce((Reroll_Dice_Target_Position.position - Reroll_Dices[i].transform.position) * randomPower, ForceMode.Impulse);
            Reroll_Dices[i].GetComponent<Rigidbody>().AddTorque(Vector3.left * randomRotate, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.45f);
        Reroll_Cup.transform.rotation = Quaternion.Euler(0, 0, 0);

        Reroll_Ready = false;
        isRerolling = false;
        Reroll_Cup_AN.enabled = false;
        isSelecting = true;
        ScoreBoard.instance.expectedScore_Updated = false;
        ScoreBoard.instance.isSpecialScore_Detected = false;

        foreach (GameObject go in Reroll_Dices)
        {
            Dice_Select_Manager.instance.rerolled_Dices.Add(go);
        }
    }


    public void Reroll_Button()
    {
        int offset = 25;

        foreach (GameObject go in Reroll_Dices)
        {
            go.GetComponent<Rigidbody>().isKinematic = false;
            go.transform.position = new Vector3(0, 0, offset);
            go.GetComponent<Rigidbody>().velocity = Vector3.zero;
            offset += 5;
        }

        Dice_Select_Manager.instance.Reset_Values();
        Dice_Select_Manager.instance.isDiceAllStop = false;
        isSelecting = false;
        Reroll_Ready = true;

        CanRerollCount--;
    }
    
    public void getReroll_Dices()
    {
        foreach(GameObject go in Reroll_Dices)
        {
            Dice_Select_Manager.instance.rerolled_Dices.Add(go);
        }
    }

}
