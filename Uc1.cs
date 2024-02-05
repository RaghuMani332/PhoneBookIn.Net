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
        public void addContact()
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

            
        }
        public void delete()
        {

            Console.WriteLine("enter the firstname to delete");
            String firstName = Console.ReadLine();
            Console.WriteLine("enter the lastname to delete");
            String lastname = Console.ReadLine();
            lastname=lastname.ToLower();
            firstName = firstName.ToLower();
            Person p = new Person();
            List<Contact> list = p.getContact();
            // showContact();
            // Contact foud= (Contact)from cont in list where cont.firstname.Equals(firstName) select cont;
            int count = 0;
            Contact rem = null;
            foreach (Contact cont in list)
            {
                if (cont.firstname.Equals(firstName) && cont.lastname.Equals(lastname))
                {
                    rem = cont;
                    count++;
                }

            }
            if (count == 0)
            {
                Console.WriteLine("in valid name try again");
                choice();
            }
            list.Remove(rem);

            p.update(list);
            Console.WriteLine("updated");
            foreach (Contact contact in p.getContact())
            {
                Console.WriteLine(contact);
            }
            choice();

        }
        public bool updateContact()
        {
            Console.WriteLine("enter first name");
            String firstName = Console.ReadLine();
            Console.WriteLine("enter the lastname");
            String lastname = Console.ReadLine();
            lastname = lastname.ToLower();
            firstName = firstName.ToLower();
            Person p = new Person();
            List<Contact> list = p.getContact();
            int count = 0;
            foreach (Contact cont in list)
            {
                if (cont.firstname.Equals(firstName))
                {
                    Console.WriteLine(cont);
                    count++;
                }

            }
            if (count == 0)
            {
                Console.WriteLine("no contact found in the given name and available contacts are....");
                showContact();
                Console.WriteLine("to continue update press y/n");
                if (Console.ReadLine().Equals("y"))
                    this.updateContact();
                return false;
            }
            Console.WriteLine("enter the option");




            Console.WriteLine("press \n 1 for firstname \n 2 for lastname \n 3 for email \n 4 for mobile number \n 5 for zipcode \n 6 for state");
            // ulong updatedcontact=ulong.Parse(Console.ReadLine());
            int option = int.Parse(Console.ReadLine());
            foreach (Contact cont in list)
            {
                if (cont.firstname.Equals(firstName)&&cont.lastname.Equals(lastname))
                {
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine("enter first name to update");
                            cont.firstname = Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("enter last name to update");
                            cont.lastname = Console.ReadLine();
                            break;
                        case 3:
                            Console.WriteLine("enter email to update");
                            cont.email = Console.ReadLine();
                            break;
                        case 4:
                            Console.WriteLine("enter mobileNumber to update");
                            cont.cno = ulong.Parse(Console.ReadLine());
                            break;
                        case 5:
                            Console.WriteLine("enter zip code to update");
                            cont.address.pin = int.Parse(Console.ReadLine());
                            break;
                        case 6:
                            Console.WriteLine("enter State to update");
                            cont.address.state = Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("invalid choice");
                            break;

                    }


                }
            }
            p.update(list);
            Console.WriteLine("updated successfully");
            choice();
            return true;
        }

        public void choice()
        {
            Console.WriteLine(" 0 for add contact \n 1 for delete contaaact \n 2 for update contact \n 3 for showall");
            String choice = Console.ReadLine();
            if (choice.Equals("0"))
                addContact();
            else if (choice.Equals("1"))
                delete();
            else if (choice.Equals("2"))
                updateContact();
            else if (choice.Equals("3"))
                showContact();
            else
                Environment.Exit(0);

        }
        public void showContact()
        {
            Person p = new Person();
            List<Contact> list = p.getContact();
            foreach (Contact cont in list)
            {
                Console.WriteLine(cont);
            }
            choice();
        }

    }

}
