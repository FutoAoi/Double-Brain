using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform _mouseClickPlayer;
    [SerializeField] Transform _KeyBoardPlayer;

    [SerializeField] float _deltaY;
    [SerializeField] float _deltaZ;
    

    private Transform _tf;
    private float _cameraX;
    private float _cameraY;
    private float _cameraZ;
    private float _mousePos;
    private float _keyPos;

    private void Start()
    {
        _tf = GetComponent<Transform>();
    }

    private void Update()
    {
        _mousePos = _mouseClickPlayer.position.z;
        _keyPos = _KeyBoardPlayer.position.z;
        _cameraX = _tf.position.x;
        _cameraY = 4f + Mathf.Abs(_mousePos - _keyPos) * _deltaY;
        _cameraZ = (_mousePos + _keyPos) / 2 - Mathf.Abs(_mousePos - _keyPos) * _deltaZ;
        
        _tf.position = new Vector3(_cameraX, _cameraY, _cameraZ);
    }
}
