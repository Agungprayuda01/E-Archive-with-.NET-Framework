using System.ComponentModel.DataAnnotations;

namespace E_Archive.Models
{
    public class Arsip
    {
        [Key] public int Id { get; set; }
        public required string Name { get; set; }
        public required string PathArsip { get; set; }
        public required string Owner {  get; set; }
    }
}
