using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("d")]
    public class AuthorViewModel
    {
        public AuthorViewModel()
        {
            BookViewModel = new List<BookViewModel>();
        }
        //[Column("DA")]
        public int Id { get; set; }
        //[Column("AA")]
        public string Name { get; set; }
        //[Column("A")]
        public bool IsAuthor { get; set; }
        public IList<BookViewModel> BookViewModel { get; set; }
    }
}