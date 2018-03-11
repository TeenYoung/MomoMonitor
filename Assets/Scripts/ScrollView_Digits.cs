using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView_Digits : MonoBehaviour
{

    public RectTransform viewPort, content;
    public RectTransform[] elements; //each elements in content
    public RectTransform centerToCompare;  //empty object to compare 
    public GridLayoutGroup contentDigits;

    private int input;

    private int distanceBetweenEles; //相邻两个元素的距离，在Start方法计算
    private float[] distanceToCenter; //每个元素距离center的距离，在Update方法计算
    private int minEleNum; //在所有元素中，距离centerToCompare最近的元素索引
    private bool dragging = false; //Element是否在被拖拽

    
    private bool select = false;

    private Vector2 tempPosition;


    // Use this for initialization
    void Start()
    {
        int eleLength = elements.Length;
        distanceToCenter = new float[eleLength];

        //Get distance between elements
        //distanceBetweenEles = (int)Mathf.Abs(elements[1].localPosition.y - elements[0].localPosition.y);
        //distanceBetweenEles = (int)Mathf.Abs(elements[1].transform.position.y - elements[0].transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dragging)
        {
            MoveToCenter();           
            input = minEleNum;
        }
        
        print("input = " + input);
    }


    public void MoveToCenter()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            //distanceToCenter[i] = Math.Abs(centerToCompare.position.y - elements[i].position.y);//计算每个元素距离center的距离
            distanceToCenter[i] = Math.Abs(centerToCompare.transform.position.y - elements[i].transform.position.y);//计算每个元素距离center的距离             
            //print(distanceToCenter[i] + "i=" + i );
        }

        float minDist = Mathf.Min(distanceToCenter);
        //if (minDist > 150) minDist = minDist - 150;

        //print("minDist" + minDist);

        for (int i = 0; i < elements.Length; i++)
        {
            if (minDist == distanceToCenter[i])
            {
                minEleNum = i; //找到最小距离的元素索引
            }
            
        }
        

        if (!dragging)
        { //如果目前没有在滑动
          //tempPosition.y = centerToCompare.transform.position.y - (contentDigits.spacing.y + contentDigits.cellSize.y) * (minEleNum - 1);
            LerpEleToCenter(content.transform.position.y + minDist - contentDigits.spacing.y); //LerpEleToCenter作用是自然地滑到目标距离
        }
    }



    void LerpEleToCenter(float positionY)
    {        
        float newY = Mathf.Lerp(content.transform.position.y, positionY, Time.deltaTime * 5f); //使用Mathf.Lerp函数让数据的顺滑地变化
        Vector2 newPosition = new Vector2(content.transform.position.x, newY);//目标距离
        content.transform.position = newPosition;
    }


    public void BeginDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

}
