using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections;
using System.Drawing;
namespace AAS.Entities.Test
{

    public class CommonTestCaseSourceProvider
    {
        public string[] NullOrEmptyOrWhitespace = { "", " ", null };
        public string[] InvalidAlphabet = { "abcde!@$!$" };
        public string[] InvalidDigit = { "abcd", "onetwothree", "@#@!adad" };

        public static AttendanceTime newAttendanceTimeSample(Employee employee = null)
        {
            WorkSchedule workHours = new WorkSchedule();
            DateTime entryTime = new DateTime(2014, 10, 11, 09, 0, 0);
            DateTime exitTime = new DateTime(2014, 10, 11, 17, 0, 0);
            workHours.DefineRange(entryTime.DayOfWeek, 09, 17, WorkSchedule.State.Work);
            AttendanceTime attendanceTime = new AttendanceTime()
            {
                Employee = employee,
                EntryTime = entryTime,
                ExitTime = exitTime,
              //  AttendanceHours = workHours
            };
            return attendanceTime;
        }
        public static ContactInformation newContactInformationSample()
        {
            ContactInformation contactInformation = new ContactInformation()
            {
                Label = "Home",
                PhoneNumber = "02199115532",
                CellphoneNumber = "09126512321",
                Email = "aaa@bbb.com",
                Address = "No 3, Valiasr, Tehran",
                PostalCode = "1434567891",
            };

            return contactInformation;
        }
        public static Employee newEmployeeSample(ContactInformation contactInformation = null)
        {
            Employee employee = new Employee()
            {
                FirstName = "Ahmad",
                LastName = "Rahimi",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1980, 01, 01),
                DateOfEmployement = new DateTime(2000, 04, 12),
                IdentityNumber = "13654",
                NationalID = new IRNationalID("000-000004-3"),
                EmployeeID = new EmployeeID(0, 4, 1),
                ProfilePicture = new Bitmap(@"MockFiles\image_300x300_bg0_fgf.jpg"),
            };
            if (contactInformation != null)
                employee.ContactInformations.Add(contactInformation);
            return employee;
        }
    }
}
