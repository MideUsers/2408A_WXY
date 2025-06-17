using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

public class FindParentPath
{
    [MenuItem("GameObject/获取物体路径", false, -100)]
    static void Find()
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject) //如果选中
        {
            string path = "" + selectedGameObject.name;
            Transform parent = selectedGameObject.transform;
            while (true)
            {
                parent = parent.transform.parent;
                try
                {
                    if (parent == null|| parent.parent == null || parent.parent.name == "WndRoot" || parent.parent.name == "Canvas (Environment)")
                    {
                        break;
                    }
                }catch(Exception e)
                {

                }
                
                path = parent.name + "/" + path;
            }
            Debug.Log(path); 
            TextEditor t = new TextEditor();
            t.text = path;
            t.OnFocus();
            t.Copy();
        }
    }

    [MenuItem("GameObject/获取物体transform参数", false, -100)]
    static void FindTransformData()
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject)
        {
            Debug.Log("position:" + selectedGameObject.transform.localPosition.x + "|" + selectedGameObject.transform.localPosition.y + "|" + selectedGameObject.transform.localPosition.z + "    " +
                "rotaxion:" + selectedGameObject.transform.localEulerAngles.x + "|" + selectedGameObject.transform.localEulerAngles.y + "|" + selectedGameObject.transform.localEulerAngles.z + "    " +
                "scale:" + selectedGameObject.transform.localScale.x + "|" + selectedGameObject.transform.localScale.y + "|" + selectedGameObject.transform.localScale.z + "    "
                );
        }
    }
}