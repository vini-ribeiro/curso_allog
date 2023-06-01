using Univali.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers
(
    options => 
    {
        options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
    }
);
// preciso usar a biblioteca pq nao vem no aspnet 
// o json patch nao e imbutido por padrao no net core entao a gente precisa de uma bbliooteca agora tenho suporte ao httppatch. json patch ela precisa 
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
