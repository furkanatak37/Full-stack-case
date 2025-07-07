// Program.cs

var builder = WebApplication.CreateBuilder(args);

// --- CORS Politikası Tanımlama Başlangıcı ---

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Hata mesajınızdaki origin'i ve localhost'u buraya ekleyin.
                          policy.WithOrigins("http://192.168.1.102:3000", 
                                             "http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// --- CORS Politikası Tanımlama Sonu ---


// Add services to the container.
// Önceki adımlarda eklediğimiz servisler
builder.Services.AddHttpClient<backend.Services.GoldPriceService>();
builder.Services.AddSingleton<backend.Services.GoldPriceService>();

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

// --- CORS Politikasını Uygulama Başlangıcı ---

// UseCors, UseRouting'den sonra ve UseAuthorization'dan önce gelmelidir.
app.UseCors(MyAllowSpecificOrigins);

// --- CORS Politikasını Uygulama Sonu ---


app.UseAuthorization();

app.MapControllers();

app.Run();