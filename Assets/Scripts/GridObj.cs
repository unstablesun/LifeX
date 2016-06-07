using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GridObj : MonoBehaviour {


	public List<List<GameObject>> GridCells { get; set; } 

	private int mCols = 1;
	private int mRows = 1;

	private float mStartX;
	private float mStartY;
	private float mDeltaX;
	private float mDeltaY;


	public static GridObj Instance;

	void Awake () {
		
		Instance = this;

	}
		
	// Use this for initialization
	void Start () {

		GridCells = new List<List<GameObject>> ();

		SetCoorSys (-8.0f, -3.0f, 0.2f, 0.2f);
		CreateGird (64, 32);

		CreateRandomGrid (25);

	}
		
	// Update is called once per frame
	void Update () {
	

		ProcessGrid ();
		FlipGrid ();
	}

	public void SetCoorSys(float startX, float startY, float deltaX, float deltaY)
	{
		mStartX = startX;
		mStartY = startY;
		mDeltaX = deltaX;
		mDeltaY = deltaY;

	}

	public void CreateGird(int cols, int rows)
	{
		mCols = cols;
		mRows = rows;

		float x = mStartX, y = mStartY, z = 1f;

		for (int r = 0; r < mRows; r++) {

			List<GameObject> RowCells = new List<GameObject> ();

			x = mStartX;
			for (int c = 0; c < mCols; c++) {

				GameObject _cellObj = Instantiate (Resources.Load ("Prefabs/CellObj", typeof(GameObject))) as GameObject;
				_cellObj.transform.position = new Vector3(x, y, z);

				CellObj cellObjectScript = _cellObj.GetComponent<CellObj> ();
				cellObjectScript.SetCurrentSliceData (false, 0f, 0f, 0f);
				cellObjectScript.ClearSlices ();//some redundancey

				RowCells.Add (_cellObj);

				x += mDeltaX;
			}

			y += mDeltaY;

			GridCells.Add (RowCells);
		}
		
	}

	public void ProcessGrid()
	{
		for (int r = 1; r < mRows - 4; r++) {

			List<GameObject> rowA = GridCells [r];
			List<GameObject> rowB = GridCells [r+1];
			List<GameObject> rowC = GridCells [r+2];

			for (int c = 1; c < mCols - 4; c++) {

				int fitness = 0; 
				fitness += GetCellLife (rowB [c - 1]);
				fitness += GetCellLife (rowB [c + 1]);
				fitness += GetCellLife (rowA [c - 1]);
				fitness += GetCellLife (rowA [c]);
				fitness += GetCellLife (rowA [c + 1]);
				fitness += GetCellLife (rowC [c - 1]);
				fitness += GetCellLife (rowC [c]);
				fitness += GetCellLife (rowC [c + 1]);

				ProcessRule (rowB [c], fitness);
			}
		}
	}

	private int GetCellLife(GameObject _cellObj)
	{
		CellObj cellObjectScript = _cellObj.GetComponent<CellObj> ();
		if (cellObjectScript.GetCurrentSliceIsAlive ())
			return 1;
		else
			return 0;
	}


	//public bool GetRuleResult(int state, int fitness)
	private void ProcessRule(GameObject _cellObj, int fitness)
	{
		bool updatedState;
		CellObj cellObjectScript = _cellObj.GetComponent<CellObj> ();

		bool isAlive = cellObjectScript.GetCurrentSliceIsAlive ();

		if (isAlive == false && fitness == 3)
			updatedState = true;
		else if (isAlive == true && (fitness >= 2 && fitness <= 3))
			updatedState = true;
		else
			updatedState = false;

		cellObjectScript.SetOffScreenSliceState (updatedState);
	}



	public void FlipGrid()
	{
		for (int r = 0; r < mRows; r++) {

			List<GameObject> rowCells = GridCells [r];

			for (int c = 0; c < mCols; c++) {

				GameObject _cellObj = rowCells [c];
				CellObj cellObjectScript = _cellObj.GetComponent<CellObj> ();

				cellObjectScript.FlipSlices ();
			}
		}
	}


	public void CreateRandomGrid(int frequency)
	{
		for (int r = 1; r < mRows-1; r++) {

			List<GameObject> rowCells = GridCells [r];

			for (int c = 1; c < mCols-1; c++) {

				int value = Random.Range (0, 100);
				if (value < frequency) {
					GameObject _cellObj = rowCells [c];
					CellObj cellObjectScript = _cellObj.GetComponent<CellObj> ();


					cellObjectScript.SetAlive ();
				}
			}
		}
	}

		
		
}
