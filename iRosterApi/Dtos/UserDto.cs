namespace iRosterApi.Dtos;

public class UserDto
{
    public int UserId { get; set; }
    public string UserIdentityId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool SharePhone { get; set; }
    public bool ShareEmail { get; set; }
    public bool ShareName { get; set; }
    public int Status { get; set; }
    public IEnumerable<RosterItemDto>? RosterItems { get; set; }
    public IEnumerable<OrgGroupDto>? RosterGroups { get; set; }
}