using System.Collections.Generic;

namespace IpRestriction
{
    /// <summary>
    /// IpRestrictor interface
    /// </summary>
    public interface  IIpRestrictor
    {
        /// <summary>
        /// <para>Instruct restrictor how to proceed with black/white list checking</para>
        /// <para>"AllList" instruct to check if the country Ip address exists in any of the white list or black list</para>
        /// <para>"WhiteListOnly" instruct to check if the country Ip address exists in white list only</para>
        /// <para>"BlackListOnly" instruct to check if the country Ip address exists in black list only</para>
        /// </summary>
        IpCheckListRule ListRuleCheck { get; set; }
        /// <summary>
        /// Flag that instruct what is the allowance of countryIp address if not found in any of the white nor black lists
        /// </summary>
        bool RuleIpNotFound { get; set; }
        /// <summary>
        /// The country list to be restricted
        /// </summary>
        List<string> BlackListCountries { get; }
        /// <summary>
        /// The country list to be allowed
        /// </summary>
        List<string> WhiteListCountries { get; }
        /// <summary>
        /// Get ip address allowance for an of ip based on white/black country lists as well as the <see cref="IIpRestrictor.RuleIpNotFound"/>        
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        IpResultInfo GetIpResult(string ip);
        /// <summary>
        /// Get ip address allowance for a list on ip based of white/black lists
        /// </summary>
        /// <param name="ips"></param>
        /// <returns></returns>
        List<IpResultInfo> GetIpResults(string[] ips);
    }
}
