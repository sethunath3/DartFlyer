using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DartBoard : MonoBehaviour {

	public DartCell dartCell;

	ArrayList al = new ArrayList(36);

    
    float cellGap;
    float boardBorderWidth;
    int noOfCellRows;
    float cellWidth;

    int[] colourOrder;

    List<DartCell> cells = new List<DartCell>();

    // Use this for initialization
    void Start () {

		cellGap = 0.0f;
		boardBorderWidth = 0.6f;
		noOfCellRows = 6;
		cellWidth = (transform.localScale.x-(boardBorderWidth*2)-(cellGap*(noOfCellRows-1)))/noOfCellRows;

		al.Clear();
		al.Add(Color.red);
		al.Add(Color.yellow);
		al.Add(Color.green);
		al.Add(Color.blue);
		al.Add(Color.black);
		al.Add(Color.white);

		colourOrder =  new int[]{1,0,5,2,1,2,3,5,4,3,0,3,4,3,1,5,4,1,1,0,3,2,5,4,2,5,4,5,0,2,4,0,2,3,1,0};


		float boardStartPointX = transform.position.x - (transform.localScale.x/2)+boardBorderWidth;
		float boardStartPointY = transform.position.y - (transform.localScale.y/2)+boardBorderWidth;

		float x =boardStartPointX;
		for(int i = 0; i<noOfCellRows; i++,x+=(cellWidth+cellGap))
        {
      		float y =boardStartPointY;
			for(int j = 0; j<noOfCellRows; j++,y+=(cellWidth+cellGap))
            {
                DartCell block = (DartCell)Instantiate(dartCell,dartCell.transform.position, dartCell.transform.rotation)as DartCell;
				block.transform.localScale=new Vector3(cellWidth,cellWidth,cellWidth);
       			block.transform.position = new Vector3((float)( x+(cellWidth/2)), (float)(y+(cellWidth/2)), transform.position.z);
				int index = colourOrder[(i*noOfCellRows) + j];

                block.SetCellColor((Color)al[index]);

                block.gameObject.transform.parent = transform;
                cells.Add(block);
            }
		}

        ReShuffleColors();

    }

    public void ReShuffleColors()
    {
        for (int i = 0; i < al.Count; i++)
        {
            Color temp = (Color)al[i];
            int randomIndex = Random.Range(i, al.Count);
            al[i] = al[randomIndex];
            al[randomIndex] = temp;
        }

        int colorindex = 0;
        int index = 0;
        foreach (DartCell cell in cells)
        {
            index = colourOrder[colorindex];
            cell.SetCellColor((Color)al[index]);
            colorindex++;
        }
    }
}
