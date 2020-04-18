using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBoxResizer : MonoBehaviour
{
	public Image image;
	public TMP_Text text;

	public Vector2 buffer = new Vector2(10, 10);


	public void Start()
	{
		
	}

	public void Update()
	{
		
	}

	public void Resize(float textWidth, float textHeight)
	{
		if (image && text)
		{
			// width gets ignored now
			Vector2 sizeDelta = image.rectTransform.sizeDelta;
			sizeDelta.y = textHeight + buffer.y * 2;

			image.rectTransform.sizeDelta = sizeDelta;
			//text.rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
			//Vector2 textOffsetMin = text.rectTransform.offsetMin;
			//textOffsetMin.y = image.rectTransform.offsetMin.y;
			//text.rectTransform.offsetMin = -textOffsetMin;
			
			/*Vector3 textPos = text.rectTransform.anchoredPosition;
			textPos.y = image.rectTransform.rect.yMin; // top align
			text.rectTransform.anchoredPosition = textPos;*/
		}
	}
}
