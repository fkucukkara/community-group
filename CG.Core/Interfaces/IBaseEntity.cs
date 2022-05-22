using System.ComponentModel.DataAnnotations;

namespace CG.Core.Interfaces
{
    public interface IBaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
