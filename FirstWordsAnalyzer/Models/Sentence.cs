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
    
    public partial class Sentence
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sentence()
        {
            this.SentenceWords = new HashSet<SentenceWord>();
        }
    
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Text { get; set; }
        public decimal Length { get; set; }
    
        public virtual Book Book { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SentenceWord> SentenceWords { get; set; }
    }
}
