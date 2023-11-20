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

    [SerializeField]
    public List<Notes> noteCombos;
}
