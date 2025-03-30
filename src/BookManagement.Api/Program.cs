using BookManagement.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddData(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddProviders();
builder.Services.AddExceptionHandlers();

builder.Services.AddControllers();
builder.Services.AddSwaggerGenWithAuth();

var app = builder.Build()

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
    await app.SeedDataAsync();
}

await app.AddDefaultDataAsync();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
