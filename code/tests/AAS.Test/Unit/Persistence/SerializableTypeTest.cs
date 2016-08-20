using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AAS.Persistence.UserTypes;
using AAS.Entities;
using System.Data;
namespace AAS.Persistence.Test.Unit
{
    public class SerializableTypeTest : AssertionHelper
    {
        #region Test Data
        private IRNationalID fixtureIRNationalID;
        private ParsableType<IRNationalID> fixtureSerializableType; 
        #endregion

        #region Test SetUp
        [SetUp]
        public void SetUp()
        {
            fixtureSerializableType = new ParsableType<IRNationalID>();
            fixtureIRNationalID = new IRNationalID("000-000004-3");

        }
        
        #endregion
        
        #region SqlTypes
        [Test]
        public void SqlTypes_WhenAccessed_ReturnsDbTypeStringAsFirstIndex()
        {

            Expect(() => fixtureSerializableType.SqlTypes.First(), Is.EqualTo(new NHibernate.SqlTypes.SqlType(DbType.String)));
        }
        #endregion

        #region ReturnedType
        [Test]
        public void ReturnedType_WhenAccessed_ReturnsGenericParameterType()
        {
            Expect(() => fixtureSerializableType.ReturnedType, Is.EqualTo(fixtureIRNationalID.GetType()));
        }
        #endregion

        #region Replace
        [Test]
        public void Replace_WhenCalled_ReturnsOriginal()
        {
            Expect(()=>fixtureSerializableType.Replace(fixtureIRNationalID,null,null),
                Is.EqualTo(fixtureIRNationalID));
        }
        #endregion

        #region NullSafeGet
        [Test]
        public void NullSafeSet_WhenCalled_NullSafeGet()
        {
            //Expect(() => fixtureSerializableType.NullSafeGet()
        }
        #endregion

        #region IsMutable
        [Test] 
        public void IsMutable_WhenAccessed_ReturnsFalse()
        {
            Expect(() => fixtureSerializableType.IsMutable, Is.EqualTo(false));
        }
        #endregion

        #region GetHashCode
        [Test]
        public void GetHashCode_WhenCalled_ReturnsGetHashCode()
        {
            Expect(fixtureSerializableType.GetHashCode(fixtureIRNationalID), Is.EqualTo(fixtureIRNationalID.GetHashCode()));
        }
        #endregion

        #region Equals
        [Test]
        public void Equals_WithEqualInput_ReturnsTrue()
        {
            Expect(() => fixtureSerializableType.Equals(fixtureIRNationalID, fixtureIRNationalID),Is.True);
        }
        [Test]
        public void Equals_WithUnequalInput_ReturnsFalse()
        {
            Expect(() => fixtureSerializableType.Equals(fixtureIRNationalID, 
            new IRNationalID("000-000005-1")), Is.False);
        }
        [Test]
        public void Equals_WithNullInput_ReturnsFalse()
        {
            Expect(() => fixtureSerializableType.Equals(null, fixtureIRNationalID), Is.False);
        }
        #endregion

        #region Disassemble
        [Test]
        public void Disassemble_WithInput_ReturnsInput()
        {
            Expect(() => fixtureSerializableType.Disassemble("1234"), Is.EqualTo("1234"));
        }
        #endregion

        #region DeepCopy
        [Test]
        public void DeepCopy_WhenCalledClonesInput_ReturnsClonedInstance()
        {
            Expect(() => fixtureSerializableType.DeepCopy(fixtureIRNationalID), Is.EqualTo(fixtureIRNationalID));
        }
        [Test]
        public void DeepCopy_WhenTMethodCalledClonesInput_ReturnsClonedInstance()
        {
            Expect(() => fixtureSerializableType.DeepCopy(fixtureIRNationalID), Is.EqualTo(fixtureIRNationalID));
        }
        [Test]
        public void DeepCopy_WhenObjectMethodCalledClonesInput_ReturnsClonedInstance()
        {
            Expect(() => fixtureSerializableType.DeepCopy((object)fixtureIRNationalID), Is.EqualTo(fixtureIRNationalID));
        }
        #endregion

        #region Assemble
        [Test]
        public void Assemble_WhenCalled_ReturnsCached()
        {
            Expect(()=>fixtureSerializableType.Assemble(fixtureIRNationalID,null), Is.EqualTo(fixtureIRNationalID));
        }
        #endregion
    }
}
