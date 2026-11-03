namespace WebAppSekunder
{
    public class Teller
    {
        public int Sekund { get; set; }
        public  int SalvageRig { get; set; }
        public  int Credits { get; set; }
        public int Syntech { get; set; }
        public int PowerPlant { get; set; }
        public double Structure { get; set; }
        public static string[] Ships = { "Frigate", "Destroyer", "Cruiser", "Battleship", "Carrier", "Dreadnought" };
        public int ShipClass { get; set; }
        public string Ship => Ships[ShipClass];

        public int SecondsLeft { get; set; }
        public string Building { get; set; } = "";
    }
}
