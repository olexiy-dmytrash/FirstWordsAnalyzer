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
        public int Quantity { get; set; }
        public string WordText { get; set; }
        public string FirstTranslationVariant { get; set; }
        public int SentenceId { get; set; }
        public string SentenceText { get; set; }

    }
}