using MongoDB.Driver;
using MongoDB.Bson;
using FairwayAPI.Services;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
var connectionString = Environment.GetEnvironmentVariable("FairwayConnectionString");

// Add services to the container.

builder.Services.AddSingleton(new ClubInviteService(connectionString));
builder.Services.AddSingleton(new ClubService(connectionString));
builder.Services.AddSingleton(new CourseService(connectionString));
builder.Services.AddSingleton(new GameService(connectionString));
builder.Services.AddSingleton(new GameInviteService(connectionString));
builder.Services.AddSingleton(new LeagueService(connectionString));
builder.Services.AddSingleton(new MembershipRequestService(connectionString));
builder.Services.AddSingleton(new OngoingGameService(connectionString));
builder.Services.AddSingleton(new TransactionService(connectionString));
builder.Services.AddSingleton(new UpcomingGameService(connectionString));
builder.Services.AddSingleton(new UserService(connectionString));
builder.Services.AddSingleton(new ClubGameReceiptService(connectionString));
builder.Services.AddSingleton(new BuddyInviteService(connectionString));
builder.Services.AddSingleton(new LeagueGameReceiptService(connectionString));
builder.Services.AddSingleton(new FriendshipRequestService(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
