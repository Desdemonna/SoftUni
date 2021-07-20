using System;
using System.Linq;
using Telephony.Models;
using Telephony.Exceptions;

namespace Telephony
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            string[] phoneNumbers = Console.ReadLine().Split();
            string[] urls = Console.ReadLine().Split();

            Smartphone smartphone = new Smartphone();
            StationaryPhone stationaryPhone = new StationaryPhone();

            for (int i = 0; i < phoneNumbers.Length; i++)
            {
                string currPhoneNumber = phoneNumbers[i];
                try
                {
                    if (currPhoneNumber.Length == 7)
                    {
                        Console.WriteLine(stationaryPhone.Call(currPhoneNumber));
                    }
                    else
                    {
                        Console.WriteLine(smartphone.Call(currPhoneNumber));
                    }
                }
                catch (InvalidPhoneNumber ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }

            for (int i = 0; i < urls.Length; i++)
            {
                string url = urls[i];
                try
                {
                    Console.WriteLine(smartphone.Browse(url));
                }
                catch (InvalidURL ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }
        }
    }
}
