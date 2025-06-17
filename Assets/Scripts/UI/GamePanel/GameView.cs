using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameView : ViewBase
{
    Transform itemParent;

    public List<GameItem> itemObjs;
    public Dictionary<int, List<GameItem>> dic = new Dictionary<int, List<GameItem>>();
    Random random = new Random(1000);
    public override void Init(UIWindow uiWindow)
    {
        base.Init(uiWindow);
        itemParent = uiWindow.transform.Find("ItemParent");
        itemObjs = new List<GameItem>();
        InstantiateAllItems();
    }

    private void InstantiateAllItems()
    {
        for (int i = 0; i < 81; i++)
        {
            int index = random.Next(0, 6);
            var item = GameObject.Instantiate<GameObject>(ResourceMgr.Instance.ResLoadAsset<GameObject>("Item"), itemParent);
            var gameItem = item.transform.GetChild(0).GetComponent<GameItem>();
            gameItem.Init(this, index, i);
            itemObjs.Add(gameItem);
        }
    }


    /// <summary>
    /// 当前拖拽的颜色 当前拖拽的物体 目标拖拽的颜色 目标拖拽的物体
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <param name="targetIndex"></param>
    /// <param name="targetItem"></param>
    /// <returns></returns>
    public bool GetClear(int index, GameItem item, int targetIndex, GameItem targetItem)
    {
        AddToDic(targetIndex, targetItem);
        AddToDic(index, item);
        JudgeUpColor(item.index + 9, index);
        JudgeDownColor(item.index - 9, index);
        JudgeLeftColor(item.index - 1, index);
        JudgeRightColor(item.index + 1, index);
        JudgeUpColor(targetItem.index + 9, targetIndex);
        JudgeDownColor(targetItem.index - 9, targetIndex);
        JudgeLeftColor(targetItem.index - 1, targetIndex);
        JudgeRightColor(targetItem.index + 1, targetIndex);
        bool bl = false;
        foreach (var list in dic)
        {
            if (list.Value.Count >= 3)
                bl = true;
        }
        return bl;
    }


    private void JudgeUpColor(int index, int type)
    {
        if (index < itemObjs.Count && index >= 0)
        {
            if (type == itemObjs[index].colorIndex)
            {
                AddToDic(type, itemObjs[index]);
                JudgeUpColor(index + 9, type);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    private void JudgeDownColor(int index1, int index2)
    {
        if (index1 < itemObjs.Count && index1 >= 0)
        {
            if (index2 == itemObjs[index1].colorIndex)
            {
                AddToDic(index2, itemObjs[index1]);
                JudgeDownColor(index1 - 9, index2);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    private void JudgeLeftColor(int index1, int index2)
    {
        if ((index1 < itemObjs.Count && index1 >= 0) && ((index1 + 1) / 9 == index1 / 9))
        {
            if (index2 == itemObjs[index1].colorIndex)
            {
                AddToDic(index2, itemObjs[index1]);
                JudgeLeftColor(index1 - 1, index2);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    private void JudgeRightColor(int index1, int index2)
    {
        if ((index1 < itemObjs.Count && index1 >= 0) && ((index1 - 1) / 9 == index1 / 9))
        {
            if (index2 == itemObjs[index1].colorIndex)
            {
                AddToDic(index2, itemObjs[index1]);
                JudgeRightColor(index1 + 1, index2);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void AddToDic(int colorIndex, GameItem item)
    {
        if (!dic.ContainsKey(colorIndex))
        {
            dic[colorIndex] = new List<GameItem>();
        }
        if (!dic[colorIndex].Contains(item))
        {
            dic[colorIndex].Add(item);
        }
    }

    internal void ClearDic()
    {
        List<GameItem> localList = new List<GameItem>();
        foreach (var list in dic.Values)
        {
            if (list.Count >= 3)
            {
                foreach (var item in list)
                {
                    Debug.Log(item.index + "----------------------" + item.colorIndex);
                    localList.Add(item);
                }
            }
        }
        localList.Sort((a, b) => { return a.index - b.index; });
        for (int i = 0; i < localList.Count; i++)
        {
            SetColor(localList[i], ref localList);
        }
        dic.Clear();
    }

    private void SetColor(GameItem item, ref List<GameItem> localList)
    {
        GameItem localItem = GetItem(item.index, localList);
        if (localItem != null)
        {
            item.ChangeColor(localItem.colorIndex);
            if (!localList.Contains(localItem))
                localList.Add(localItem);
        }
        else
        {
            int index = random.Next(0, 6);
            item.ChangeColor(index);
        }
    }

    private GameItem GetItem(int index, List<GameItem> localList)
    {
        if (index + 9 < itemObjs.Count && !localList.Contains(itemObjs[index + 9]))
        {
            return itemObjs[index + 9];
        }
        else if (index + 9 < itemObjs.Count && localList.Contains(itemObjs[index + 9]))
        {
            return GetItem(index + 9, localList);
        }
        else
        {
            return null;
        }
    }
}
