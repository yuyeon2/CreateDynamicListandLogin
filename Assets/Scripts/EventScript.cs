using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventScript : MonoBehaviour
{
    LoadAccountList loadAccountList;
    GameObject objWarningPopup;
    Text txtWaringPopup;

    public InputField inputID;
    public InputField inputPW;
    string txtInputID;
    string txtInputPW;

    string path = "";

    public bool isFirstLogin = false;
    private void Awake()
    {
        loadAccountList = GameObject.Find("AccountListMgr").GetComponent<LoadAccountList>();

        loadAccountList.isUserLoginCheck = false;
        objWarningPopup = GameObject.Find("PanelLogin").transform.Find("WarningPopup").gameObject;
        txtWaringPopup = objWarningPopup.GetComponent<Text>();
    }

    void Update()
    {
        MoveInputField();
    }

    void MoveInputField()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputID.isFocused)
            {
                inputPW.Select();
            }
            else if (inputPW.isFocused)
            {
                inputID.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            LoginBtnClickEvent();
        }
    }

    public void LoginBtnClickEvent()
    {

        txtInputID = inputID.GetComponent<InputField>().text;
        txtInputPW = inputPW.GetComponent<InputField>().text;
 
        loadAccountList.LoadAccountUser(txtInputID, txtInputPW);

        if (loadAccountList.isUserLoginCheck)
        {
            SceneManager.LoadScene("02.UserRegister");
            loadAccountList.firstLogin = true;
        }
        else if (!loadAccountList.isUserLoginCheck)
        {
            objWarningPopup.SetActive(true);
        }
    }
}
