using IP2Location;
using System;
using System.IO;

namespace IpRestriction
{
    /// <summary>
    /// Singleton class that load and inspect Ip2Location database
    /// </summary>
    public static class IpSingleton
    {
        private static Component _ip2Location;
        /// <summary>
        /// The Ip component that holds the reference to the Ip database
        /// </summary>
        public static Component Ip2Location
        {
            get
            {
                return _ip2Location;
            }
        }
        /// <summary>
        /// Load Ip2Location database from the .bin file
        /// </summary>
        /// <param name="IpDbPath"></param>
        public static void LoadDb(string IpDbPath)
        {
            if (string.IsNullOrEmpty(IpDbPath) || !File.Exists(IpDbPath))
            {
                throw new Exception("Ip database bin file not found.");
            }
            var ip2location = new Component
            {
                IPDatabasePath = IpDbPath,
                UseMemoryMappedFile = true,
            };
            _ip2Location = ip2location;
        }
    }
}
