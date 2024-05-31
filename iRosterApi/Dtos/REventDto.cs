namespace iRosterApi.Dtos;

public class REventDto
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public DateTime StartTime { get; set; }
    public string TimeZone { get; set; }
    public IEnumerable<UserRosterItemDto>? RosterParticipants { get; set; }
    
}