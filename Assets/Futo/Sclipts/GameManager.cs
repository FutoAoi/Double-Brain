using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<ICharacter> _characterList = new List<ICharacter>();

    private void Start()
    {
        var characters = FindObjectsByType<MonoBehaviour>(
                         FindObjectsInactive.Include,
                         FindObjectsSortMode.None
                         ).OfType<ICharacter>();
        foreach (var charactor in characters)
        {
            _characterList.Add(charactor);
            charactor.CharacterSetup();
        }
    }
    private void Update()
    {
        foreach (var character in _characterList)
        {
            character.CharacterUpdate();
        }
    }

}
