using NeuroPulse.Services.Di;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.RegistarServices();


var app = builder.Build();
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); //
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
