using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        [StringLength(1024)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(1024)]
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
