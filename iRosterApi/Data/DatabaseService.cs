using System.Data;
using Dapper;
using iRosterApi.Common;
using iRosterApi.Helpers;
using Microsoft.Data.SqlClient;

namespace iRosterApi.Data;

public class DatabaseService : IDatabaseService
{
    private readonly string _connString;
    private readonly IConfiguration _configuration;

    public DatabaseService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connString = configuration.GetConnectionString("SQLDbConnection");
    }
    
    public async Task<TResult> GetValueSingleRow<TResult>(string sql)
    {
        try
        {

            using (var db = new SqlConnection(_connString))
            {
                var res = await db.QueryFirstAsync<TResult>(sql);
                return res;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.ToString()}\nSQL= {sql}");
            //await Log.Error(e, $"SQL= {sql}");
            throw e;
        }
    }

    public async Task<IEnumerable<TResult>> GetValuesList<TResult>(string sql)
    {
        try
        {

            using (var db = new SqlConnection(_connString))
            {
                var res = await db.QueryAsync<TResult>(sql);
                return res;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.ToString()}\nSQL= {sql}");
            //await Log.Error(e, $"SQL= {sql}");
            return null;
        }
    }

    /// <summary>
    /// always include @RowsAffected and @ErrMsg
    /// </summary>
    /// <param name="procName"></param>
    /// <param name="parameters"></param>
    /// <returns></returns> 
    public async Task<ActionResult> ExecProcedure(string procName, List<ParameterInfo> parameters)
    {
        var parmDescription = string.Empty;
        try
        {
            var parms = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                parms.Add(parameter.VariableName, parameter.Value, parameter.DataType, parameter.Direction);
                parmDescription += parameter.VariableName;
                if (parameter.Direction == ParameterDirection.Input)
                    parmDescription += $"={parameter.Value},";
            }
            using (var db = new SqlConnection(_connString))
            {
                var check = await db.ExecuteAsync(procName, parms,commandType: CommandType.StoredProcedure);
                var rowsAffected = parms.Get<int>("RowsAffected");
                var errMsg = parms.Get<string>("ErrMsg");
                if (string.IsNullOrWhiteSpace(errMsg))
                {
                    return new ActionResult() { Success = true, Message = $"{rowsAffected} rows updated" };
                }

                return new ActionResult() { Success = false, Message = $"{errMsg} Procedure {procName} with parms {parmDescription}" };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.ToString()} \nProcedure {procName} with parms {parmDescription}");
            return new ActionResult(){Success = false, Message = e.Message};
            
        }
    }

}