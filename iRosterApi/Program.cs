
using System.Data;
using AutoMapper;
using iRosterApi.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using iRosterApi.Data;
using iRosterApi.Dtos;
using iRosterApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConBuilder = new SqlConnectionStringBuilder();

sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
//sqlConBuilder.UserID = builder.Configuration["UserID"];
//sqlConBuilder.Password = builder.Configuration["Password"];
builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConBuilder.ConnectionString));
//builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:5001";
    options.TokenValidationParameters = new TokenValidationParameters() { ValidateAudience = false };
});
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("api/v1/rostersummary/{userId}", async (IDatabaseService dbService, int userId) =>
{
    try
    {
        var sql = SqlGenerator.GetRosterSummaryForUser(userId);
        Console.WriteLine(sql);
        var jsonResult = await dbService.GetValueSingleRow<string>(sql);
        return Results.Ok(jsonResult);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
        //await Log.Error(e);
        return Results.Problem($"Error {e.Message}");
    }
}).RequireAuthorization();

app.MapGet("api/v1/groupadmin/{groupId}", async (IDatabaseService dbService, int groupId) =>
{
    try
    {
        var jsonResult =
            await dbService.GetValuesList<OrgGroupDto>(SqlGenerator.GetGroupAdminSummary(groupId));
        return Results.Ok(jsonResult);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
        //await Log.Error(e);
        return Results.Problem($"Error {e.Message}");
    }
}).RequireAuthorization();

app.MapPost("api/v1/volunteer/{rosterId}/{userId}", async (IDatabaseService dbService, int rosterId, int userId) =>
{
    try
    {
        var jsonResult = await dbService.GetValueSingleRow<string>(SqlGenerator.VolunteerSql(rosterId, userId));
        return Results.Ok(jsonResult);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem($"Error {e.Message}");
    }
}).RequireAuthorization();

app.MapPost("api/v1/setunavailable/{rosterId}/{userId}", async (IDatabaseService dbService, int rosterId, int userId) =>
{
    try
    {
        var jsonResult = await dbService.GetValueSingleRow<string>(SqlGenerator.SetUnavailableSql(rosterId, userId));
        return Results.Ok(jsonResult);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem($"Error {e.Message}");
    }
}).RequireAuthorization();

app.MapPut("api/v1/updaterostertemplate/{templateUserReventTimeId}/{newUserId}", async (IDatabaseService databaseService, int templateUserReventTimeId, int newUserId) =>
{
    try
    {
        var parameters = new List<ParameterInfo>();
        parameters.Add(new ParameterInfo() { VariableName = "newUserId", DataType = DbType.Int32, Value = newUserId.ToString() });
        parameters.Add(new ParameterInfo(){VariableName = "templateId",DataType = DbType.Int32, Value = templateUserReventTimeId.ToString()});
        var check = await databaseService.ExecProcedure("ChangeUserRosterTemplate", parameters);
        if (check.Success)
            return Results.Ok(check);
        
        return Results.Problem(check.Message);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
}).RequireAuthorization();

/*
app.MapPost("api/v1/commands", async (ICommandRepo repo, IMapper mapper, CommandCreateDto cmdCreateDto) => {
    var commandModel = mapper.Map<Command>(cmdCreateDto);

    await repo.CreateCommand(commandModel);
    await repo.SaveChanges();

    var cmdReadDto = mapper.Map<CommandReadDto>(commandModel);

    return Results.Created($"api/v1/commands/{cmdReadDto.Id}", cmdReadDto);

});

app.MapPut("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id, CommandUpdateDto cmdUpdateDto) => {
    var command = await repo.GetCommandById(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    mapper.Map(cmdUpdateDto, command);

    await repo.SaveChanges();

    return Results.NoContent();
});

app.MapDelete("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) => {
    var command = await repo.GetCommandById(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    repo.DeleteCommand(command);

    await repo.SaveChanges();

    return Results.NoContent();

});
*/

app.Run();

