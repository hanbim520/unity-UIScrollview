using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
public class classTest
{
    public int index = 0;
}

[DisallowMultipleComponent]
public class test : MonoBehaviour {
    Dictionary<classTest, int> TestDic = new Dictionary<classTest, int>();
    List<classTest> TestLst = new List<classTest>();
    // Use this for initialization

    public UIScrollView GridScrollViewCenter;
    public UIScrollView HorScrollViewCenter;
    public UIScrollView VerScrollViewCenter;

    public UIScrollView HorScrollViewLeft;
    public UIScrollView HorScrollViewRight;

    public UIScrollView VerScrollViewUp;
    public UIScrollView VerScrollViewDown;
    public UIScrollView GridScrollView;
    public UIScrollView GridScrollViewHor;

    public GameObject cellObject;

    public GameObject cellRightObject;
    public GameObject cellDownObject;
    public void OnClicked()
    {
        //         scrollView.ClearCells();
        //         DestroyImmediate(scrollView.gameObject);
    }
    
    void Start() {
        //         MapMgr.Instance.InitData("DemoMap");
        //         TestLst.Add(new classTest());
        //         TestLst.Add(new classTest());
        //         TestLst.Add(new classTest());
        // 
        //         TestDic.Add(TestLst[0], 0);
        //         TestDic.Add(TestLst[1], 1);
        // 
        //         Debug.Log("test");
        //         Debug.LogWarning("test");
        //         Debug.LogError("test");
        // 
        //         List<int> datas = new List<int>();
        //         for (int i = 0; i < 100; ++i)
        //         {
        //             datas.Add(i);
        //         }
        List<classTest> listItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            listItem.Add(testcccc);
            TestDic.Add(new classTest(),i);
        }

        listItem = GridScrollViewCenter.DealForCenter<classTest>(listItem);
        GridScrollViewCenter.UpdateScrollView<Cell>(listItem, cellObject, onInitializeItem);

        List<classTest> listGridItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            listGridItem.Add(testcccc);
            TestDic.Add(new classTest(), i);
        }

        GridScrollView.UpdateScrollView<Cell>(listGridItem, cellObject, onInitializeItem);
        GridScrollViewHor.UpdateScrollView<Cell>(listGridItem, cellObject, onInitializeItem);


        List<classTest> horlistItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            horlistItem.Add(testcccc);
            TestDic.Add(new classTest(), i);
        }
        horlistItem = HorScrollViewCenter.DealForCenter<classTest>(horlistItem);
        HorScrollViewCenter.UpdateScrollView<Cell>(listItem, cellObject, onInitializeItem);


        List<classTest> verlistItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            verlistItem.Add(testcccc);
            TestDic.Add(new classTest(), i);
        }
        verlistItem = VerScrollViewCenter.DealForCenter<classTest>(verlistItem);
        VerScrollViewCenter.UpdateScrollView<Cell>(listItem, cellObject, onInitializeItem);

        List<classTest> verUplistItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            verUplistItem.Add(testcccc);
            TestDic.Add(new classTest(), i);
        }
        VerScrollViewUp.UpdateScrollView<Cell>(verUplistItem, cellObject, onInitializeItem);

        List<classTest> verDownlistItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            verDownlistItem.Add(testcccc);
            TestDic.Add(new classTest(), i);
        }
        VerScrollViewDown.UpdateScrollView<Cell>(verDownlistItem, cellDownObject, onInitializeItem);


        List<classTest> horLeftlistItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            horLeftlistItem.Add(testcccc);
            TestDic.Add(new classTest(), i);
        }
        HorScrollViewLeft.UpdateScrollView<Cell>(horLeftlistItem, cellObject, onInitializeItem);

        List<classTest> horRightlistItem = new List<classTest>();
        for (int i = 0; i < 300; i++)
        {
            classTest testcccc = new classTest();
            testcccc.index = i;
            horRightlistItem.Add(testcccc);
            TestDic.Add(new classTest(), i);
        }
        HorScrollViewRight.UpdateScrollView<Cell>(horRightlistItem, cellRightObject, onInitializeItem);
    }

    private void onInitializeItem(GameObject go, int dataIndex)
    {

       
       	//	Debug.Log ("go = "+go.name+"_dataIndex = "+dataIndex);
        // 		
        // 				Text text = go.transform.FindChild ("Text").GetComponent<Text>();
        // 				text.text = "i:" + dataIndex+"_N:"+listItem[dataIndex].Name();
        // 		
        // 				//add按钮监听【添加功能】
        // 				Button addbutton = go.transform.FindChild ("Add").GetComponent<Button> ();
        // 				addbutton.onClick.RemoveAllListeners ();
        // 				addbutton.onClick.AddListener (delegate() {
        // 					listItem.Insert(dataIndex+1,new Item("Insert"+Random.Range(1,1000)));
        // 					warpContent.AddItem(dataIndex+1);
        // 				});
        // 		
        // 				//sub按钮监听【删除功能】
        // 				Button subButton = go.transform.FindChild ("Sub").GetComponent<Button> ();
        // 				subButton.onClick.RemoveAllListeners ();
        // 				subButton.onClick.AddListener (delegate() {
        // 					listItem.RemoveAt(dataIndex);
        // 					warpContent.DelItem(dataIndex);
        // 				});

    }
    //     private void OnDrawGizmos()
    //     {
    //         Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), new Vector3(transform.position.x, transform.position.y + 10, transform.position.z));
    //     }
    //     int outRes = 0;
    // Update is called once per frame

    byte[] aaaray = new byte[] { 1, 0, 0, 0 };
   
    void Update () {
//         BitConverter.GetBytes(32);
//         BitConverter.ToInt32(aaaray,0);
//         Debug.Log("Log");
//         Debug.LogWarning("LogWarning");
//         Debug.LogError("LogError");
        //         float aaaaa = 1f;
        //         Fix fx = SDGame.Common.GetFix(aaaaa);
        //         Debug.Log("intValue=> " + intValue.x);
        //         intValue.x = (int)fx;
        //         fx = 2;
        //         // intValue = (int)fx;
        //         Debug.Log("intValue=> " + intValue.x);
        var enumerator = TestDic.GetEnumerator();
        while (enumerator.MoveNext())
        {

        }
       enumerator.Dispose();

        foreach (KeyValuePair<classTest, int> pair in TestDic)
        {
            
        }
        //   GameObject.ReferenceEquals(enumerator.Current.Key, TestLst[0]);
    }
}
