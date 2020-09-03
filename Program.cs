using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace BotWhatsapp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Dynamic();
            //await Static();            
        }

        public async static Task Dynamic()
        {  
            var targets = new List<string>();
            var ended = false;            
            var message = string.Empty;

            Console.WriteLine("--- Hello - LF WhatsApp ---");            

            do
            {
                Console.WriteLine("Type a name (group, contact, etc)");

                var input = Console.ReadLine().ToString();

                if (!string.IsNullOrEmpty(input))
                {
                    targets.Add(input);

                    Console.WriteLine("Are you done? (y/n)");
                    var done = Console.ReadLine().ToString();

                    if (!string.IsNullOrEmpty(done) && string.Equals(done.ToLower(), "y"))
                        ended = !ended;
                }                     

            } while (!ended);

            Console.WriteLine("Type your message");

            do
            {                
                var input = Console.ReadLine().ToString();

                if (input != null && !string.IsNullOrEmpty(input))
                    message = input;

            } while (string.IsNullOrEmpty(message));          

            await GoToWhatsApp(targets, message);
        }

        public async static Task Static()
        {
            var list = new List<string>()
            {
                "Contact 1...",               
            };
        
            var message = "Default message!!!";

            await GoToWhatsApp(list, message);
        }

        public async static Task GoToWhatsApp(List<string> targets, string message)
        {
            var count = 0;

            Console.WriteLine("\nStarting process, do the scan! To stop sending, close the console window");

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                // if true, you won't be able to scan
                Headless = false,
                ExecutablePath = "C:/Program Files (x86)/Google/Chrome/Application/chrome.exe",
            });

            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://web.whatsapp.com");

            //scan your code...
            Thread.Sleep(15000);

            try
            {
                while (true)
                {
                    foreach (var grupo in targets)
                    {
                        Console.WriteLine($"Sending message ({message}) to {grupo}.");
                        
                        // getting chat
                        var chat = await page.WaitForXPathAsync($"//span[@title='{grupo}']");
                        Thread.Sleep(1000);
                        await chat.ClickAsync();

                        //getting chat field
                        var field = await page.QuerySelectorAsync("._3uMse");
                        Thread.Sleep(1000);
                        await field.ClickAsync();

                        //typing 
                        await chat.TypeAsync(message);

                        //sending message
                        var button = await page.WaitForXPathAsync("//span[@data-icon='send']");
                        Thread.Sleep(1000);
                        await button.ClickAsync();

                        count++;

                        Console.WriteLine($"Messages sent: {count}");

                        Thread.Sleep(1000);
                     
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! Try again...", e.Message);
            }            
        }
    }
}
