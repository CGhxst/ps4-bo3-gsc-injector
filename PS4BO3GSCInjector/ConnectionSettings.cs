using System.Net;
using System.Net.Sockets;

namespace PS4BO3GSCInjector
{
    public static class ConnectionSettings
    {
        public static bool TryParsePayloadEndpoint(string ip, string port, out IPEndPoint endpoint)
        {
            endpoint = null;
            if (!IPAddress.TryParse(ip, out var address) ||
                address.AddressFamily != AddressFamily.InterNetwork ||
                !int.TryParse(port, out var portNumber) ||
                portNumber <= IPEndPoint.MinPort ||
                portNumber > IPEndPoint.MaxPort)
            {
                return false;
            }

            endpoint = new IPEndPoint(address, portNumber);
            return true;
        }
    }
}
