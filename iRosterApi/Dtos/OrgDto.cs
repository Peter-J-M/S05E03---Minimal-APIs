namespace iRosterApi.Dtos;

public class OrgDto
{
    public int OrgId { get; set; }
    public string OrgName { get; set; }
    public string UpComingHdrText { get; set; }
    public string RosterGroupListHdrText { get; set; }
    public IEnumerable<OrgGroupDto> OrgGroups { get; set; }
}
