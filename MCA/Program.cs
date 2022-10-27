using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MCA
{
    internal class Program
    {
        HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Program program = new Program();
            await program.GetProductItems();
        }

        private async Task GetProductItems()
        {
            string response = await client.GetStringAsync("https://interview-task-api.mca.dev/qr-scanner-codes/alpha-qr-gFpwhsQ8fkY1");

            List<Product> product = JsonConvert.DeserializeObject<List<Product>>(response);

            List<Product> domesticProduct = new List<Product>();
            List<Product> importedProduct = new List<Product>();

            double domCost = 0.00;
            double impCost = 0.00;
            int domCount = 0;
            int impCount = 0;

            foreach (var item in product)
            {
                if (item.domestic)
                {
                    domesticProduct.Add(item);
                    domCost += item.price;
                    domCount++;
                }
                else
                {
                    importedProduct.Add(item);
                    impCost += item.price;
                    impCount++;
                }
            }

            domesticProduct.Sort(delegate (Product x, Product y)
            {
                return x.name.CompareTo(y.name);
            });

            importedProduct.Sort(delegate (Product x, Product y)
            {
                return x.name.CompareTo(y.name);
            });

            Console.WriteLine(". Domestic");
            foreach (var item in domesticProduct)
            {
                Console.WriteLine("... "+ item.name);
                Console.WriteLine("    Price: ${0:N1}",item.price);
                if (item.description.Length > 10)
                {
                    Console.WriteLine("    " + item.description.Substring(0, 10) + "...");
                }
                if (item.weight == 0)
                {
                    Console.WriteLine("    Weight: N/A");
                }
                else
                {
                    Console.WriteLine("    Weight: " + item.weight + "g");
                }
            }

            Console.WriteLine(". Imported");
            foreach (var item in importedProduct)
            {
                Console.WriteLine("... " + item.name);
                Console.WriteLine("    Price: ${0:N1}", item.price);
                if (item.description.Length > 10)
                {
                    Console.WriteLine("    " + item.description.Substring(0, 10) + "...");
                }
                if (item.weight == 0)
                {
                    Console.WriteLine("    Weight: N/A");
                }
                else
                {
                    Console.WriteLine("    Weight: " + item.weight + "g");
                }
            }
            Console.WriteLine(String.Format("Domestic cost: ${0:N1}", domCost));
            Console.WriteLine(String.Format("Imported cost: ${0:N1}", impCost));
            Console.WriteLine("Domestic count: " + domCount);
            Console.WriteLine("Imported count: " + impCount);
        }
    }
    class Product
    {
        public string name { get; set; }
        public bool domestic { get; set; }
        public double price { get; set; }
        public int weight { get; set; }
        public string description { get; set; }
    }
}
