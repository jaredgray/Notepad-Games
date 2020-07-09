using System.Linq;
using System.Net.NetworkInformation;

namespace NPPGames.Core.Infrastructure
{
    public static class MachineInfo
    {

        /*
        if one were to try and port this to the web or some other deployment that didn't have access to a unique id
        this might pose a challenge wherein either finding a clever way to implement this method or implementing some
        kind of login for the user

        If porting to the web, this stackoverflow answer is a good start. (if it doesn't exist some day, a google or
        whatever browser of the day search surely will turn up some options)
        https://stackoverflow.com/a/41575609/2933290
         
         */

        public static string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                        .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        .Select(nic => nic.GetPhysicalAddress().ToString())
                        .FirstOrDefault();
        }
    }
}
