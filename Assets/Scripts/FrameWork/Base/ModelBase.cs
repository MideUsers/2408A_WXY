using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 操作数据 处理数据 数据获取 数据推送
/// </summary>
public class ModelBase
{
    public UIWindow uiWindow;
    public virtual void Init(UIWindow uiWindow)
    {
        this.uiWindow = uiWindow;
        RegisterListener();
    }
    public virtual void OnEnable()
    {

    }

    public virtual void OnDestory()
    {

    }

    public virtual void RegisterListener()
    {

    }
}
