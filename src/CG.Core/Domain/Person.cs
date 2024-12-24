namespace CG.Core.Domain;

public class Person : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Occupation { get; set; }
    public int? CommunityGroupId { get; set; }
    public CommunityGroup CommunityGroup { get; set; } = null;
}
