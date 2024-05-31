namespace iRosterApi.Dtos;

public class OrgGroupDto
{
    public int OrgGroupId { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public IEnumerable<REventDto> Events { get; set; }
}