using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Tilemap groundTileMap;
    public Tilemap obstaclesTileMap;
    Vector2 target;
    Animator anim;


    private void Start()
    {
        target = transform.position;
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        Vector2 current = transform.position;
        if (current == target)
        {

            int x = (int)Input.GetAxisRaw("Horizontal");
            int y = (int)Input.GetAxisRaw("Vertical");

            if (x != 0) //LAZYYYYYYYYY
            {
                y = 0; //michael jackson
            }
            target += new Vector2(x, y);

            Vector3Int ObstacleTarget = GetCellLocation(target, obstaclesTileMap);
            Vector3Int BackgroundTarget = GetCellLocation(target, groundTileMap);

            if (obstaclesTileMap.GetTile(ObstacleTarget) != null || groundTileMap.GetTile(BackgroundTarget) == null)
            {
                target = current;
            }
            else
            {
                anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
                anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
            }

        }
        else
        {
            transform.position = Vector3.MoveTowards(current, target, Time.deltaTime * speed);
        }
    }

    Vector3Int GetCellLocation(Vector3 location, Tilemap tilemap)
    {
        return tilemap.WorldToCell(location);
    }
}
