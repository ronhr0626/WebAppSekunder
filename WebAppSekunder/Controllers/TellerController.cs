using Microsoft.AspNetCore.Mvc;
using WebAppSekunder;

[ApiController]
[Route("[controller]")]
public class TellerController(Teller teller) : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        string text =
            $"Tick: {teller.Sekund}\n" +
            $"Building {teller.Ship}: {teller.Structure:F3} % (+{teller.Factory * 0.005:F3} %/tick)\n" +
            $"Credits: {teller.Credits} (+{teller.Miner}/tick)\n" +
            $"Miner Lv {teller.Miner} (Next: {5 * teller.Miner * teller.Miner} cr) | Factory Lv {teller.Factory} (Next: {100 + 5 * teller.Factory * teller.Factory} cr)\n" +
            (teller.SecondsLeft > 0 ? $"Constructing {teller.Building}: {teller.SecondsLeft}s left" : "Idle");

        if (teller.Structure >= 100 && teller.ShipClass < Teller.Ships.Length - 1)
            text += $"\n\n{teller.Ship} complete! Upgrade to {Teller.Ships[teller.ShipClass + 1]}?";

        return text;
    }

    [HttpGet("image")]
    public string GetImage()
    {
        if (teller.ShipClass != 0)
            return "99.png";

        int stage = teller.Structure >= 100 ? 5 : (int)(teller.Structure / 25) + 1;
        return $"{stage}.png";
    }

    [HttpPost("upgradeship")]
    public string UpgradeShip()
    {
        if (teller.Structure < 100)
            return $"{teller.Ship} is not complete yet";
        if (teller.ShipClass >= Teller.Ships.Length - 1)
            return "Already at the largest ship";

        teller.ShipClass++;
        teller.Structure = 0;
        return $"Now building {teller.Ship}";
    }

    [HttpPost("miner")]
    public string UpgradeMiner()
    {
        if (teller.SecondsLeft > 0)
            return $"Already building {teller.Building}";

        int cost = 5 * teller.Miner * teller.Miner;
        if (teller.Credits < cost)
            return $"Not enough credits (need {cost})";

        teller.Credits -= cost;
        teller.Building = "Miner";
        teller.SecondsLeft = 2 * (teller.Miner + 1);
        return $"Building Miner Lv {teller.Miner + 1}";
    }

    [HttpPost("factory")]
    public string UpgradeFactory()
    {
        if (teller.SecondsLeft > 0)
            return $"Already building {teller.Building}";

        int cost = 100 + 5 * teller.Factory * teller.Factory;
        if (teller.Credits < cost)
            return $"Not enough credits (need {cost})";

        teller.Credits -= cost;
        teller.Building = "Factory";
        teller.SecondsLeft = 2 * (teller.Factory + 1);
        return $"Building Factory Lv {teller.Factory + 1}";
    }

    [HttpPost("testboost")]
    public string TestBoost()
    {
        teller.Miner += 10;
        teller.Factory += 10;
        return $"TEST: Miner Lv {teller.Miner}, Factory Lv {teller.Factory}";
    }
}