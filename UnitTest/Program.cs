using System;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string keePassDBFile = "C:\\tools\\Helper API KeePass DB\\HelperAPIInfo.kdbx";
            string keePassKeyFile = "C:\\tools\\Helper API KeePass DB\\HelperAPIInfo.keyx";
            Utility.KeePass.FileReader kpReader = new Utility.KeePass.FileReader(keePassDBFile, keePassKeyFile);

            Console.WriteLine("Utility.KeePass.FileReader.getData: " + kpReader.getData("ff058083-a8fa-4178-9ef5-128bc2d5166d", "Password"));
            Console.WriteLine("Utility.KeePass.FileReader.getDataByGroup: " + kpReader.getDataByGroup("ff058083-a8fa-4178-9ef5-128bc2d5166d", "Access Keys", "Password"));

        }
    }
}
