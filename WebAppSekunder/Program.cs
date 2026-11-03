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
    <button onclick="upgrade('miner')">Upgrade Miner</button>
    <button onclick="upgrade('factory')">Upgrade Factory</button>
    <div id="shipChoice" style="display:none; margin-top:10px">
        <button onclick="upgrade('upgradeship')">Yes</button>
        <button onclick="declined = true">No</button>
    </div>
    <p id="svar"></p>
    <script>
        let declined = false;

        setInterval(async () => {
            const svar = await fetch("/teller");
            const text = await svar.text();
            document.getElementById("tall").innerText = text;

            const ready = text.includes("Upgrade to");
            if (!ready) declined = false;
            document.getElementById("shipChoice").style.display =
                ready && !declined ? "block" : "none";
        }, 1000);

        async function upgrade(hva) {
            const svar = await fetch("/teller/" + hva, { method: "POST" });
            document.getElementById("svar").innerText = await svar.text();
        }
    </script>
    """, "text/html"));
app.MapControllers();

app.Run();
