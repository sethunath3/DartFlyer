using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DartBoard : MonoBehaviour {

	public GameObject dartCell;
	public Texture2D cellTexture;

	ArrayList al = new ArrayList(36);

	// Use this for initialization
	void Start () {

		Sprite sprite = Resources.Load<Sprite>(cellTexture.name);
		// foreach (Sprite asprite in sprites) {
		// 	Debug.Log(asprite.name);
        //  }

		float cellGap = 0.0f;
		float boardBorderWidth = 0.6f;
		int noOfCellRows = 6;
		float cellWidth = (transform.localScale.x-(boardBorderWidth*2)-(cellGap*(noOfCellRows-1)))/noOfCellRows;

		al.Clear();
		al.Add(Color.red);
		al.Add(Color.yellow);
		al.Add(Color.green);
		al.Add(Color.blue);
		al.Add(Color.black);
		al.Add(Color.white);

		for (int i = 0; i < al.Count; i++) 
		{
         	Color temp = (Color)al[i];
         	int randomIndex = Random.Range(i, al.Count);
         	al[i] = al[randomIndex];
         	al[randomIndex] = temp;
     	}

		int[] colourOrder =  new int[]{1,0,5,2,1,2,3,5,4,3,0,3,4,3,1,5,4,1,1,0,3,2,5,4,2,5,4,5,0,2,4,0,2,3,1,0};



		//float boardHeight = ((noOfCellRows-1)*cellGap) + (2*boardBorderWidth) + (noOfCellRows*cellWidth);
		//transform.localScale = new Vector3(boardHeight,boardHeight, transform.localScale.z);

		float boardStartPointX = transform.position.x - (transform.localScale.x/2)+boardBorderWidth;
		float boardStartPointY = transform.position.y - (transform.localScale.y/2)+boardBorderWidth;

		float boardEndPointX = transform.position.x + (transform.localScale.x/2)+boardBorderWidth;
		float boardEndPointY = transform.position.y + (transform.localScale.y/2)+boardBorderWidth;

		float x =boardStartPointX;
		for(int i = 0; i<noOfCellRows; i++,x+=(cellWidth+cellGap)) {
       		//yield return new WaitForSeconds(spawnSpeed);
      		float y =boardStartPointY;
			for(int j = 0; j<noOfCellRows; j++,y+=(cellWidth+cellGap)) {
       			//yield return new WaitForSeconds(spawnSpeed);
  
       			GameObject block = (GameObject)Instantiate(dartCell,dartCell.transform.position, dartCell.transform.rotation)as GameObject;
				block.transform.localScale=new Vector3(cellWidth,cellWidth,cellWidth);
       			block.transform.position = new Vector3((float)( x+(cellWidth/2)), (float)(y+(cellWidth/2)), transform.position.z);
				Material m_Material = block.GetComponent<Renderer>().material;
				int index = colourOrder[(i*noOfCellRows) + j];
				m_Material.color = (Color)al[index];
				m_Material.mainTexture = sprite.texture;

				block.gameObject.transform.parent = transform;
			}
		}
	}

	Texture2D ConvertSpriteToTexture(Sprite sprite)
             {
                 try
                 {
                     if (sprite.rect.width != sprite.texture.width)
                     {
                         Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                         Color[] colors = newText.GetPixels();
                         Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                                      (int)System.Math.Ceiling(sprite.textureRect.y),
                                                                      (int)System.Math.Ceiling(sprite.textureRect.width),
                                                                      (int)System.Math.Ceiling(sprite.textureRect.height));
                         Debug.Log(colors.Length+"_"+ newColors.Length);
                         newText.SetPixels(newColors);
                         newText.Apply();
                         return newText;
                     }
                     else
                         return sprite.texture;
                 }catch
                 {
                     return sprite.texture;
                 }
             }
	
	// Update is called once per frame
	void Update () {
		
	}
}
