
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace RecordingListsForSony
{
    public class CustomDateTimeConverter : IXmlSerializable
    {
        public DateTime Value { get; set; }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            var dateTimeStr = reader.ReadElementContentAsString();
            Value = DateTime.ParseExact(dateTimeStr, "yyyy-MM-ddTHH:mm:sszzz", null);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        }

        public static implicit operator DateTime(CustomDateTimeConverter converter) => converter.Value;
        public static implicit operator CustomDateTimeConverter(DateTime dateTime) => new CustomDateTimeConverter { Value = dateTime };
    }

}
