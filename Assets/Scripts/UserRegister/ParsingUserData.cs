
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ParsingUserData : MonoBehaviour
{
    public GameObject objScrollViewContent;
    public TMP_InputField inputUserId;
    public TMP_InputField inputUserPw;
    public TMP_InputField inputUserDate;

    public GameObject objItemList;

    public MainObjectData mainObject;
    public InnerObjectData innerObject;

    public List<InnerObjectData> objectListLoad;
    public List<InnerObjectData> objectListSave;
    public InnerObjectData createSubObject(string userId, string userPw, string userDate, int index_)
    {
        InnerObjectData myInnerObject = new InnerObjectData();

        innerObject.userId = userId;
        innerObject.userPw = userPw;
        innerObject.userDate = userDate;
        innerObject.userIndex = index_;

        return innerObject;
    }

    string valUserID;
    string valUserPW;
    string valUserDate;
    string nowtime;

    int saveCount = 0;
    int userIndex_;

    bool isUserIdOverlab = false;

    string path = "";
    LoadAccountList loadAccountList;

    Coroutine co;

    public GameObject imgOverLab;
    public GameObject imgFail;
    public GameObject imgOverCount;

    bool isOverlabID = false;

    TextMeshProUGUI txtUserId;
    TextMeshProUGUI txtUserPw;
    TextMeshProUGUI txtUserDate;

    GameObject btnDelete;

    Button button;

    public int loadDataLength;
    GameObject childObj;

    private void Start()
    {
        loadAccountList = GameObject.Find("AccountListMgr").GetComponent<LoadAccountList>();
        isOverlabID = false;
        isUserIdOverlab = false;
        path = Application.dataPath + "/Resources/Json/UserRegister/UserRegist.json";

        objectListLoad = new List<InnerObjectData>();
        NowDate();
        LoadData();
    }

    public void BackBtn()
    {
        SceneManager.LoadScene("01.LoginScene");

    }
    public void SaveData()
    {
        loadAccountList.LoadPath();

        valUserID = inputUserId.text;
        valUserPW = inputUserPw.text;
        valUserDate = inputUserDate.text;

        if (!isUserIdOverlab)
        {
            for (int i=0; i < mainObject.UserRegister.Length;i++)
            {
                if (valUserID == mainObject.UserRegister[i].userId)
                {
                    isOverlabID = true;
                }
            }

            if (isOverlabID)
            {
                if (co != null)
                {
                    StopCoroutine(co);
                    imgOverLab.SetActive(false);
                    imgFail.SetActive(false);
                    imgOverCount.SetActive(false);
                }
                co = StartCoroutine(HideObj(imgOverLab));
                isOverlabID = false;
            }
            else if (!isOverlabID)
            {
                Debug.Log("d1");

                if (valUserPW.Length != 0 && valUserID.Length != 0)
                {
                    Debug.Log("d2");
                    Debug.Log(loadDataLength);
                    Debug.Log(loadDataLength);
                    if (loadDataLength < 10)
                    {
                        Debug.Log("d3");

                        objectListSave = new List<InnerObjectData>();

                        DataInput(valUserID, valUserPW, valUserDate, loadDataLength);

                        mainObject.UserRegister = objectListLoad.ToArray();

                        string generatedJsonString = JsonUtility.ToJson(mainObject);

                        File.WriteAllText(path, generatedJsonString);

                        //load
                        string jsonString = File.ReadAllText(path);

                        mainObject = JsonUtility.FromJson<MainObjectData>(jsonString);

                        loadDataLength = mainObject.UserRegister.Length;

                        objectListLoad = new List<InnerObjectData>();

                        for (int i = 0; i < loadDataLength; i++)
                        {
                            objectListLoad.Add(mainObject.UserRegister[i]);
                        }

                        string _valUserID = mainObject.UserRegister[loadDataLength - 1].userId;
                        string _valUserPW = mainObject.UserRegister[loadDataLength - 1].userPw;
                        string _valUserDate = mainObject.UserRegister[loadDataLength - 1].userDate;

                        userIndex_ = mainObject.UserRegister[loadDataLength - 1].userIndex;

                        CreateList(loadDataLength - 1, _valUserID, _valUserPW, _valUserDate);
                    }
                    else if (loadDataLength == 10)
                    {
                        Debug.Log("D");
                        if (co != null)
                        {
                            imgOverLab.SetActive(false);
                            imgFail.SetActive(false);
                            imgOverCount.SetActive(false);

                            StopCoroutine(co);
                        }
                        co = StartCoroutine(HideObj(imgOverCount));
                    }

                }
                else if (valUserID.Length == 0 || valUserPW.Length == 0)
                {
                    if (co != null)
                    {
                        imgOverLab.SetActive(false);
                        imgFail.SetActive(false);
                        imgOverCount.SetActive(false);

                        StopCoroutine(co);
                    }
                    co = StartCoroutine(HideObj(imgFail));

                }
               
            }
        }
    }

    IEnumerator HideObj(GameObject Obj)
    {
        Obj.SetActive(true);
        yield return new WaitForSeconds(3f);
        Obj.SetActive(false);
        isUserIdOverlab = false;
    }

    void CreateList(int num, string userId, string userPw, string userDate)
    {
        int val = 0;

        var index = Instantiate(objItemList, new Vector3(0, val, 0), Quaternion.identity);
        index.transform.SetParent(objScrollViewContent.transform);
        val -= 200;

        index.name = "Item_" + num;
        button = GetComponent<Button>();
        button = index.transform.GetChild(3).GetComponent<Button>();
        button.onClick.AddListener((DeleteObj));

        txtUserId = objScrollViewContent.transform.GetChild(num).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        txtUserPw = objScrollViewContent.transform.GetChild(num).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        txtUserDate = objScrollViewContent.transform.GetChild(num).transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        txtUserId.text = userId;
        txtUserPw.text = userPw;
        txtUserDate.text = userDate;
    }

    void DataInput(string userId, string userPw, string userDate, int index_)
    {
        objectListLoad.Add(createSubObject(userId, userPw, userDate, index_));
    }
    void LoadData()
    {
        try
        {
            if (!File.Exists(path))
            {
                Debug.Log("There is no file in the folder");
            }
            else
            {
            
                string jsonString = File.ReadAllText(path);

                mainObject = JsonUtility.FromJson<MainObjectData>(jsonString);
                loadDataLength = mainObject.UserRegister.Length;

                for (int i=0; i < loadDataLength; i++)
                {
                    
                    objectListLoad.Add(mainObject.UserRegister[i]);
                    string valUserID_ = mainObject.UserRegister[i].userId;
                    string valUserPW_ = mainObject.UserRegister[i].userPw;
                    string valUserDate_ = mainObject.UserRegister[i].userDate;
                    CreateList(i, valUserID_, valUserPW_, valUserDate_) ;

                }
                mainObject.UserRegister = objectListLoad.ToArray();
            }
        }
        catch (Exception e)
        {

        }
    }

    void DeleteObj()
    {
        int objlength = GameObject.Find("BgUserRegister/Scroll View/Viewport/Content").gameObject.transform.childCount;
        GameObject obj = GameObject.Find("BgUserRegister/Scroll View/Viewport/Content");

        string ButtonName = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        int splitNum = 0;

        for (int i = 0; i < objlength; i++)
        {
            if (obj.transform.GetChild(i).gameObject.name == ButtonName)
            {
                splitNum = i;
                childObj = obj.transform.GetChild(splitNum).gameObject;
            }
      }

        Destroy(childObj);
        objectListLoad.RemoveAt(splitNum);

        mainObject.UserRegister = objectListLoad.ToArray();

        for (int i = 0; i < mainObject.UserRegister.Length; i++)
        {
            objectListLoad[i].userIndex = i;
        }

        loadDataLength = mainObject.UserRegister.Length;

        string generatedJsonString = JsonUtility.ToJson(mainObject);

        File.WriteAllText(Application.dataPath + "/Resources/Json/UserRegister/UserRegist.json", generatedJsonString);
    }
    public void NowDate()
    {
        nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        inputUserDate.text = nowtime;
    }
}