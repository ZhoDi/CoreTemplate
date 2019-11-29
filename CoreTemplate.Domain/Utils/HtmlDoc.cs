using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// Html文档处理
    /// </summary>
    public class HtmlDoc
    {
        /// <summary>
        /// Html
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="html"></param>
        public HtmlDoc(string html)
        {
            Html = html;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static HtmlDoc Load(string html)
        {
            return new HtmlDoc(html);
        }

        /// <summary>
        /// 获取所有标记节点
        /// </summary>
        public HtmlDoc[] GetElements(string tag)
        {
            int index = -1;
            string startTag1 = "<" + tag + " ", startTag2 = "<" + tag + ">", endTag = "</" + tag + ">";
            List<int> listStartIndex = new List<int>(), listEndIndex = new List<int>();

            do
            {
                index = Html.IndexOf(startTag1, index + 1);
                if (index != -1)
                    listStartIndex.Add(index);
            }
            while (index != -1);

            do
            {
                index = Html.IndexOf(startTag2, index + 1);
                if (index != -1)
                    listStartIndex.Add(index);
            }
            while (index != -1);

            do
            {
                index = Html.IndexOf(endTag, index + 1);
                if (index != -1)
                    listEndIndex.Add(index + endTag.Length);
            }
            while (index != -1);

            //if ((listStartIndex.Count == 0) || (listStartIndex.Count != listEndIndex.Count))
            //    return listElement.ToArray();

            listStartIndex = listStartIndex.OrderByDescending(m => m).ToList();
            listEndIndex = listEndIndex.OrderBy(m => m).ToList();

            List<HtmlDoc> listElement = new List<HtmlDoc>();

            foreach (int startIndex in listStartIndex)
            {
                foreach (int endIndex in listEndIndex)
                {
                    if (endIndex > startIndex)
                    {
                        listElement.Insert(0, new HtmlDoc(Html.Substring(startIndex, endIndex - startIndex)));
                        listEndIndex.Remove(endIndex);
                        break;
                    }
                }
            }

            return listElement.ToArray();
        }

        /// <summary>
        /// 获取默认标记节点即第一个标记点
        /// </summary>
        public HtmlDoc GetElement(string tag)
        {
            return GetElements(tag).FirstOrDefault();
        }

        /// <summary>
        /// 获取最后一个标记节点
        /// </summary>
        public HtmlDoc GetLastElement(string tag)
        {
            var elements = GetElements(tag);
            if (elements.Length > 0)
                return elements[elements.Length - 1];
            else
                return null;
        }

        /// <summary>
        /// 获取包含关键字的元素
        /// </summary>
        public HtmlDoc GetElementByKeyword(string tag, string keyword)
        {
            var elements = GetElements(tag);
            return elements.FirstOrDefault(m => m.Html.Contains(keyword));
        }

        /// <summary>
        /// 获取包含关键字的元素集
        /// </summary>
        public HtmlDoc[] GetElementsByKeyword(string tag, string keyword)
        {
            var elements = GetElements(tag);
            return elements.Where(m => m.Html.Contains(keyword)).ToArray();
        }

        /// <summary>
        /// 获取固定头部元素
        /// </summary>
        public HtmlDoc GetElementByHead(string tag, string head)
        {
            var elements = GetElements(tag);
            return elements.FirstOrDefault(m => m.Html.Substring(0, head.Length) == head);
        }

        /// <summary>
        /// 获取固定头部元素
        /// </summary>
        public HtmlDoc[] GetElementsByHead(string tag, string head)
        {
            var elements = GetElements(tag);
            return elements.Where(m => m.Html.Substring(0, head.Length) == head).ToArray();
        }

        /// <summary>
        /// 获取节点间值
        /// </summary>
        public string Value()
        {
            int startIndex, endIndex, length;
            bool isElementExits;
            startIndex = Html.IndexOf(">") + 1;
            endIndex = Html.LastIndexOf("<");
            isElementExits = (startIndex != 0 && endIndex != -1);
            if (isElementExits)
            {
                length = endIndex - startIndex;
                return Html.Substring(startIndex, length);
            }
            else
                return Html;
        }

        /// <summary>
        /// 获取节点间值,递归
        /// </summary>
        public string Value(int recursionTime)
        {
            var element = this;
            for (int i = 0; i <= recursionTime; i++)
                element = new HtmlDoc(element.Value());
            return element.Html;
        }

        /// <summary>
        /// 获取元素的属性
        /// </summary>
        public Dictionary<string, string> Attributes()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            string[] attrs = Html.SplitNoEmpty(' ');
            foreach (string attr in attrs)
            {
                if (attr.Contains('='))
                {
                    string[] keyValue = attr.SplitNoEmpty('=');
                    if (keyValue.Length == 2)
                        attributes.Set(keyValue[0], keyValue[1].RemoveChar('"', '\''));
                }
            }
            return attributes;
        }

        /// <summary>
        /// 获取元素的属性
        /// </summary>
        public string AttributeValue(string key)
        {
            return Attributes().Get(key).ToString();
        }
    }
}
