//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirstWordsAnalyzer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PotentialCognate
    {
        public Nullable<int> BasicWordId { get; set; }
        public int DerivedWordId { get; set; }
        public string BasicWordText { get; set; }
        public string DerivedWordText { get; set; }
        public string BasicWordFirstTranslationVariant { get; set; }
        public string DerivedWordFirstTranslationVariant { get; set; }
    }
}