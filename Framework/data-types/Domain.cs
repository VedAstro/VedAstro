//using Genso.DDNS.Syntax;
//using System;
//using System.Xml.Linq;

//namespace Genso.Framework
//{

//    /// <summary>
//    /// A data representation of a Domain
//    /// </summary>
//    public class Domain
//    {
//        public Domain(string subDomain, string topDomain)
//        {
//            SubDomain = subDomain;
//            TopDomain = topDomain;
//        }

//        public Domain(string subDomain, string topDomain, bool isChecked)
//        {
//            SubDomain = subDomain;
//            TopDomain = topDomain;
//            IsChecked = isChecked;
//        }

//        public string TopDomain { get; }
//        public string SubDomain { get; }
//        public string FullDomain => SubDomain + "." + TopDomain;
//        public bool IsChecked { get; set; }
//        public string IpAddress { get; set; }



//        /// <summary>
//        /// Gets the domain in xml form
//        /// </summary>
//        public XElement toXml()
//        {
//            var subDomain = new XElement(DataFiles.Client.Config.SubDomain, SubDomain);
//            var topDomain = new XElement(DataFiles.Client.Config.TopDomain, TopDomain);
//            var fullDomain = new XElement(DataFiles.Client.Config.Domain, subDomain, topDomain);

//            //return the full domain in xml
//            return fullDomain;
//        }


//        /// <summary>
//        /// Gets a new instance of domain from XML representation of it
//        /// Needed when getting data from Server
//        /// </summary>
//        public static Domain fromXml(XElement domainXml)
//        {
//            //get sub domain & top domain from XML 
//            var subDomain = domainXml.Element(DataFiles.Client.Config.SubDomain).Value;
//            var topDomain = domainXml.Element(DataFiles.Client.Config.TopDomain).Value;

//            return new Domain(subDomain, topDomain);
//        }


//        public override bool Equals(object obj)
//        {
//            return (obj as Domain)?.FullDomain == this.FullDomain;
//        }
//    }
//}