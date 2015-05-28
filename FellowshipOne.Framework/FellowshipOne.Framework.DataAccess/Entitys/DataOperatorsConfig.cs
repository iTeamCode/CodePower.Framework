using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace CodePower.Framework.Entitys
{
    [XmlRoot("DataOperators")]
    public class DataOperatorsConfig
    {
        [XmlElement("DataCommand", typeof(DataCommandConfig))]
        public List<DataCommandConfig> DataCommands { get; set; }
    }

    public class DataCommandConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("DataSourceID")]
        public string DataSourceID { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlElement("CommandText")]
        public string CommandText { get; set; }

        [XmlArrayItem("Param", typeof(ParameterConfig))]
        public List<ParameterConfig> Parameters { get; set; }
    }


    public class ParameterConfig
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("DBType")]
        public DbType DBType { get; set; }

        [XmlAttribute("Size")]
        public int Size { get; set; }
    }
}
