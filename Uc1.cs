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

   
}
