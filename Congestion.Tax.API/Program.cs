using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Congestion.Tax.API.MapperProfile;
using Congestion.Tax.Business;
using Congestion.Tax.Persistence.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<EntranceProfile>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IEntranceRepository, EntranceRepository>();
builder.Services.AddScoped<ITaxCalculator, CongestionTaxCalculator>();

builder.Services.AddDbContext<TaxDbContext>(options =>
    options.UseInMemoryDatabase(databaseName: "MyDatabaseName"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
}); 
app.Run();
