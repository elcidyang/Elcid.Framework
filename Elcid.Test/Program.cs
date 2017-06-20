using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Elcid.Utilities;

namespace Elcid.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //var doc = XmlHelper.CreateXmlDocument();
            //doc.Save("D:\\1.xml");
            XDocument doc = new XDocument(new XElement("body",
                                           new XElement("level1",
                                               new XElement("level2", "text"),
                                               new XElement("level2", "other text"))));
            doc.Save("D:\\document.xml");
        }
    }
}
