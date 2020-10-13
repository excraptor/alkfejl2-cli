using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;

namespace game_of_thrones
{
    class Program
    {

        public static string path1 = @"character-deaths.csv";
        public static string path2 = @"character-deaths2.csv";

        static void Main(string[] args)
        {
            List<string> lines1 = File.ReadAllLines(path1).ToList();
            List<string> lines2 = File.ReadAllLines(path2).ToList();

            //task3
            List<string> characterData = lines1.Union(lines2).ToList();
            characterData.RemoveAt(0);

            List<Character> characters = new List<Character>();

            foreach(var line in characterData) 
            {
                
                string[] characterAttr = line.Split(",");
                characters.Add(new Character() 
                    {
                        Name=characterAttr[0],
                        Allegiances=characterAttr[1],
                        DeathYear=characterAttr[2] == "" ? -1 : int.Parse(characterAttr[2]),
                        BookOfDeath = characterAttr[3] == "" ? -1 : int.Parse(characterAttr[3]),
                        DeathChapter = characterAttr[4] == "" ? -1 : int.Parse(characterAttr[4]),
                        BookIntroChapter = characterAttr[5] == "" ? 0 :int.Parse(characterAttr[5]),                        
                        Gender = int.Parse(characterAttr[6]),                        
                        Nobility = int.Parse(characterAttr[7]),                        
                        GoT = int.Parse(characterAttr[8]),                        
                        CoK = int.Parse(characterAttr[9]),                        
                        SoS = int.Parse(characterAttr[10]),                        
                        FfC = int.Parse(characterAttr[11]),                        
                        DwD = int.Parse(characterAttr[12]),             
                    });
                    
            }
            var characters1 = from character in characters 
                            orderby character.Name descending
                            select character;
            characters = characters1.ToList();
            
            var charactersTask3 = from c in characters
                                    orderby c.Name
                                    select new {
                                        Name=c.Name,
                                        Allegiances=c.Allegiances,
                                        DeathYear=c.DeathYear
                                    };
            using(StreamWriter file = new StreamWriter("task3.csv"))
            {
                file.WriteLine("Name,Allegiances,DeathYear");
                foreach(var c in charactersTask3) 
                {
                    file.WriteLine(c.Name + "," + c.Allegiances + "," + c.DeathYear);
                }
            }

            //task4
            var deadPerHouse = from character in characters
                                where character.DeathYear != -1
                                group character.DeathYear by character.Allegiances into c
                                select new { Allegiance=c.Key, NumberOfDead = c.ToList().Count };
            foreach(var dead in deadPerHouse) {
                Console.WriteLine("house: " + dead.Allegiance + ", dead: " + dead.NumberOfDead);
            }
            using(StreamWriter file = new StreamWriter("task4.csv"))
            {
                file.WriteLine("House,NumberOfDead");
                foreach(var c in deadPerHouse) 
                {
                    file.WriteLine(c.Allegiance + "," + c.NumberOfDead);
                }
            }

            //task5
            var maxDeaths = deadPerHouse.Where(x => x.Allegiance != "None").Max(x => x.NumberOfDead);
            var maxDeathsHouse = from house in deadPerHouse
                                    where house.NumberOfDead == maxDeaths
                                    select house.Allegiance;
            Console.WriteLine("max deaths house: " + maxDeathsHouse.ElementAt(0) + ", deaths: " + maxDeaths);
            using(StreamWriter file = new StreamWriter("task5.csv"))
            {
                file.WriteLine("MaxHouse,MaxDeaths");
                file.WriteLine(maxDeathsHouse.ElementAt(0) + "," + maxDeaths);
            }

            //task6
            var minDeaths = deadPerHouse.Where(x => x.Allegiance != "None").Min(x => x.NumberOfDead);
            var minDeathsHouse = from house in deadPerHouse
                                    where house.NumberOfDead == minDeaths
                                    select house.Allegiance;
            Console.WriteLine("min deaths house: " + minDeathsHouse.ElementAt(0) + ", deaths: " + minDeaths);
            using(StreamWriter file = new StreamWriter("task6.csv"))
            {
                file.WriteLine("MinHouse,MinDeaths");
                file.WriteLine(minDeathsHouse.ElementAt(0) + "," + minDeaths);
            }


            //task7
            var survivedUntilFourthBook = from character in characters
                                            where character.BookOfDeath >= 4
                                            orderby character.Name
                                            select new {
                                                Name = character.Name,
                                                BookOfDeath = character.BookOfDeath,
                                                Allegiance = character.Allegiances
                                            };
            foreach(var c in survivedUntilFourthBook) 
            {
                Console.WriteLine("Name: " + c.Name + "\tBook of Death: " + c.BookOfDeath + "\tAllegiance: " + c.Allegiance);
            }
            using(StreamWriter file = new StreamWriter("task7.csv"))
            {
                file.WriteLine("Name,BookOfDeath,Allegiances");
                foreach(var c in survivedUntilFourthBook)
                {
                    file.WriteLine(c.Name + "," + c.BookOfDeath + "," + c.Allegiance);
                }
            }
            
            //task8
            var males = characters.Count(c => c.Gender == 1);
            var females = characters.Count(c => c.Gender == 0);
            Console.WriteLine("males: " + males + ", females: " + females);
            using(StreamWriter file = new StreamWriter("task8.csv"))
            {
                file.WriteLine("Males,Females");
                file.WriteLine(males + "," + females);
            }

            //task9
            var noAllegiances = from c in characters 
                                where c.Allegiances == "None"
                                select c.Name;
            Console.WriteLine("No allegiances:");
            foreach(var c in noAllegiances) 
            {
                Console.WriteLine(c);
            }
            using (StreamWriter file = new StreamWriter(@"task9.csv")) 
            {
                file.WriteLine("NoAllegiances");
                foreach(var c in noAllegiances) 
                {
                    file.WriteLine(c);
                }
            }
        }
    }
}
