using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour {

	List<Record> records;
	ModeManagerMode currentMode;

	// Use this for initialization
	void Start () {
		List<ModeAssigner> assigners = GameObject.FindObjectOfType (typeof(ModeAssigner));
		foreach (ModeAssigner assigner in assigners) {
			Register (assigner.gameObject, assigner.mode);
		}
	}

	public void Register(GameObject obj, ModeManagerMode mode){
		if (records == null)
		{
			records = new List<Record> ();
		}
		records.Add (new Record (obj, mode));

	}

	public void SetMode(ModeManagerMode mode){
		if (currentMode == mode) {
			return;
		}
		currentMode = mode;
		foreach (Record rec in records)
		{
			if (rec.mode == currentMode)
			{
				rec.gameObject.SetActive (true);
			}
			else {
				rec.gameObject.SetActive (false);
			}
		}
	}

	class Record{
		public GameObject gameObject;
		public ModeManagerMode mode;
		public bool active;				

		public Record(GameObject obj, ModeManagerMode m){
			gameObject = obj;
			mode =  m;
			active = true;
		}		
	}

	public enum ModeManagerMode{
		All,
		Capture,
		Preview,
		Edit,
		Share
	}
}
