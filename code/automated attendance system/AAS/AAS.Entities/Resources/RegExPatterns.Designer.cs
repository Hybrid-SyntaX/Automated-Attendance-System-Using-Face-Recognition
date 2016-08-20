﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AAS.Entities.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RegExPatterns {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RegExPatterns() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AAS.Entities.Resources.RegExPatterns", typeof(RegExPatterns).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^(\+\d)?\d{4}\-?\d{3}\-?\d{4}$.
        /// </summary>
        internal static string CellphoneNumber {
            get {
                return ResourceManager.GetString("CellphoneNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$.
        /// </summary>
        internal static string EmailFormat {
            get {
                return ResourceManager.GetString("EmailFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^\d{1,3}\-?\d{6}\-?\d{1}$.
        /// </summary>
        internal static string IRNationalID {
            get {
                return ResourceManager.GetString("IRNationalID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^\+?\d?\d?\d{3}-?\d{4}-?\d{4}$.
        /// </summary>
        internal static string PhoneNumber {
            get {
                return ResourceManager.GetString("PhoneNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[\d-[02]]{10}$.
        /// </summary>
        internal static string PosalCode {
            get {
                return ResourceManager.GetString("PosalCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[\d|ABCDEF]{1,6}\-[\d|ABCDEF]{1,6}\-[\d|ABCDEF]{1,6}\-[\d|ABCDEF]{1,6}\-[\d|ABCDEF]{1,6}\-[\d|ABCDEF]{1,6}\-[\d|ABCDEF]{1,6}$.
        /// </summary>
        internal static string WorkSchedule {
            get {
                return ResourceManager.GetString("WorkSchedule", resourceCulture);
            }
        }
    }
}