using IP2Location;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IpRestriction
{
    /// <summary>
    /// Enumeration of the country IP list check rule
    /// </summary>
    public enum IpCheckListRule
    {
        /// <summary>
        /// Check IP address against white list only
        /// </summary>
        WhiteListOnly,
        /// <summary>
        /// Check IP address  against black list only
        /// </summary>
        BlackListOnly,
        /// <summary>
        /// Check IP address  against black list and white list
        /// </summary>
        AllLists
    }
    /// <summary>
    /// Implements restriction rules
    /// </summary>
    public class IpRestrictor : IIpRestrictor
    {
        #region Constructor
        /// <summary>
        /// Costructor of IPRestrictor that pass rhe IP db path
        /// </summary>
        /// <param name="ip2LocationDbPath"></param>
        /// <param name="whiteListCountries"></param>
        /// <param name="blackListCountries"></param>
        public IpRestrictor(string ip2LocationDbPath, List<string> whiteListCountries, List<string> blackListCountries)
        {
            //check for whitelist/blacklist intersection and raise an exception if intersection is not null
            if (whiteListCountries != null && blackListCountries != null)
            {
                var intersect = blackListCountries.Select(m => m.ToUpper()).Intersect(whiteListCountries.Select(m => m.ToUpper()));
                if (intersect.Any())
                {
                    throw new Exception("Country black list and white list have common items: " +
                                        string.Join(",", intersect));
                }
            }
            //make sure lists are always uppercase
            this.BlackListCountries = (blackListCountries ?? new List<string>()).Select(m => m.ToUpper()).ToList();
            this.WhiteListCountries = (whiteListCountries ?? new List<string>()).Select(m => m.ToUpper()).ToList();
            if (!File.Exists(ip2LocationDbPath))
            {
                throw new Exception("File not found. The IP2Location database .bin file is missing./n " +
                                    "Please check the file path<<" + ip2LocationDbPath + ">>.");
            }
            if (IpSingleton.Ip2Location == null)
            {
                IpSingleton.LoadDb(ip2LocationDbPath);
            }
        }
        #endregion

        #region Public
        /// <summary>
        /// <para>Instruct restrictor how to proceed with black/white list checking</para>
        /// <para>"AllList" instruct to check if the country Ip address exists in any of the white list or black list</para>
        /// <para>"WhiteListOnly" instruct to check if the country Ip address exists in white list only</para>
        /// <para>"BlackListOnly" instruct to check if the country Ip address exists in black list only</para>
        /// </summary>
        public IpCheckListRule ListRuleCheck { get; set; } = IpCheckListRule.AllLists;
        /// <summary>
        /// Flag that instruct what is the allowance of countryIp address if not found in any of the white nor black lists
        /// </summary>
        public bool RuleIpNotFound { get; set; } = true;

        /// <summary>
        /// The country list to be restricted
        /// </summary>
        public List<string> BlackListCountries { get; }
        /// <summary>
        /// The country list to be allowed
        /// </summary>
        public List<string> WhiteListCountries { get; }

        /// <summary>
        /// Get ip address allowance for a list on ip based of white/black lists
        /// </summary>
        /// <param name="ips"></param>
        /// <returns></returns>
        public List<IpResultInfo> GetIpResults(string[] ips)
        {
            if (ips == null || ips.Length == 0)
            {
                return null;
            }
            var ipListResult = new List<IpResultInfo>();

            foreach (var ipItem in ips)
            {
                var ipResultInfo = GetIpResult(ipItem);
                ipListResult.Add(ipResultInfo);
            }

            return ipListResult;
        }
        /// <summary>
        /// Get ip address allowance for an of ip based on white/black country lists as well as the <see cref="IpRestrictor.RuleIpNotFound"/>        
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public IpResultInfo GetIpResult(string ip)
        {
            var ipInfo = IpReturn(ip);
            bool isIpAllowed = false;
            var ipInList = InCountryList.NeitherList;

            IpResultInfo ipResultInfo;
            switch (ListRuleCheck)
            {
                case IpCheckListRule.AllLists:
                    var inWhiteList = this.CheckIpInList(WhiteListCountries, ip);
                    var inBlackList = this.CheckIpInList(BlackListCountries, ip);
                    if (inWhiteList)
                    {
                        ipInList = InCountryList.InWhiteList;
                    }
                    else if (inBlackList)
                    {
                        ipInList = InCountryList.InBlackList;
                    }

                    isIpAllowed = inWhiteList || (!inBlackList && RuleIpNotFound);
                    break;
                case IpCheckListRule.BlackListOnly:
                    isIpAllowed = !this.CheckIpInList(BlackListCountries, ip);
                    if (!isIpAllowed)
                    {
                        ipInList = InCountryList.InBlackList;
                    }
                    break;
                case IpCheckListRule.WhiteListOnly:
                    isIpAllowed = this.CheckIpInList(WhiteListCountries, ip);
                    if (isIpAllowed)
                    {
                        ipInList = InCountryList.InWhiteList;
                    }
                    break;
            }
            ipResultInfo = new IpResultInfo(isIpAllowed, ipInfo, ipInList);
            return ipResultInfo;
        }
        #endregion

        #region Private
        private IPResult IpReturn(string ip)
        {
            var ipResult = IpSingleton.Ip2Location.IPQuery(ip);
            return ipResult;
        }
        private bool CheckIpInList(List<string> listToCheck, string ip)
        {
            return listToCheck.Where(x => x == IpReturn(ip).CountryShort.ToUpper()).Any();
        }
        #endregion
    }
}
