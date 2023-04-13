using System.Collections;
using System.Net;
using System.Numerics;

namespace SubNetworkHelper;



public class SubNetworkHelperInt
{
    public IPAddress Mask { get; }
    public IPAddress IP { get; }
    public IPAddress Wildcard { get; }
    public IPAddress Network { get; }
    public IPAddress Broadcast { get; }
    
    public IPAddress FirstIP { get; }
    public IPAddress EndIP { get; }
    
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

    }

}