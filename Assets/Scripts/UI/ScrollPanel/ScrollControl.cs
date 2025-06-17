using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollControl : ControlBase
{
    public override void Init(UIWindow uiWindow)
    {
        base.Init(uiWindow);
        MessageCenter.Instance.SendMessage((int)MessageID.RECEPTIONSCROLL,null);
    }
}
