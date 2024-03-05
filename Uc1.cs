using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContactBook
{
    public class ContactNameAlreadyExistsException : Exception
    {
        public ContactNameAlreadyExistsException() { }

        public ContactNameAlreadyExistsException(string message)
            : base(message) { }

        public ContactNameAlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException() { }

        public EmailAlreadyExistsException(string message)
            : base(message) { }

        public EmailAlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class MobileNumberAlreadyExistsException : Exception
    {
        public MobileNumberAlreadyExistsException() { }

        public MobileNumberAlreadyExistsException(string message)
            : base(message) { }

        public MobileNumberAlreadyExistsException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class InvalidInputFormatException : Exception
    {
        public InvalidInputFormatException() { }

        public InvalidInputFormatException(string message)
            : base(message) { }

        public InvalidInputFormatException(string message, Exception inner)
            : base(message, inner) { }
    }

    class Person
    {
        private static List<Contact> contacts = new List<Contact>();

        static Person()
        {
            ConvertFromFileToVariable();
        }

        public static void ConvertFromFileToVariable()
        {
            String path = "D:\\BridgeLabz\\CSHARP\\ContactBook\\ContactBook\\contacts.csv";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            List<String> lines = File.ReadAllLines(path).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                Contact c = new Contact();
                String[] fields = lines[i].Split(",");

                c.address = new Address();
                c.firstname = fields[0];
                c.lastname = fields[1];
                c.cno = ulong.Parse(fields[2]);
                c.email = fields[3];
                c.address.state = fields[4];
                c.address.pin = int.Parse(fields[5]);
                contacts.Add(c);
            }
        }

        public void Update(List<Contact> contact)
        {
            contacts = contact;
        }

        public void SetContact(Contact contact)
        {
            contacts.Add(contact);
        }

        public List<Contact> GetContact()
        {
            return contacts;
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
            return $"FIRSTNAME {firstname} LASTNAME {lastname} CNO {cno} EMAIL {email} ADDRESS {address}";
        }
    }

    class Address
    {
        public int pin { get; set; }
        public String state { get; set; }

        public override string ToString()
        {
            return $"PinCode is {pin} State is {state}";
        }
    }

    class Methods
    {
        String path = "D:\\BridgeLabz\\CSHARP\\ContactBook\\ContactBook\\contacts.csv";

        public void AddContact()
        {
            Console.WriteLine("Enter name");
            string firstname = Console.ReadLine();
            Console.WriteLine("Enter last name");
            string lastname = Console.ReadLine();
            Console.WriteLine("Enter email");
            string email = Console.ReadLine();
            Console.WriteLine("Enter contact no");
            ulong phone;
            if (!ulong.TryParse(Console.ReadLine(), out phone))
            {
                try
                {
                    throw new InvalidInputFormatException("Invalid phone number format. Please enter a valid 10-digit phone number.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Enter State");
            string state = Console.ReadLine();
            Console.WriteLine("Enter zipcode");
            int pin;
            if (!int.TryParse(Console.ReadLine(), out pin))
            {
                try
                {
                    throw new InvalidInputFormatException("Invalid pin code format. Please enter a valid 6-digit pin code.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine();

            // Check if the contact already exists
            Person p = new Person();
            List<Contact> contacts = p.GetContact();
            bool contactNameExists = contacts.Any(c => c.firstname.Equals(firstname, StringComparison.OrdinalIgnoreCase) &&
                                                       c.lastname.Equals(lastname, StringComparison.OrdinalIgnoreCase));
            bool emailExists = contacts.Any(c => c.email.Equals(email, StringComparison.OrdinalIgnoreCase));
            bool phoneExists = contacts.Any(c => c.cno == phone);

            if (contactNameExists || emailExists || phoneExists)
            {
                Console.WriteLine("Contact already exists. Not adding to the list.");
                Choice(); // You might want to return here instead if you don't want to continue after this.
                return;
            }

            // Validate email, phone, and pin
            bool validEmail = Regex.IsMatch(email, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");
            bool validPhone = Regex.IsMatch(phone.ToString(), @"^\d{10}$");
            bool validPin = Regex.IsMatch(pin.ToString(), @"^\d{6}$");

            if (!validEmail || !validPhone || !validPin)
            {
                throw new InvalidInputFormatException("Invalid input format.");
            }

            Address a = new Address { pin = pin, state = state };
            Contact c = new Contact { firstname = firstname, lastname = lastname, email = email, cno = phone, address = a };
            p.SetContact(c);

            foreach (Contact contact in p.GetContact())
            {
                Console.WriteLine(contact);
            }
            AddInFile(p);

            Choice();
        }

        public void UpdateInFile(Person p)
        {
            List<String> lines = File.ReadAllLines(path).ToList();
            List<Contact> contacts = p.GetContact();

            using (StreamWriter sw = new StreamWriter(path, append: false))
            {
                // Write header line
                sw.WriteLine("firstName,LastName,c.no,Email,state,pin");

                foreach (var line in lines.Skip(1)) // Skip header line
                {
                    var fields = line.Split(',');
                    var existingContact = contacts.FirstOrDefault(c =>
                        c.firstname == fields[0] &&
                        c.lastname == fields[1] &&
                        c.cno.ToString() == fields[2] &&
                        c.email == fields[3] &&
                        c.address.state == fields[4] &&
                        c.address.pin.ToString() == fields[5]);

                    // Write the updated line if the contact is found in the updated list
                    if (existingContact != null)
                    {
                        String toUpdate = String.Format("{0},{1},{2},{3},{4},{5}",
                                                        existingContact.firstname,
                                                        existingContact.lastname,
                                                        existingContact.cno,
                                                        existingContact.email,
                                                        existingContact.address.state,
                                                        existingContact.address.pin);
                        sw.WriteLine(toUpdate);
                    }
                }
            }
        }

        public void AddInFile(Person p)
        {
            Console.WriteLine("file method invoked");
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            List<String> oldcont = File.ReadAllLines(path).ToList();
            StreamWriter sw = new StreamWriter(path, append: true);
            sw.AutoFlush = true;
            if (oldcont.Count == 0)
            {
                sw.WriteLine("firstName,LastName,c.no,Email,state,pin");
            }
            Contact ContToAdd = p.GetContact()[p.GetContact().Count() - 1];
            String toAdd = String.Format("{0},{1},{2},{3},{4},{5}", ContToAdd.firstname, ContToAdd.lastname, ContToAdd.cno, ContToAdd.email, ContToAdd.address.state, ContToAdd.address.pin);
            sw.WriteLine(toAdd);
            sw.Dispose();
            sw.Close();
        }

        public void Delete()
        {
            Console.WriteLine("Enter the first name to delete");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter the last name to delete");
            string lastName = Console.ReadLine();
            lastName = lastName.ToLower();
            firstName = firstName.ToLower();

            Person p = new Person();
            List<Contact> list = p.GetContact();

            int count = 0;
            Contact rem = null;

            foreach (Contact cont in list)
            {
                if (cont.firstname.Equals(firstName, StringComparison.OrdinalIgnoreCase) && cont.lastname.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    rem = cont;
                    count++;
                }
            }

            if (count == 0)
            {
                Console.WriteLine("Invalid name. Please try again.");
                Choice();
            }

            try
            {
                list.Remove(rem);
                p.Update(list);
                Console.WriteLine("Updated");

                foreach (Contact contact in p.GetContact())
                {
                    Console.WriteLine(contact);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting contact: " + ex.Message);
            }
            UpdateInFile(p);
            Choice();
        }

        public bool UpdateContact()
        {
            Console.WriteLine("Enter first name");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter the last name");
            string lastName = Console.ReadLine();
            lastName = lastName.ToLower();
            firstName = firstName.ToLower();

            Person p = new Person();
            List<Contact> list = p.GetContact();
            int count = 0;

            foreach (Contact cont in list)
            {
                if (cont.firstname.Equals(firstName, StringComparison.OrdinalIgnoreCase) && cont.lastname.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(cont);
                    count++;
                }
            }

            if (count == 0)
            {
                Console.WriteLine("No contact found with the given name.");
                ShowContact();
                Console.WriteLine("Do you want to continue updating? (y/n)");
                if (Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase))
                    return UpdateContact();
                return false;
            }

            Console.WriteLine("Enter the option to update:");
            Console.WriteLine("Press\n1 for First Name\n2 for Last Name\n3 for Email\n4 for Mobile Number\n5 for Zip Code\n6 for State");
            int option;

            if (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 6)
            {
                Console.WriteLine("Invalid option.");
                Choice();
                return false;
            }

            try
            {
                foreach (Contact cont in list)
                {
                    if (cont.firstname.Equals(firstName, StringComparison.OrdinalIgnoreCase) && cont.lastname.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                    {
                        switch (option)
                        {
                            case 1:
                                Console.WriteLine("Enter first name to update");
                                string newFirstName = Console.ReadLine();
                                if (!list.Any(c => c.firstname.Equals(newFirstName, StringComparison.OrdinalIgnoreCase)))
                                    cont.firstname = newFirstName;
                                else
                                    Console.WriteLine("Contact with this first name already exists.");
                                break;
                            case 2:
                                Console.WriteLine("Enter last name to update");
                                string newLastName = Console.ReadLine();
                                if (!list.Any(c => c.lastname.Equals(newLastName, StringComparison.OrdinalIgnoreCase)))
                                    cont.lastname = newLastName;
                                else
                                    Console.WriteLine("Contact with this last name already exists.");
                                break;
                            case 3:
                                Console.WriteLine("Enter email to update");
                                string newEmail = Console.ReadLine();
                                if (Regex.IsMatch(newEmail, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$") && !list.Any(c => c.email.Equals(newEmail, StringComparison.OrdinalIgnoreCase)))
                                    cont.email = newEmail;
                                else
                                    Console.WriteLine("Invalid email or contact with this email already exists.");
                                break;
                            case 4:
                                Console.WriteLine("Enter mobile number to update");
                                ulong newPhone;
                                if (ulong.TryParse(Console.ReadLine(), out newPhone) && Regex.IsMatch(newPhone.ToString(), @"^\d{10}$") && !list.Any(c => c.cno == newPhone))
                                    cont.cno = newPhone;
                                else
                                    Console.WriteLine("Invalid mobile number or contact with this number already exists.");
                                break;
                            case 5:
                                Console.WriteLine("Enter zip code to update");
                                int newPin;
                                if (int.TryParse(Console.ReadLine(), out newPin) && Regex.IsMatch(newPin.ToString(), @"^\d{6}$"))
                                    cont.address.pin = newPin;
                                else
                                    Console.WriteLine("Invalid zip code.");
                                break;
                            case 6:
                                Console.WriteLine("Enter state to update");
                                cont.address.state = Console.ReadLine();
                                break;
                            default:
                                Console.WriteLine("Invalid choice");
                                break;
                        }
                    }
                }

                p.Update(list);
                UpdateInFile(p);

                Choice();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating contact: " + ex.Message);
                Choice();
                return false;
            }
        }

        public void Choice()
        {
            Console.WriteLine("0 for Add Contact\n1 for Delete Contact\n2 for Update Contact\n3 for Show All Contacts\n4 for GetByPinOrState\n5 for Get Count Based on PinOrState\n6 for Delete All\n7 for Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    AddContact();
                    break;
                case "1":
                    Delete();
                    break;
                case "2":
                    UpdateContact();
                    break;
                case "3":
                    ShowContact();
                    break;
                case "4":
                    GetByPinOrState();
                    Choice();
                    break;
                case "5":
                    Console.WriteLine(GetByPinOrState());
                    Choice();
                    break;
                case "6":
                    Console.WriteLine("Deleting all contacts...");
                    DeleteAll();
                    Choice();
                    break;
                case "7":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    Choice();
                    break;
            }
        }

        public int GetByPinOrState()
        {
            Console.WriteLine("Press 1 for search by state \nPress 2 for search by ZipCode");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Enter state ");
                    string stateToSearch = Console.ReadLine();
                    List<Contact> contacts = new Person().GetContact();
                    var v = contacts.Where(state => state.address.state.Equals(stateToSearch, StringComparison.OrdinalIgnoreCase));
                    foreach (var i in v)
                    {
                        Console.WriteLine(i);
                    }
                    return v.Count();
                case "2":
                    Console.WriteLine("Enter Zipcode");
                    int pinToSearch;
                    if (int.TryParse(Console.ReadLine(), out pinToSearch))
                    {
                        List<Contact> contacts1 = new Person().GetContact();
                        var v1 = contacts1.Where(pin => pin.address.pin == pinToSearch);
                        foreach (var i in v1)
                        {
                            Console.WriteLine(i);
                        }
                        return v1.Count();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Zipcode format.");
                        return 0;
                    }
                default:
                    Console.WriteLine("Invalid choice");
                    return 0;
            }
        }

        public void ShowContact()
        {
            Person p = new Person();
            List<Contact> list = p.GetContact();

            foreach (Contact cont in list)
            {
                Console.WriteLine(cont);
            }
            Choice();
        }

        public void DeleteAll()
        {
            File.WriteAllText(path, string.Empty);
            Console.WriteLine("All contacts deleted successfully.");
        }
    }
}

    

