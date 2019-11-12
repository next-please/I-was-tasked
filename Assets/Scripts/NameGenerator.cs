using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static Dictionary<Enums.Race, string[]> raceFlavors = new Dictionary<Enums.Race, string[]>();
    private static Dictionary<Enums.Race, string[]> raceFirstNames = new Dictionary<Enums.Race, string[]>();
    private static string[,] raceJobTitles;
    private static bool initialized = false;
    private static System.Random rngesus = new System.Random();

    public static string GenerateName(Enums.Job job, Enums.Race race)
    {
        if (!initialized)
        {
            InitializeNames();
        }

        string fullName;

        //add first name
        string firstName = raceFirstNames[race][rngesus.Next(0, raceFirstNames[race].Length)];
        fullName = firstName;

        //add flavor
        string flavorWord = raceFlavors[race][rngesus.Next(0, raceFlavors[race].Length)];
        if (race == Enums.Race.Human || race == Enums.Race.Orc)
        {
            fullName = flavorWord + " " + fullName;
        }
        else if (race == Enums.Race.Elf)
        {
            fullName = fullName + " of " + flavorWord;
        }
        else if (race == Enums.Race.Undead)
        {
            fullName = fullName + " the " + flavorWord;
        }

        return fullName;
    }

    public static string GenerateName(Enums.Race race, Enums.Job job)
    {
        return GenerateName(job, race);
    }

    public static string GetTitle(Enums.Race race, Enums.Job job)
    {
        return raceJobTitles[(int)race, (int)job];
    }

    private static void InitializeNames()
    {
        raceFirstNames.Add(Enums.Race.Human,
            new string[]{ "Adonis", "Alexandre", "Allen", "Arcadius", "Camden", "Cristopher", "Currier", "Edwardson", "Egon", "Eric", "Everett", "Gustavo", "Hamlet", "Hanniel", "Horton", "Jörn", "Kade", "Kunz", "Peppi", "Pierson", "Quentrell", "Ramsay", "Remi", "Roberto", "Steffen", "Terenz", "Tripp", "Tyree", "Waldo", "Warfield", "Wess", "Wyndam", "Amira", "Bretta", "Cherrelle", "Cinzia", "Claral", "Darlene", "Elisabetta", "Gabriella", "Halsey", "Ira", "Jacqueleen", "Jenny", "Joyelle", "Jule", "Karoline", "Kelsy", "Kleopatra", "Lesly", "Linda", "Mistey", "Nadia", "Nanine", "Natalia", "Natalii", "Nathalie", "Nayeli", "Nikoline", "Selene", "Sorren", "Urte", "Valentina", "Zoe" });
        raceFirstNames.Add(Enums.Race.Elf,
            new string[] { "Atere", "Dreion", "Elbodrach", "Elporion", "Flibas", "Folaeisin", "Gaeerdh", "Halirdan", "Hocitran", "Ingodmer", "Leyrd", "Llenn", "Miiearelar", "Nloiil", "Oacaflar", "Olathan", "Oncaros", "Oryaauthin", "Oryahai", "Oslimitar", "Peil", "Piaas", "Ruahidon", "Saneiil", "Sannaran", "Sytaisar", "Taearanduil", "Ualeon", "Urarod", "Virmashal", "Zeniflar", "Zhoobor", "Amaoimnda", "Anaeene", "Annasta", "Ashouilos", "Axouria", "Bemril", "Ciyirian", "Claiah", "Cyiocia", "Duramara", "Econath", "Fayorel", "Gaoti", "Gwinath", "Hyiola", "Isironel", "Iytafain", "Kasiaya", "Kearlda", "Leiodyl", "Lyioal", "Mnuorele", "Naiswana", "Namrele", "Nuoern", "Pheeeia", "Ratehandra", "Suhnee", "Susarue", "Taeoua", "Yatela", "Ysmheira" });
        raceFirstNames.Add(Enums.Race.Orc,
            new string[] { "Agronak", "Balogog", "Diggu", "Drigka", "Ghamonk", "Drikdarok", "Fogugh", "Gaakt", "Ghamborz", "Gulm", "Jorgagu", "Karguk", "Krognak", "Krothu", "Mazhug", "Oggugat", "Opkagut", "Ortguth", "Romarod", "Seakgu", "Snakha", "Surgha", "Ulag", "Urbul", "Urg", "Vrothu", "Vultog", "Wauktug", "Xugaa", "Xugarf", "Yaghed", "Zilge", "Agrob", "Bagrak", "Bashuk", "Bor", "Borgakh", "Bulfim", "Bumph", "Burub", "Burzob", "Durgat", "Durz", "Gashnakh", "Ghak", "Ghak", "Gharol", "Glob", "Homraz", "Lagakh", "Mazoga", "Mog", "Mor", "Murob", "Murzush", "Oghash", "Rogmesh", "Sharog", "Shel", "Ugak", "Ugor", "Urog", "Ushat", "Yotul" });
        raceFirstNames.Add(Enums.Race.Undead,
            new string[] { "Anzug", "Bigyuz", "Bittereyes", "Bitterscrub", "Boutriq", "Buk", "Bunkrag", "Chag", "Chitgaq", "Chuc", "Churvod", "Dustface", "Forgeblower", "Goretaker", "Grimeclaw", "Grimesnarl", "Hairbone", "Iqrud", "Jad", "Jirdaq", "Khungut", "Kic", "Kix", "Kug", "Kulgoq", "Muckleg", "Mudshaper", "Netherchaser", "Ommax", "Oukdad", "Pestfeet", "Plaguemaw", "Raot", "Rittuax", "Ruq", "Slushdripper", "Smutsurge", "Spiderlegs", "Stormchewer", "Urvox", "Vac", "Vaz", "Voidlegs", "Voidsnout", "Vukkag", "Vuq", "Wooddribbler", "Zat", "Zig", "Zut" });
        raceFlavors.Add(Enums.Race.Human,
            new string[] { "Lord", "Baron", "Count", "Countess", "Prince", "Duke", "King", "Queen", "Earl", "Princess", "Baronet", "Baroness" });
        raceFlavors.Add(Enums.Race.Elf,
            new string[] { "Lindon", "Lorien", "Mirkwood", "Rivendell", "Edhellond", "Emelsari", "Emyrenhil", "Raen Dorei", "Eyallion", "Y'hlenora", "Gaan Serine", "Illelnora", "Ishfarius", "Onysrion" });
        raceFlavors.Add(Enums.Race.Orc,
            new string[] { "Warlord", "Warchief", "Cheiftain", "Overlord", "Taskmaster", "Raider", "Raidleader", "Wolfrider", "Clansman", "Overseer" });
        raceFlavors.Add(Enums.Race.Undead,
            new string[] { "Wretched", "Vile", "Ghastly", "Distrought", "Broken", "Deformed", "Misshapen", "Forgotten", "Hopeless", "Lost", "Forsaken" });
        
        raceJobTitles = new string[,] {
            { "Druid of the Wild", "Crusader", "Fire Mage", "Cleric", "Trickster", "", "" },
            { "Nature Spirit", "Nature Guardian", "Wild Magician", "Nephilim", "Assassin", "", "" },
            { "Protector of the Earth", "Warrior", "Thundercaller", "Inquisitor", "Swashbuckler", "", "" },
            { "Druid of the Moon", "Berserker", "Lich", "Death Prophet", "Zero", "", "" },
            { "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "" }
        };

        initialized = true;
    }

}
