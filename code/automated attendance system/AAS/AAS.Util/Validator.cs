using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
namespace AAS.Util
{
    /// <summary>
    /// اعتبار رشته ورودی در موارد مختلف را برسی می کند
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// ترکیب دو متد IsNullOrEmpty(string) و IsNullOrWhiteSpace(string) از کلاس <c>string</c>
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <returns>حالت موفقیت آمیز بودن شرط</returns>
        public static bool IsNullOrEmptyOrWhitespace(string input)
        {
            return string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// صحت از عددی بودن ورودی
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <returns>حالت موفقیت آمیز بودن شرط</returns>
        public static bool IsDigit(string input)
        {
            if (IsNullOrEmptyOrWhitespace(input))
                return false;
            return Regex.IsMatch(input, @"^\d+$");
        }
        
        /// <summary>
        /// ورودی فقط حاوی حروف الفبا و فاصله است
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <returns>حالت موفقیت آمیز بودن شرط</returns>
        public static bool IsAlphabetWithSpaceInBetween(string input)
        {
            //^(\w(\D|\s)|(\D|\s)\w)+$
            //^(\w\D|\D\w)+$                 //fails some strings
            if (IsNullOrEmptyOrWhitespace(input))
                return false;

            return Regex.IsMatch(input, @"^(\w\D\w?|\w?\D\w)+$");
        }
        
        /// <summary>
        /// Wrapper برای Regex.IsMatch(string,string)
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <param name="pattern">عبارت RegEx</param>
        /// <returns>امکان وجود عبارت منظم در رشته ورودی</returns>
        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// Wrapper برای Regex.IsMatch(string,string,RegexOptions)
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <param name="pattern">عبارت RegEx</param>
        /// <param name="options">تنظیمات مربوط به RegEx</param>
        /// <returns>امکان وجود عبارت منظم در رشته ورودی</returns>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// Wrapper برای Regex.IsMatch(string,string,RegexOptions,TimeSpan)
        /// </summary>
        /// <param name="input">رشته ورودی</param>
        /// <param name="pattern">عبارت RegEx</param>
        /// <param name="options">تنظیمات مربوط به RegEx</param>
        /// <param name="matchTimeout">حداکثر زمان ارزیابی عبارت</param>
        /// <returns>امکان وجود عبارت منظم در رشته ورودی</returns>
        public static bool IsMatch(string input, string pattern, RegexOptions options,TimeSpan matchTimeout)
        {
            return Regex.IsMatch(input, pattern, options, matchTimeout);
        }
    }
}
