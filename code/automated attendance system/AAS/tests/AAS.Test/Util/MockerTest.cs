using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace AAS.Test.Util.Test
{

    public class MockerTest : AssertionHelper
    {
        class MockableClass
        {
            private static object staticField = 0;
            private object privateField = 0;

            private object privateMethod()
            {
                return 0;
            }
            private static object privateStaticMethod()
            {
                return 0;
            }
            public static object StaticField
            {
                get
                {
                    return staticField;
                }
            }
            public object PrivateField
            {
                get
                {
                    return privateField;
                }
            }
        }
        [Test]
        public void MockPrivateField_WhenCalled_SuccessfullyMocksAndReverts()
        {
            MockableClass mockableClass = new MockableClass();
            Mocker.MockPrivateField(typeof(MockableClass), mockableClass, "privateField", 12,
            () =>
            {
                Expect(() => mockableClass.PrivateField, Is.EqualTo(12));
            });
            Expect(() => mockableClass.PrivateField, Is.EqualTo(0));
        }
        [Test]
        public void MockStaticPrivateField_WhenCalled_SuccessfullyMocksAndReverts()
        {
            Mocker.MockPrivateStaticField(typeof(MockableClass), "staticField", 12,
            () =>
            {
                Expect(() => MockableClass.StaticField, Is.EqualTo(12));
            });
            Expect(() => MockableClass.StaticField, Is.EqualTo(0));
        }
        [Test]
        public void CallPrivateStaticMethod_WhenCalled_SuccessfullyCalls()
        {
            Expect(() => Mocker.CallPrivateStaticMethod(typeof(MockableClass), "privateStaticMethod", null), Is.EqualTo(0));
        }
        [Test]
        public void CallPrivateMethod_WhenCalled_SuccessfullyCalls()
        {
            MockableClass mockableClass = new MockableClass();
            Expect(() => Mocker.CallPrivateMethod(typeof(MockableClass),mockableClass, "privateMethod", null), Is.EqualTo(0));
        }
        [Test]
        public void GetPrivateFieldValue_WhenCalled_ReturnsPrivateFieldValid()
        {
            MockableClass mockableClass=new MockableClass();
            Expect(Mocker.GetPrivateFieldValue(typeof(MockableClass),mockableClass,"privateField"),Is.EqualTo(0));
        }
    }
}
