using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public GameEvent onMineClicked;


    [Header("Tile Sprites")]
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite flaggedTile;
    [SerializeField] private List<Sprite> clickedTiles;
    [SerializeField] private Sprite mineTile;
    [SerializeField] private Sprite mineWrongTile;
    [SerializeField] private Sprite mineHitTile;

    public int col;
    public int row;
    public GameManager gameManager;

    private SpriteRenderer spriteRenderer;
    public bool flagged = false;
    public bool active = true;
    public bool isMine = false;
    public int mineCount = 0;


    void Awake()
    {
   
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                ClickedTile();
            }
            else if (Input.GetMouseButtonDown(1))
            {
  
                flagged = !flagged;
                if (flagged)
                {
                    spriteRenderer.sprite = flaggedTile;
                }
                else
                {
                    spriteRenderer.sprite = unclickedTile;
                }
            }
        }
    }

    public void ClickedTile()
    {
        if (active && !flagged)
        {
            if (gameManager.primerClick)
            {   
                gameManager.AsegurarPrimerClickSeguro(col, row);
                gameManager.primerClick = false;
            }
            active = false;
            if (isMine)
            {
                //Abre los minijuegos cuando clicka una mina
                SceneManager.LoadScene("Minijuegos", LoadSceneMode.Additive);
                spriteRenderer.sprite = mineHitTile;
                
                
                

            }
            else
            {
                spriteRenderer.sprite = clickedTiles[mineCount];
                if (mineCount == 0) // se expande si se clickea una casilla sin mina y sin numero
                {
                    // Notify GameManager to reveal adjacent tiles
                    gameManager.ClickVecinos(col, row);
                }
                gameManager.CheckGameOver();
            }
        }
    }
    public void ShowGameOverState() 
    {
        if (active)
        {
            active = false;
            if (isMine && !flagged)
            {
                spriteRenderer.sprite = mineTile; // Mina no marcada
            }
            else if (!isMine && flagged)
            {
                spriteRenderer.sprite = mineWrongTile; // Mina marcada incorrectamente
            }
        }
            
    }
    public void SetFlaggedIfMine()
    {
        if (isMine)
        {
            flagged = true;
            spriteRenderer.sprite = flaggedTile;
        }
    }
}