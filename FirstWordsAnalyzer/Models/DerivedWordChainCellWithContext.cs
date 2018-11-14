using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstWordsAnalyzer.Models
{
    public class DerivedWordChainCellWithContext
    {
        public int Id { get; set; }
        public int BasicWordId { get; set; }
        public int DerivedWordId { get; set; }
        public int Distance { get; set; }
        public int DerivedWordQuantity { get; set; }
        public string DerivedWordText { get; set; }
        public string DerivedWordFirstTranslationVariant { get; set; }
        public int DerivedWordSentenceId { get; set; }
        public string DerivedWordSentenceText { get; set; }

    }
}