using System.Collections;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SubNetworkHelper;



public class SubNetworkHelperInt
{
    public IPAddress Mask { get; private set; }
    public IPAddress IP { get; private set; }
    public IPAddress Wildcard { get; private set; }
    public IPAddress Network { get; private set; }
    public IPAddress Broadcast { get; private set; }
    
    public IPAddress FirstIP { get; private set; }
    public IPAddress EndIP { get; private set; }

    public Dictionary<string, IPAddress> SubNetworkPropierties = new Dictionary<string, IPAddress>(){};

    public SubNetworkHelperInt(IPAddress ip, IPAddress mask)
    {
        
        Mask = mask;
        IP = ip;
        
        byte[] bmask = mask.GetAddressBytes();
        byte[] bip = ip.GetAddressBytes();
        uint uintmask = BitConverter.ToUInt32(bmask);
        uint uintip = BitConverter.ToUInt32(bip);
        
        uint uintwildcard = ~uintmask;
        byte[] bwildcard = BitConverter.GetBytes(uintwildcard);
        Wildcard = new IPAddress(bwildcard);

        uint uintnetwork = uintip & uintmask;
        byte[] bnetwork = BitConverter.GetBytes(uintnetwork);
        Network = new IPAddress(bnetwork);

        uint uintbroadcast = uintnetwork | uintwildcard;
        byte[] bbroadcast = BitConverter.GetBytes(uintbroadcast);
        Broadcast = new IPAddress(bbroadcast);

        byte[] bfirstip = bnetwork;
        bfirstip[^1] = (byte)(bfirstip[^1] + 1);
        FirstIP = new IPAddress(bfirstip);

        byte[] bendip = bbroadcast;
        bendip[^1] = (byte)(bendip[^1] - 1);
        EndIP = new IPAddress(bendip);

        SubNetworkPropierties["IP"] = IP;
        SubNetworkPropierties["MASK"] = Mask;
        SubNetworkPropierties["Wildcard"] = Wildcard;
        SubNetworkPropierties["Network"] = Network;
        SubNetworkPropierties["Broadcast"] = Broadcast;
        SubNetworkPropierties["FirstIP"] = FirstIP;
        SubNetworkPropierties["EndIP"] = EndIP;

    }

    public List<IPAddress> GetRangeIpAddresses()
    {

        List<IPAddress> ipAddresses = new List<IPAddress>();
        byte[] bnextip = FirstIP.GetAddressBytes();
        byte[] bendip = EndIP.GetAddressBytes();
        int octet = 1;
        while (octet != bendip.Length)
        {
            if (bnextip[^octet] == bendip[^octet])
            {
                octet++;
                continue;
            }
            bnextip[^octet]++;
            IPAddress nextip = new IPAddress(bnextip);
            ipAddresses.Add(nextip);
        }
        return ipAddresses;
    }

    public List<IPAddress> GetRangeIpAddressesInt()
    {
        List<IPAddress> ipAddresses = new List<IPAddress>();
        uint firstip = BitConverter.ToUInt32(FirstIP.GetAddressBytes());
        uint endip = BitConverter.ToUInt32(EndIP.GetAddressBytes());

        int octet = 3;
        int bit = 32;
        firstip = firstip | (uint)(1 << bit);
        return ipAddresses;
    }

    public static byte[] GetBytes(uint ip)
    {
        byte[] bip = new[]
        {
            (byte)(ip),
            (byte)(ip >> 8),
            (byte)(ip >> 16),
            (byte)(ip >> 24)
        };
        return bip;
    }

}