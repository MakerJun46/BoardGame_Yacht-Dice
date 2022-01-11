using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;

    public Text SpecialScore_Text;

    Calc_Dice_Score score_Calculator = new Calc_Dice_Score();

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

    public List<TextMeshProUGUI> player1_upperSection_Buttons;
    public List<TextMeshProUGUI> player2_upperSection_Buttons;
    public List<TextMeshProUGUI> player1_lowerSection_Buttons;
    public List<TextMeshProUGUI> player2_lowerSection_Buttons;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player1_Subtotal = 0;
        player2_Subtotal = 0;
        player1_Total = 0;
        player2_Total = 0;

        player1_upperSection_Buttons.Add(Player1_Aces);
        player1_upperSection_Buttons.Add(Player1_Dueces);
        player1_upperSection_Buttons.Add(Player1_Threes);
        player1_upperSection_Buttons.Add(Player1_Fours);
        player1_upperSection_Buttons.Add(Player1_Fives);
        player1_upperSection_Buttons.Add(Player1_Sixes);

        player2_upperSection_Buttons.Add(Player2_Aces);
        player2_upperSection_Buttons.Add(Player2_Dueces);
        player2_upperSection_Buttons.Add(Player2_Threes);
        player2_upperSection_Buttons.Add(Player2_Fours);
        player2_upperSection_Buttons.Add(Player2_Fives);
        player2_upperSection_Buttons.Add(Player2_Sixes);

        player1_lowerSection_Buttons.Add(Player1_Choice);
        player1_lowerSection_Buttons.Add(Player1_4ofaKind);
        player1_lowerSection_Buttons.Add(Player1_FullHouse);
        player1_lowerSection_Buttons.Add(Player1_S_Straight);
        player1_lowerSection_Buttons.Add(Player1_L_Straight);
        player1_lowerSection_Buttons.Add(Player1_Yacht);

        player2_lowerSection_Buttons.Add(Player2_Choice);
        player2_lowerSection_Buttons.Add(Player2_4ofaKind);
        player2_lowerSection_Buttons.Add(Player2_FullHouse);
        player2_lowerSection_Buttons.Add(Player2_S_Straight);
        player2_lowerSection_Buttons.Add(Player2_L_Straight);
        player2_lowerSection_Buttons.Add(Player2_Yacht);

        score_Calculator.connect_Text(SpecialScore_Text);
    }

    void Update()
    {
        if (GameManager.GetInstance().isSelecting && Dice_Select_Manager.instance.isDiceAllStop)
        {
            score_Calculator.detect_special_score();
        }

        if (!player1_get_bonus || !player2_get_bonus)
            BonusScoreDetect();

        UpdateScore();
        Update_expected_Score();
    }

    public void Update_expected_Score()
    {
        if(!GameManager.GetInstance().isSelecting)
        {
            
        }
    }

    public void UpdateScore()
    {
        Player1_Total_Text.text = (player1_Total + player1_Subtotal).ToString();
        Player2_Total_Text.text = (player2_Total + player2_Subtotal).ToString();

        Player1_Subtotal_Text.text = player1_Subtotal.ToString();
        Player2_Subtotal_Text.text = player2_Subtotal.ToString();
    }

    public void BonusScoreDetect()
    {
        if(player1_Subtotal >= 63 && !player1_get_bonus)
        {
            player1_get_bonus = true;
            Player1_Bonus_Text.text = "+35";
        }

        if(player2_Subtotal >= 63 && !player2_get_bonus)
        {
            player2_get_bonus = true;
            Player2_Bonus_Text.text = "+35";
        }
    }

    public void AcesButton()
    {
        if(GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Aces.text = score_Calculator.AcesScore.ToString();
            player1_Subtotal += score_Calculator.AcesScore;
        }
        else
        {
            Player2_Aces.text = score_Calculator.AcesScore.ToString();
            player2_Subtotal += score_Calculator.AcesScore;
        }
    }

    public void DeucesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Dueces.text = score_Calculator.DeucesScore.ToString();
            player1_Subtotal += score_Calculator.DeucesScore;
        }
        else
        {
            Player2_Dueces.text = score_Calculator.DeucesScore.ToString();
            player2_Subtotal += score_Calculator.DeucesScore;
        }
    }

    public void ThreesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Threes.text = score_Calculator.ThreesScore.ToString();
            player1_Subtotal += score_Calculator.ThreesScore;
        }
        else
        {
            Player2_Threes.text = score_Calculator.ThreesScore.ToString();
            player2_Subtotal += score_Calculator.ThreesScore;
        }
    }

    public void FoursButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Fours.text = score_Calculator.FoursScore.ToString();
            player1_Subtotal += score_Calculator.FoursScore;
        }
        else
        {
            Player2_Fours.text = score_Calculator.FoursScore.ToString();
            player2_Subtotal += score_Calculator.FoursScore;
        }
    }

    public void FivesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Fives.text = score_Calculator.FivesScore.ToString();
            player1_Subtotal += score_Calculator.FivesScore;
        }
        else
        {
            Player2_Fives.text = score_Calculator.FivesScore.ToString();
            player2_Subtotal += score_Calculator.FivesScore;
        }
    }

    public void SixesButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Sixes.text = score_Calculator.SixesScore.ToString();
            player1_Subtotal += score_Calculator.SixesScore;
        }
        else
        {
            Player2_Sixes.text = score_Calculator.SixesScore.ToString();
            player2_Subtotal += score_Calculator.SixesScore;
        }
    }

    public void ChoiceButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Choice.text = score_Calculator.ChoiceScore.ToString();
            player1_Total += score_Calculator.ChoiceScore;
        }
        else
        {
            Player2_Choice.text = score_Calculator.ChoiceScore.ToString();
            player2_Total += score_Calculator.ChoiceScore;
        }
    }

    public void FourofaKindButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_4ofaKind.text = score_Calculator.FourofaKindScore.ToString();
            player1_Total += score_Calculator.FourofaKindScore;
        }
        else
        {
            Player2_4ofaKind.text = score_Calculator.FourofaKindScore.ToString();
            player2_Total += score_Calculator.FourofaKindScore;
        }
    }

    public void FullHouseButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_FullHouse.text = score_Calculator.FullHouseScore.ToString();
            player1_Total += score_Calculator.FullHouseScore;
        }
        else
        {
            Player2_FullHouse.text = score_Calculator.FullHouseScore.ToString();
            player2_Total += score_Calculator.FullHouseScore;
        }
    }

    public void S_StraightButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_S_Straight.text = score_Calculator.S_StraightScore.ToString();
            player1_Total += score_Calculator.S_StraightScore;
        }
        else
        {
            Player2_S_Straight.text = score_Calculator.S_StraightScore.ToString();
            player2_Total += score_Calculator.S_StraightScore;
        }
    }

    public void L_StraightButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_L_Straight.text = score_Calculator.L_StraightScore.ToString();
            player1_Total += score_Calculator.L_StraightScore;
        }
        else
        {
            Player2_L_Straight.text = score_Calculator.L_StraightScore.ToString();
            player2_Total += score_Calculator.L_StraightScore;
        }
    }

    public void YachtButton()
    {
        if (GameManager.GetInstance().Turn_player == 1)
        {
            Player1_Yacht.text = score_Calculator.YachtScore.ToString();
            player1_Total += score_Calculator.YachtScore;
        }
        else
        {
            Player2_Yacht.text = score_Calculator.YachtScore.ToString();
            player2_Total += score_Calculator.YachtScore;
        }
    }
}
