using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Util;
using AAS.Entities.Test;
namespace AAS.Util.Test.Unit
{
    public class ValidatorTest:AssertionHelper
    {
        [Test,TestCase("a"),TestCase("1"),TestCase(" a "),TestCase("a b")]
        public void IsNullOrEmptyOrWhitespace_ContainsSomething_ReturnsFalse(string invalidInput)
        {
            Expect(Validator.IsNullOrEmptyOrWhitespace(invalidInput), Is.False);
        }

        [Test,TestCase("abcd"),TestCase("@#3"),TestCase("dad ada ad")]
        public void IsDigit_ProvidedWithNonDigit_ReturnsFalse(string nonDigit)
        {
            Expect(Validator.IsNullOrEmptyOrWhitespace(nonDigit), Is.False);
        }
        [Test] public void IsDigit_ProvidedWithDigit_ReturnsTrue()
        {
            string digit = "123456";
            Expect(Validator.IsDigit(digit), Is.True);
        }
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void IsDigit_IsNullOrEmptyOrWhitespace_ReturnsFalse(string invalidInput)
        {
            Expect(Validator.IsDigit(invalidInput), Is.False);
        }
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void IsAlphabetWithSpaceInBetween_WithInvalidInput_ReturnsFalse(string invalidInput)
        {
            Expect(Validator.IsAlphabetWithSpaceInBetween(invalidInput), Is.False);
        }
        [Test, TestCaseSource(typeof(CommonTestCaseSourceProvider), "NullOrEmptyOrWhitespace")]
        public void IsAlphabetWithSpaceInBetween_IsNullOrEmptyOrWhitespace_ReturnsFalse(string invalidInput)
        {
            Expect(Validator.IsAlphabetWithSpaceInBetween(invalidInput), Is.False);
        }


        [Test, TestCase("Test", @"\w{4}", RegexOptions.None, 1)]
        public void IsMatch_WithValidInput_MatchesRegExIsMatchOutput(string input, string pattern,RegexOptions options,int timeout)
        {
            Expect(Validator.IsMatch(input, pattern, options, TimeSpan.FromMinutes(timeout)), 
            Is.EqualTo(Regex.IsMatch(input, pattern, options, TimeSpan.FromMinutes(timeout))));
        }
    }
}
