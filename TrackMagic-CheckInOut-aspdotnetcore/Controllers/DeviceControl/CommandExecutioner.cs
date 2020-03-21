using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace TrackMagic_CheckInOut_aspdotnetcore
{
    public class CommandExecutioner
    {
        //public  CommandExecutioner()
        //{
        //}

        public static void RunTest()
        {
            // lets say we want to run this command:    
            //  t=$(echo 'this is a test'); echo "$t" | grep -o 'is a'
            //  var output = ExecuteBashCommand("t=$(echo 'this is a test'); echo \"$t\" | grep -o 'is a'");
            //var output = ExecuteBashCommand("t=$(echo '");
            // output the result
            // Console.WriteLine(output);

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("", "-c \"fswebcam test-image - dh1.jpg'\"");
            }
            else
            {
                Process.Start("/bin/bash", "-c \"echo 'Unsupported platform - looking for linux'\"");  //works
            }

        }

        public static string ExecuteBashCommand(string command)
        {
            // according to: https://stackoverflow.com/a/15262019/637142
            // thans to this we will pass everything as one command
            command = command.Replace("\"", "\"\"");

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            proc.WaitForExit();

            return proc.StandardOutput.ReadToEnd();
        }


    }
}
