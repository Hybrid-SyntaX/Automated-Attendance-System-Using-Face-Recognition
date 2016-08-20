using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace AAS.Util
{
    /// <summary>
    /// Extention Method های کلاس <c>BitArray</c>
    /// </summary>
    public static class BitArrayExtensions
    {
        /// <summary>
        /// تبدیل به رشته حاوی اعداد باینری
        /// </summary>
        /// <param name="bitArray"><c>BitArray</c> جاری</param>
        /// <returns>رشته ای از مقدار <c>BitArray</c> تبدیل شده به اعداد بانری</returns>
        public static string ToBinaryString(this BitArray bitArray)
        {
            ///[Converting to binary string]
            string result = "";
            for (int i = 0; i < bitArray.Length; i++)
                result += Convert.ToByte(bitArray[i]);

            return result;
            ///[Converting to binary string]
        }

        /// <summary>
        /// تبدیل به رشته حاوی اعداد هگزادسیمال
        /// </summary>
        /// <param name="bitArray"><c>BitArray</c> جاری</param>
        /// <returns>رشته ای از مقدار <c>BitArray</c> تبدیل شده به اعداد هگزادسیمال</returns>
        public static string ToHexadecimalString(this BitArray bitArray)
        {
            ///[Converting to hexadecimal string]
            return Convert.ToString(Convert.ToInt32(bitArray.ToBinaryString(), 2), 16);
            ///[Converting to hexadecimal string]
        }

        /// <summary>
        /// تبدیل <c>BitArray</c> به یک لیست از نوع boolean
        /// </summary>
        /// <param name="bitArray"><c>BitArray</c> جاری</param>
        /// <returns>لیست تبدیل شده</returns>
        public static List<bool> ToList(this BitArray bitArray)
        {
            ///[Converting to boolean list]
            bool[] arrayOfBools = new bool[bitArray.Length];
            bitArray.CopyTo(arrayOfBools, 0);
            return new List<bool>(arrayOfBools.ToList());
            ///[Converting to boolean list]
        }
    }
}
