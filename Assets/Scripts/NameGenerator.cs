using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static Dictionary<Enums.Job, string[]> jobFlavors = new Dictionary<Enums.Job, string[]>();
    private static Dictionary<Enums.Race, string[]> raceLastNames = new Dictionary<Enums.Race, string[]>();
    private static Dictionary<Enums.Race, string[]> raceFirstNames = new Dictionary<Enums.Race, string[]>();
    private static string[] titles;
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

        //add last name
        if (race != Enums.Race.Undead)
        {
            string lastName = raceLastNames[race][rngesus.Next(0, raceLastNames[race].Length)];
            fullName += " " + lastName;
        }

        //add title
        fullName = titles[rngesus.Next(0, titles.Length)] + " " + fullName;

        //add class
        string jobFlavor = "";
        string flavorWord = jobFlavors[job][rngesus.Next(0, jobFlavors[job].Length)];
        if (job == Enums.Job.Priest || job == Enums.Job.Druid)
        {
            jobFlavor = job.ToString() + " of the " + flavorWord;
        }
        else if (job == Enums.Job.Knight || job == Enums.Job.Mage)
        {
            jobFlavor = flavorWord + " " + job.ToString();
        }
        else if (job == Enums.Job.Rogue)
        {
            jobFlavor = "the " + flavorWord;
        }
        fullName += ", " + jobFlavor;

        return fullName;
    }

    public static string GenerateName(Enums.Race race, Enums.Job job)
    {
        return GenerateName(job, race);
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
        raceLastNames.Add(Enums.Race.Human,
            new string[] { "Abdullah", "Adison", "Ansgar", "Bernard", "Booth", "Branford", "Carlyle", "Claiborne", "Corbinian", "Danny", "Darrin", "Demarcus", "Dureau", "Farolt", "Florentin", "Florianus", "Fontane", "Fridolin", "Hall", "Hamza", "Harvey", "Isaias", "Jerker", "Jesper", "Leonore", "Lindon", "Lorenz", "Marc", "Ozzie", "Redman", "Seabert", "Thurmon", "Alena", "Amelia", "Christine", "Delphine", "Derrica", "Doreen", "Easter", "Elka", "Fayanna", "Frauke", "Hailee", "Hannchen", "Isabeau", "Jasmyn", "Jeanine", "Joselyn", "Josette", "Josina", "Jozlyn", "Kleopatra", "Leticia", "Marcellia", "Mystique", "Nicolette", "Orsine", "Pauline", "Philomena", "Quinn", "Tara", "Wiebke", "Winifrieda", "Zuri" });
        raceLastNames.Add(Enums.Race.Elf,
            new string[] { "Akkihazel", "Amelyun", "Anlirm", "Claelar", "Daiem", "Edyhuryn", "Ehlron", "Ellndar", "Elrevaran", "Fherath", "Gaawellenar", "Giuedacil", "Glaodel", "Incndar", "Iyreborn", "Jasinaril", "Lorreth", "Oeniikoth", "Oncoacvar", "Piahanthar", "Pyrios", "Renost", "Rotitar", "Skaeron", "Sonoden", "Tamsx", "Theathiel", "Voodan", "Wisidyr", "Wyoather", "Yhaan", "Zaoaral", "Aereheira", "Aiilyntra", "Ameenyn", "Ariossa", "Azaondi", "Bonoissa", "Croyrl", "Daasola", "Edheleyn", "Enaegil", "Esyindra", "Faoh", "Hirewen", "Ikirlda", "Imitalia", "Irhirla", "Kaaaela", "Kaindra", "Lihandra", "Mueogil", "Nakorna", "Naoen", "Nyerae", "Raeaarzah", "Riniadyl", "Saelthria", "Tehhie", "Tehoinn", "Viaoyn", "Yaarrel", "Ygrha", "Zorin" });
        raceLastNames.Add(Enums.Race.Orc,
            new string[] { "Arob", "Baghig", "Bazur", "Brugo", "Bulgan", "Fozhug", "Fubdagog", "Guthug", "Hibub", "Jughragh", "Koffutto", "Krognak", "Lugrub", "Moth", "Mudagog", "Nulgha", "Oghuglat", "Oglub", "Orpigig", "Quomaugh", "Sombilge", "Spoguk", "Turge", "Ulag", "Umruigig", "Uraugh", "Viggu", "Wumanok", "Xago", "Yakha", "Zlog", "Zumhug", "Bagrak", "Bum", "Bumph", "Burub", "Dulug", "Dura", "Durz", "Garakh", "Gashnakh", "Ghak", "Gharol", "Gluronk", "Gonk", "Kharzug", "Lash", "Lagakh", "Mog", "Mogak", "Mor", "Morn", "Murbol", "Murob", "Murzush", "Nargol", "Rogbut", "Rogmesh", "Shagdub", "Sharn", "Umog", "Ushug", "Yazgash" });
        raceLastNames.Add(Enums.Race.Undead,
            new string[] { "", "", "", "" });
        jobFlavors.Add(Enums.Job.Knight,
            new string[] { "Dragon", "Chaos", "White", "Black" });
        jobFlavors.Add(Enums.Job.Druid,
            new string[] { "Moon", "Stars", "Waves", "Earth" });
        jobFlavors.Add(Enums.Job.Mage,
            new string[] { "Arcane", "Frost", "Fire", "Freeze" });
        jobFlavors.Add(Enums.Job.Priest,
            new string[] { "Light", "Shadow", "Fallen", "Damned" });
        jobFlavors.Add(Enums.Job.Rogue,
            new string[] { "Silent", "Deadly", "Patient", "Cruel" });

        titles = new string[] { "Lord", "Pontious", "Baron", "Count", "Countess", "Viscount", "Earl", "Duke", "Prince", "Marquess", "Marquis", "Princess", "Duchess", "Marchioness", "King", "Queen", "Emperor", "Empress", "Viscountess", "Baroness", "Baronet", "Baronetess", "Knight", "Esquire", "Chevalier", "Squire", "Kaiser", "Kaiserin", "Monarch" };

        initialized = true;
    }

}
