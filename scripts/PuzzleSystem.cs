using UnityEngine;
using System.Collections.Generic;

public class PuzzleSystem : MonoBehaviour
{
    [System.Serializable]
    public class Puzzle
    {
        public string puzzleID;
        public string puzzleName;
        public bool isCompleted;
        public List<PuzzleElement> elements;
    }

    [System.Serializable]
    public class PuzzleElement
    {
        public string elementID;
        public GameObject elementObject;
        public bool isActivated;
    }

    [SerializeField] private List<Puzzle> puzzles = new List<Puzzle>();
    
    private void Start()
    {
        InitializePuzzles();
    }

    private void InitializePuzzles()
    {
        foreach (Puzzle puzzle in puzzles)
        {
            puzzle.isCompleted = false;
            foreach (PuzzleElement element in puzzle.elements)
            {
                element.isActivated = false;
            }
        }
    }

    public void ActivatePuzzleElement(string puzzleID, string elementID)
    {
        Puzzle puzzle = puzzles.Find(p => p.puzzleID == puzzleID);
        if (puzzle != null)
        {
            PuzzleElement element = puzzle.elements.Find(e => e.elementID == elementID);
            if (element != null)
            {
                element.isActivated = true;
                Debug.Log($"Puzzle Element Activated: {elementID}");
                CheckPuzzleCompletion(puzzle);
            }
        }
    }

    private void CheckPuzzleCompletion(Puzzle puzzle)
    {
        bool allActivated = true;
        foreach (PuzzleElement element in puzzle.elements)
        {
            if (!element.isActivated)
            {
                allActivated = false;
                break;
            }
        }

        if (allActivated && !puzzle.isCompleted)
        {
            puzzle.isCompleted = true;
            OnPuzzleCompleted(puzzle);
        }
    }

    private void OnPuzzleCompleted(Puzzle puzzle)
    {
        Debug.Log($"Puzzle Completed: {puzzle.puzzleName}");
    }

    public bool IsPuzzleCompleted(string puzzleID)
    {
        Puzzle puzzle = puzzles.Find(p => p.puzzleID == puzzleID);
        return puzzle != null && puzzle.isCompleted;
    }
}
