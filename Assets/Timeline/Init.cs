using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        Debug.Log(operation.progress);
        yield return operation;
    }
}
