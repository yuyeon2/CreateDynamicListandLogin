using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
public class UserDataObject : MonoBehaviour
{
    public MainObjectData mainObject;
    public InnerObjectData innerObject;

    List<InnerObjectData> objectList = new List<InnerObjectData>();

    public InnerObjectData createSubObject(string userId, string userPd, string userDate)
    {
        InnerObjectData _InnerObject = new InnerObjectData();

        innerObject.userId = userId;
        innerObject.userPw = userPd;
        innerObject.userDate = userDate;

        return innerObject;
    }
}

//유저 계정 관리
[Serializable]
public class MainObjectData
{
    public InnerObjectData[] UserRegister;
}

[Serializable]
public class InnerObjectData
{
    public string userId;
    public string userPw;
    public string userDate;
    public int userIndex;
}


