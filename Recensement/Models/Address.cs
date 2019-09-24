using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recensement.Models
{
    public class Address
    {
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Status { get; set; }

        public List<decimal> Persons { get; set; }

        public override string ToString()
        {
            string result = "\r\n\t{\r\n\t\t status: " + Status + " ,\r\n\t\t PostalCode: " + PostalCode + " ,\r\n\t\t City: " + City + " ,\r\n\t\t Street: " + Street + " ,\r\n\t\t Number: " + Number + " ,\r\n\t\t Persons:\r\n\t\t[\r\n\t\t ";
            string personResult = "\n\t\t";
            foreach (decimal person in Persons)
            {
                personResult += person + ",\n\t\t";
            }

            return result + personResult + "\r\n\t\t]\r\n\t}\r\n]";

        }



    }
}
