using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGameObjects : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject river;
    public GameObject boat;
    public GameObject leftDust;
    public GameObject rightDust;
    public GameObject priestOne;
    public GameObject priestTwo;
    public GameObject priestThree;
    public GameObject demonOne;
    public GameObject demonTwo;
    public GameObject demonThree;
    public int CountPeoPleOnBoat;
    public bool peopleOnBoatLeft;
    public bool peopleOnBoatRight;

    public int leftCoastDemonNum;
    public int leftCoastPriestNum;
    public int rightCoastDemonNum;
    public int rightCoastPriestNum;

    public bool pause;
    public bool haveEndedGame;

    void Start()
    {
        river = (GameObject)Resources.Load("Prefabs/River");
        river = Instantiate(river);
        boat = (GameObject)Resources.Load("Prefabs/Boat");
        boat = Instantiate(boat);

        leftDust = (GameObject)Resources.Load("Prefabs/DustLeft");
        leftDust = Instantiate(leftDust);
        rightDust = (GameObject)Resources.Load("Prefabs/DustRight");
        rightDust = Instantiate(rightDust);

        priestOne = (GameObject)Resources.Load("Prefabs/priestOne");
        priestOne = Instantiate(priestOne);

        priestTwo = (GameObject)Resources.Load("Prefabs/priestTwo");
        priestTwo = Instantiate(priestTwo);

        priestThree = (GameObject)Resources.Load("Prefabs/priestThree");
        priestThree = Instantiate(priestThree);

        demonOne = (GameObject)Resources.Load("Prefabs/DemonOne");
        demonOne = Instantiate(demonOne);
        demonTwo = (GameObject)Resources.Load("Prefabs/DemonTwo");
        demonTwo = Instantiate(demonTwo);
        demonThree = (GameObject)Resources.Load("Prefabs/DemonThree");
        demonThree = Instantiate(demonThree);
        BeginGame();

    }

    void BeginGame()
    {
        haveEndedGame = false;
        peopleOnBoatRight = false;
        peopleOnBoatLeft = false;
        leftCoastDemonNum = 0;
        leftCoastPriestNum = 0;
        rightCoastDemonNum = 3;
        rightCoastPriestNum = 3;

        pause = false;
        CountPeoPleOnBoat = 0;
        // 初始化所有对象
        
    }

    // Update is called once per frame
    void Update()
    {
        // 如果检测
        if (haveEndedGame && !river.GetComponent<View>().gameEndOrNot)
        {
            haveEndedGame = false;
            BeginGame();
            ReStartGame();
        }
        if (!haveEndedGame)
        {
            AllPeopleClick();  //处理所有人物点击函数
            BoatClick();       //处理船的点击函数
            Checked();
        }
        //检查游戏成功或者失败情况

    }

    public void BoatClick()
    {
        // 当所有人物运动都是静止的时候才可以开船；
        if ((boat.GetComponent<EventClick>().click && CountPeoPleOnBoat <= 0)) boat.GetComponent<EventClick>().click = false;
        if (boat.GetComponent<EventClick>().click && CountPeoPleOnBoat > 0 && AllPeopleStanding())
        {

            if (priestOne.GetComponent<EventClickPeoPle>().onBoat)
            {
                priestOne.GetComponent<EventClickPeoPle>().AcrossRiver();
                CountPriestsNumOnEachCoast();
            }

            if (demonOne.GetComponent<EventClickPeoPle>().onBoat)
            {
                demonOne.GetComponent<EventClickPeoPle>().AcrossRiver();
                CountDemonsNumOnEachCoast();
            }

            if (priestTwo.GetComponent<EventClickPeoPle>().onBoat)
            {
                priestTwo.GetComponent<EventClickPeoPle>().AcrossRiver();
                CountPriestsNumOnEachCoast();
            }
            if (demonTwo.GetComponent<EventClickPeoPle>().onBoat)
            {
                demonTwo.GetComponent<EventClickPeoPle>().AcrossRiver();
                CountDemonsNumOnEachCoast();
            }

            if (priestThree.GetComponent<EventClickPeoPle>().onBoat)
            {
                priestThree.GetComponent<EventClickPeoPle>().AcrossRiver();
                CountPriestsNumOnEachCoast();
            }

            if (demonThree.GetComponent<EventClickPeoPle>().onBoat)
            {
                demonThree.GetComponent<EventClickPeoPle>().AcrossRiver();
                CountDemonsNumOnEachCoast();
            }

            boat.GetComponent<EventClick>().MoveAcrossRiver();
        }

    }

    public void Checked()  // 测试有没有一岸 魔鬼的数量大于牧师的数量
    {
        //输了的情况
        if (rightCoastPriestNum < rightCoastDemonNum && rightCoastPriestNum != 0)
        {
            river.GetComponent<View>().GameEnd("*YOU   LOSE*");
            haveEndedGame = true;

        }
        else if (leftCoastPriestNum < leftCoastDemonNum && leftCoastPriestNum != 0)
        {
            river.GetComponent<View>().GameEnd("*YOU   LOSE*");
            haveEndedGame = true;
        }
        else if (river.GetComponent<View>().ShowTime == 0)
        {
            river.GetComponent<View>().GameEnd("*YOU   LOSE*");
            haveEndedGame = true;
        }
        //赢了的情况 
        else if (leftCoastPriestNum == 3 && leftCoastDemonNum == 3 && PeopleNotOnBoat()) // 且所有人不在船上
        {
            river.GetComponent<View>().GameEnd("*YOU    WIN*");
            haveEndedGame = true;
        }
    }

    public bool PeopleNotOnBoat()
    {
        return (!priestOne.GetComponent<EventClickPeoPle>().onBoat) && (!priestTwo.GetComponent<EventClickPeoPle>().onBoat) && (!priestThree.GetComponent<EventClickPeoPle>().onBoat) && (!demonOne.GetComponent<EventClickPeoPle>().onBoat) && (!demonTwo.GetComponent<EventClickPeoPle>().onBoat) && (!demonThree.GetComponent<EventClickPeoPle>().onBoat);
    }

    public void ReStartGame()
    {
        priestOne.GetComponent<EventClickPeoPle>().Begin();
        priestTwo.GetComponent<EventClickPeoPle>().Begin();
        priestThree.GetComponent<EventClickPeoPle>().Begin();

        demonOne.GetComponent<EventClickPeoPle>().Begin();
        demonTwo.GetComponent<EventClickPeoPle>().Begin();
        demonThree.GetComponent<EventClickPeoPle>().Begin();

        boat.GetComponent<EventClick>().Begin();

    }

    public void CountPriestsNumOnEachCoast()
    {                                                               // 动态改变两岸魔鬼牧师的数量
        if (boat.GetComponent<EventClick>().leftSide)               // 从左往右
        {
            rightCoastPriestNum += 1;
            leftCoastPriestNum -= 1;

        }
        else
        {
            rightCoastPriestNum -= 1;
            leftCoastPriestNum += 1;
        }
    }

    public void CountDemonsNumOnEachCoast()
    {
        if (boat.GetComponent<EventClick>().leftSide)               // 从左往右
        {
            rightCoastDemonNum += 1;
            leftCoastDemonNum -= 1;

        }
        else
        {
            rightCoastDemonNum -= 1;
            leftCoastDemonNum += 1;
        }
    }

    public bool AllPeopleStanding()
    {
        return (priestOne.GetComponent<EventClickPeoPle>().pause && priestTwo.GetComponent<EventClickPeoPle>().pause && priestThree.GetComponent<EventClickPeoPle>().pause &&
                demonOne.GetComponent<EventClickPeoPle>().pause && demonTwo.GetComponent<EventClickPeoPle>().pause && demonThree.GetComponent<EventClickPeoPle>().pause);
    }

    public void AllPeopleClick() //处理所有人物被点击的事件函数
    {
        // 当船运动的时候所有人不许动
        if (!boat.GetComponent<EventClick>().pause) return;
        if (priestOne.GetComponent<EventClickPeoPle>().click) PeopleClick(ref priestOne);
        if (priestTwo.GetComponent<EventClickPeoPle>().click) PeopleClick(ref priestTwo);
        if (priestThree.GetComponent<EventClickPeoPle>().click) PeopleClick(ref priestThree);
        if (demonOne.GetComponent<EventClickPeoPle>().click) PeopleClick(ref demonOne);
        if (demonTwo.GetComponent<EventClickPeoPle>().click) PeopleClick(ref demonTwo);
        if (demonThree.GetComponent<EventClickPeoPle>().click) PeopleClick(ref demonThree);

    }

    public void PeopleClick(ref GameObject gobj)
    {
        if (gobj.GetComponent<EventClickPeoPle>().LeftOrRight != boat.GetComponent<EventClick>().leftSide) return; // 当船和移动的人物不在同一岸时不能移动
        gobj.GetComponent<EventClickPeoPle>().click = false;

        if (!gobj.GetComponent<EventClickPeoPle>().onBoat)  // 当牧师不在船上的时候 
        {
            if (CountPeoPleOnBoat >= 2) return;
            CountPeoPleOnBoat += 1;
            if (!peopleOnBoatLeft)
            {
                gobj.GetComponent<EventClickPeoPle>().MovePeoPle(1);
                peopleOnBoatLeft = true;
            }
            else
            {
                gobj.GetComponent<EventClickPeoPle>().MovePeoPle(2);
                peopleOnBoatRight = true;
            }


        }
        else
        {
            int onBoatLeftOrRight = gobj.GetComponent<EventClickPeoPle>().onBoatLeftOrRight;
            if (onBoatLeftOrRight == 1)
            {
                peopleOnBoatLeft = false;
            }
            else
            {
                peopleOnBoatRight = false;
            }
            gobj.GetComponent<EventClickPeoPle>().MovePeoPle(onBoatLeftOrRight);
            CountPeoPleOnBoat -= 1;

        }
    }
}
