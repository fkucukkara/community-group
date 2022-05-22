using Microsoft.EntityFrameworkCore;
using System;

namespace CG.Core.Domain
{
    [Index(nameof(Email), IsUnique = true)]
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }

        public int? CommunityGroupId { get; set; }
        public CommunityGroup CommunityGroup { get; set; }
    }
}
