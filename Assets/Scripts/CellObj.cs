using UnityEngine;
using System.Collections;

public class CellObj : MonoBehaviour {



	public GameObject lifeSprite = null;

	public struct VirtualSlice
	{

		public bool IsAlive { get; set; }  
		public float r, g, b;

	}

	public VirtualSlice[] _slices = new VirtualSlice[2];

	public int _currentSlice = 0;
	public int _aliveTicks = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	//
	//}


	//SLICE ACCESS
	public void SetCurrentSlice(int s)
	{
		_currentSlice = s;
	}


	public void ToggleCurrentSlice()
	{
		if (_currentSlice == 0) {
			_currentSlice = 1;
		} else {
			_currentSlice = 0;
		}
	}


	public void SetCurrentSliceData(bool isAlive, float r, float g, float b)
	{
		_slices[_currentSlice].IsAlive = isAlive;
		_slices[_currentSlice].r = r;
		_slices[_currentSlice].g = g;
		_slices[_currentSlice].b = b;
	}


	public void ClearSlices()
	{
		_slices [0].IsAlive = false;
		_slices [1].IsAlive = false;
		SetLifeColor (0f, 0f, 0f, 0.25f);
	}

	public void SetAlive()
	{
		_currentSlice = 0;

		_slices [_currentSlice].IsAlive = true;
		SetLifeColor (0.5f, 0f, 1f, 1f);
	}
		

	public bool GetCurrentSliceIsAlive()
	{
		return ( _slices [_currentSlice].IsAlive );
	}


	public void SetOffScreenSliceState(bool state)
	{
		int offScreenSlice = 0;
		if (_currentSlice == 0)
			offScreenSlice = 1;

		if(state == true) {
			_aliveTicks++;
		} else {
			_aliveTicks = 0;
		}

		_slices [offScreenSlice].IsAlive = state;
	}

		
	public void FlipSlices()
	{
		_slices [_currentSlice].IsAlive = false;

		ToggleCurrentSlice ();

		if (_slices [_currentSlice].IsAlive) {
			if(_aliveTicks == 1)
				SetLifeColor (0.5f, 0f, 1f, 1f);
			else if(_aliveTicks == 2)
				SetLifeColor (0.7f, 0.2f, 1f, 1f);
			else if(_aliveTicks == 3)
				SetLifeColor (0.9f, 0.4f, 1f, 1f);
			else if(_aliveTicks == 4)
				SetLifeColor (1.0f, 0.6f, 1f, 1f);
			else
				SetLifeColor (1f, 1f, 1f, 1f);
		} else {
			SetLifeColor (0.2f, 0.3f, 0f, 0.25f);
		}

		//stable cell die off
		if(_aliveTicks > 64)
		{
			int value = Random.Range (0, 100);
			if (value < 10) {
				_slices [_currentSlice].IsAlive = false;
				SetLifeColor (0.2f, 0.3f, 0f, 0.25f);
				_aliveTicks = 0;
			}
				
		}

	}


	public void SetLifeColor (float r, float g, float b, float a) 
	{
		lifeSprite.GetComponent<Renderer> ().material.color = new Color (r, g, b,  a);
	}


}
