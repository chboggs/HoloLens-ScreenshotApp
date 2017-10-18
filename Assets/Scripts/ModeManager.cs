using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{

    List<Record> records;
    public ModeManagerMode currentMode = ModeManagerMode.Init;

    // Use this for initialization
    void Start()
    {
        ModeAssigner[] assigners = GameObject.FindObjectsOfType(typeof(ModeAssigner)) as ModeAssigner[];
        foreach (ModeAssigner assigner in assigners)
        {
            Register(assigner.gameObject, assigner.mode);
        }
        Debug.Log("registered: " + records.Count.ToString());
    }

    public void Register(GameObject obj, ModeManagerMode mode)
    {
        if (records == null)
        {
            records = new List<Record>();
        }
        records.Add(new Record(obj, mode));
        obj.SetActive(mode == ModeManagerMode.Init);

    }

    public void SetMode(ModeManagerMode mode)
    {
        if (currentMode == mode)
        {
            Debug.Log("same mode");
            return;
        }
        currentMode = mode;
        Debug.Log("changing mode");
        foreach (Record rec in records)
        {
            if (rec.mode == currentMode)
            {
                rec.gameObject.SetActive(true);
            }
            else
            {
                rec.gameObject.SetActive(false);
            }
        }
    }

    class Record
    {
        public GameObject gameObject;
        public ModeManagerMode mode;
        public bool active;

        public Record(GameObject obj, ModeManagerMode m)
        {
            gameObject = obj;
            mode = m;
            active = true;
        }
    }

    public enum ModeManagerMode
    {
        All,
        Init,
        Capture,
        Preview,
        Edit,
        Gallery,
        Share,
        Help
    }
}

    
