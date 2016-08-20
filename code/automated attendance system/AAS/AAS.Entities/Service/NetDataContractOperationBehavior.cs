using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using System.Xml;
using System.ServiceModel.Channels;
namespace AAS.Service.Serialization
{
    /// <summary>
    /// یک Serializer برای رد و بدل کردن نمونه های ایجاد شده در سیستم بین Server و Client
    /// </summary>
    public class NetDataContractOperationBehavior : DataContractSerializerOperationBehavior
    {
        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <param name="operation">یک OperationDescription که نماینگار Operation است</param>
        public NetDataContractOperationBehavior(OperationDescription operation)
            : base(operation)
        {
        }
        /// <summary>
        /// متد سازنده
        /// </summary>
        /// <param name="operation">یک OperationDescription که نماینگار Operation است</param>
        /// <param name="dataContractFormatAttribute">یک DataContractFormatAttribute برای کنترل عمل serialization</param>
        public NetDataContractOperationBehavior(OperationDescription operation, DataContractFormatAttribute dataContractFormatAttribute)
            : base(operation, dataContractFormatAttribute)
        {
        }
        /// <summary>
        /// ایجاد یک نمونه که از کلاس XmlObjectSerializer به ارث می برد، برای Serialize و Deserialize کردن.
        /// </summary>
        /// <param name="type">نوع</param>
        /// <param name="name">نام</param>
        /// <param name="ns">namespace نوع مورد نظر</param>
        /// <param name="knownTypes">یک لیست که حاوی نوع های شناخته شده است</param>
        /// <returns>یک نمونه از کلاسی که XmlObjectSerializer را به ارث می برد.</returns>
        public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns,
            IList<Type> knownTypes)
        {
            return new NetDataContractSerializer(name, ns);
        }
        /// <summary>
        /// ایجاد یک نمونه که از کلاس XmlObjectSerializer به ارث می برد، برای Serialize و Deserialize کردن با یک XmlDictionaryString که حاوی namespace است.
        /// </summary>
        /// <param name="type">نوع</param>
        /// <param name="name">نام</param>
        /// <param name="ns">یک XmlDictionaryString حاوی namespace مورد نظر</param>
        /// <param name="knownTypes">یک لیست که حاوی نوع های شناخته شده است</param>
        /// <returns>یک نمونه از کلاسی که XmlObjectSerializer را به ارث می برد.</returns>
        public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name,
            XmlDictionaryString ns, IList<Type> knownTypes)
        {
            return new NetDataContractSerializer(name, ns);
        }
    }
    /// <summary>
    /// یک Attribute برای علامت گذاری متد هایی که باید از Serializer سفارشی NetDataContractOperationBehavior استفاده کنند.
    /// </summary>
    public class UseNetDataContractSerializerAttribute : Attribute, IOperationBehavior
    {
        /// <summary>
        /// ارسال اطلاعات به binding ها در زمان اجرا
        /// </summary>
        /// <param name="description">operation در حال برسی</param>
        /// <param name="parameters">مجموعه ای از نمونه هایی که قرار است این رفتار را پشتیبانی کنند</param>
        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
        }

        /// <summary>
        /// یک تغییر یا دنبال Client در یک operation
        /// </summary>
        /// <param name="description">operation در حال برسی</param>
        /// <param name="proxy">پراکسی</param>
        public void ApplyClientBehavior(OperationDescription description,
            System.ServiceModel.Dispatcher.ClientOperation proxy)
        {
            ReplaceDataContractSerializerOperationBehavior(description);
        }

        /// <summary>
        /// یک تغییر یا دنبال سرویس در یک operation
        /// </summary>
        /// <param name="description">operation در حال برسی</param>
        /// <param name="dispatch">پراکسی</param>
        public void ApplyDispatchBehavior(OperationDescription description,
            System.ServiceModel.Dispatcher.DispatchOperation dispatch)
        {
            ReplaceDataContractSerializerOperationBehavior(description);
        }

        /// <summary>
        /// تایید می کند که opreation به شرایط مورد نظر پایبند است
        /// </summary>
        /// <param name="description">opreation که قرار است برسی شود.</param>
        public void Validate(OperationDescription description)
        {
        }
        /// <summary>
        /// جایگزینی DataContractSerializerOperationBehavior
        /// </summary>
        /// <param name="description">opreation که قرار است برسی شود.</param>
        private static void ReplaceDataContractSerializerOperationBehavior(OperationDescription description)
        {
            DataContractSerializerOperationBehavior dcsOperationBehavior =
            description.Behaviors.Find<DataContractSerializerOperationBehavior>();

            if (dcsOperationBehavior != null)
            {
                description.Behaviors.Remove(dcsOperationBehavior);
                description.Behaviors.Add(new NetDataContractOperationBehavior(description));
            }
        }
    }
}
