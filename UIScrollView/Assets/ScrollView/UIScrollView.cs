/**
 * ClassName: UIScrollView.cs
 * 
 * @author NavyZhang
 * @date 2018-01-30 上午11:15:43
 * @version 1.0
 * 
 * 
 * ............................................
 *                       _oo0oo_
 *                      o8888888o
 *                      88" . "88
 *                      (| -_- |)
 *                      0\  =  /0
 *                    ___/`---'\___
 *                  .' \\|     |// '.
 *                 / \\|||  :  |||// \
 *                / _||||| -卍-|||||- \
 *               |   | \\\  -  /// |   |
 *               | \_|  ''\---/''  |_/ |
 *               \  .-\__  '-'  ___/-. /
 *             ___'. .'  /--.--\  `. .'___
 *          ."" '<  `.___\_<|>_/___.' >' "".
 *         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
 *         \  \ `_.   \_ __\ /__ _/   .-` /  /
 *     =====`-.____`.___ \_____/___.-`___.-'=====
 *                       `=---='
 *                       
 *..................佛祖开光 ,永无BUG...................
 * 
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    public delegate void ScrollViewRefreshCallBack(GameObject go, int dataIndex);
    [DisallowMultipleComponent]
    public class UIScrollView : MonoBehaviour
    {

       

        private ScrollViewRefreshCallBack onRefreshCallBack;

        public enum Alignment
        {
            eHorizontal,
            eVertical,
        }

        public enum CellAlignMent
        {
            eLeft,
            eCenter,
            eRight,
            eUp,
            eDown
        }
        public Alignment alignment = Alignment.eHorizontal;
        public CellAlignMent cellAlignMent = CellAlignMent.eLeft;
        public int CountOfRow = 1;
        public int CountOfCol = 1;
        public float HorizontalSpace = 0f;
        public float VerticalSpace = 0f;
        public ScrollRect m_ScrollRect;
        public RectTransform m_Content;

        private int ViewCount = 5;
        private GameObject m_ItemPrefab;
        private float CellWidth = 0f;
        private float CellHeight = 0f;
        private int dataCount;
        private int curScrollPerLineIndex = -1;
        private List<UILoopItem> listItem;
        private Queue<UILoopItem> unUseItem;
        private IList _datas;

        void Awake()
        {
            listItem = new List<UILoopItem>();
            unUseItem = new Queue<UILoopItem>();
           
        }
        public static void Reverse(IList arr, int begin, int end)
        {
            if (null == arr)
            {
                throw new ArgumentNullException("arr", "Array不能为null");
            }
            if (begin < 0 || end <= 0)
            {
                throw new ArgumentOutOfRangeException("开始或结束索引没有正确设置");
            }
            if (end > arr.Count)
            {
                throw new ArgumentOutOfRangeException("end", "结束索引超出数组长度");
            }
            while (begin < end)
            {
                object temp = arr[end];
                arr[end] = arr[begin];
                arr[begin] = temp;
                begin++;
                end--;
            }
        }

        public List<T> DealForCenter<T>(List<T> list) where T : class
        {
            int left = list.Count / 2;
            int right = list.Count / 2;
            if(list.Count % 2 == 0)
            {
                right--;
            }else
            {
                left++;
            }
            List<T> tmpList = new List<T>();
            for (int i = 0; i < list.Count; ++i)
                tmpList.Add(list[i]);
            for (int i = 0; i < list.Count; ++i)
            { 
                if (i % 2 != 0)
                {
                    tmpList[left++] = list[i];
                }
                else
                {
                    tmpList[right--] = list[i];
                }
            }
            return tmpList;
        }
       
        public void ScrollTo(float verticalNormalizedPosition,float horizontalNormalizedPosition)
        {
            m_ScrollRect.verticalNormalizedPosition = verticalNormalizedPosition;//初始化scroll的位置
            m_ScrollRect.horizontalNormalizedPosition = horizontalNormalizedPosition;
        }
        public void UpdateScrollView<T>(IList datas, GameObject prefab, ScrollViewRefreshCallBack callback = null) where T:MonoBehaviour
        {
            onRefreshCallBack = callback;
            if (m_ScrollRect == null || m_Content == null || datas == null)
            {
                Debug.LogError("参数错误");
                return;
            }
            ScrollTo(1, 0);
             _datas = datas;
            CellWidth = prefab.GetComponent<RectTransform>().rect.width;
            CellHeight = prefab.GetComponent<RectTransform>().rect.height;
            //             m_ItemPrefab  = GameObject.Instantiate(prefab) as GameObject;
            //             m_ItemPrefab.AddComponent<T>();
            if(prefab.GetComponent<T>() == null)
                prefab.AddComponent<T>();
            m_ItemPrefab = prefab;

            if (alignment == Alignment.eHorizontal)
            {
                ViewCount = Mathf.CeilToInt(transform.parent.GetComponent<RectTransform>().rect.width / CellWidth) + 1;
            }
            else
                ViewCount = Mathf.CeilToInt(transform.parent.GetComponent<RectTransform>().rect.height / CellHeight) + 1;
            if (datas.Count <= 0)
            {
                return;
            }
            setDataCount(datas.Count);

            m_ScrollRect.onValueChanged.RemoveAllListeners();
            m_ScrollRect.onValueChanged.AddListener(onValueChanged);

            unUseItem.Clear();
            listItem.Clear();

           
            if(alignment == Alignment.eHorizontal && cellAlignMent == CellAlignMent.eCenter)
            {
                setUpdateRectItem(0);
                if (ViewCount > datas.Count / CountOfCol)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + ((transform.GetComponent<RectTransform>().rect.width - m_Content.GetComponent<RectTransform>().rect.width) / 2), transform.localPosition.y);
                }
                else
                {
                    transform.localPosition = Vector3.zero;
                }
                m_ScrollRect.content.position = new Vector3(m_ScrollRect.content.position.x - m_Content.GetComponent<RectTransform>().rect.width / 2 + transform.GetComponent<RectTransform>().rect.width / 2f, m_ScrollRect.content.position.y);
                setUpdateRectItem(getCurScrollPerLineIndex());
            }else if (alignment == Alignment.eVertical && cellAlignMent == CellAlignMent.eCenter)
            {
                setUpdateRectItem(0);
                if (ViewCount > datas.Count / CountOfCol)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y - ((transform.GetComponent<RectTransform>().rect.height - m_Content.GetComponent<RectTransform>().rect.height) / 2));
                }else
                {
                    transform.localPosition = Vector3.zero;
                }
                m_ScrollRect.content.position = new Vector3(m_ScrollRect.content.position.x, m_ScrollRect.content.position.y + m_Content.GetComponent<RectTransform>().rect.height / 2 - transform.GetComponent<RectTransform>().rect.height / 2f);
                setUpdateRectItem(getCurScrollPerLineIndex());
            }else
            {
                setUpdateRectItem(0);
            }
        }



        private void setDataCount(int count)
        {
            if (dataCount == count)
            {
                return;
            }
            dataCount = count;
            setUpdateContentSize();
        }

        private void onValueChanged(Vector2 vt2)
        {
            switch (alignment)
            {
                case Alignment.eVertical:
                    float y = vt2.y;
                    if (y >= 1.0f || y <= 0.0f)
                    {
                        return;
                    }
                    break;
                case Alignment.eHorizontal:
                    float x = vt2.x;
                    if (x <= 0.0f || x >= 1.0f)
                    {
                        return;
                    }
                    break;
            }
            int _curScrollPerLineIndex = getCurScrollPerLineIndex();
            if (_curScrollPerLineIndex == curScrollPerLineIndex)
            {
                return;
            }
            setUpdateRectItem(_curScrollPerLineIndex);
        }
        //RefreshItem
        private void setUpdateRectItem(int scrollPerLineIndex)
        {
            if (scrollPerLineIndex < 0)
            {
                return;
            }
            int startDataIndex = 0;
            int endDataIndex = 0;
            curScrollPerLineIndex = scrollPerLineIndex;
            if (alignment == Alignment.eHorizontal)
            {
                
                startDataIndex = curScrollPerLineIndex * CountOfRow;
                endDataIndex = (curScrollPerLineIndex + ViewCount) * CountOfRow;
            }
            else
            {
                startDataIndex = curScrollPerLineIndex * CountOfCol;
                endDataIndex = (curScrollPerLineIndex + ViewCount) * CountOfCol;
            }

            //move item when unused
            for (int i = listItem.Count - 1; i >= 0; i--)
            {
                UILoopItem item = listItem[i];
                int index = item.Index;
                if (index < startDataIndex || index >= endDataIndex)
                {
                    item.Index = 0;
                    listItem.Remove(item);
                    unUseItem.Enqueue(item);
                }else
                {
                    item.UpdateItem(_datas, item.Index);
                }
            }
           
            for (int dataIndex = startDataIndex; dataIndex < endDataIndex; dataIndex++)
            {
                if (dataIndex >= dataCount)
                {
                    continue;
                }
                if (isExistDataByDataIndex(dataIndex))
                {
                    continue;
                }
                createItem(dataIndex);
            }
        }



       //add item
        public void AddItem(int dataIndex)
        {
            if (dataIndex < 0 || dataIndex > dataCount)
            {
                return;
            }
            bool isNeedAdd = false;
            for (int i = listItem.Count - 1; i >= 0; i--)
            {
                UILoopItem item = listItem[i];
                if (item.Index >= (dataCount - 1))
                {
                    isNeedAdd = true;
                    break;
                }
            }
            setDataCount(dataCount + 1);

            if (isNeedAdd)
            {
                for (int i = 0; i < listItem.Count; i++)
                {
                    UILoopItem item = listItem[i];
                    int oldIndex = item.Index;
                    if (oldIndex >= dataIndex)
                    {
                        item.Index = oldIndex + 1;
                        item.UpdateItem(_datas, item.Index);
                    }
                    item = null;
                }
                setUpdateRectItem(getCurScrollPerLineIndex());
            }
            else
            {
                for (int i = 0; i < listItem.Count; i++)
                {
                    UILoopItem item = listItem[i];
                    int oldIndex = item.Index;
                    if (oldIndex >= dataIndex)
                    {
                        item.Index = oldIndex;
                        item.UpdateItem(_datas, item.Index);
                    }
                    item = null;
                }
            }

        }

        public void DelItem(int dataIndex)
        {
            if (dataIndex < 0 || dataIndex >= dataCount)
            {
                return;
            }
            //1.只更新数据，不销毁gameObject,也不移除gameobject
            //2.更新数据，且移除gameObject,不销毁gameObject
            //3.更新数据，销毁gameObject

            bool isNeedDestroyGameObject = (listItem.Count >= dataCount);
            setDataCount(dataCount - 1);

            for (int i = listItem.Count - 1; i >= 0; i--)
            {
                UILoopItem item = listItem[i];
                int oldIndex = item.Index;
                if (oldIndex == dataIndex)
                {
                    listItem.Remove(item);
                    if (isNeedDestroyGameObject)
                    {
                        GameObject.Destroy(item.gameObject);
                    }
                    else
                    {
                        item.Index = -1;
                        unUseItem.Enqueue(item);
                    }
                }
                if (oldIndex > dataIndex)
                {
                    item.Index = oldIndex - 1;
                }
            }
            setUpdateRectItem(getCurScrollPerLineIndex());
        }

        int _index = 0;
        bool changeDir = false;
        public Vector3 getLocalPositionByIndex(int index)
        {
            float x = 0f;
            float y = 0f;
            float z = 0f;
            switch (alignment)
            {
                case Alignment.eHorizontal: //水平方向
                    if(cellAlignMent == CellAlignMent.eRight)
                    {
                        x = -(index / CountOfRow) * (CellWidth + HorizontalSpace);
                        y = -(index % CountOfRow) * (CellHeight + VerticalSpace);
                    }else if(cellAlignMent== CellAlignMent.eLeft)
                    {
                        x = (index / CountOfRow) * (CellWidth + HorizontalSpace);
                        y = -(index % CountOfRow) * (CellHeight + VerticalSpace);
                    }
                    else if (cellAlignMent == CellAlignMent.eCenter)
                    {
                        //                         if(index % 2 != 0)
                        //                         {
                        //                             x = -1 * Mathf.CeilToInt(index / 2f) * (CellWidth + HorizontalSpace);
                        //                         }
                        //                         else
                        //                         {
                        //                             x = (index / 2) * (CellWidth + HorizontalSpace);
                        //                         //                         }
                        //                         x = (index - _datas.Count / 2) * (CellWidth + HorizontalSpace);
                        //                         Debug.Log(string.Format("x=>{0},index=>{1}", x, index));
                        //                         y = -(index % CountOfRow) * (CellHeight + VerticalSpace);
                        x = (index / CountOfRow) * (CellWidth + HorizontalSpace);
                        y = -(index % CountOfRow) * (CellHeight + VerticalSpace);
                    }
                    else
                    {
                        Debug.LogError("布局方向错误");
                    }
                   
                    break;
                case Alignment.eVertical://垂着方向
                    if(cellAlignMent == CellAlignMent.eUp)
                    {
                        x = (index % CountOfCol) * (CellWidth + HorizontalSpace);
                        y = -(index / CountOfCol) * (CellHeight + VerticalSpace);
                    }else if (cellAlignMent == CellAlignMent.eDown)
                    {
                        x = (index % CountOfCol) * (CellWidth + HorizontalSpace);
                        y = (index / CountOfCol) * (CellHeight + VerticalSpace);
                    }
                    else if (cellAlignMent == CellAlignMent.eCenter)
                    {
                        x = (index % CountOfCol) * (CellWidth + HorizontalSpace);
                        y = -(index / CountOfCol) * (CellHeight + VerticalSpace);
                    }
                    else
                    {
                        Debug.LogError("布局方向错误");
                    }
                   
                    break;
            }
            return new Vector3(x, y, z);
        }

       
        private void createItem(int dataIndex)
        {
            UILoopItem item;
            if (unUseItem.Count > 0)
            {
                item = unUseItem.Dequeue();
            }
            else
            {
                GameObject cell = addChild(m_ItemPrefab, m_Content);
                item = cell.GetComponent<UILoopItem>();
                item.SetCallBack(onRefreshCallBack);
            }
            item.ScrollViewContent = this;
            item.Index = dataIndex;
            listItem.Add(item);
            item.InitChildNodes();
            item.UpdateItem(_datas, item.Index);
        }

      
        private bool isExistDataByDataIndex(int dataIndex)
        {
            if (listItem == null || listItem.Count <= 0)
            {
                return false;
            }
            for (int i = 0; i < listItem.Count; i++)
            {
                if (listItem[i].Index == dataIndex)
                {
                    return true;
                }
            }
            return false;
        }


        private int getCurScrollPerLineIndex()
        {
            switch (alignment)
            {
                case Alignment.eHorizontal: //水平方向
                    return Mathf.FloorToInt(Mathf.Abs(m_Content.anchoredPosition.x) / (CellWidth + HorizontalSpace));
                case Alignment.eVertical://垂着方向
                    return Mathf.FloorToInt(Mathf.Abs(m_Content.anchoredPosition.y) / (CellHeight + VerticalSpace));
            }
            return 0;
        }

       
        private void setUpdateContentSize()
        {
            int lineCount = 0;
            switch (alignment)
            {
                case Alignment.eHorizontal:
                    {
                        lineCount = Mathf.CeilToInt((float)dataCount / CountOfRow);
                        m_Content.sizeDelta = new Vector2(CellWidth * lineCount + HorizontalSpace * (lineCount - 1), m_Content.sizeDelta.y);
                    }
                    break;
                case Alignment.eVertical:
                    {
                        lineCount = Mathf.CeilToInt((float)dataCount / CountOfCol);
                        m_Content.sizeDelta = new Vector2(m_Content.sizeDelta.x, CellHeight * lineCount + VerticalSpace * (lineCount - 1));
                    }
                    break;
            }

        }

      
        private GameObject addChild(GameObject goPrefab, Transform parent)
        {
            if (goPrefab == null || parent == null)
            {
                Debug.LogError("异常。UIWarpContent.cs addChild(goPrefab = null  || parent = null)");
                return null;
            }
            GameObject goChild = GameObject.Instantiate(goPrefab) as GameObject;
            goChild.SetActive(true);
            goChild.layer = parent.gameObject.layer;
            goChild.transform.SetParent(parent, false);

            return goChild;
        }

        void OnDestroy()
        {

            m_ScrollRect = null;
            m_Content = null;
            m_ItemPrefab = null;
            onRefreshCallBack = null;

            listItem.Clear();
            unUseItem.Clear();

            

            listItem = null;
            unUseItem = null;

        }
    }

}
