using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

/// <summary>
/// 游戏物品控制脚本
/// 负责处理单个游戏物品的显示、拖拽交互和交换逻辑
/// 实现了物品的颜色变化、拖拽检测和匹配判断功能
/// </summary>
public class GameItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 存储可随机生成的颜色数组
    Color[] stringArr = new Color[6]
    {
       Color.red, Color.green, Color.blue, Color.gray, Color.yellow, Color.black
    };

    Image image;         // 物品图像组件，用于显示颜色
    public int colorIndex; // 当前物品的颜色索引
    bool isDrag;         // 是否正在被拖拽的标志
    GameView gameView;   // 游戏主视图引用
    Vector3 nowPos;      // 当前位置
    int num = 9;         // 网格行数/列数
    public int index;    // 物品在网格中的索引
    public Vector3 startPos; // 初始位置
    Vector3 targetPos = Vector3.zero; // 目标位置
    int targetIndex = 0;  // 目标物品索引
    Text text;            // 文本组件，用于显示调试信息

    /// <summary>
    /// 初始化物品
    /// </summary>
    /// <param name="gameView">游戏主视图引用</param>
    /// <param name="index">颜色索引</param>
    /// <param name="localIndex">网格中的位置索引</param>
    public void Init(GameView gameView, int index, int localIndex)
    {
        this.index = localIndex;
        Random random = new Random();
        image = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        startPos = transform.localPosition;
        this.gameView = gameView;
        ChangeColor(index);
    }

    /// <summary>
    /// 修改物品颜色
    /// </summary>
    public void ChangeColor(int index)
    {
        // 设置物品颜色并更新显示信息
        colorIndex = index;
        image.color = stringArr[index];
        text.text = colorIndex.ToString() + "\n" + this.index.ToString();
    }

    /// <summary>
    /// 开始拖拽事件处理
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 设置拖拽状态并初始化相关变量
        isDrag = true;
        nowPos = transform.position;
        targetIndex = -1;
        gameView.dic.Clear(); // 清空检测字典，准备新的匹配检测
    }

    /// <summary>
    /// 拖拽过程中事件处理
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // 物品跟随鼠标移动，并根据移动方向和距离确定目标位置
        if (isDrag && targetIndex == -1)
        {
            // 获取并转换鼠标位置
            Vector3 mousePosition = Input.mousePosition;
            Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
            Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
            worldPosition.z = transform.position.z;

            // 判断拖拽方向（水平或垂直）
            if (Math.Abs(worldPosition.x - nowPos.x) > Math.Abs(worldPosition.y - nowPos.y))
            {
                // 水平拖拽
                worldPosition.y = nowPos.y;
                // 向右拖拽且不在最右列
                if (worldPosition.x - nowPos.x > 0.2f && index % num != 8)
                {
                    targetPos = startPos + new Vector3(100, 0, 0);
                    targetIndex = index + 1;
                }
                // 向左拖拽且不在最左列
                else if (worldPosition.x - nowPos.x < -0.2f && index % 9 != 0)
                {
                    targetPos = startPos + new Vector3(-100, 0, 0);
                    targetIndex = index - 1;
                }
            }

            // 垂直拖拽
            if (Math.Abs(worldPosition.x - nowPos.x) < Math.Abs(worldPosition.y - nowPos.y))
            {
                worldPosition.x = nowPos.x;
                // 向上拖拽且不在最上行
                if (worldPosition.y - nowPos.y > 0.2f && index < num * (num - 1))
                {
                    targetPos = startPos + new Vector3(0, 100, 0);
                    targetIndex = index + num;
                }
                // 向下拖拽且不在最下行
                else if (worldPosition.y - nowPos.y < -0.2f && index >= num)
                {
                    targetPos = startPos + new Vector3(0, -100, 0);
                    targetIndex = index - num;
                }
            }

            // 如果找到目标位置，使用DOTween动画移动物品
            if (targetIndex >= 0)
            {
                transform.DOLocalMove(targetPos, 0.3f);
                gameView.itemObjs[targetIndex].transform.DOLocalMove(-targetPos, 0.3f);
            }
        }
    }

    /// <summary>
    /// 结束拖拽事件处理
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        if (targetIndex >= 0)
        {
            // 停止当前动画
            DOTween.Kill(transform, true);
            DOTween.Kill(gameView.itemObjs[targetIndex].transform, true);

            // 交换物品
            ChangeItem();

            // 检查交换后是否形成匹配
            if (!gameView.GetClear(colorIndex, this, gameView.itemObjs[targetIndex].colorIndex, gameView.itemObjs[targetIndex]))
            {
                // 没有匹配，恢复原状
                ChangeItem();
                transform.DOLocalMove(Vector3.zero, 0.3f);
                gameView.itemObjs[targetIndex].transform.DOLocalMove(Vector3.zero, 0.3f);
            }
            else
            {
                // 形成匹配，执行消除逻辑
                gameView.ClearDic();
            }
        }
    }

    /// <summary>
    /// 交换两个物品的位置和颜色
    /// </summary>
    private void ChangeItem()
    {
        // 重置位置
        transform.localPosition = Vector3.zero;
        gameView.itemObjs[targetIndex].transform.localPosition = Vector3.zero;

        // 交换颜色
        int localColorIndex = colorIndex;
        ChangeColor(gameView.itemObjs[targetIndex].colorIndex);
        gameView.itemObjs[targetIndex].ChangeColor(localColorIndex);
    }
}
