using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calc_Dice_Score : MonoBehaviour
{
    public bool isYacht;
    public bool isLargeStraight;
    public bool isSmallStraight;
    public bool isFullHouse;
    public bool isFourofaKind;

    public int AcesScore;
    public int DeucesScore;
    public int ThreesScore;
    public int FoursScore;
    public int FivesScore;
    public int SixesScore;

    public int ChoiceScore;
    public int FourofaKindScore;
    public int FullHouseScore;
    public int S_StraightScore;
    public int L_StraightScore;
    public int YachtScore;

    public Text SpecialScore_Text;

    public void detect_special_score()
    {
        List<Dice> Dices = new List<Dice>();
        foreach (GameObject go in GameManager.GetInstance().Dices)
        {
            Dices.Add(go.GetComponent<Dice>());
        }

        Dices.Sort(Dice_Number_cmp);

        isYacht = is_Yacht(Dices);
        isLargeStraight = is_LargeStraight(Dices);
        isSmallStraight = is_SmallStraight(Dices);
        isFullHouse = is_FullHouse(Dices);
        isFourofaKind = is_FourofaKind(Dices);

        Debug.Log("isYacht : " + isYacht);
        Debug.Log("isBigstraight : " + isLargeStraight);
        Debug.Log("isLittleStraight : " + isSmallStraight);
        Debug.Log("isFullHouse : " + isFullHouse);
        Debug.Log("isFourofaKind : " + isFourofaKind);

        Calc_All_Score(Dices);

        Text_Update();
    }

    public void Calc_All_Score(List<Dice> Dices)
    {
        int[] NumberCount = { 0, 0, 0, 0, 0, 0 };

        foreach(Dice info in Dices)
        {
            NumberCount[info.thisTurn_Number - 1]++;
        }

        AcesScore = Aces(NumberCount);
        DeucesScore = Deuces(NumberCount);
        ThreesScore = Threes(NumberCount);
        FoursScore = Fours(NumberCount);
        FivesScore = Fives(NumberCount);
        SixesScore = Sixes(NumberCount);

        YachtScore = isYacht ? 50 : 0;
        L_StraightScore = isLargeStraight ? 30 : 0;
        S_StraightScore = isSmallStraight ? 30 : 0;
        FourofaKindScore = isFourofaKind ? FourofaKind(NumberCount) : 0;
        FullHouseScore = isFullHouse ? FullHouse(NumberCount) : 0;

        ChoiceScore = Choice(NumberCount);

    }

    public int Aces(int[] NumberCount) => (NumberCount[0]);
    public int Deuces(int[] NumberCount) => (NumberCount[1] * 2);
    public int Threes(int[] NumberCount) => (NumberCount[2] * 3);
    public int Fours(int[] NumberCount) => (NumberCount[3] * 4);
    public int Fives(int[] NumberCount) => (NumberCount[4] * 5);
    public int Sixes(int[] NumberCount) => (NumberCount[5] * 6);

    public int Choice(int[] NumberCount)
    {
        int sum = 0;
        for(int i = 0; i < NumberCount.Length; i++)
        {
            sum += NumberCount[i] * (i + 1);
        }

        return sum;
    }

    public int FullHouse(int[] NumberCount)
    {
        int ThreeCountN = Array.FindIndex(NumberCount, x => x == 3) + 1;
        int twoCountN = Array.FindIndex(NumberCount, x => x == 2) + 1;

        return ThreeCountN * 3 + twoCountN * 2;
    }

    public int FourofaKind(int[] NumberCount)
    {
        int FourCountN = Array.FindIndex(NumberCount, x => x == 4) + 1;
        int OneCountN = Array.FindIndex(NumberCount, x => x == 1) + 1;

        return FourCountN * 4 + OneCountN;
    }

    public bool is_Yacht(List<Dice> Dices)
    {
        int firstNumber = Dices[0].thisTurn_Number;

        foreach (Dice info in Dices)
        {
            if (firstNumber != info.thisTurn_Number)
                return false;
        }

        return true;
    }
    public bool is_LargeStraight(List<Dice> Dices)
    {
        int number = 2;

        foreach (Dice info in Dices)
        {
            if (info.thisTurn_Number != number)
                return false;

            number++;
        }

        return true;
    }
    public bool is_SmallStraight(List<Dice> Dices)
    {
        int number = 1;

        foreach (Dice info in Dices)
        {
            if (info.thisTurn_Number != number)
                return false;

            number++;
        }

        return true;
    }
    public bool is_FullHouse(List<Dice> Dices)
    {
        int[] NumberCount = { 0, 0, 0, 0, 0, 0 };

        foreach (Dice info in Dices)
        {
            NumberCount[info.thisTurn_Number - 1]++;
        }

        bool twoNumbers = Array.Exists(NumberCount, x => x == 2);
        bool threeNumbers = Array.Exists(NumberCount, x => x == 3);

        return twoNumbers && threeNumbers;
    }

    public bool is_FourofaKind(List<Dice> Dices)
    {
        bool twoNumbers = false;
        bool threeNumbers = false;

        int[] NumberCount = { 0, 0, 0, 0, 0, 0 };

        foreach (Dice info in Dices)
        {
            NumberCount[info.thisTurn_Number - 1]++;
        }

        return Array.Exists(NumberCount, x => x == 4);
    }

    public void connect_Text(Text text)
    {
        SpecialScore_Text = text;
    }
    
    public void Text_Update()
    {
        SpecialScore_Text.text = "";
        if(isYacht)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "isYacht!\n";
        }
        if(isLargeStraight)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "isBigStraight!\n";
        }
        if(isSmallStraight)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "isLittleStraight!\n";
        }
        if(isFullHouse)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "isFullHouse!\n";
        }
        if(isFourofaKind)
        {
            SpecialScore_Text.text = SpecialScore_Text.text + "isFourofaKind!\n";
        }
    }

    public int Dice_Number_cmp(Dice a, Dice b) => (a.thisTurn_Number > b.thisTurn_Number ? 1 : -1);
}
