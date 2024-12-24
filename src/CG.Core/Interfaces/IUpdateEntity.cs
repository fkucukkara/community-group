using System;

namespace CG.Core.Interfaces;

public interface IUpdateEntity
{
    public DateTime? ModifiedOn { get; set; }
}
