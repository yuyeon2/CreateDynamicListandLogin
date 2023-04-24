using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public class LoadAccountList : MonoBehaviour
{
    string userResgisterPath = "";

    public MainObjectData mainUserData;
    public List<InnerObjectData> innerUserData;

    public bool firstLogin = false;

    private static LoadAccountList instance;
    public static LoadAccountList Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadAccountList>();
            }

            return instance;
        }
    }
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        LoadPath();
        isUserLoginCheck = false;
    }

    public bool isUserLoginCheck;
    public string userID = "";
    public string userPW = "";

    public void LoadPath()
    {
        userResgisterPath = Application.dataPath + "/Resources/Json/UserRegister/UserRegist.json";
    }

    public void LoadAccountUser(string id, string pw)
    {
        LoadPath();

        string jsonString = File.ReadAllText(userResgisterPath);

        mainUserData = JsonUtility.FromJson<MainObjectData>(jsonString);
        for (int i=0; i < mainUserData.UserRegister.Length; i++)
        {
         
            if(id.Equals(mainUserData.UserRegister[i].userId) && pw.Equals(mainUserData.UserRegister[i].userPw))
            {
                userID = mainUserData.UserRegister[i].userId;
                userPW = mainUserData.UserRegister[i].userPw;

                isUserLoginCheck = true;
                break;
            }
            else
            {
              
            }
        }
    }
}
