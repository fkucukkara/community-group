using System.Collections.Generic;

namespace CG.Core.Domain
{
    public class CommunityGroup : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Person> Persons { get; set; }
    }
}
