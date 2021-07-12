using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Directory.Models
{
    [Table("Options")]
    public class Option
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuestionId { get; set; }
    }
}