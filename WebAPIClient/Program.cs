using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPIClient
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class GOTCharacter
    {
        [JsonProperty(PropertyName ="name")]
        public string Name { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("culture")]
        public string Culture { get; set; }

        [JsonProperty("aliases")]
        public string[] Aliases { get; set; }

        [JsonProperty("playedBy")]
        public string[] PlayedBy { get; set; }

    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepositories();
        }

        //async key indicates that it will run independently of main program flow, and will use await and be bound to a promise(task)
        //task class represents work to be done, it doesnt return a value and usually async execution.
        private static async Task ProcessRepositories()
        {
            while (true)
            {
                try //try-catch looks for an exception
                {
                    Console.WriteLine("Enter GOT character's first name. Press Enter without writing a name to quit the program.");

                    var GOTFirstName = Console.ReadLine();

                    Console.WriteLine("Enter GOT character's last name");

                    var GOTLastName = Console.ReadLine();

                    if (string.IsNullOrEmpty(GOTFirstName))
                    {
                        break;
                    }

                    //httpClient method to make an API call with user input
                    var result = await client.GetAsync("https://www.anapioficeandfire.com/api/characters/?name=" + GOTFirstName + "+" + GOTLastName);

                    //serializes HTTP content to a string as an async operation.
                    //serialization is the process of converting an object into a stream of bytes or transmit it to a memory/database/file
                    //purpose of serialization if to save the state of an object to be able to recreate it
                    var resultRead = await result.Content.ReadAsStringAsync();

                    var gotcharacterList = JsonConvert.DeserializeObject<List<GOTCharacter>>(resultRead);
                    var gotcharacter = gotcharacterList[0];

                    Console.WriteLine(resultRead);

                    Console.WriteLine("-----");
                    Console.WriteLine("Character name: " + gotcharacter.Name);
                    Console.WriteLine("Character gender: " + gotcharacter.Gender);
                    Console.WriteLine("Character culture: " + gotcharacter.Culture);
                    Console.Write("Character aliases: ");
                    for (int i = 0; i < gotcharacter.Aliases.Length; i++)
                    {
                        Console.Write($"{gotcharacter.Aliases[i]}, ");
                    }
                    Console.WriteLine("");
                    Console.Write("Character played by :");
                    for (int i = 0; i < gotcharacter.PlayedBy.Length; i++)
                    {
                        Console.Write($"{gotcharacter.PlayedBy[i]}, ");
                    }

                    Console.WriteLine("");
                    Console.WriteLine("-----");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("ERROR. please enter a valid GOT character name!");
                }
            }
        }
    }
}