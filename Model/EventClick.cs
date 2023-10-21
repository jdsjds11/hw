using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerClickHandler
{
    public bool leftSide;
    public bool pause;
    public bool click;
    
    public Transform boat;

    
    // public float time;
    // Start is called before the first frame update
    void Start()
    {
        leftSide = false; // true为左边，false为右边；
        pause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            if (leftSide) {
                boat.position = Vector3.MoveTowards(boat.localPosition, new Vector3((float)1.3, (float)0.4, (float)-1), (float)2 * Time.deltaTime);
                if (boat.position == new Vector3((float)1.3, (float)0.4, (float)-1)) {
                    pause = true;
                    leftSide = false;
                }
                
            } else
            {
                boat.position = Vector3.MoveTowards(boat.localPosition, new Vector3((float)-1.3, (float)0.4, (float)-1), (float)2 * Time.deltaTime);
                if (boat.position == new Vector3((float)-1.3, (float)0.4, (float)-1)) {
                    pause = true;
                    leftSide = true;
                }
                
            }
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pause) return;
        click = true;
    }

    public void MoveAcrossRiver()
    {
        click = false;
        pause = false;
    }

    public void Begin()
    {
        boat.position = new Vector3((float)1.3, (float)0.4, (float)-1);
        pause = true;
        leftSide = false;
    }

}
