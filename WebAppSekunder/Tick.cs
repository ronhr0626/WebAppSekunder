using WebAppSekunder;

public class TickService(Teller teller) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            teller.Sekund++;
            teller.Credits += teller.Miner;
            teller.Structure = Math.Min(100, teller.Structure + teller.Factory * 0.005);

            if (teller.SecondsLeft > 0)
            {
                teller.SecondsLeft--;
                if (teller.SecondsLeft == 0)
                {
                    if (teller.Building == "Miner") teller.Miner++;
                    if (teller.Building == "Factory") teller.Factory++;
                    teller.Building = "";
                }
            }
        }
    }
}