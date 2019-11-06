using System;

namespace MikroStok.Domain.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
    }
}