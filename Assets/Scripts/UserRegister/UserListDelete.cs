using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;

public class UserListDelete : MonoBehaviour
{
    ParsingUserData parsingUserData;
    public List<GameObject> listObj = new List<GameObject>();

    private void Start()
    {
        parsingUserData = GameObject.Find("Mgr_ad").GetComponent<ParsingUserData>();
    }

    public void Del()
    {
        GameObject obj = this.gameObject.transform.parent.gameObject;
        string nameSplit = obj.name;
        int splitNum = 0;

        if (nameSplit.Length == 6)
        {
            splitNum = Convert.ToInt32(nameSplit.Substring(nameSplit.Length - 1, 1));
        }
        else if (nameSplit.Length == 7)
        {
            splitNum = Convert.ToInt32(nameSplit.Substring(nameSplit.Length - 2, 2));
        }
        Debug.Log(splitNum);
        Debug.Log("★List Count : " + parsingUserData.objectListLoad.Count);

        Destroy(obj);

        Debug.Log(parsingUserData.objectListLoad[splitNum].userIndex);

        parsingUserData.objectListLoad.RemoveAt(splitNum);

        parsingUserData.mainObject.UserRegister = parsingUserData.objectListLoad.ToArray();

        for (int i = 0; i < parsingUserData.mainObject.UserRegister.Length; i++)
        {
            parsingUserData.objectListLoad[i].userIndex = i;
        }

        parsingUserData.loadDataLength = parsingUserData.mainObject.UserRegister.Length;

        string generatedJsonString = JsonUtility.ToJson(parsingUserData.mainObject);

        File.WriteAllText(Application.dataPath + "/Scripts/Json/UserRegist.json", generatedJsonString);

    }

}
