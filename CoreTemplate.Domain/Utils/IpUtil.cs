using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CommonUtils
{
    public static class IpUtil
    {
        /// <summary>
        /// 获取IP
        /// </summary>
        public static IPAddress Parse(string ipOrDomain)
        {
            try
            {
                return Dns.GetHostAddresses(ipOrDomain)[0];
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取EndPoint
        /// </summary>
        public static IPEndPoint EndPoint(string remote)
        {
            try
            {
                var cells = remote.Split(':', ',', ' ');
                var ip = Parse(cells[0]);
                return new IPEndPoint(ip, int.Parse(cells[1]));
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取EndPoint
        /// </summary>
        public static IPEndPoint EndPoint(string ipOrDomain, int port)
        {
            try
            {
                return new IPEndPoint(Parse(ipOrDomain), port);
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex);
                return null;
            }
        }
    }
}
