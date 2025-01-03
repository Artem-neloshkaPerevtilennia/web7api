var builder = WebApplication.CreateBuilder();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGithubIO",
        policy =>
        {
            policy.WithOrigins("https://artem-neloshkaperevtilennia.github.io").AllowAnyMethod().AllowAnyHeader();
        });
}); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowGithubIO");
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.Run();