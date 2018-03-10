using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView_Digits : MonoBehaviour {

    public RectTransform viewPort, content;
    public RectTransform[] elements; //each elements in content
    public RectTransform centerToCompare;  //empty object to compare 
    public GridLayoutGroup contentDigits;

    private int distanceBetweenEles; //相邻两个元素的距离，在Start方法计算
    private float[] distanceToCenter; //每个元素距离center的距离，在Update方法计算
    private int minEleNum; //在所有元素中，距离centerToCompare最近的元素索引
    private bool dragging = false; //Element是否在被拖拽

    private Vector2 tempPosition ;


    // Use this for initialization
    void Start () {
        int eleLength = elements.Length;
        distanceToCenter = new float[eleLength];

        //Get distance between elements
        distanceBetweenEles = (int)Mathf.Abs(elements[1].anchoredPosition.x - elements[0].anchoredPosition.x);
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < elements.Length; i++)
        {
            distanceToCenter[i] = Math.Abs(centerToCompare.position.y - elements[i].position.y);//计算每个元素距离center的距离
        }
        float minDist = Mathf.Min(distanceToCenter);

        for (int i = 0; i < elements.Length; i++)
        {
            if (minDist == distanceToCenter[i])
            {
                minEleNum = i; //找到最小距离的元素索引
            }
        }

        //print(minEleNum);
        tempPosition = centerToCompare.position;
        tempPosition.y = content.position.y - (contentDigits.spacing.y + contentDigits.cellSize.y) * (minEleNum-1);
        content.position = tempPosition;

        print(tempPosition.y);

        //if (!dragging)
        //{ //如果目前没有在滑动
        //    LerpEleToCenter(minEleNum * -distanceBetweenEles); //LerpEleToCenter作用是自然地滑到目标距离
        //}

    }

    
    void LerpEleToCenter(int position)
    {
        float newX = Mathf.Lerp(viewPort.anchoredPosition.y, position, Time.deltaTime * 20f); //使用Mathf.Lerp函数让数据的顺滑地变化
        Vector2 newPosition = new Vector2(newX, viewPort.anchoredPosition.y);//目标距离

        viewPort.anchoredPosition = newPosition;
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
