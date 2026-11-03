using Microsoft.AspNetCore.Mvc;
using WebAppSekunder;

[ApiController]
[Route("[controller]")]
public class TellerController(Teller teller) : ControllerBase
{
    [HttpGet]
    public string Get() =>
        $"Tick: {teller.Sekund}\n" +
        $"Building {teller.Ship}: {teller.Structure:F3} % (+{teller.PowerPlant * 0.005:F3} %/tick)\n" +
        $"Credits: {teller.Credits} (+{teller.SalvageRig}/tick)\n" +
        $"Salvage Rig Lv {teller.SalvageRig} (Next: {5 * teller.SalvageRig * teller.SalvageRig} cr) | Power Plant Lv {teller.PowerPlant} (Next: {100 + 5 * teller.PowerPlant * teller.PowerPlant} cr)\n" +
        (teller.SecondsLeft > 0 ? $"Constructing {teller.Building}: {teller.SecondsLeft}s left" : "Idle");

    [HttpPost("knapp")]
    public string Knapp()
    {
        if (teller.SecondsLeft > 0)
            return $"Already building {teller.Building}";

        int cost = 5 * teller.SalvageRig * teller.SalvageRig;
        if (teller.Credits < cost)
            return $"Not enough credits (need {cost})";

        teller.Credits -= cost;
        teller.Building = "Salvage Rig";
        teller.SecondsLeft = 2 * (teller.SalvageRig + 1);
        return $"Building Salvage Rig Lv {teller.SalvageRig + 1}";
    }

    [HttpPost("powerplant")]
    public string PowerPlant()
    {
        if (teller.SecondsLeft > 0)
            return $"Already building {teller.Building}";

        int cost = 100 + 5 * teller.PowerPlant * teller.PowerPlant;
        if (teller.Credits < cost)
            return $"Not enough credits (need {cost})";

        teller.Credits -= cost;
        teller.Building = "Power Plant";
        teller.SecondsLeft = 2 * (teller.PowerPlant + 1);
        return $"Building Power Plant Lv {teller.PowerPlant + 1}";
    }
}