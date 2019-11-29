using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 编码补充
    /// 不要使用Encoding.Default
    /// Encoding.Default会随着操作系统变化
    /// </summary>
    public static class Encodings
    {
        public static Encoding GB2312 { get; }

        static Encodings()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            GB2312 = Encoding.GetEncoding(936);
        }

        public static Encoding GBK { get; } = GB2312;

        public static byte[] UTF8BomBytes { get; } = { 0xEF, 0xBB, 0xBF };

        public static Encoding UTF8Bom { get; } = new UTF8Encoding(true);

        public static Encoding UTF8NoBom { get; } = new UTF8Encoding(false);
    }
}
