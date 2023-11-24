using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Notes
{
    public List<float> notes;
}

[CreateAssetMenu(menuName = "Song")]
public class Song : ScriptableObject
{
    public float bpm;
    public AudioClip track;
    public float firstBeatOffset;
    public int damageOffset;
    public int defenseOffset;
    public string stat1;
    public string stat2;

    [SerializeField]
    public List<Notes> noteCombos;
}
