using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Directory.Models
{
    [Table("AdditionBlocks")]
    public class AdditionBlocks
    {
        [Key]
        public int Id { get; set; }
        public int IdOption { get; set; }
        public string DocxKey { get; set; }
        public string Information { get; set; }
    }
}