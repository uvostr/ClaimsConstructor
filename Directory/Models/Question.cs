﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Directory.Models
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public int LawsuitId { get; set; }
        public string Tip { get; set; }
        public int OrderNumber { get; set; }
        public int DependenceOptionId { get; set; }
        public string TypeInput { get; set; }


    }
}