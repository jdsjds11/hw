using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    // Start is called before the first frame update
    private float time;
    public int ShowTime; // 显示时间是整数
    public string ShowMessage;
    public bool gameEndOrNot;
    public bool gameReStart;
    void Start()
    {
        begin();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameEndOrNot)
        {
            time -= Time.deltaTime;
            ShowTime = (int)time;
        }
        
    }

    void OnGUI()
    {

        //小字体初始化
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 20;

        //大字体初始化
        GUIStyle bigStyle = new GUIStyle();
        bigStyle.normal.textColor = Color.white;
        bigStyle.fontSize = 30;

        GUI.Label(new Rect(150, 0, 50, 200), "Priests and Devils", bigStyle);
        GUI.Label(new Rect(0, 30, 100, 50), "Time: " + ShowTime, style);

        bigStyle.normal.textColor = Color.red;
        bigStyle.fontSize = 50;
        // "*YOU   LOSE*" "*YOU    WIN*"
        

        // 游戏结束
        if (gameEndOrNot)
        {
            if (GUI.Button(new Rect(240, 110, 100, 50), "RESTART"))
            {
                begin();
            }
            GUI.Label(new Rect(120, 50, 50, 200), ShowMessage, bigStyle);
        }
    }

    public void GameEnd(string t_showMessage)
    {
        ShowMessage = t_showMessage;
        gameEndOrNot = true;
    }

    public void begin()
    {
        ShowMessage = "";
        time = 60;
        ShowTime = 60;
        gameReStart = false;
        gameEndOrNot = false;
    }
}
