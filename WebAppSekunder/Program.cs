using WebAppSekunder;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<Teller>();
builder.Services.AddHostedService<TickService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

app.UseAuthorization();

app.MapGet("/", () => Results.Content("""
    <img id="shipImg" src="/Images/1.png" width="600" onerror="this.src='/Images/99.png'">
    <h2 id="tall">...</h2>
    <div id="shipChoice" style="display:none; margin-bottom:10px">
        <button onclick="upgrade('upgradeship')">Yes</button>
        <button onclick="declined = true">No</button>
    </div>
    <button onclick="upgrade('miner')">Upgrade Miner</button>
    <button onclick="upgrade('factory')">Upgrade Factory</button>
    <button onclick="upgrade('testboost')" style="margin-left:20px">TEST +10 levels</button>
    <p id="svar"></p>
    <script>
        let declined = false;
        let lastImage = "1.png";

        setInterval(async () => {
            const svar = await fetch("/teller");
            const text = await svar.text();
            document.getElementById("tall").innerText = text;

            const image = await (await fetch("/teller/image")).text();
            if (image !== lastImage) {
                lastImage = image;
                document.getElementById("shipImg").src = "/Images/" + image;
            }

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