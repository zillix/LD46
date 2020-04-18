using System;
using UnityEngine;
public interface ITextBox
{
	string text{get;set;}
	//	GameObject target{set;}
	void onTextStarted();
	void hide();
	void show();
}
