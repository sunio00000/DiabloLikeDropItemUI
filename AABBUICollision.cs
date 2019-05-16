using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomRect
{
    public Vector2 min;
    public Vector2 max;
    public CustomRect(Vector2 n, Vector2 x)
    {
        min = n;
        max = x;
    }

    public CustomRect(RectTransform rect)
    {
        min = new Vector2(rect.localPosition.x,
                                   rect.localPosition.y);
        max = new Vector2(rect.localPosition.x + rect.sizeDelta.x,
                           rect.localPosition.y + rect.sizeDelta.y);
    }
}

public class AABBUICollision : MonoBehaviour
{
    public GameObject Target; public Transform Plane;
    public CustomRect A,B;
    private float snap = 13.1f;
    public int ArrSize;
    public GameObject[] Texts = new GameObject[40];
    public GameObject[] Obj = new GameObject[40];
    [SerializeField]
    private List<RectTransform> RectList = new List<RectTransform>();

    void Start()
    {
        NewTest();
    }

    void Update()
    {
        CursorOnItem();
    }

    public void NewObject()
    {
        GameObject go = Instantiate(Target);
        go.transform.SetParent(Plane);
        go.transform.position = Vector3.zero;
        go.transform.localScale = Vector3.one;
        NewTest(true, go.transform);
    }

    public void DelObject()
    {
        Obj = GameObject.FindGameObjectsWithTag("Muzz");
        Destroy(Obj[Obj.Length - 1]);
        NewTest(false, null);
    }

    public void NewTest()
    {
        RectList.Clear();
        Obj = GameObject.FindGameObjectsWithTag("Muzz");
        for (int i = 0; i < transform.childCount - 3; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < Obj.Length; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        Texts = GameObject.FindGameObjectsWithTag("Item");
        Debug.Log(Texts.Length);
        for (int i = 0; i < Texts.Length; ++i)
        {
            Obj[i].transform.localPosition = new Vector3(Random.Range(-3.5f, 3.5f), 0.05f, Random.Range(-3.5f, 3.5f));
            Texts[i].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(Obj[i].transform.position);
            Texts[i].GetComponent<RectTransform>().position = new Vector3(Texts[i].GetComponent<RectTransform>().position.x,
                Texts[i].GetComponent<RectTransform>().position.y, 0);
            Texts[i].name = "weapon" + Random.Range(100, 1000);
            Obj[i].name = Texts[i].name;
            Texts[i].GetComponentInChildren<Text>().text = Texts[i].name;
        }

        RectList.Add(Texts[0].GetComponent<RectTransform>());

        for (int i = 1; i < Texts.Length; ++i)
        {
            B = new CustomRect(Texts[i].GetComponent<RectTransform>());
            for (int j = 0; j < RectList.Count; ++j)
            {
                A = new CustomRect(RectList[j]);
                while (true)
                {
                    if (IsOverlap(A, B))
                    {
                        if (Random.Range(0.0f, 1.0f) < 0.5f)
                        {
                            Texts[i].GetComponent<RectTransform>().localPosition += Vector3.left * 20.1f;
                        }
                        else
                        {
                            Texts[i].GetComponent<RectTransform>().localPosition -= Vector3.left * 20.1f;
                        }
                        if (IsOverlap(A, B))
                            Texts[i].GetComponent<RectTransform>().localPosition += Vector3.up * snap;
                        B = new CustomRect(Texts[i].GetComponent<RectTransform>());
                        j = 0;
                    }
                    else break;
                }
            }
            RectList.Add(Texts[i].GetComponent<RectTransform>());
        }
    }

    public void NewTest(bool AddTrueDelFalse, Transform Sub)
    {
        int size;
        RectList.Clear();
        Obj = GameObject.FindGameObjectsWithTag("Muzz");
        for (int i=0; i<transform.childCount-3; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (AddTrueDelFalse) size = Obj.Length;
        else size = Obj.Length - 1;
        for (int i = 0; i < size; ++i) 
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        Texts = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < Texts.Length; ++i)
        {
            if (Sub != null) 
                Sub.transform.localPosition = new Vector3(Random.Range(-3.5f, 3.5f), 0.05f, Random.Range(-3.5f, 3.5f));
            Texts[i].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(Obj[i].transform.position);
            Texts[i].GetComponent<RectTransform>().position = new Vector3(Texts[i].GetComponent<RectTransform>().position.x,
                Texts[i].GetComponent<RectTransform>().position.y, 0);
            Texts[i].name = "weapon" + Random.Range(100, 1000);
            Obj[i].name = Texts[i].name;
            Texts[i].GetComponentInChildren<Text>().text = Texts[i].name;
        }

        RectList.Add(Texts[0].GetComponent<RectTransform>());

        for (int i = 1; i < Texts.Length; ++i)
        {
            B = new CustomRect(Texts[i].GetComponent<RectTransform>());
            for (int j = 0; j < RectList.Count; ++j)
            {
                A = new CustomRect(RectList[j]);
                while (true)
                {
                    if (IsOverlap(A, B))
                    {
                        if (Random.Range(0.0f, 1.0f) < 0.5f)
                        {
                            Texts[i].GetComponent<RectTransform>().localPosition += Vector3.left * 20f;
                        }
                        else
                        {
                            Texts[i].GetComponent<RectTransform>().localPosition -= Vector3.left * 20f;
                        }
                        if(IsOverlap(A,B))
                            Texts[i].GetComponent<RectTransform>().localPosition += Vector3.up * snap;
                        B = new CustomRect(Texts[i].GetComponent<RectTransform>());
                        j = 0;
                    }
                    else break;
                }
            }
            RectList.Add(Texts[i].GetComponent<RectTransform>());
        }
    }

    private bool IsOverlap(CustomRect a, CustomRect b)
    {
        if (a.max.x < b.min.x || a.max.y < b.min.y) return false;
        else if (a.min.x > b.max.x || a.min.y > b.max.y) return false;
        return true;
    }

    private void CursorOnItem()
    {
        foreach(var g in Obj)
        {
            if(g!=null)
                g.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        foreach (var t in Texts)
        {
            if (t!= null)
                t.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10000.0f))
        {
            hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
            foreach(var t in Texts)
            {
                if (hit.transform.name == t.name)
                {
                    t.transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
                }
            }
        }
    }
}
