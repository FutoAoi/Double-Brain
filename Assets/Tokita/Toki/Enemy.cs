using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour,ICharacter
{
    private Collider _collider;
    private KeyBoardPlayer _keyBoardPlayer;
    private MouseClickPlayer _mouseClickPlayer;




    

    public void CharacterSetup()
    {
        _collider = GetComponent<BoxCollider>();

        _keyBoardPlayer = GetComponent<KeyBoardPlayer>();

    }

    public void CharacterUpdate()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
