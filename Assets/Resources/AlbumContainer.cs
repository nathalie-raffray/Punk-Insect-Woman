using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("AlbumCollection")]
public class AlbumContainer 
{
    [XmlArray("Albums")]
    /*[XmlAttribute("num")]
    public int num;*/
    [XmlArrayItem("Album")]

    public List<Album> albums = new List<Album>();

    public static AlbumContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(AlbumContainer));
        StringReader reader = new StringReader(_xml.text);

        AlbumContainer albums = serializer.Deserialize(reader) as AlbumContainer;

        reader.Close();

        return albums;
    }
}
