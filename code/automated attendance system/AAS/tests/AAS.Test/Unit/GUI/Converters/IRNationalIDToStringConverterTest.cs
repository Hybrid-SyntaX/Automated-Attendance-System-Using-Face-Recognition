using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Entities;
using System.Windows.Data;
using System.Windows.Markup;
using AAS.GUI.Converters;
namespace AAS.ManagementClient.Converters.Test.Unit
{
    public class IRNationalIDToStringConverterTest : AssertionHelper
    {
        #region SetUp & Test Data
        IRNationalIDToStringConverter iRNationalIDToStringConverterFixture;
        IRNationalID irnationalID_0000000043;
        [SetUp]
        public void SetUp()
        {
            iRNationalIDToStringConverterFixture = new IRNationalIDToStringConverter();
            irnationalID_0000000043 = new IRNationalID("000-000004-3");
        } 
        #endregion

        #region Implements MarkupExtension & IValueConverter
        [Test]
        public void IRNationalIDToStringConverter_ImplementsMarkupExtensionAndIValueConverter_Passes()
        {
            Expect(() => iRNationalIDToStringConverterFixture, Is.InstanceOf<IValueConverter>().And.InstanceOf<MarkupExtension>());
        }
        #endregion

        #region Convert
        [Test]
        public void Convert_WhenCalled_IRNationalIDIsConvertedToString()
        {
           Expect(()=>
           iRNationalIDToStringConverterFixture.Convert(irnationalID_0000000043, typeof(string), null, null), 
           Is.EqualTo("000-000004-3"));
        }
        #endregion

        #region ConvertBack
        [Test]
        public void ConvertBack_WhenCalled_StringIsConvertedToIRNationalID()
        {
            Expect(() =>
            iRNationalIDToStringConverterFixture.ConvertBack("000-000004-3",typeof(IRNationalID),null,null),
            Is.EqualTo(irnationalID_0000000043));
        }
        #endregion

        #region ProvideValue
        [Test]
        public void ProvideValue_WhenCalled_CurrentInstaceIsReturned()
        {
            Expect(() =>
            iRNationalIDToStringConverterFixture.ProvideValue(null),
            Is.EqualTo(iRNationalIDToStringConverterFixture));
        }
        #endregion
    }
}
