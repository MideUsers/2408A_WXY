using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ScrollView : ViewBase
{
    Transform contentLeft;
    Transform contentRight;
    GameObject itemLeft;
    GameObject itemRight;
    GameObject player1;
    GameObject player2;
    GameObject player3;
    PlayableDirector player1Director;
    Button Botton1;
    Button Botton2;
    Button Botton3;
    int kind = 1;
    int nowPlayerIndex = 1;
    public override void Init(UIWindow uiWindow)
    {
        base.Init(uiWindow);
        var player = GameObject.Instantiate(ResourceMgr.Instance.ResLoadAsset<GameObject>("Player"),Boot.Instance.transform);
        player.transform.position = Vector3.up;
        player.AddComponent<PlayerControl>();

        contentLeft = uiWindow.transform.Find("Left/Viewport/Content");
        contentRight = uiWindow.transform.Find("Right/Viewport/Content");
        itemLeft = uiWindow.transform.Find("Left/Viewport/Content/Item").gameObject;
        itemRight = uiWindow.transform.Find("Right/Viewport/Content/Item").gameObject;
        Botton1.onClick.AddListener(() =>
        {
            nowPlayerIndex = 1;
            ChangePlayer();
        });
        Botton2.onClick.AddListener(() =>
        {
            nowPlayerIndex = 2;
            ChangePlayer();
        });
        Botton3.onClick.AddListener(() =>
        {
            nowPlayerIndex = 3;
            ChangePlayer();
        });
    }

    private void ChangePlayer()
    {
        player1.SetActive(nowPlayerIndex == 1); 
        player2.SetActive(nowPlayerIndex == 2); 
        player3.SetActive(nowPlayerIndex == 3); 
    }

    internal void Refresh(Message message)
    {
        Dictionary<int, List<int>> dic = message.Data as Dictionary<int, List<int>>;
        itemLeft.SetActive(false);
        itemRight.SetActive(false);
        for (int i = 0; i < (dic.Count > contentLeft.childCount ? dic.Count : contentLeft.childCount); i++)
        {
            if (contentLeft.childCount > i)
            {
                contentLeft.GetChild(i).gameObject.SetActive(true);
            }
            else
                GameObject.Instantiate(itemLeft, contentLeft).SetActive(true);


            if (i > dic.Count)
                contentLeft.GetChild(i).gameObject.SetActive(false);
        }


    }
    RaycastHit hit;
    public override void Update()
    {
        base.Update();
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Boot.Instance.uiCamera.ScreenPointToRay(Input.mousePosition);

            // 发射射线并检查是否碰撞
            if (Physics.Raycast(ray, out hit))
            {
                // 如果射线击中物体，打印出该物体的名字
                Debug.Log("Hit " + hit.transform.name);
                if (hit.collider.gameObject == player1)
                {
                    player1Director.Pause();
                    player1Director.time = 0;
                    player1Director.Play();
                }
                if (hit.collider.gameObject == player1)
                {

                }
                if (hit.collider.gameObject == player1)
                {

                }
            }
        }
    }

    public override void RegisterListener()
    {
        base.RegisterListener();
        MessageCenter.Instance.AddListener((int)MessageID.SENDSCROLL, Refresh);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        player1.SetActive(false);
        player2.SetActive(false);
        player3.SetActive(false);
    }
}
