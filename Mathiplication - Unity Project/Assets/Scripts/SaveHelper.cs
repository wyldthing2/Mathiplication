using System.IO;
using System.Xml.Serialization;

public static class SaveHelper
{
    public static string Serialize<T>(this T toSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toSerialize);
        return writer.ToString();

    }

    public static T DeSerialize<T>(this string toDeSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(toDeSerialize);
        return (T)xml.Deserialize(reader);
    }

}
