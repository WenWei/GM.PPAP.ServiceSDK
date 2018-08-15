using System.Collections.Generic;

namespace GM.PPAP.ServiceSDK.Base
{
    /// <summary>
    /// Interface to wrap parameters of a resource
    /// </summary>
    /// <typeparam name="T">Resource type</typeparam>
    public interface IOptions<T> where T : Resource
    {
        /// <summary>
        /// Generate the list of parameters for the request
        /// </summary>
        ///
        /// <returns>List of parameters for the request</returns>
        List<KeyValuePair<string, string>> GetParams();
    }
}