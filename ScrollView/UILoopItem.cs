using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    public class UILoopItem : MonoBehaviour
    {
       
        private NoGcString noGcString = new NoGcString(8);
        private int index;
        private UIScrollView warpScrollViewContent;
        public UIScrollView ScrollViewContent
        {
            set
            {
                warpScrollViewContent = value;
            }
        }

        private ScrollViewRefreshCallBack callBack = null;
        public void SetCallBack(ScrollViewRefreshCallBack callback)
        {
            this.callBack = callback;
        }
        public virtual void OnDestroy()
        {
            warpScrollViewContent = null;
        }
        public int Index
        {
            set
            {
                index = value;
                transform.localPosition = warpScrollViewContent.getLocalPositionByIndex(index);

                noGcString.Clear();
                if (index < 10)
                {
                    noGcString.Append("0");
                    noGcString.Append(index);
                }
                else
                    noGcString.Append(index);
                gameObject.name = noGcString.GetValueForNotVisual();
                if (callBack != null && index >= 0)
                {
                    callBack(gameObject, index);
                }
            }
            get
            {
                return index;
            }
        }

        public virtual void InitChildNodes() { }

        public virtual void UpdateItem(IList _datas, int index) { }
    }

}
