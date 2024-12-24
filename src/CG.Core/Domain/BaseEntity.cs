using CG.Core.Interfaces;
using System;

namespace CG.Core.Domain;

public class BaseEntity : IBaseEntity, ICreateDate, ISoftDeleteEntity, IUpdateEntity
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
