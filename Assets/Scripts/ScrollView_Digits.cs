using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollView_Digits : MonoBehaviour
{

    public RectTransform viewPort, content;
    public List<Button> elements; //each elements in content
    public RectTransform centerToCompare;  //empty object to compare 
    public GridLayoutGroup contentDigits;

    private int input; //獲得輸入值，存檔時調用即可

    private int distanceBetweenEles; //相邻两个元素的距离，在Start方法计算
    private float[] distanceToCenter; //每个元素距离center的距离，在Update方法计算
    private int minEleNum; //在所有元素中，距离centerToCompare最近的元素索引
    private bool dragging = false; //Element是否在被拖拽
    private bool moveUp = false, moveDown = false;

    private int maxEleNum; //距離centerToCompare最遠的元素索引

    private Vector2 tempPosition;
    private Vector2 first, second; //鼠標點擊和釋放時的坐標

    // Use this for initialization
    void Start()
    {
        int eleLength = elements.Count;
        distanceToCenter = new float[eleLength];

        //Get distance between elements
        //distanceBetweenEles = (int)Mathf.Abs(elements[1].localPosition.y - elements[0].localPosition.y);
        //distanceBetweenEles = (int)Mathf.Abs(elements[1].transform.position.y - elements[0].transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //停止拖曳時把選項居中，且把選項賦值給input
        if (!dragging)
        {
            MoveToCenter();           
            input = minEleNum;
        }

        //拖曳時改變content的位置確保循環顯示
        //if (dragging)
        //{
        //    if (moveUp) AddOnTop();
        //    if (moveDown) AddToButton();
        //    print("movedown = " + moveDown);

        //    if (content.transform.position.y < centerToCompare.transform.position.y) elements.Remove(elements[maxEleNum]);
        //    //else if (content.transform.position.y >= centerToCompare.transform.position.y + (15 + 150) * (elements.Count - 1)) AddToButton();
        //    else if (content.transform.position.y >= centerToCompare.transform.position.y + (contentDigits.spacing.y + contentDigits.cellsize.y) * (elements.Count - 1)) AddToButton();
        //}
    }


    public void MoveToCenter()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            //distanceToCenter[i] = Math.Abs(centerToCompare.position.y - elements[i].position.y);//计算每个元素距离center的距离
            distanceToCenter[i] = Math.Abs(centerToCompare.transform.position.y - elements[i].transform.position.y);//计算每个元素距离center的距离             
            //print(distanceToCenter[i] + "i=" + i );
        }

        float minDist = Mathf.Min(distanceToCenter);
        float maxDist = Mathf.Max(distanceToCenter);

        //print("minDist" + minDist);

        for (int i = 0; i < elements.Count; i++)
        {
            if (minDist == distanceToCenter[i])
            {
                minEleNum = i; //找到最小距离的元素索引
            }
            if (maxDist == distanceToCenter[i])
            {
                maxEleNum = i;
            }
            
        }
        

        if (!dragging)
        { //如果目前没有在滑动
          //tempPosition.y = centerToCompare.transform.position.y - (contentDigits.spacing.y + contentDigits.cellSize.y) * (minEleNum - 1);
            //LerpEleToCenter(content.transform.position.y + minDist - 15); //用LerpEleToCenter自然地滑到目标距离
            LerpEleToCenter(content.transform.position.y + minDist - contentDigits.spacing.y); //用LerpEleToCenter自然地滑到目标距离
            input = System.Convert.ToInt32(content.GetChild(minEleNum).GetComponentInChildren<Text>().text);//把選中的button的text轉化為int輸出給input

            //print(input);
        }        
    }



    void LerpEleToCenter(float positionY)
    {        
        float newY = Mathf.Lerp(content.transform.position.y, positionY, Time.deltaTime * 5f); //使用Mathf.Lerp函数让数据的顺滑地变化
        Vector2 newPosition = new Vector2(content.transform.position.x, newY);//目标距离
        content.transform.position = newPosition;
    }

    //void AddOnTop()
    //{
    //    Button tempListContent = elements[elements.Count - 1];
    //    elements.Remove(elements[elements.Count]);
    //    elements.Insert(0, tempListContent);
    //}

    //void AddToButton()
    //{
    //    Button tempListContent = elements[0];
    //    elements.Remove(elements[elements.Count - 1]);
    //    elements.Add(tempListContent);
    //}

    public void BeginDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown) first = Event.current.mousePosition;
        if (Event.current.type == EventType.MouseDrag) second = Event.current.mousePosition;
        if (first.y < second.y) moveUp = true;
        if (first.y > second.y) moveDown = true;
    }
}
