using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : UILoopItem
{
    private bool isClicked = false;
    private Text text;
    private Button button;
    private GameObject gameobject;
    public override void InitChildNodes()
    {

        base.InitChildNodes();
         text = transform.Find("Text").GetComponent<Text>();
//          button = transform.Find("parent").Find("Button").GetComponent<Button>();
//         button.onClick.AddListener(delegate ()
//         {
//             if (!isClicked)
//                 gameobject.transform.localScale = new Vector3(1.5f, 1.5f);
//             else
//                 gameobject.transform.localScale = new Vector3(1f, 1f);
//             isClicked = !isClicked;
//             Debug.Log("dd");
//         });
    }
    public override void UpdateItem(IList _datas, int index)
    {
        base.UpdateItem(_datas, index);
        List<classTest> datas = (List<classTest>)_datas;
        text.text = datas[index].index.ToString();
    }
}
