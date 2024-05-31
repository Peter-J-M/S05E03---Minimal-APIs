namespace iRosterApi.Dtos;

public class UserRosterItemDto
{
    public int OrgId { get; set; }
    public int UserId { get; set; }
    public string OrgName { get; set; }
    public string EventName { get; set; }
    public DateTime StartTime { get; set; }
    public string RoleName { get; set; }
}
