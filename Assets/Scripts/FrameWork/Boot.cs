using UnityEngine;

public class Boot : UnitySingleton<Boot>
{
    public Camera uiCamera;
    //声音
    public AudioSource audio;

    //ui三个层级所需要Transform 
    //三个canvas canvas都是camera渲染类型 通过调节距离控制层级
    //相机必须是depthonly 并且改成正交类型
    public Transform BackRoot;
    public Transform UIRoot;
    public Transform TipRoot;


    void Start()
    {
        Init();
        uiCamera = transform.GetComponentInChildren<Camera>();
        UIManager.Instance.OpenWindow("LoginPanel");
        //GameScenesManager.Instance.LoadSceneAsync("Game", "PlayerPanel");
    }

    void Init()
    {
        gameObject.AddComponent<GameScenesManager>().Init(this);
        UIManager.Instance.Init(TipRoot, UIRoot, BackRoot);
    }

    public void ChangeAudio(AudioClip clip)
    {
        audio.clip = clip;
        audio.time = 0;
        audio.Play();
    }
}
