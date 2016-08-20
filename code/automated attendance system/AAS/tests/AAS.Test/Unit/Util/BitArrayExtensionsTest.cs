using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Util;
using System.Collections;

namespace AAS.Util.Test.Unit
{
    public class BitArrayExtensionsTest : AssertionHelper
    {
        #region Test Data
        private static BitArray[] bitArrays = new BitArray[]
        { 
            new BitArray(new bool[]{false,true,true,true}){Length=4},
            new BitArray(4,false),
            new BitArray(4,true),
            new BitArray(new bool[]{true,true,true,true,false,false,true,false}){Length=8},
            new BitArray(new bool[]{false,false,false,false,false,false,true,false}){Length=8},
        };
        private List<TestCaseData> BitArrayToBinaryString = new List<TestCaseData> 
        { 
            new TestCaseData(bitArrays[0]).Returns("0111"),
            new TestCaseData(bitArrays[1]).Returns("0000"),
            new TestCaseData(bitArrays[2]).Returns("1111"),
            new TestCaseData(bitArrays[3]).Returns("11110010"),
            new TestCaseData(bitArrays[4]).Returns("00000010"),
        };
        private List<TestCaseData> BitArrayToHexacdemicalString = new List<TestCaseData> 
        { 
            new TestCaseData(bitArrays[0]).Returns("7"),
            new TestCaseData(bitArrays[1]).Returns("0"),
            new TestCaseData(bitArrays[2]).Returns("f"),
            new TestCaseData(bitArrays[3]).Returns("f2"),
            new TestCaseData(bitArrays[4]).Returns("2"),
        };

        
        #endregion
       
        #region ToBinaryString
        [Test, TestCaseSource("BitArrayToBinaryString")]
        public string ToBinaryString_WithBitArrayInput_ReturnsBinaryString(BitArray bitArray)
        {
            return bitArray.ToBinaryString();
        }
        #endregion

        #region ToHexadecimalString
        [Test, TestCaseSource("BitArrayToHexacdemicalString")]
        public string ToHexadecimalString_WithBitArrayInput_ReturnsHexadecimalString(BitArray bitArray)
        {
            return bitArray.ToHexadecimalString();
        }
        #endregion
    }
}
