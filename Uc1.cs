using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactBook
{
     class Person
    {

        private static List<Contact> contact=new List<Contact>();
        

        public void update(List<Contact> contact)
        {
            Person.contact = contact;
        }
        public void setContact(Contact contact)
        {
            Person.contact.Add(contact);   
        }
        public List<Contact> getContact()
        {
           return contact;
        }
    }
    class Contact
    {
        public String firstname { get; set; }
        public String lastname { get; set; }
        public ulong cno { get; set; }
        public String email { get; set; }

        public Address address { get; set; }

        public override string ToString()
        {
            return String.Format("FIRSTNAME {0} LASTNAME {1} CNO {2} EMAIL {3} ADDRESS {4}",firstname,lastname,cno,email,address);
        }
    }
    class Address
    {
       public int pin { get; set; }
       public String state { get; set; }

        public override string ToString()
        {
            return String.Format("PinCode is {0} State is {1}",pin,state);
        }
    }
    class Methods
    {
        public bool addContact()
        {
            Console.WriteLine("Enter name");
            String firstname = Console.ReadLine();
            Console.WriteLine("enter last name");
            String lastname = Console.ReadLine();
            Console.WriteLine("enter email");
            String email = Console.ReadLine();
            Console.WriteLine("enter contactno");
            ulong phone = ulong.Parse(Console.ReadLine());
            Console.WriteLine("enter State");
            String state = Console.ReadLine();
            Console.WriteLine("enter zipcode");
            int pin = int.Parse(Console.ReadLine());
            Console.WriteLine();
            List<bool> regex = new List<bool>();
            // regex.Add(true);
            regex.Add(Regex.IsMatch(email, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"));
            regex.Add(Regex.IsMatch(phone + "", @"^\d{10}$"));
            // regex.Add(true);
            regex.Add(Regex.IsMatch("" + pin, @"^\d{6}$"));

            bool v = regex.Any(n => n == false);
            if (v)
            {
                Console.WriteLine("enter details in a valid format");
                choice();
                return false;
            }
            Address a = new Address();
            a.pin = pin;
            a.state = state;
            Contact c = new Contact();
            c.email = email;
            c.cno = phone;
            c.firstname = firstname;
            c.lastname = lastname;
            c.address = a;
            Person p = new Person();
            p.setContact(c);

            foreach (Contact contact in p.getContact())
            {
                Console.WriteLine(contact);
            }
            choice();

            return true;
        }
        public void choice()
        {
            Console.WriteLine("to add more contact press 0 \n 1 for delete contaaact \n 2 for update contact \n 3 for showall");
            String choice = Console.ReadLine();
            if (choice.Equals("0"))
                addContact();
           

        }

    }

}
