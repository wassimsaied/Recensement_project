using Newtonsoft.Json;
using Recensement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Recensement
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Address> oldAddresses = new List<Address>();
            List<Address> newAddresses = new List<Address>();
            List<Address> resultAddresses = new List<Address>();
            string oldAddressesFilepath = @"..\..\Test_case\ancien_recensement.json";
            string newAddressesFilepath = @"..\..\Test_case\nouveau_recensement.json";



            // deserialize JSON  from a file
            try
            {
                using (StreamReader file = File.OpenText(oldAddressesFilepath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    oldAddresses = (List<Address>)serializer.Deserialize(file, typeof(List<Address>));
                }
                using (StreamReader file = File.OpenText(newAddressesFilepath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    newAddresses = (List<Address>)serializer.Deserialize(file, typeof(List<Address>));
                }

            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found");
                Console.ReadLine();
                return;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("file not found");
                Console.ReadLine();
                return;
            }
            //checking for old Address duplication
            bool oldAddressesIsDuplicated = oldAddresses.GroupBy(a => new { a.City, a.Street, a.Number }).Any(c => c.Count() > 1);
            if (oldAddressesIsDuplicated)
            {
                Console.WriteLine("l'ancien recensement contient des adresse dupliquées ");
                Console.ReadLine();
                return;
            }
            //checking for new Address duplication
            bool newwAddressesIsDuplicated = newAddresses.GroupBy(a => new { a.City, a.Street, a.Number }).Any(c => c.Count() > 1);
            if (newwAddressesIsDuplicated)
            {
                Console.WriteLine("le nouveau recensement contient des adresse dupliquées ");
                Console.ReadLine();
                return;
            }


            foreach (Address address in oldAddresses)
            {
                //checking for same Address existance
                Address currentAddress = newAddresses.Where(c => (c.PostalCode == address.PostalCode) && (c.City == address.City) && (c.Number == address.Number)).FirstOrDefault();

                if (currentAddress == null)
                {
                    address.Status = "adresse supprimée";
                }
                else if (address.Persons.All(currentAddress.Persons.Contains) && address.Persons.Count == currentAddress.Persons.Count)
                {

                    address.Status = "pas de changement";
                    newAddresses.Remove(currentAddress);

                }
                else
                {
                    address.Status = "changement dans la liste des occupants";
                    newAddresses.Remove(currentAddress);
                }

            }

            newAddresses.ForEach(a => a.Status = "nouvelle adresse");

            resultAddresses.AddRange(oldAddresses);
            resultAddresses.AddRange(newAddresses);

            // display result 
            Console.WriteLine("[");
            foreach (Address item in resultAddresses)
            {
                Console.WriteLine(item.ToString());

            }
            Console.WriteLine("\r\n]");
            Console.ReadLine();



        }
    }
}
