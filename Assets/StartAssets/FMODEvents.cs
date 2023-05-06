using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class FMODEvents : Singleton<FMODEvents>
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference musicMainMenu { get; private set; }
    [field: SerializeField] public EventReference musicGameplay { get; private set; }
    [field: SerializeField] public List<EventReference> musicPlaylist { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField] public EventReference levelComplete { get; private set; }
    [field: SerializeField] public EventReference levelLose { get; private set; }
    [field: SerializeField] public EventReference robotRays { get; private set; }
    [field: SerializeField] public EventReference vineGrowing { get; private set; }
    [field: SerializeField] public EventReference damage { get; private set; }
    [field: SerializeField] public EventReference dialogue { get; private set; }
    [field: SerializeField] public EventReference dialogueBubble { get; private set; }
    [field: SerializeField] public EventReference door { get; private set; }
    [field: SerializeField] public EventReference humanYell { get; private set; }
    [field: SerializeField] public EventReference humanDying { get; private set; }
    [field: SerializeField] public EventReference itemHit { get; private set; }
    [field: SerializeField] public EventReference knifeHit { get; private set; }
    [field: SerializeField] public EventReference knifeEquip{ get; private set; }
    [field: SerializeField] public EventReference robotActivation { get; private set; }
    [field: SerializeField] public EventReference vineBurnt { get; private set; }
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
