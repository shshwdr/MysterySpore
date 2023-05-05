using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class FMODEvents : Singleton<FMODEvents>
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference musicMainMenu { get; private set; }
    [field: SerializeField] public List<EventReference> musicPlaylist { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField] public EventReference uiClick { get; private set; }

    private int nextPlaylistIndex = 0; 
    internal EventReference getNextMusicReference()
    {
        if (nextPlaylistIndex >= musicPlaylist.Count - 1)
        {
            nextPlaylistIndex = 0;
        }
        EventReference reference = musicPlaylist[nextPlaylistIndex];
        nextPlaylistIndex++;
        return musicPlaylist[nextPlaylistIndex];
    }
}
