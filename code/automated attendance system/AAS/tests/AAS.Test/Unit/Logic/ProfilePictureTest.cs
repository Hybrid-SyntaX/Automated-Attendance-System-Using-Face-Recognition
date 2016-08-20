using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Drawing;
namespace AAS.Entities.Test.Unit
{
#if false
    public class ProfilePictureTest:AssertionHelper
    {
        #region Test Data
        List<ProfilePicture> InvalidProfilePicture = new List<ProfilePicture>()
        {
            new ProfilePicture(new Bitmap(400, 400))
        };
        ProfilePicture profilePictureFixture; 
        #endregion
        
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            profilePictureFixture = new ProfilePicture(@"MockFiles\image_300x300_bg0_fgf.jpg");
        }
        #endregion

        #region IsValid
        [Test, TestCaseSource("InvalidProfilePicture")]
        public void IsValid_SizeIsTooLarge_ReturnsFalse(ProfilePicture invalidProfilePicture)
        {
            Expect(() => ProfilePicture.IsValid(invalidProfilePicture), Is.False);
        }
        
        #endregion

        #region Path
        //[Test]
        //public void Path_DoesNotExist_ThrowsFileNotFoundException()
        //{
        //    Expect(() => new ProfilePicture(@"\non_existing\path\file.jpg"),Throws.Exception.TypeOf<System.IO.FileNotFoundException>());
        //}
        //[Test]
        //public void Path_Exists_Passes()
        //{
        //    Expect(() => new ProfilePicture(@"MockFiles\image_300x300_bg0_fgf.jpg"), Throws.Nothing);
        //}
        #endregion

        #region Equals
        [Test]
        public void Equals_IsEqual_Passes()
        {
	        ProfilePicture A = profilePictureFixture;
            ProfilePicture B = profilePictureFixture;
	        Expect(A.Equals(B), Is.True);
        }
        [Test]
        public void Equals_IsNotEqual_Fails()
        {
            ProfilePicture A = profilePictureFixture;
	        ProfilePicture B = new ProfilePicture();
	        Expect(A.Equals(B), Is.False);
        }
        [Test]
        public void Equals_InputIsNull_Fails()
        {
	        ProfilePicture A = new ProfilePicture();
	        Expect(A.Equals(null), Is.False);
        }
        #endregion

        #region == and != Operators
        [Test]
        public void EqualOperator_WithEqualInput_ReturnsTrue()
        {
            ProfilePicture A = profilePictureFixture;
            ProfilePicture B = profilePictureFixture;
            Expect(()=>A==B, Is.True);
        }
        [Test]
        public void EqualOperator_WithUnequalInput_ReturnsFalse()
        {
            ProfilePicture A = profilePictureFixture;
            ProfilePicture B = new ProfilePicture();
            Expect(()=>A==B, Is.False);
        }
        [Test]
        public void EqualOperator_WithNullInput_ReturnsFalse()
        {
            ProfilePicture A = null;
            ProfilePicture B = new ProfilePicture();
            Expect(()=>A==B, Is.False);
        }
        [Test]
        public void UnequalOperator_WithNullInput_ReturnsTrue()
        {
            ProfilePicture A = null;
            ProfilePicture B = new ProfilePicture();
            Expect(()=>A!=B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithUnequalInput_ReturnsTrue()
        {

            ProfilePicture A = profilePictureFixture;
            ProfilePicture B = new ProfilePicture();
            Expect(()=>A!=B, Is.True);
        }
        [Test]
        public void UnequalOperator_WithEqualInput_ReturnsFalse()
        {
            ProfilePicture A = new ProfilePicture();
            ProfilePicture B = new ProfilePicture();
            Expect(()=>A!=B, Is.False);
        }
        #endregion

        #region Clone
        [Test]
        public void Clone_ClonedProfilePicture_IsEqualToOriginalInstance()
        {
            Expect(()=>profilePictureFixture.Clone(), Is.EqualTo(profilePictureFixture));
        }
        #endregion

        #region GetHashCode
        [Test]
        public void GetHashCode_WhenCalled_Passes()
        {
            Expect(profilePictureFixture.GetHashCode(), Is.EqualTo(profilePictureFixture.Clone().GetHashCode()));
        }
        #endregion

        #region ToString
        [Test]
        public void ToString_WhenCalled_ReturnsPath()
        {
            Expect(profilePictureFixture.ToString(), Is.EqualTo(@"MockFiles\image_300x300_bg0_fgf.jpg"));
        }
        #endregion

    }
#endif
}
