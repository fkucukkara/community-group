using System;

namespace CG.Core.Interfaces
{
    public interface ISoftDeleteEntity
    {
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
