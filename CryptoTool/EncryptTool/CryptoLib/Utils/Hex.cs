using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptTool.CryptoLib.Utils
{
    class Hex
    {
        /// <summary>
        /// 将十六进制字符解码为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <returns></returns>
        static public byte[] Decode(string str)
        {
            if (String.IsNullOrEmpty(str) || str.Length % 2 != 0)
                throw new ArgumentException("所给字符串参数长度有误");

            string temp = str.ToLower();
            foreach (char c in temp.ToCharArray()) {
                if (!Char.IsLetterOrDigit(c) || (Char.IsLetter(c) && (c < 'a' || c > 'f')))
                    throw new ArgumentException("所给字符串参数有误");
            }
            byte[] midres = new byte[str.Length];
            for(int i = 0; i < str.Length; i++){
                midres[i] = Char.IsDigit(str[i]) ? (byte)(str[i] - '0') : (byte)(str[i] - 'a' + 10);
            }
            byte[] result = new byte[str.Length / 2];
            for(int i = 0;i < str.Length / 2; i++)
            {
                result[i] = (byte)((midres[2 * i] << 4) | midres[2 * i + 1]);
            }
            return result;
        }

        /// <summary>
        /// 将字节拆分成十六字节数组
        /// </summary>
        /// <param name="bs"></param>
        /// <exception cref="ArgumentException">参数错误</exception>
        /// <returns></returns>
        static public byte[] Encode(byte[] input)
        {
            byte[] CodeByte = { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 97, 98, 99, 100, 101, 102 };
            if (input == null)
                throw new ArgumentException("所给字符串参数长度有误");

            byte[] result = new byte[2 * input.Length];
            for(int i = 0;i < input.Length; i++){
                result[2 * i] = CodeByte[(input[i] & 0xF0) >> 4];
                result[2 * i + 1] = CodeByte[(input[i] & 0x0F)];
            }
            return result;
        }
    }
}
