using System;
using Marten.Schema;

namespace MikroStok.Domain.Models
{
    public class Stock
    {
        public Guid Id { get; set; }
        [DuplicateField(PgType = "uuid")]
        public Guid WarehouseId { get; set; }
        [DuplicateField(PgType = "integer")]
        public int Version { get; set; }
        [DuplicateField(PgType = "text")]
        public string ProductName { get; set; }
        [DuplicateField(PgType = "integer")]
        public int Count { get; set; }
    }
}