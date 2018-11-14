using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstWordsAnalyzer.Models
{
    public class DerivedWordChainCell
    {
        public int BasicWordId { get; set; }
        public int DerivedWordId { get; set; }
        public int Distance { get; set; }
        public int Quantity { get; set; }

    }
}