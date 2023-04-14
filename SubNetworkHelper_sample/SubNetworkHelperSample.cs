

using System.Net;
using SubNetworkHelper;

namespace SubNetworkHelper_sample;


public class SubNetworkHelperSample
{
    public static void main()
    {
        IPAddress ip = IPAddress.Parse("192.168.0.1");
        IPAddress mask = IPAddress.Parse("255.255.252.0");
        SubNetworkHelperInt SubNetwork = new SubNetworkHelperInt(ip, mask);
        
        Console.WriteLine(SubNetwork.IP.ToString());
        Console.WriteLine(SubNetwork.Mask.ToString());
        Console.WriteLine(SubNetwork.Broadcast.ToString());
        Console.WriteLine(SubNetwork.Network.ToString());
        Console.WriteLine(SubNetwork.Wildcard.ToString());
        Console.WriteLine(SubNetwork.FirstIP.ToString());
        Console.WriteLine(SubNetwork.EndIP.ToString());
        Console.WriteLine();
        foreach (var (prop, value) in SubNetwork.SubNetworkPropierties)
        {
            Console.WriteLine(prop + " = " + value.ToString());
        }

        List<IPAddress> subNetIpAddresses = SubNetwork.GetRangeIpAddresses();
        
        Console.WriteLine();
        Console.WriteLine("IP Addresses in subnetwork: " + String.Join(", ", subNetIpAddresses));

    }
}