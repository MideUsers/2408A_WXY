using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : ViewBase
{
    Button loginBtn;
    public override void Init(UIWindow uiWindow)
    {
        base.Init(uiWindow);
        loginBtn = uiWindow.transform.Find("LoginBtn").GetComponent<Button>();
        loginBtn.onClick.AddListener(() =>
        {
            GameScenesManager.Instance.LoadSceneAsync("Game", "GamePanel");
        });
    }
}
