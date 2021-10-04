using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartCell : MonoBehaviour
{
    [SerializeField] Sprite sprite;

    GamePlayManager gameplayManager;
    Color cellColor;
    Material m_Material;
    void Awake()
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        gameplayManager = mainCamera.GetComponent<GamePlayManager>();
        m_Material = GetComponent<Renderer>().material;
        m_Material.mainTexture = sprite.texture;
    }

    public void SetCellColor(Color _cellColor)
    {
        m_Material.color = _cellColor;
        cellColor = _cellColor;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.gameObject.tag == "Dart")
        {
            gameplayManager.HitOnDartBoard(cellColor);
        }
    }
}
