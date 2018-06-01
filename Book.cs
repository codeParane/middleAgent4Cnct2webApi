using System.ComponentModel.DataAnnotations;
namespace Middle_app
{ 
    public class Book
    {
        [Key]
        public string Books{get; set;}
        public string Authors{get; set;}
       
    }
}