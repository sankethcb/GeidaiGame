using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicQuestion
{
    public List<int> IndexList;
}

public class MusicMinigame : MonoBehaviour, IMinigame, IGamePlayable
{
    public bool IsPlaying => throw new NotImplementedException();

    public event Action OnGameStart;
    public event Action OnGameComplete;
    public List<MusicNoteBlock> NoteBlocks;

    public List<Sprite> MusicNotes;

    public List<MusicQuestion> QuestionList;

    bool inProgress = false;

    int currentQuestion = 0;
    public void ExitMinigame()
    {
        inProgress = false;

        foreach (MusicNoteBlock noteBlock in NoteBlocks)
        {
            noteBlock.SpriteRenderer.sprite = MusicNotes[0];
            noteBlock.OnTap -= NextNote;
        }
    }

    public IEnumerator Play()
    {
        StartMinigame();

        while (inProgress)
            yield return null;
    }

    public void StartMinigame()
    {
        inProgress = true;

        foreach (MusicNoteBlock noteBlock in NoteBlocks)
        {
            noteBlock.OnTap += NextNote;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Play());
    }

    void NextNote(MusicNoteBlock noteBlock)
    {
        int index = MusicNotes.IndexOf(noteBlock.SpriteRenderer.sprite);

        noteBlock.SpriteRenderer.sprite = MusicNotes[(index + 1) == MusicNotes.Count ? 0 : index + 1];
    }

    public void CheckMusic()
    {
        for (int i = 0; i < QuestionList[currentQuestion].IndexList.Count; i++)
        {
            if (NoteBlocks[i].SpriteRenderer.sprite != MusicNotes[QuestionList[currentQuestion].IndexList[i]])
            {
                Debug.Log("Incorrect");
                return;
            }
        }

        currentQuestion++;
        Debug.Log("Question audio");

        Debug.Log("Correct");

    }
}
