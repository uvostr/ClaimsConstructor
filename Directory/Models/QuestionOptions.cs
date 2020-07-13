using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Directory.Models
{
    public class QuestionOptions
    {
        public QuestionOptions(Question _question, List<Option> _options)
        {
            question = _question;
            options = _options;
        }
        public Question question { get; set; }
        public List<Option> options { get; set; }
    }
}