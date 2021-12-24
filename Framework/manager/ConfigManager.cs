//using Genso.DDNS.Syntax;
//using Genso.Framework;
//using System;
//using System.Linq;

//namespace Genso.Framework
//{
//    /// <summary>
//    /// Encapsulates the config files
//    /// </summary>
//    public class ConfigManager
//    {
//        private Data _config;

//        /** EVENTS **/
//        public event ProgramHandler NoInternetError;
//        public event ProgramHandler ServerUnavailableError;
//        public event RequestHandler RequestFail;
//        public event RequestHandler RequestPass;



//        public ConfigManager(Data config)
//        {
//            _config = config;
//        }



//        /// <summary>
//        /// Gets the DNS server's domain address (for connecting)
//        /// </summary>
//        public string getDnsAddress() => _config.getValue<string>(DataFiles.API.Config.DNServerDomain);
//        /// <summary>
//        /// Gets the port number to connect to the DNS server
//        /// </summary>
//        public int getDnsPort() => _config.getValue<int>(DataFiles.API.Config.DNServerPort);
//        /// <summary>
//        /// Check if the inputed domain is in the top domain list (config file)
//        /// </summary>
//        public bool isDomainInTopList(string domain)
//        {
//            //get the top domain list
//            var topDomainRecord = _config.getRecord(DataFiles.API.Config.TopDomain);
//            var domainList = topDomainRecord.Elements();

//            //search the list for a match
//            var found = from re in domainList where re.Value == domain select re;

//            //if found in list, let caller know found in list
//            return found.Any() ? true : false;

//        }

//        /// <summary>
//        /// Gets the name of the DNS server, needed for making queries to the server
//        /// AKA the computer's name
//        /// </summary>
//        public string getServerName() => _config.getValue<string>(DataFiles.Server.Config.ServerName);

//        /// <summary>
//        /// Gets the port used for listening for TCP updates from API server at DNS server 
//        /// </summary>
//        public int getListenPort() => _config.getValue<int>(DataFiles.Server.Config.ListenPort);

//    }
//}