using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Directory.Models
{
    public class OptionAdditions
    {
        public OptionAdditions() { }
        public OptionAdditions(Option _option, List<AdditionBlocks> _additions)
        {
            option = _option;
            additions = _additions;
        }
        public Option option { get; set; }
        public List<AdditionBlocks> additions { get; set; }
    }
}