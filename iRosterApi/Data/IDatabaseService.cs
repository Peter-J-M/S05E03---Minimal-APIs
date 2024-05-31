using iRosterApi.Common;

namespace iRosterApi.Data;

public interface IDatabaseService
{
    Task<TResult> GetValueSingleRow<TResult>(string sql);
    Task<IEnumerable<TResult>> GetValuesList<TResult>(string sql);

    /// <summary>
    /// always include @RowsAffected and @ErrMsg
    /// </summary>
    /// <param name="procName"></param>
    /// <param name="parameters"></param>
    /// <returns></returns> 
    Task<ActionResult> ExecProcedure(string procName, List<ParameterInfo> parameters);
}