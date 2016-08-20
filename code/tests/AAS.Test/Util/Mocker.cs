using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AAS.Test.Util
{
    public class Mocker
    {

        public static void MockPrivateField(Type type,object obj ,string name, object value, Action then)
        {
            FieldInfo field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
                throw new Exception("Field was not found");

            object temp = field.GetValue(obj);
            field.SetValue(obj, value);
            then();
            field.SetValue(obj, temp);

        }
        public static void MockPrivateStaticField(Type type, string name, object value, Action then)
        {
            FieldInfo field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Static);
            object temp = field.GetValue(null);
            field.SetValue(null, value);
            then();
            field.SetValue(null, temp);
            
        }
        public static object CallPrivateStaticMethod(Type type,string name,params object [] parameters)
        {
            return type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, parameters);
        }
        public static object CallPrivateMethod(Type type, object obj,string name, params object[] parameters)
        {
            return type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, parameters);
        }
        public static object GetPrivateFieldValue(Type type,object obj,string name)
        {
            return type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }
    }
}
