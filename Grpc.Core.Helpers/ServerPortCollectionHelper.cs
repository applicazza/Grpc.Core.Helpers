using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Helpers;
using System.Net.NetworkInformation;

namespace Grpc.Core.Helpers
{
    public static class ServerPortCollectionHelper
    {
        public static IEnumerable<EndPoint> ToEndpoints(this ServerPort port)
        {
            var endpoints = new List<EndPoint>();

            if (port.Host == IPAddress.Any.ToString())
            {
                var ipAddresses = NetworkInterface.GetAllNetworkInterfaces().GetIpAddresses();

                endpoints.AddRange(ipAddresses.Select(address => new DnsEndPoint(address.ToString(), port.BoundPort)));
            }
            else if (port.Host == IPAddress.IPv6Any.ToString())
            {
                var ipAddresses = NetworkInterface.GetAllNetworkInterfaces().GetIpAddresses();

                endpoints.AddRange(ipAddresses.Select(address => new DnsEndPoint(address.ToString(), port.BoundPort)));
            }
            else
            {
                endpoints.Add(new DnsEndPoint(port.Host, port.BoundPort));
            }

            return endpoints;
        }

        public static IEnumerable<EndPoint> ToEndpoints(this Server.ServerPortCollection collection)
        {
            return collection.SelectMany(p => p.ToEndpoints());
        }
    }
}