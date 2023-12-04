using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
    private static JournalManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than one player manager");
        }
        instance = this;
    }
    public static JournalManager GetInstance()
    {
        return instance;
    }

    public int journalIndex;
    [SerializeField] private TextAsset[] journalEntries;

    public int playNextJournalEntry()
    {
        if (journalEntries[journalIndex] != null )
        {
            DialogueManager.GetInstance().EnterDialogueMode(journalEntries[journalIndex], null);
            journalIndex++;
            return journalIndex - 1;
        }
        return 0;
    }

    public void playJournalEntry( int index )
    {
        DialogueManager.GetInstance().EnterDialogueMode(journalEntries[index], null);
    }
}
