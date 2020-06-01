using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumLoader : MonoBehaviour
{
    public const string path = "albums";

    const string spritePath = "Sprites/records/";

    const string musicPath = "Music/";

    //public GameObject rpObject; 
    //private RecordPlayer rp;

    //public GameObject albumParent;
    public GameObject albumPrefab;

    private void Awake()
    {
        //rp = rpObject.GetComponent<RecordPlayer>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AlbumContainer ac = AlbumContainer.Load(path);

        foreach(Album album in ac.albums)
        {
            LoadAlbumDescription(album);

            GameObject albumObject = Instantiate(albumPrefab);
            albumObject.name = album.artist + " - " + album.title;
            albumObject.transform.SetParent(transform);
            albumObject.GetComponent<SpriteRenderer>().sprite = Resources.Load(spritePath + album.sprite) as Sprite;

            AudioManager am = albumObject.GetComponent<AudioManager>();
            AlbumInfo ai = albumObject.GetComponent<AlbumInfo>();

            ai.a = album;

            am.numSongsA = album.numTracksA;
            am.numSongsB = album.numTracksB;

            MusicGameState.currentAlbum = albumObject;
            MusicGameState.currAM = am;

            am.sounds = new AudioManager.Sound[album.numTracksA + album.numTracksB];
            Debug.Log("sounds.length: " + am.sounds.Length);

            string[] songNames = album.songs.Trim().Split(',');
            //int i = 0;

            for(int i = 0; i<am.sounds.Length; i++)
            {
                am.sounds[i].name = (i+1).ToString();
                Debug.Log(musicPath + album.musicFolder + "/" + (i + 1) + ".mp3");
                am.sounds[i].clip = Resources.Load(musicPath + album.musicFolder + "/" + (i+1)) as AudioClip;
                //if (am.sounds[i].clip == null) Debug.Log("resources load failed!");
                am.sounds[i].loop = false;
            }

            am.Init();
        }
    }

    private void LoadAlbumDescription(Album album)
    {
        ConversationNode node = new ConversationNode();

        string text = album.artist + " - " + album.title + (char)10 + (char)10; //new line feed
        text += "Track List: " + (char)10;

        string[] songANames = album.songsA.Trim().Split(',');
        int i = 1;
        foreach(string song in songANames)
        {
            text += "A" + i + " " + song + (char)10;
            i++;
        }
        i = 1;
        string[] songBNames = album.songsB.Trim().Split(',');
        foreach (string song in songANames)
        {
            text += "B" + i + " " + song + (char)10;
            i++;
        }
        node.retort.Add(text);

        ConversationManagerSystem.conversations.Add(album.title, node);

        node = new ConversationNode();
        text = album.artist + " - " + album.title + (char)10 + (char)10;
        text += "Label: " + album.label + (char)10;
        text += "Catalogue Number: " + album.catalogue + (char)10;
        text += "Format: " + "Vinyl" + (char)10;
        text += "Country: " + album.country + (char)10;
        text += "Released: " + album.released + (char)10;
        text += "Genre: " + album.genre + (char)10;
        text += "Style: " + album.style + (char)10;
        text += "Credits: " + (char)10 + album.credits + (char)10;

        node.retort.Add(text);

        ConversationManagerSystem.conversations.Add(album.title + " info", node);

        node = new ConversationNode();
        text = album.artist + " - " + album.title + (char)10 + (char)10;
        text += "Notes: " + album.notes;

        node.retort.Add(text);

        ConversationManagerSystem.conversations.Add(album.title + " notes", node);

    }

}
