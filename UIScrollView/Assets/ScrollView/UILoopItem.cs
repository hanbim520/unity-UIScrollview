/**
 * ClassName: UILoopItem.cs
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    public class UILoopItem : MonoBehaviour
    {
       
        private Gc.NoGcString noGcString = new Gc.NoGcString(8);
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
