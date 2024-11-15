using System.ComponentModel.DataAnnotations;

namespace E_Archive.Models
{
    public class Pegawai
    {
        [Key] public required string Nik { get; set; }
        public required string Name { get; set; }
        public required string Alamat { get; set; }
        public required string Tanggallahir { get; set; }
        public required string Fotopath { get; set; }
        public required string Email { get; set; }
        public string? Role { get; set; }
    }
}
