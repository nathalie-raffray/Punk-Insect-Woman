using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Album
{

    [XmlAttribute("artist")]
    public string artist;

    [XmlAttribute("title")]
    public string title;

    [XmlElement("NumTracksA")]
    public int numTracksA;

    [XmlElement("NumTracksB")]
    public int numTracksB;

    [XmlElement("Sprite")]
    public string sprite;

    [XmlElement("MusicFolder")]
    public string musicFolder;

    [XmlElement("Label")]
    public string label;

    [XmlElement("Catalogue")]
    public string catalogue;

    [XmlElement("Country")]
    public string country;

    [XmlElement("Released")]
    public int released;

    [XmlElement("Genre")]
    public string genre;

    [XmlElement("Style")]
    public string style;

    [XmlElement("Credits")]
    public string credits;

    [XmlElement("Songs")]
    public string songs;

    [XmlElement("SongsA")]
    public string songsA;

    [XmlElement("SongsB")]
    public string songsB;

    [XmlElement("Notes")]
    public string notes;

}
