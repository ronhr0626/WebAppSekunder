using WebAppSekunder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<Teller>();
builder.Services.AddHostedService<TickService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", () => Results.Content("""
    <h2 id="tall">...</h2>
    <button onclick="upgrade()">Upgrade Salvage Rig</button>
    <button onclick="powerPlant()">Upgrade Power Plant</button>
    <p id="svar"></p>
    <script>
        setInterval(async () => {
            const svar = await fetch("/teller");
            document.getElementById("tall").innerText = await svar.text();
        }, 1000);

        async function upgrade() {
            const svar = await fetch("/teller/knapp", { method: "POST" });
            document.getElementById("svar").innerText = await svar.text();
        }

        async function powerPlant() {
            const svar = await fetch("/teller/powerplant", { method: "POST" });
            document.getElementById("svar").innerText = await svar.text();
        }
    </script>
    """, "text/html"));
app.MapControllers();

app.Run();
