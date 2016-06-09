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

	const int surroundingCells = 8;

	private int[] rulesSurvive = new int[surroundingCells];
	private int[] rulesBorn = new int[surroundingCells];

	public static GridObj Instance;

	void Awake () {
		
		Instance = this;

	}
		
	// Use this for initialization
	void Start () {

		GridCells = new List<List<GameObject>> ();

		SetCoorSys (-9.0f, -3.7f, 0.09f, 0.09f);
		CreateGird (200, 106);

		CreateRandomGrid (50, 4, 4);

		SetRuleVariation (1);
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
		for (int r = 1; r < mRows - 2; r++) {

			List<GameObject> rowA = GridCells [r];
			List<GameObject> rowB = GridCells [r+1];
			List<GameObject> rowC = GridCells [r+2];

			for (int c = 1; c < mCols - 2; c++) {

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
		CellObj cellObjectScript = _cellObj.GetComponent<CellObj> ();

		bool isAlive = cellObjectScript.GetCurrentSliceIsAlive ();

		bool lifeState = false;

		//survive
		if (isAlive == true) {

			for (int i = 0; i < surroundingCells; i++) {
			
				int rule = rulesSurvive [i];
				if (rule == -1) {
					break;
				} else {
					if (fitness == rule) {

						lifeState = true;
						break;
					}
				}
			}
		}

		//born
		if (isAlive == false) {

			for (int i = 0; i < surroundingCells; i++) {

				int rule = rulesBorn [i];
				if (rule == -1) {
					break;
				} else {
					if (fitness == rule) {

						lifeState = true;
						break;
					}
				}
			}
		}
			
		cellObjectScript.SetOffScreenSliceState (lifeState);
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


	public void CreateRandomGrid(int frequency, int rangeX, int rangeY)
	{
		for (int r = rangeY; r < mRows-rangeY; r++) {

			List<GameObject> rowCells = GridCells [r];

			for (int c = rangeX; c < mCols-rangeX; c++) {

				int value = Random.Range (0, 100);
				if (value < frequency) {
					GameObject _cellObj = rowCells [c];
					CellObj cellObjectScript = _cellObj.GetComponent<CellObj> ();


					cellObjectScript.SetAlive ();
				}
			}
		}
	}



	private void SetRuleVariation(int rule)
	{
		if (rule == 1) {

			//Conways rule
			//32/3

			rulesSurvive [0] = 2;
			rulesSurvive [1] = 3;
			rulesSurvive [2] = -1;

			rulesBorn [0] = 3;
			rulesBorn [1] = -1;

		} else if (rule == 2){

			//34 Life
			//34/34

			rulesSurvive [0] = 3;
			rulesSurvive [1] = 4;
			rulesSurvive [2] = -1;

			rulesBorn [0] = 3;
			rulesBorn [1] = 4;
			rulesBorn [2] = -1;

		} else if (rule == 3){

			//Coagulations
			//235678/378

			rulesSurvive [0] = 2;
			rulesSurvive [1] = 3;
			rulesSurvive [2] = 5;
			rulesSurvive [3] = 6;
			rulesSurvive [4] = 7;
			rulesSurvive [5] = 8;
			rulesSurvive [6] = -1;

			rulesBorn [0] = 3;
			rulesBorn [1] = 7;
			rulesBorn [3] = 8;
			rulesBorn [4] = -1;

		}
	}

		
		
}
