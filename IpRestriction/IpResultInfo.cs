using IP2Location;

namespace IpRestriction
{
    /// <summary>
    /// Enumeration of IP result appartenance
    /// </summary>
    public enum InCountryList
    {
        /// <summary>
        /// It exists in white list
        /// </summary>
        InWhiteList,
        /// <summary>
        /// It exists in black list
        /// </summary>
        InBlackList,
        /// <summary>
        /// It does not exist in neither white lists
        /// </summary>
        NeitherList
    }
    /// <summary>
    /// Result information for an IP address
    /// </summary>
    public class IpResultInfo
    {
        private IPResult _ipInfo;
        private bool _allowed = false;
        private InCountryList _itemInList = InCountryList.NeitherList;

        /// <summary>
        /// Passing parameters in constructor
        /// </summary>
        /// <param name="allowed"></param>
        /// <param name="ipInfo"></param>
        /// <param name="inList"></param>
        public IpResultInfo(bool allowed, IPResult ipInfo, InCountryList inList)
        {
            _allowed = allowed;
            _ipInfo = ipInfo;
            _itemInList = inList;
        }
        /// <summary>
        /// Returns the type of which list item is in
        /// </summary>
        public InCountryList ItemInList
        {
            get
            {
                return _itemInList;
            }
        }
        /// <summary>
        /// Returns the flag that tells if IP address is allowed by the system
        /// </summary>
        public bool Allowed
        {
            get
            {
                return _allowed;
            }
        }
        /// <summary>
        /// Return geo information about the IP address
        /// </summary>
        public IPResult IpInfo
        {
            get
            {
                return _ipInfo;
            }
        }
    }
}
