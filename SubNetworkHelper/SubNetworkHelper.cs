namespace SubNetworkHelper;
using System.Net;

public class SubNetworkHelper
    {
        private byte[] _ipBytes;
        private byte[] _maskBytes;
        private byte[] _dnsBytes = new byte[]{8,8,8,8};

        private byte[] _startIPAddress;
        public IPAddress StartIpAddress => new IPAddress(_startIPAddress);
        
        private byte[] _endIPAddress;
        public IPAddress EndIpAddress => new IPAddress(_endIPAddress);

        private byte[][] _networkRange;
        public IPAddress[] NetworkRange
        {
            get
            {
                return new IPAddress[] {new IPAddress(_networkRange[0]),
                                        new IPAddress(_networkRange[1])};
            }
        } 

        private byte[] _networkIP;
        public IPAddress NetworkIp => new IPAddress(_networkIP);

        private byte[] _wildcardIP;
        public IPAddress WildcardIp => new IPAddress(_wildcardIP);

        private byte[] _broadcastIP;
        public IPAddress BroadcastIp => new IPAddress(_broadcastIP);

        public SubNetworkHelper(IPAddress ip, IPAddress mask, IPAddress dns)
        {
            _ipBytes = ip.GetAddressBytes();
            _maskBytes = mask.GetAddressBytes();
            _dnsBytes = dns.GetAddressBytes();
            _networkIP = new byte[4];
            _wildcardIP = new byte[4];
            _broadcastIP = new byte[4];
            _startIPAddress = new byte[4];
            _endIPAddress = new byte[4];
            
            OctetHelper octet;
            int octetindexer = 0;
            bool lastOctet = false;
            foreach (var (ipoctet, maskoctet) in Enumerable.Zip(_ipBytes, _maskBytes))
            {
                // Флаг требуется для проверки последний ли октет рассматривается
                lastOctet = octetindexer == 3;
                
                octet = new OctetHelper(ipoctet, maskoctet);
                _networkIP[octetindexer] = octet.NetworkOctet;
                _wildcardIP[octetindexer] = octet.WildcardOctet;
                _broadcastIP[octetindexer] = octet.BroadcastOctet;
                _startIPAddress[octetindexer] = octet.GetMinHostOctet(lastOctet);
                _endIPAddress[octetindexer] = octet.GetMaxHostOctet(lastOctet);
                octetindexer++;
            }

            _networkRange = new byte[2][] { _startIPAddress, _endIPAddress };

        }

        public IPAddress[] GetIPRange()
        {
            return new IPAddress[]
            {
                new IPAddress(_startIPAddress),
                new IPAddress(_endIPAddress)
            };
        }
    }
    
    public class OctetHelper
    {
        public static byte _ipOctet;
        public static byte _maskOctet;
        private static byte _networkOctet;
        private static byte _wildcardOctet;
        private static byte _broadcastOctet;
        
        public OctetHelper(byte ipOctet, byte maskOctet)
        {
            _ipOctet = ipOctet;
            _maskOctet = maskOctet;
            _networkOctet = (byte)(_ipOctet & _maskOctet);
            _wildcardOctet = (byte)~_maskOctet;
            _broadcastOctet = (byte)(_networkOctet | _wildcardOctet);
        }
        
        public OctetHelper(byte maskOctet)
        {
            _maskOctet = maskOctet;
        }

        public byte NetworkOctet
        {
            get => _networkOctet;
        }

        public byte WildcardOctet
        {
            get => _wildcardOctet;
        }
        
        public byte BroadcastOctet
        {
            get => _broadcastOctet;
        }
        
        
        public byte GetMinHostOctet(bool lastOctet)
        {
            return lastOctet ? (byte)(_networkOctet + (byte)1) : (byte)(_networkOctet);
        }
        public byte GetMaxHostOctet(bool lastOctet)
        {
            return lastOctet ? (byte)(_broadcastOctet - (byte)1) : (byte)(_broadcastOctet);
        }

    }