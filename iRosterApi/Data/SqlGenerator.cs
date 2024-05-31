namespace iRosterApi.Data;

public class SqlGenerator
{
    public static string GetRosterSummaryForUser(int userId)
    {
        return $"select Dev.dbo.GetUserAppInfo({userId}) userInfo";
    }

    public static string GetGroupAdminSummary(int groupId)
    {
        return $"";
    }

    public static string GetOrgAdminSummary(int orgId)
    {
        return $"";
    }

    public static string SetUnavailableSql(int rosterId, int userId, int updatedBy)
    {
        return $"";
    }

    public static string VolunteerSql(int rosterId, int userId, int updatedBy)
    {
        return $"";
    }

    public static string SetUnavailableSql(int rosterId, int userId)
    {
        return SetUnavailableSql(rosterId, userId, userId);
    }

    public static string VolunteerSql(int rosterId, int userId)
    {
        return VolunteerSql(rosterId, userId, userId);
    }
}