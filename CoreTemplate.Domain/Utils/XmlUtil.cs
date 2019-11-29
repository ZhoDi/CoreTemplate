using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace CommonUtils
{
    public static class XmlUtil
    {
        /// <summary>
        /// 初始化xml
        /// </summary>
        public static XmlDocument LoadXml(string xmlString)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);
            return xml;
        }

        /// <summary>
        /// 初始化xml
        /// </summary>
        public static XmlDocument LoadFile(string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            return xml;
        }

        /// <summary>
        /// 转换为json
        /// </summary>
        public static object ConvertToJson(XmlDocument xml)
        {
            return JsonConvert.SerializeXmlNode(xml, Newtonsoft.Json.Formatting.Indented, true);
        }

        /// <summary>
        /// 获取某一节点的所有元素的属性字典
        /// </summary>
        public static MapKeyString GetMap(string xmlPath, string nodeTag, string nodeKey, string nodeValue, string elementTag, string elementKey1, string elementKey2)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlPath);
            var nodes = xml.GetElementsByTagName(nodeTag);
            var node = GetNodeByAttr(nodes, nodeKey, nodeValue);
            MapKeyString map = new MapKeyString();
            foreach (XmlNode element in node.ChildNodes)
                if (element.Name == elementTag)
                    map.Add(element.Attributes.GetValue(elementKey1), element.Attributes.GetValue(elementKey2));
            return map;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        private static string GetValue(this XmlAttributeCollection attrs, string key)
        {
            foreach (XmlAttribute attr in attrs)
                if (attr.Name == key)
                    return attr.Value;
            return null;
        }

        /// <summary>
        /// 根据属性获取节点
        /// </summary>
        private static XmlNode GetNodeByAttr(XmlNodeList nodes, string key, string value)
        {
            foreach (XmlNode node in nodes)
                if (node.Attributes.GetValue(key) == value)
                    return node;
            return null;
        }
    }
}
