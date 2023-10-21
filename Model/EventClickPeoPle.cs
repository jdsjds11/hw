using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClickPeoPle : MonoBehaviour, IPointerClickHandler
{
    public bool pause;
    public bool onBoat;
    public Transform person;

    public bool LeftOrRight;        //人在左边还是右边，左边：true //以河的中心区分左右
    public Vector3 onBoatPosition;
    public int onBoatLeftOrRight;  //人在上船后，在船的左边还是右边；以在河的右边为例。如果在河的左边，则是镜像。 1为右边，2为左。0为不在船上

    public Vector3 leftStartPosition;
    public Vector3 leftFirstDestination;
    public Vector3 leftMidDestination;
    public Vector3 leftFinalDestination;

    public Vector3 rightStartPosition;
    public Vector3 rightFirstDestination; 
    public Vector3 rightMidDestination;
    public Vector3 rightFinalDestination;

    public bool leftZeroStep;
    public bool leftFistStep;
    public bool leftMidStep;
    public bool leftFinalStep;

    public bool rightZeroStep; // 从第一个位置点返回起始位置
    public bool rightFistStep;
    public bool rightMidStep;
    public bool rightFinalStep;

    public bool rightToBoat;
    public bool boatToRight;
    public bool leftToBoat;
    public bool boatToLeft;

    public bool leftAcrossRiver;        // 从河的右边到左边
    public bool rightAcrossRiver;       // 从河的左边到右边
    public bool finishAcrossed;         // 是否度过了河
    public bool finishOnBoatPosition;   // 是否到船上准确位置

    public bool click;                  // 被点击。与ModelGameObject交互

    // public float time;
    // Start is called before the first frame update

    void Start()
    {
        ChangePosition(person.position);
        Begin();
    }

    public void Begin()
    {
        onBoatLeftOrRight = 0;
        click = false;
        LeftOrRight = false;
        
        leftZeroStep = false;
        leftFistStep = false;
        leftMidStep = false;
        leftFinalStep = false;

        rightZeroStep = false;
        rightFistStep = false;
        rightMidStep = false;
        rightFinalStep = false;

        onBoat = false;
        pause = true;

        rightToBoat = false;
        boatToRight = false;
        leftToBoat = false;
        boatToLeft = false;

        leftAcrossRiver = false;
        rightAcrossRiver = false;
        finishAcrossed = false;
        finishOnBoatPosition = false;

        person.position = rightStartPosition;
    }

    public void ChangePosition(Vector3 position)  
    {
        
        float para = 1;
        if (position.x < 0) para = -1;
        rightStartPosition = position;
        rightFirstDestination = new Vector3(position.x, position.y + (float)0.55, position.z);
        rightMidDestination = new Vector3((float)1.25 * para, rightFirstDestination.y, rightFirstDestination.z);
        rightFinalDestination = new Vector3(rightMidDestination.x, (float)0.8, rightMidDestination.z);


        leftStartPosition = new Vector3((float)-1 * rightStartPosition.x, rightStartPosition.y, rightStartPosition.z);
        leftFirstDestination = new Vector3((float)-1 * position.x, position.y + (float)0.55, position.z);
        leftMidDestination = new Vector3((float)-1.5, rightFirstDestination.y, rightFirstDestination.z);
        leftFinalDestination = new Vector3((float)-1 * rightMidDestination.x, (float)0.8, rightMidDestination.z);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (pause) return;
        if (rightToBoat)
        {
            if (!MoveToBoat(ref rightFistStep, rightFirstDestination)) return;
            if (!MoveToBoat(ref rightMidStep, rightMidDestination)) return;
            if (!MoveToBoat(ref rightFinalStep, rightFinalDestination)) return;
            if (!MoveToBoat(ref finishOnBoatPosition, onBoatPosition)) return;

            finishOnBoatPosition = false;
            onBoat = true;
            pause = true;
            rightToBoat = false;

            rightZeroStep = false;
            rightFistStep = false;
            rightMidStep = false;
            rightFinalStep = false;
        }
        else if (boatToRight)
        {
            if (!MoveToBoat(ref rightFinalStep, rightFinalDestination)) return;
            if (!MoveToBoat(ref rightMidStep, rightMidDestination)) return;
            if (!MoveToBoat(ref rightFistStep, rightFirstDestination)) return;
            if (!MoveToBoat(ref rightZeroStep, rightStartPosition)) return;

            onBoat = false;
            pause = true;
            boatToRight = false;

            rightZeroStep = false;
            rightFistStep = false;
            rightMidStep = false;
            rightFinalStep = false;
        }

        else if (leftToBoat)
        {

            if (!MoveToBoat(ref leftFistStep, leftFirstDestination)) return;
            if (!MoveToBoat(ref leftMidStep, leftMidDestination)) return;
            if (!MoveToBoat(ref leftFinalStep, leftFinalDestination)) return;
            if (!MoveToBoat(ref finishOnBoatPosition, onBoatPosition)) return;
            finishOnBoatPosition = false;
            onBoat = true;
            pause = true;
            leftToBoat = false;
            leftZeroStep = false;
            leftFistStep = false;
            leftMidStep = false;
            leftFinalStep = false;
        }
        else if (boatToLeft)
        {
            if (!MoveToBoat(ref leftFinalStep, leftFinalDestination)) return;
            if (!MoveToBoat(ref leftMidStep, leftMidDestination)) return;
            if (!MoveToBoat(ref leftFistStep, leftFirstDestination)) return;
            if (!MoveToBoat(ref leftZeroStep, leftStartPosition)) return;
            onBoat = false;
            pause = true;
            boatToLeft = false;

            leftZeroStep = false;
            leftFistStep = false;
            leftMidStep = false;
            leftFinalStep = false;
        }
        else if (leftAcrossRiver) // 从左到右渡河
        {
            
            if (!MoveAcrossRiver(ref finishAcrossed, onBoatPosition)) return;

            pause = true;
            finishAcrossed = false;
            leftAcrossRiver = false;
        } 
        else if (rightAcrossRiver)
        {
            if (!MoveAcrossRiver(ref finishAcrossed, onBoatPosition)) return;
            pause = true;
            finishAcrossed = false;
            rightAcrossRiver = false;
        }
    }

    public void AcrossRiver()
    {
        if (!onBoat || !pause) return;
        
        if (!LeftOrRight)
        {
            LeftOrRight = true;
            rightAcrossRiver = true;
            onBoatPosition = new Vector3((float)-2.5 + onBoatPosition.x, (float)0.8, (float)-1.1);
        }
        else
        {

            LeftOrRight = false;
            leftAcrossRiver = true;   //leftAcroossRiver 从左到右渡河
            
            onBoatPosition = new Vector3((float)2.5 + onBoatPosition.x, (float)0.8, (float)-1.1);
        }
        pause = false;
        

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pause) return;
        // 请求ModleGameObject响应
        click = true;
        return;
    }

    public void MovePeoPle(int t_onBoatPosition) // On...: 1代表在船的左边，2代表在船的右边;  
    {
        
        // 选择路径
        if (!onBoat && !LeftOrRight)
        {
            rightToBoat = true;
        }
        else if (onBoat && !LeftOrRight)
        {
           
            boatToRight = true;
        }
        else if (!onBoat && LeftOrRight)
        {
            leftToBoat = true;
        }
        else if (onBoat && LeftOrRight)
        {
            boatToLeft = true;
        }
        bool right = false;
        if (rightToBoat || boatToRight) right = true;

        if (right)
        {
            if (t_onBoatPosition == 1)
            {
                onBoatPosition = new Vector3((float)1, (float)0.8, (float)-1.1);
                onBoatLeftOrRight = 1;

            }
            else if (t_onBoatPosition == 2)
            {
                onBoatPosition = new Vector3((float)1.5, (float)0.8, (float)-1.1);
                onBoatLeftOrRight = 2;
            }
        }else
        {
            if (t_onBoatPosition == 1)
            {
                onBoatPosition = new Vector3((float)-1.5, (float)0.8, (float)-1.1);
                onBoatLeftOrRight = 1;

            }
            else if (t_onBoatPosition == 2)
            {
                onBoatPosition = new Vector3((float)-1, (float)0.8, (float)-1.1);
                onBoatLeftOrRight = 2;
            }
        }
        
        pause = false;
    }

    

    bool MoveToBoat(ref bool step, Vector3 destination)
    {
        if (step) return step;
        person.position = Vector3.MoveTowards(person.localPosition,destination, (float)5 * Time.deltaTime);
        if (person.position == destination)
        {
                step = true;
        }
        return step;
    }

    bool MoveAcrossRiver(ref bool step, Vector3 destination)
    {
        if (step) return step;
        person.position = Vector3.MoveTowards(person.localPosition, destination, (float)2 * Time.deltaTime);
        if (person.position == destination)
        {
            step = true;
        }
        return step;
    }

}

