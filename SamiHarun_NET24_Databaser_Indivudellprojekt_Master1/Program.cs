using SamiHarun_NET24_Databaser_Indivudellprojekt_Master1.Data;
using SamiHarun_NET24_Databaser_Indivudellprojekt_Master1.Models;

namespace SamiHarun_NET24_Databaser_Indivudellprojekt_Master1
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public class Program
    {
        private static SkolaContext _context = new SkolaContext();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Välj en funktion:");
                Console.WriteLine("1. Hämta alla personal");
                Console.WriteLine("2. Hämta alla studenter");
                Console.WriteLine("3. Hämta alla studenter i en viss klass");
                Console.WriteLine("4. Hämta betyg som satts senaste månaden");
                Console.WriteLine("5. Lägg till en ny student");
                Console.WriteLine("6. Lägg till ny personal");
                Console.WriteLine("7. Visa alla kurser");
                Console.WriteLine("8. Visa antal år arbetade för personal");
                Console.WriteLine("9. Antal lärare per avdelning");
                Console.WriteLine("10. Visa total lön per avdelning");
                Console.WriteLine("11. Visa medellön per avdelning");
                Console.WriteLine("12. Visa information om en elev (via Stored Procedure)");
                Console.WriteLine("13. Sätt betyg på en elev (med Transactions)");
                Console.WriteLine("0. Avsluta");
                Console.Write("Välj ett alternativ: ");

                string val = Console.ReadLine();

                switch (val)
                {
                    case "1":
                        VisaPersonal();
                        break;
                    case "2":
                        VisaStudenter();
                        break;
                    case "3":
                        VisaStudenterIKlass();
                        break;
                    case "4":
                        VisaBetyg();
                        break;
                    case "5":
                        LaggTillStudent();
                        break;
                    case "6":
                        LaggTillPersonal();
                        break;
                    case "7":
                        VisaAllaKurser();
                        break;
                    case "8":
                        VisaAntalArArbetade();
                        break;
                    case "9":
                        AntalLararePerAvdelning();
                        break;
                    case "10":
                        VisaTotalLonPerAvdelning();
                        break;
                    case "11":
                        VisaMedellonPerAvdelning();
                        break;
                    case "12":
                        
                        Console.WriteLine("Ange elevens Id:");
                        int studentId = int.Parse(Console.ReadLine());

                        
                        VisaElevInfo(studentId);
                        break;
                    case "13":
                        SattBetygMedTransaction();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }

                Console.WriteLine("\nTryck på Enter för att fortsätta...");
                Console.ReadLine();
            }
        }
        // Visa alla personal
        private static void VisaPersonal()
        {
            Console.WriteLine("Välj befattning att visa (Lärare, Administratör, Rektor): ");
            string befattning = Console.ReadLine();

            
            var personal = _context.Personals.AsQueryable();

            
            if (!string.IsNullOrEmpty(befattning))
            {
                
                personal = personal.AsEnumerable()
                                   .Where(p => p.Befattning.Contains(befattning, StringComparison.OrdinalIgnoreCase))
                                   .AsQueryable();
            }
            // Skriv ut resultatet
            foreach (var p in personal)
            {
                Console.WriteLine($"{p.Namn} - {p.Befattning}");
            }
        }

        // Visa alla studenter
        private static void VisaStudenter()
        {
            Console.WriteLine("Vill du sortera på (1) Förnamn eller (2) Efternamn?");
            string sortType = Console.ReadLine();

            Console.WriteLine("Vill du ha stigande eller fallande sortering? (1 = Stigande, 2 = Fallande)");
            string order = Console.ReadLine();

            var students = _context.Studenters.AsQueryable();

            if (sortType == "2") // Efternamn
            {
                
                if (order == "1")
                    students = students.OrderBy(s => s.Namn); 
                else
                    students = students.OrderByDescending(s => s.Namn); // Sortera i fallande ordning
            }
            else // Förnamn
            {
                if (order == "1")
                    students = students.OrderBy(s => s.Namn); 
                else
                    students = students.OrderByDescending(s => s.Namn); // Sortera i fallande ordning
            }

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Namn} ({student.Klass})");
            }
        }

        // Visa studenter i en viss klass
        private static void VisaStudenterIKlass()
        {
            Console.WriteLine("Välj klass att visa (OOP24, NET24, etc.): ");
            string klass = Console.ReadLine();

            var studentsInClass = _context.Studenters
            .Where(s => s.Klass.ToLower() == klass.ToLower())
            .ToList();

            foreach (var student in studentsInClass)
            {
                Console.WriteLine($"{student.Namn} ({student.Klass})");
            }
        }

        // Visa betyg som satts senaste månaden
        private static void VisaBetyg()
        {
            var betyg = _context.Betygs
                .Include(b => b.Student) 
                .Include(b => b.Kurs)    
                .Include(b => b.Larare)  
                .Where(b => b.Datum >= DateTime.Now.AddMonths(-1))
                .Select(b => new
                {
                    StudentNamn = b.Student.Namn,
                    KursNamn = b.Kurs.Kursnamn,
                    LarareNamn = b.Larare.Namn, 
                    Betyg = b.Betyg1
                })
                .ToList();

            foreach (var b in betyg)
            {
                Console.WriteLine($"{b.StudentNamn} - {b.KursNamn} - {b.LarareNamn} - {b.Betyg}");
            }
        }

        // Lägg till en ny student
        private static void LaggTillStudent()
        {

            Console.WriteLine("Ange namn på elev:");
            string namn = Console.ReadLine();

            Console.WriteLine("Ange personnummer på elev:");
            string personnummer = Console.ReadLine();

            Console.WriteLine("Ange klass på elev:");
            string klass = Console.ReadLine();


            // Skapa en ny student utan att ange ID, eftersom det sätts automatiskt av databasen
            var nyElev = new Studenter
            {
                Namn = namn,
                Personnummer = personnummer,
                Klass = klass
            };

            // Lägger till den nya studenten till databasen
            using (var context = new SkolaContext())
            {
                
                context.Studenters.Add(nyElev);  
                context.SaveChanges();  
            }

            
            Console.WriteLine("Eleven har lagts till. Tryck på en tangent för att återgå till menyn.");
            Console.ReadKey();
        }

        // Lägg till ny personal
        private static void LaggTillPersonal()
        {
            Console.WriteLine("Ange namn på personal: ");
            string namn = Console.ReadLine();

            Console.WriteLine("Ange befattning (Lärare, Administratör, Rektor): ");
            string befattning = Console.ReadLine();

            var newPersonal = new Personal
            {
                Namn = namn,
                Befattning = befattning
            };

            _context.Personals.Add(newPersonal);
            _context.SaveChanges();

            Console.WriteLine($"Personal {namn} har lagts till.");
        }

        private static void VisaAllaKurser()
        {
            var kurser = _context.Kursers.ToList();
            foreach (var kurs in kurser)
            {
                Console.WriteLine(kurs.Kursnamn);
            }
        }

        // Visa alla personal med år av tjänst
        private static void VisaAntalArArbetade()
        {
            Console.WriteLine("Välj befattning att visa (Lärare, Administratör, Rektor eller lämna tomt för alla): ");
            string befattning = Console.ReadLine();

            var personal = _context.Personals
                .Select(p => new
                {
                    p.Namn,
                    p.Befattning,
                    AntalAr = DateTime.Now.Year - (p.Anstallningsdatum.HasValue ? p.Anstallningsdatum.Value.Year : DateTime.Now.Year)
                });

            if (!string.IsNullOrEmpty(befattning))
            {
                personal = personal.Where(p => p.Befattning.Contains(befattning, StringComparison.OrdinalIgnoreCase));
            }

            Console.WriteLine("\nNamn - Befattning - Antal År");
            Console.WriteLine(new string('-', 30));
            foreach (var p in personal)
            {
                Console.WriteLine($"{p.Namn} - {p.Befattning} - {p.AntalAr} år");
            }
        }

        // Antal lärare per avdelning
        private static void AntalLararePerAvdelning()
        {
            var antalLarare = _context.Personals
                .Where(p => p.Befattning == "Lärare")
                .GroupBy(p => p.Avdelning)
                .Select(g => new
                {
                    Avdelning = g.Key,
                    AntalLarare = g.Count()
                })
                .ToList();

            foreach (var item in antalLarare)
            {
                Console.WriteLine($"Avdelning: {item.Avdelning}, Antal lärare: {item.AntalLarare}");
            }
        }

        // Visa total lön per avdelning
        private static void VisaTotalLonPerAvdelning()
        {
            var totalLonPerAvdelning = _context.Personals
                .Where(p => p.Befattning == "Lärare")
                .GroupBy(p => p.Avdelning)
                .Select(g => new
                {
                    Avdelning = g.Key,
                    TotalLon = g.Sum(p => p.Lon)
                })
                .ToList();

            foreach (var item in totalLonPerAvdelning)
            {
                Console.WriteLine($"Avdelning: {item.Avdelning}, Total lön: {item.TotalLon}");
            }
        }

        private static void VisaMedellonPerAvdelning()
        {
            var medellonPerAvdelning = _context.Personals
                .Where(p => p.Befattning == "Lärare")
                .GroupBy(p => p.Avdelning)
                .Select(g => new
                {
                    Avdelning = g.Key,
                    Medellon = g.Average(p => p.Lon)
                })
                .ToList();

            Console.WriteLine("Avdelning - Medellön");
            Console.WriteLine(new string('-', 25));

            foreach (var item in medellonPerAvdelning)
            {
                Console.WriteLine($"{item.Avdelning} - {item.Medellon:F2} kr");
            }
        }
        private static void VisaElevInfo(int studentId)
        {
            
            var elevInfoList = _context.ElevInfo
                .FromSqlRaw("EXEC GetElevInfo @StudentId = {0}", studentId)
                .ToList();

            
            if (elevInfoList.Any())
            {
                Console.WriteLine($"Elev: {elevInfoList.First().StudentNamn}");
                Console.WriteLine($"Personnummer: {elevInfoList.First().Personnummer}");
                Console.WriteLine($"Klass: {elevInfoList.First().Klass}");

                
                foreach (var elevInfo in elevInfoList)
                {
                    Console.WriteLine($"Kurs: {elevInfo.Kurs}, Läraren: {elevInfo.Larare}, Betyg: {elevInfo.SenasteBetyg}");
                }
            }
            else
            {
                Console.WriteLine("Studenten kunde inte hittas.");
            }
        }

        // Sätt betyg på en elev
        private static void SattBetygMedTransaction()
        {
            Console.WriteLine("Ange elevens ID:");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }

            Console.WriteLine("Ange kursens ID:");
            if (!int.TryParse(Console.ReadLine(), out int kursId))
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }

            Console.WriteLine("Ange lärarens ID:");
            if (!int.TryParse(Console.ReadLine(), out int larareId))
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }

            Console.WriteLine("Ange betyg (A-F):");
            string betyg = Console.ReadLine();

            if (string.IsNullOrEmpty(betyg) || !"ABCDEF".Contains(betyg.ToUpper()))
            {
                Console.WriteLine("Ogiltigt betyg. Ange ett betyg mellan A och F.");
                return;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    
                    var nyttBetyg = new Betyg
                    {
                        StudentId = studentId,
                        KursId = kursId,
                        LarareId = larareId,
                        Betyg1 = betyg.ToUpper(),
                        Datum = DateTime.Now
                    };

                    _context.Betygs.Add(nyttBetyg);
                    _context.SaveChanges();

                    
                    transaction.Commit();
                    Console.WriteLine("Betyget har lagts till.");
                }
                catch (Exception ex)
                {
                    // Rollback transaction vid fel
                    transaction.Rollback();
                    Console.WriteLine($"Något gick fel: {ex.Message}");
                }
            }

        }
    }
}
