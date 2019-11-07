using System;
using Marten.Schema;

namespace MikroStok.Domain.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        
        [DuplicateField(PgType = "integer")]
        public int Version { get; set; }
        [DuplicateField(PgType = "text")]
        public string Name { get; set; }
    }
}