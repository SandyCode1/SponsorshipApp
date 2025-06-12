using SponsorshipApp.API.Background;
using SponsorshipApp.Application.Interfaces;
using SponsorshipApp.Application.Services;
using SponsorshipApp.Infrastructure.Interfaces;
using SponsorshipApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICommunityProjectRepository, InMemoryCommunityProjectRepository>();
builder.Services.AddSingleton<ISponsorshipPlanRepository, InMemorySponsorshipPlanRepository>();
builder.Services.AddSingleton<ICommunityProjectService, CommunityProjectService>();
builder.Services.AddSingleton<ISponsorshipPlanService, SponsorshipPlanService>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddHostedService<SponsorshipScheduler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
