using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.KeePass
{
    public class FileReader
    {
        private string KeepassDBFilePath { get; set; }
        private string KeepassKeyFilePath { get; set; }
        private string KeepassMasterPassword { get; set; }

        public FileReader() : base()
        {
            KeepassDBFilePath = string.Empty;
            KeepassKeyFilePath = string.Empty;
            KeepassMasterPassword = string.Empty;
        }
        public FileReader(string fileLocation, string keyLocation) : base()
        {
            KeepassDBFilePath = base64Encode(fileLocation);
            KeepassKeyFilePath = base64Encode(keyLocation);
            KeepassMasterPassword = string.Empty;
        }
        public FileReader(string fileLocation, string keyLocation, string masterPassword) : base()
        {
            KeepassDBFilePath = base64Encode(fileLocation);

            if (!(string.IsNullOrEmpty(keyLocation)))
            {
                KeepassKeyFilePath = base64Encode(keyLocation);
            }

            KeepassMasterPassword = base64Encode(masterPassword);
        }
        public void setDBFilePath(string value)
        {
            KeepassDBFilePath = base64Encode(value);
        }
        public void setKeyFilePath(string value)
        {
            KeepassKeyFilePath = base64Encode(value);
        }
        public void setMasterPassword(string value)
        {
            KeepassMasterPassword = base64Encode(value);
        }
        public string getData(string value, string kpColumn2Search = "Title", string kpColumn2Return = "Password", string v = null)
        {
            string returnValue = string.Empty;
            var ioconninfo = new KeePassLib.Serialization.IOConnectionInfo();
            if (!(string.IsNullOrEmpty(KeepassDBFilePath)))
            {

                ioconninfo.Path = base64Decode(KeepassDBFilePath);
                KeePassLib.Keys.CompositeKey compkey = new KeePassLib.Keys.CompositeKey();
                if (string.IsNullOrEmpty(KeepassKeyFilePath) && string.IsNullOrEmpty(KeepassMasterPassword))
                {
                    throw new Exception("A Key file or Master Password has not been set!");
                }
                else
                {
                    if (!(string.IsNullOrEmpty(KeepassKeyFilePath))) { compkey.AddUserKey(new KeePassLib.Keys.KcpKeyFile(base64Decode(KeepassKeyFilePath))); }
                    if (!(string.IsNullOrEmpty(KeepassMasterPassword))) { compkey.AddUserKey(new KeePassLib.Keys.KcpPassword(base64Decode(KeepassMasterPassword))); }
                    var db = new KeePassLib.PwDatabase();

                    try
                    {
                        db.Open(ioconninfo, compkey, null);

                        KeePassLib.Collections.PwObjectList<KeePassLib.PwEntry> entries = db.RootGroup.GetEntries(true);
                        //var data =  from entry in db.rootgroup.getentries(true) where entry.strings.readsafe("title") == "tyler-u-client-id" select entry;

                        KeePassLib.PwEntry pw = entries.FirstOrDefault(i => i.Strings.ReadSafe(kpColumn2Search) == value);

                        if (pw != null)
                            returnValue = pw.Strings.ReadSafe(kpColumn2Return);
                        else
                            returnValue = string.Empty;

                        pw = null;

                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (db.IsOpen)
                        {
                            db.Close();
                            db = null;
                        }
                    }
                }
            }
            else
                throw new Exception("Keepass DB Path has not been set!");

            return returnValue;
        }

        public kpEntry getData(string value, string kpColumn2Search = "Title")
        {
            kpEntry returnValue = new kpEntry();
            var ioconninfo = new KeePassLib.Serialization.IOConnectionInfo();
            if (!(string.IsNullOrEmpty(KeepassDBFilePath)))
            {

                ioconninfo.Path = base64Decode(KeepassDBFilePath);
                KeePassLib.Keys.CompositeKey compkey = new KeePassLib.Keys.CompositeKey();
                if (string.IsNullOrEmpty(KeepassKeyFilePath) && string.IsNullOrEmpty(KeepassMasterPassword))
                {
                    throw new Exception("A Key file or Master Password has not been set!");
                }
                else
                {
                    if (!(string.IsNullOrEmpty(KeepassKeyFilePath))) { compkey.AddUserKey(new KeePassLib.Keys.KcpKeyFile(base64Decode(KeepassKeyFilePath))); }
                    if (!(string.IsNullOrEmpty(KeepassMasterPassword))) { compkey.AddUserKey(new KeePassLib.Keys.KcpPassword(base64Decode(KeepassMasterPassword))); }
                    var db = new KeePassLib.PwDatabase();

                    try
                    {
                        db.Open(ioconninfo, compkey, null);

                        KeePassLib.Collections.PwObjectList<KeePassLib.PwEntry> entries = db.RootGroup.GetEntries(true);
                        //var data =  from entry in db.rootgroup.getentries(true) where entry.strings.readsafe("title") == "tyler-u-client-id" select entry;

                        KeePassLib.PwEntry pw = entries.FirstOrDefault(i => i.Strings.ReadSafe(kpColumn2Search) == value);

                        if (pw != null)
                            returnValue = new kpEntry()
                            {
                                Title = pw.Strings.ReadSafe("Title"),
                                UserName = pw.Strings.ReadSafe("User Name"),
                                Password = pw.Strings.ReadSafe("Password"),
                                URL = pw.Strings.ReadSafe("URL"),
                                Notes = pw.Strings.ReadSafe("Notes"),
                                Group = new kpGroup(pw.ParentGroup.Name, pw.ParentGroup.Notes)
                            };
                        else
                            returnValue = new kpEntry();

                        pw = null;

                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (db.IsOpen)
                        {
                            db.Close();
                            db = null;
                        }
                    }
                }
            }
            else
                throw new Exception("Keepass DB Path has not been set!");

            return returnValue;
        }
        public string getDataByGroup(string value, string kpGroup, string kpColumn2Search = "Title", string kpColumn2Return = "Password")
        {
            KeePassLib.PwGroup group = getGroup(kpGroup);
            if (group != null)
                return getDataByGroup(value, group, kpColumn2Search, kpColumn2Return);
            else
                throw new Exception("Group not found!");
        }
        public string getDataByGroup(string value, KeePassLib.PwGroup kpGroup, string kpColumn2Search = "Title", string kpColumn2Return = "Password")
        {
            string returnValue = string.Empty;
            var ioconninfo = new KeePassLib.Serialization.IOConnectionInfo();
            if (!(string.IsNullOrEmpty(kpGroup.ToString())))
            {
                try
                {
                    KeePassLib.PwEntry pw = kpGroup.Entries.FirstOrDefault(i => i.Strings.ReadSafe(kpColumn2Search) == value);

                    if (pw != null)
                        returnValue = pw.Strings.ReadSafe(kpColumn2Return);
                    else
                        returnValue = string.Empty;

                    pw = null;

                }
                catch
                {
                    throw;
                }

            }

            return returnValue;
        }
        public List<kpEntry> getDataByGroup(string kpGroup)
        {
            KeePassLib.PwGroup group = getGroup(kpGroup);
            if (group != null)
                return getDataByGroup(group);
            else
                throw new Exception("Group not found!");
        }
        public List<kpEntry> getDataByGroup(KeePassLib.PwGroup kpGroup)
        {
            List<kpEntry> returnData = new List<kpEntry>();
            var ioconninfo = new KeePassLib.Serialization.IOConnectionInfo();
            if (!(string.IsNullOrEmpty(kpGroup.ToString())))
            {
                try
                {
                    foreach (KeePassLib.PwEntry pw in kpGroup.Entries)
                    {
                        returnData.Add(new kpEntry()
                        {
                            Title = pw.Strings.ReadSafe("Title"),
                            UserName = pw.Strings.ReadSafe("User Name"),
                            Password = pw.Strings.ReadSafe("Password"),
                            URL = pw.Strings.ReadSafe("URL"),
                            Notes = pw.Strings.ReadSafe("Notes"),
                            Group = new kpGroup(kpGroup.Name, kpGroup.Notes)
                        });

                    }
                }
                catch
                {
                    throw;
                }

            }

            return returnData;
        }
        public KeePassLib.PwGroup getGroup(string name)
        {
            KeePassLib.PwGroup group = new KeePassLib.PwGroup();

            var ioconninfo = new KeePassLib.Serialization.IOConnectionInfo();
            if (!(string.IsNullOrEmpty(KeepassDBFilePath)))
            {

                ioconninfo.Path = base64Decode(KeepassDBFilePath);
                KeePassLib.Keys.CompositeKey compkey = new KeePassLib.Keys.CompositeKey();
                if (string.IsNullOrEmpty(KeepassKeyFilePath) && string.IsNullOrEmpty(KeepassMasterPassword))
                {
                    throw new Exception("A Key file or Master Password has not been set!");
                }
                else
                {
                    if (!(string.IsNullOrEmpty(KeepassKeyFilePath))) { compkey.AddUserKey(new KeePassLib.Keys.KcpKeyFile(base64Decode(KeepassKeyFilePath))); }
                    if (!(string.IsNullOrEmpty(KeepassMasterPassword))) { compkey.AddUserKey(new KeePassLib.Keys.KcpPassword(base64Decode(KeepassMasterPassword))); }
                    var db = new KeePassLib.PwDatabase();

                    try
                    {
                        db.Open(ioconninfo, compkey, null);
                        KeePassLib.Collections.PwObjectList<KeePassLib.PwGroup> groups = db.RootGroup.GetGroups(true);

                        group = groups.First(i => i.Name == name);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (db.IsOpen)
                        {
                            db.Close();
                            db = null;
                        }
                    }
                }
            }
            else
                throw new Exception("Keepass DB Path has not been set!");

            return group;
        }
        private static string base64Encode(string plainText)
        {
            if (!(string.IsNullOrEmpty(plainText)))
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            else
                throw new Exception("value to encode is empty or null!");
        }
        private static string base64Decode(string base64EncodedData)
        {
            if (!(string.IsNullOrEmpty(base64EncodedData)))
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            else
                throw new Exception("value to decode is empty or null!");
        }
    }
}
