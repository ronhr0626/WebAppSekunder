using WebAppSekunder;

public class TickService(Teller teller) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            teller.Sekund++;
            teller.Credits += teller.SalvageRig;
            teller.Structure = Math.Min(100, teller.Structure + teller.PowerPlant * 0.005);

            if (teller.SecondsLeft > 0)
            {
                teller.SecondsLeft--;
                if (teller.SecondsLeft == 0)
                {
                    if (teller.Building == "Salvage Rig") teller.SalvageRig++;
                    if (teller.Building == "Power Plant") teller.PowerPlant++;
                    teller.Building = "";
                }
            }
        }
    }
}