using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollModel : ModelBase
{
    public Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();

    public override void Init(UIWindow uiWindow)
    {
        base.Init(uiWindow);

        for (int i = 1; i <= 10; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                if (!dic.ContainsKey(i))
                    dic.Add(i, new List<int>());
                dic[i].Add(j);
            }
        }
    }

    public override void RegisterListener()
    {
        base.RegisterListener();
        MessageCenter.Instance.AddListener((int)MessageID.RECEPTIONSCROLL, RefreshData);
    }

    private void RefreshData(Message message)
    {
        Debug.Log("接收到RECEPTIONSCROLL消息");
        MessageCenter.Instance.SendMessage((int)MessageID.SENDSCROLL, dic);
    }
}
