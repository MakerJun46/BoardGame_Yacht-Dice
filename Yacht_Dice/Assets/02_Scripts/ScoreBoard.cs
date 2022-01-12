using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class Pair<T, U>
{
    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;

    public TextMeshProUGUI SpecialScore_Text;

    Calc_Dice_Score score_Calculator;

    public TextMeshProUGUI Player1_Aces;
    public TextMeshProUGUI Player1_Dueces;
    public TextMeshProUGUI Player1_Threes;
    public TextMeshProUGUI Player1_Fours;
    public TextMeshProUGUI Player1_Fives;
    public TextMeshProUGUI Player1_Sixes;

    public TextMeshProUGUI Player1_Choice;
    public TextMeshProUGUI Player1_4ofaKind;
    public TextMeshProUGUI Player1_FullHouse;
    public TextMeshProUGUI Player1_S_Straight;
    public TextMeshProUGUI Player1_L_Straight;
    public TextMeshProUGUI Player1_Yacht;

    public TextMeshProUGUI Player2_Aces;
    public TextMeshProUGUI Player2_Dueces;
    public TextMeshProUGUI Player2_Threes;
    public TextMeshProUGUI Player2_Fours;
    public TextMeshProUGUI Player2_Fives;
    public TextMeshProUGUI Player2_Sixes;

    public TextMeshProUGUI Player2_Choice;
    public TextMeshProUGUI Player2_4ofaKind;
    public TextMeshProUGUI Player2_FullHouse;
    public TextMeshProUGUI Player2_S_Straight;
    public TextMeshProUGUI Player2_L_Straight;
    public TextMeshProUGUI Player2_Yacht;

    public TextMeshProUGUI Player1_Subtotal_Text;
    public TextMeshProUGUI Player2_Subtotal_Text;
    public TextMeshProUGUI Player1_Bonus_Text;
    public TextMeshProUGUI Player2_Bonus_Text;
    public TextMeshProUGUI Player1_Total_Text;
    public TextMeshProUGUI Player2_Total_Text;

    public int player1_Subtotal;
    public int player2_Subtotal;
    public int player1_Total;
    public int player2_Total;

    public bool player1_get_bonus;
    public bool player2_get_bonus;

    public List<Pair<TextMeshProUGUI, bool>> player1_upperSection_Buttons;
    public List<Pair<TextMeshProUGUI, bool>> player2_upperSection_Buttons;
    public List<Pair<TextMeshProUGUI, bool>> player1_lowerSection_Buttons;
    public List<Pair<TextMeshProUGUI, bool>> player2_lowerSection_Buttons;

    public bool expectedScore_Updated;
    public bool WriteScore;
    public bool isSpecialScore_Detected;
    public Animation SpecialScore_Text_AN;

    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
        Reset_Values();
        SpecialScore_Text_AN = SpecialScore_Text.GetComponent<Animation>();
    }

    void Update()
    {
        if (GameManager.GetInstance().isSelecting && Dice_Select_Manager.instance.isDiceAllStop)
        {
            if(!isSpecialScore_Detected)
            {
                score_Calculator.detect_special_score();
                UpdateSpecialScore_Text();
            }
            Update_expected_Score();
            buttonEnable_Selecting();
        }
        else if (!GameManager.GetInstance().isSelecting)
        {
            UpdateScore();
            buttonEnable_False();
        }

        if (!player1_get_bonus || !player2_get_bonus)
            BonusScoreDetect();
    }

    public void Reset_Values()
    {
        score_Calculator = new Calc_Dice_Score();

        player1_upperSection_Buttons = new List<Pair<TextMeshProUGUI, bool>>();
        player2_upperSection_Buttons = new List<Pair<TextMeshProUGUI, bool>>();
        player1_lowerSection_Buttons = new List<Pair<TextMeshProUGUI, bool>>();
        player2_lowerSection_Buttons = new List<Pair<TextMeshProUGUI, bool>>();

        player1_Subtotal = 0;
        player2_Subtotal = 0;
        player1_Total = 0;
        player2_Total = 0;

        expectedScore_Updated = false;
        WriteScore = false;
        isSpecialScore_Detected = false;

        player1_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Aces, false));
        player1_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Dueces, false));
        player1_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Threes, false));
        player1_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Fours, false));
        player1_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Fives, false));
        player1_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Sixes, false));

        player2_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Aces, false));
        player2_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Dueces, false));
        player2_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Threes, false));
        player2_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Fours, false));
        player2_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Fives, false));
        player2_upperSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Sixes, false));

        player1_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Choice, false));
        player1_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_4ofaKind, false));
        player1_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_FullHouse, false));
        player1_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_S_Straight, false));
        player1_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_L_Straight, false));
        player1_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player1_Yacht, false));

        player2_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Choice, false));
        player2_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_4ofaKind, false));
        player2_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_FullHouse, false));
        player2_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_S_Straight, false));
        player2_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_L_Straight, false));
        player2_lowerSection_Buttons.Add(new Pair<TextMeshProUGUI, bool>(Player2_Yacht, false));

        foreach(Pair<TextMeshProUGUI, bool> p in player1_upperSection_Buttons)
        {
            p.First.text = "";
            p.First.color = new Color(0, 0, 0, 0.5f);
        }
        foreach (Pair<TextMeshProUGUI, bool> p in player2_upperSection_Buttons)
        {
            p.First.text = "";
            p.First.color = new Color(0, 0, 0, 0.5f);
        }
        foreach (Pair<TextMeshProUGUI, bool> p in player1_lowerSection_Buttons)
        {
            p.First.text = "";
            p.First.color = new Color(0, 0, 0, 0.5f);
        }
        foreach (Pair<TextMeshProUGUI, bool> p in player2_lowerSection_Buttons)
        {
            p.First.text = "";
            p.First.color = new Color(0, 0, 0, 0.5f);
        }
    }

    public void buttonEnable_Selecting()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            foreach (Pair<TextMeshProUGUI, bool> p in player1_upperSection_Buttons)
            {
                if (!p.Second)
                    p.First.gameObject.transform.parent.GetComponent<Button>().enabled = true;
            }
            foreach (Pair<TextMeshProUGUI, bool> p in player1_lowerSection_Buttons)
            {
                if (!p.Second)
                    p.First.gameObject.transform.parent.GetComponent<Button>().enabled = true;
            }
        }
        else
        {
            foreach (Pair<TextMeshProUGUI, bool> p in player2_upperSection_Buttons)
            {
                if(!p.Second)
                    p.First.gameObject.transform.parent.GetComponent<Button>().enabled = true;
            }
            foreach (Pair<TextMeshProUGUI, bool> p in player2_lowerSection_Buttons)
            {
                if (!p.Second)
                    p.First.gameObject.transform.parent.GetComponent<Button>().enabled = true;
            }
        }
    }

    public void buttonEnable_False()
    {
        foreach (Pair<TextMeshProUGUI, bool> p in player2_upperSection_Buttons)
        {
            p.First.gameObject.transform.parent.GetComponent<Button>().enabled = false;
        }
        foreach (Pair<TextMeshProUGUI, bool> p in player2_lowerSection_Buttons)
        {
            p.First.gameObject.transform.parent.GetComponent<Button>().enabled = false;
        }
        foreach (Pair<TextMeshProUGUI, bool> p in player1_upperSection_Buttons)
        {
            p.First.gameObject.transform.parent.GetComponent<Button>().enabled = false;
        }
        foreach (Pair<TextMeshProUGUI, bool> p in player1_lowerSection_Buttons)
        {
            p.First.gameObject.transform.parent.GetComponent<Button>().enabled = false;
        }

    }

    public void Update_expected_Score()
    {
        if (!expectedScore_Updated)
        {
            if (GameManager.GetInstance().Turn_player == 1)
            {
                if (!player1_upperSection_Buttons[0].Second) Player1_Aces.text = score_Calculator.AcesScore.ToString();
                if (!player1_upperSection_Buttons[1].Second) Player1_Dueces.text = score_Calculator.DeucesScore.ToString();
                if (!player1_upperSection_Buttons[2].Second) Player1_Threes.text = score_Calculator.ThreesScore.ToString();
                if (!player1_upperSection_Buttons[3].Second) Player1_Fours.text = score_Calculator.FoursScore.ToString();
                if (!player1_upperSection_Buttons[4].Second) Player1_Fives.text = score_Calculator.FivesScore.ToString();
                if (!player1_upperSection_Buttons[5].Second) Player1_Sixes.text = score_Calculator.SixesScore.ToString();

                if (!player1_lowerSection_Buttons[0].Second) Player1_Choice.text = score_Calculator.ChoiceScore.ToString();
                if (!player1_lowerSection_Buttons[1].Second) Player1_4ofaKind.text = score_Calculator.FourofaKindScore.ToString();
                if (!player1_lowerSection_Buttons[2].Second) Player1_FullHouse.text = score_Calculator.FullHouseScore.ToString();
                if (!player1_lowerSection_Buttons[3].Second) Player1_S_Straight.text = score_Calculator.S_StraightScore.ToString();
                if (!player1_lowerSection_Buttons[4].Second) Player1_L_Straight.text = score_Calculator.L_StraightScore.ToString();
                if (!player1_lowerSection_Buttons[5].Second) Player1_Yacht.text = score_Calculator.YachtScore.ToString();
            }
            else
            {
                if (!player2_upperSection_Buttons[0].Second) Player2_Aces.text = score_Calculator.AcesScore.ToString();
                if (!player2_upperSection_Buttons[1].Second) Player2_Dueces.text = score_Calculator.DeucesScore.ToString();
                if (!player2_upperSection_Buttons[2].Second) Player2_Threes.text = score_Calculator.ThreesScore.ToString();
                if (!player2_upperSection_Buttons[3].Second) Player2_Fours.text = score_Calculator.FoursScore.ToString();
                if (!player2_upperSection_Buttons[4].Second) Player2_Fives.text = score_Calculator.FivesScore.ToString();
                if (!player2_upperSection_Buttons[5].Second) Player2_Sixes.text = score_Calculator.SixesScore.ToString();

                if (!player2_lowerSection_Buttons[0].Second) Player2_Choice.text = score_Calculator.ChoiceScore.ToString();
                if (!player2_lowerSection_Buttons[1].Second) Player2_4ofaKind.text = score_Calculator.FourofaKindScore.ToString();
                if (!player2_lowerSection_Buttons[2].Second) Player2_FullHouse.text = score_Calculator.FullHouseScore.ToString();
                if (!player2_lowerSection_Buttons[3].Second) Player2_S_Straight.text = score_Calculator.S_StraightScore.ToString();
                if (!player2_lowerSection_Buttons[4].Second) Player2_L_Straight.text = score_Calculator.L_StraightScore.ToString();
                if (!player2_lowerSection_Buttons[5].Second) Player2_Yacht.text = score_Calculator.YachtScore.ToString();
            }

            expectedScore_Updated = true;
        }
    }

    public void UpdateScore()
    {
        Player1_Total_Text.text = player1_Total.ToString();
        Player2_Total_Text.text = player2_Total.ToString();

        Player1_Subtotal_Text.text = player1_Subtotal.ToString();
        Player2_Subtotal_Text.text = player2_Subtotal.ToString();

        foreach (Pair<TextMeshProUGUI, bool> text in player1_lowerSection_Buttons)
        {
            if (!text.Second)
                text.First.text = "";
        }
        foreach (Pair<TextMeshProUGUI, bool> text in player2_lowerSection_Buttons)
        {
            if (!text.Second)
                text.First.text = "";
        }
        foreach (Pair<TextMeshProUGUI, bool> text in player1_upperSection_Buttons)
        {
            if (!text.Second)
                text.First.text = "";
        }
        foreach (Pair<TextMeshProUGUI, bool> text in player2_upperSection_Buttons)
        {
            if (!text.Second)
                text.First.text = "";
        }
    }

    public void UpdateSpecialScore_Text()
    {
        SpecialScore_Text.text = "";
        if (score_Calculator.isYacht)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "Yacht!";
            SpecialScore_Text_AN.Play();
            GameManager.GetInstance().Special_Score_Sound_Audio.clip = GameManager.GetInstance().Special_Score_Sound;
            GameManager.GetInstance().Special_Score_Sound_Audio.Play();
        }
        if (score_Calculator.isLargeStraight)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "BigStraight!";
            SpecialScore_Text_AN.Play();
            GameManager.GetInstance().Special_Score_Sound_Audio.clip = GameManager.GetInstance().Special_Score_Sound;
            GameManager.GetInstance().Special_Score_Sound_Audio.Play();
        }
        if (score_Calculator.isSmallStraight)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "LittleStraight!";
            SpecialScore_Text_AN.Play();
            GameManager.GetInstance().Special_Score_Sound_Audio.clip = GameManager.GetInstance().Special_Score_Sound;
            GameManager.GetInstance().Special_Score_Sound_Audio.Play();
        }
        if (score_Calculator.isFullHouse)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "FullHouse!";
            SpecialScore_Text_AN.Play();
            GameManager.GetInstance().Special_Score_Sound_Audio.clip = GameManager.GetInstance().Special_Score_Sound;
            GameManager.GetInstance().Special_Score_Sound_Audio.Play();
        }
        if (score_Calculator.isFourofaKind)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "FourofaKind!";
            SpecialScore_Text_AN.Play();
            GameManager.GetInstance().Special_Score_Sound_Audio.clip = GameManager.GetInstance().Special_Score_Sound;
            GameManager.GetInstance().Special_Score_Sound_Audio.Play();
        }
    }

    public void BonusScoreDetect()
    {
        if (player1_Subtotal >= 63 && !player1_get_bonus)
        {
            player1_get_bonus = true;
            Player1_Bonus_Text.text = "+35";
            player1_Total += 35;
        }

        if (player2_Subtotal >= 63 && !player2_get_bonus)
        {
            player2_get_bonus = true;
            Player2_Bonus_Text.text = "+35";
            player2_Total += 35;
        }
    }

    public void AcesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Aces.text = score_Calculator.AcesScore.ToString();
            player1_Subtotal += score_Calculator.AcesScore;
            player1_Total += score_Calculator.AcesScore;

            Player1_Aces.fontStyle = FontStyles.Bold;
            Player1_Aces.color = new Color(0, 0, 0, 1);

            player1_upperSection_Buttons[0].Second = true;

            GameObject.Find("Player1_Aces").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Aces.text = score_Calculator.AcesScore.ToString();
            player2_Subtotal += score_Calculator.AcesScore;
            player2_Total += score_Calculator.AcesScore;

            Player2_Aces.fontStyle = FontStyles.Bold;
            Player2_Aces.color = new Color(0, 0, 0, 1);

            player2_upperSection_Buttons[0].Second = true;

            GameObject.Find("Player2_Aces").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void DeucesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Dueces.text = score_Calculator.DeucesScore.ToString();
            player1_Subtotal += score_Calculator.DeucesScore;
            player1_Total += score_Calculator.DeucesScore;

            Player1_Dueces.fontStyle = FontStyles.Bold;
            Player1_Dueces.color = new Color(0, 0, 0, 1);

            player1_upperSection_Buttons[1].Second = true;

            GameObject.Find("Player1_Deuces").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Dueces.text = score_Calculator.DeucesScore.ToString();
            player2_Subtotal += score_Calculator.DeucesScore;
            player2_Total += score_Calculator.DeucesScore;

            Player2_Dueces.fontStyle = FontStyles.Bold;
            Player2_Dueces.color = new Color(0, 0, 0, 1);

            player2_upperSection_Buttons[1].Second = true;

            GameObject.Find("Player2_Deuces").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void ThreesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Threes.text = score_Calculator.ThreesScore.ToString();
            player1_Subtotal += score_Calculator.ThreesScore;
            player1_Total += score_Calculator.ThreesScore;

            Player1_Threes.fontStyle = FontStyles.Bold;
            Player1_Threes.color = new Color(0, 0, 0, 1);

            player1_upperSection_Buttons[2].Second = true;

            GameObject.Find("Player1_Threes").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Threes.text = score_Calculator.ThreesScore.ToString();
            player2_Subtotal += score_Calculator.ThreesScore;
            player2_Total += score_Calculator.ThreesScore;

            Player2_Threes.fontStyle = FontStyles.Bold;
            Player2_Threes.color = new Color(0, 0, 0, 1);

            player2_upperSection_Buttons[2].Second = true;

            GameObject.Find("Player2_Threes").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void FoursButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Fours.text = score_Calculator.FoursScore.ToString();
            player1_Subtotal += score_Calculator.FoursScore;
            player1_Total += score_Calculator.FoursScore;

            Player1_Fours.fontStyle = FontStyles.Bold;
            Player1_Fours.color = new Color(0, 0, 0, 1);

            player1_upperSection_Buttons[3].Second = true;

            GameObject.Find("Player1_Fours").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Fours.text = score_Calculator.FoursScore.ToString();
            player2_Subtotal += score_Calculator.FoursScore;
            player2_Total += score_Calculator.FoursScore;

            Player2_Fours.fontStyle = FontStyles.Bold;
            Player2_Fours.color = new Color(0, 0, 0, 1);

            player2_upperSection_Buttons[3].Second = true;

            GameObject.Find("Player2_Fours").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void FivesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Fives.text = score_Calculator.FivesScore.ToString();
            player1_Subtotal += score_Calculator.FivesScore;
            player1_Total += score_Calculator.FivesScore;

            Player1_Fives.fontStyle = FontStyles.Bold;
            Player1_Fives.color = new Color(0, 0, 0, 1);

            player1_upperSection_Buttons[4].Second = true;

            GameObject.Find("Player1_Fives").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Fives.text = score_Calculator.FivesScore.ToString();
            player2_Subtotal += score_Calculator.FivesScore;
            player2_Total += score_Calculator.FivesScore;

            Player2_Fives.fontStyle = FontStyles.Bold;
            Player2_Fives.color = new Color(0, 0, 0, 1);

            player2_upperSection_Buttons[4].Second = true;

            GameObject.Find("Player2_Fives").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void SixesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Sixes.text = score_Calculator.SixesScore.ToString();
            player1_Subtotal += score_Calculator.SixesScore;
            player1_Total += score_Calculator.SixesScore;

            Player1_Sixes.fontStyle = FontStyles.Bold;
            Player1_Sixes.color = new Color(0, 0, 0, 1);

            player1_upperSection_Buttons[5].Second = true;

            GameObject.Find("Player1_Sixes").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Sixes.text = score_Calculator.SixesScore.ToString();
            player2_Subtotal += score_Calculator.SixesScore;
            player2_Total += score_Calculator.SixesScore;

            Player2_Sixes.fontStyle = FontStyles.Bold;
            Player2_Sixes.color = new Color(0, 0, 0, 1);

            player2_upperSection_Buttons[5].Second = true;

            GameObject.Find("Player2_Sixes").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void ChoiceButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Choice.text = score_Calculator.ChoiceScore.ToString();
            player1_Total += score_Calculator.ChoiceScore;

            Player1_Choice.fontStyle = FontStyles.Bold;
            Player1_Choice.color = new Color(0, 0, 0, 1);

            player1_lowerSection_Buttons[0].Second = true;

            GameObject.Find("Player1_Choice").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Choice.text = score_Calculator.ChoiceScore.ToString();
            player2_Total += score_Calculator.ChoiceScore;

            Player2_Choice.fontStyle = FontStyles.Bold;
            Player2_Choice.color = new Color(0, 0, 0, 1);

            player2_lowerSection_Buttons[0].Second = true;

            GameObject.Find("Player2_Choice").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void FourofaKindButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_4ofaKind.text = score_Calculator.FourofaKindScore.ToString();
            player1_Total += score_Calculator.FourofaKindScore;

            Player1_4ofaKind.fontStyle = FontStyles.Bold;
            Player1_4ofaKind.color = new Color(0, 0, 0, 1);

            player1_lowerSection_Buttons[1].Second = true;

            GameObject.Find("Player1_4ofaKind").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_4ofaKind.text = score_Calculator.FourofaKindScore.ToString();
            player2_Total += score_Calculator.FourofaKindScore;

            Player2_4ofaKind.fontStyle = FontStyles.Bold;
            Player2_4ofaKind.color = new Color(0, 0, 0, 1);

            player2_lowerSection_Buttons[1].Second = true;

            GameObject.Find("Player2_4ofaKind").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void FullHouseButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_FullHouse.text = score_Calculator.FullHouseScore.ToString();
            player1_Total += score_Calculator.FullHouseScore;

            Player1_FullHouse.fontStyle = FontStyles.Bold;
            Player1_FullHouse.color = new Color(0, 0, 0, 1);

            player1_lowerSection_Buttons[2].Second = true;

            GameObject.Find("Player1_FullHouse").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_FullHouse.text = score_Calculator.FullHouseScore.ToString();
            player2_Total += score_Calculator.FullHouseScore;

            Player2_FullHouse.fontStyle = FontStyles.Bold;
            Player2_FullHouse.color = new Color(0, 0, 0, 1);

            player2_lowerSection_Buttons[2].Second = true;

            GameObject.Find("Player2_FullHouse").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void S_StraightButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_S_Straight.text = score_Calculator.S_StraightScore.ToString();
            player1_Total += score_Calculator.S_StraightScore;

            Player1_S_Straight.fontStyle = FontStyles.Bold;
            Player1_S_Straight.color = new Color(0, 0, 0, 1);

            player1_lowerSection_Buttons[3].Second = true;

            GameObject.Find("Player1_S_Straight").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_S_Straight.text = score_Calculator.S_StraightScore.ToString();
            player2_Total += score_Calculator.S_StraightScore;

            Player2_S_Straight.fontStyle = FontStyles.Bold;
            Player2_S_Straight.color = new Color(0, 0, 0, 1);

            player2_lowerSection_Buttons[3].Second = true;

            GameObject.Find("Player2_S_Straight").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void L_StraightButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_L_Straight.text = score_Calculator.L_StraightScore.ToString();
            player1_Total += score_Calculator.L_StraightScore;

            Player1_L_Straight.fontStyle = FontStyles.Bold;
            Player1_L_Straight.color = new Color(0, 0, 0, 1);

            player1_lowerSection_Buttons[4].Second = true;

            GameObject.Find("Player1_L_Straight").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_L_Straight.text = score_Calculator.L_StraightScore.ToString();
            player2_Total += score_Calculator.L_StraightScore;

            Player2_L_Straight.fontStyle = FontStyles.Bold;
            Player2_L_Straight.color = new Color(0, 0, 0, 1);

            player2_lowerSection_Buttons[4].Second = true;

            GameObject.Find("Player2_L_Straight").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void YachtButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Yacht.text = score_Calculator.YachtScore.ToString();
            player1_Total += score_Calculator.YachtScore;

            Player1_Yacht.fontStyle = FontStyles.Bold;
            Player1_Yacht.color = new Color(0, 0, 0, 1);

            player1_lowerSection_Buttons[5].Second = true;

            GameObject.Find("Player1_Yacht").GetComponent<Button>().enabled = false;
        }
        else
        {
            Player2_Yacht.text = score_Calculator.YachtScore.ToString();
            player2_Total += score_Calculator.YachtScore;

            Player2_Yacht.fontStyle = FontStyles.Bold;
            Player2_Yacht.color = new Color(0, 0, 0, 1);

            player2_lowerSection_Buttons[5].Second = true;

            GameObject.Find("Player2_Yacht").GetComponent<Button>().enabled = false;
        }

        WriteScore = true;
    }

    public void Select_Button()
    {
        GameManager.GetInstance().isSelecting = false;
    }
}
