using ExcelProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelProject
{
    public class ConsoleStart
    {
        static void Main(string[] args)
        {
            var addin = new AddinStarter();

            Console.WriteLine("Started");

            string input = null;

            do
            {
                input = Console.ReadLine();

                if(input == "show")
                {
                    Console.WriteLine("recognized command: show");
                    addin.Show<AddinView>();
                } else if(input == "show2")
                {
                    Console.WriteLine("recognized command: show2");
                    addin.Show<HelloWorldView>();
                }


            } while (!String.IsNullOrWhiteSpace(input));

        }
    }
}
