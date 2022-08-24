using System;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string keePassDBFile = @"C:\tools\Tyler.HR.DataAutomation.V2\Tyler.Data.DataAutomationDatabase.kdbx";
            string keePassKeyFile = @"C:\tools\Tyler.HR.DataAutomation.V2\Tyler.Data.DataAutomationDatabase.keyx";
            Utility.KeePass.FileReader kpReader = new Utility.KeePass.FileReader(keePassDBFile, keePassKeyFile);
            string value = kpReader.getDataByGroup("report_1", "RAAS Reports", "Title", "UserName");
            Console.WriteLine("Utility.KeePass.FileReader.getData: " + value);


        }
    }
}
