using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CityGenerator : MonoBehaviour
{
    private static int iterations;
    private int currentStringPosition;
    private static float angle;
    private float variance;
    private static string currentString;
    private static string axiom;
    private Stack<TransformInfo> transformStack;
    private static Dictionary<char, string> rules;
    private float[] randomRotationValues;
    private GameObject tempObject;
    private GameObject parentCity;
    private float largestOverlapDistance, overlapDistance, overlapAllowance;
    private List<GameObject> prefabList;
    private Vector3 overlapDirection;

    public GameObject cityBlockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        iterations = 4;
        angle = 25f;
        axiom = "X";
        currentString = string.Empty;
        variance = 10f;
        currentStringPosition = 0;
        randomRotationValues = new float[100];
        overlapAllowance = 70;
       
        
        transformStack = new Stack<TransformInfo>();
        tempObject = new GameObject();
        parentCity = new GameObject();
        prefabList = new List<GameObject>();

        for (int i = 0; i < randomRotationValues.Length; i++)
        {
            randomRotationValues[i] = UnityEngine.Random.Range(-1f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            while (currentStringPosition < currentString.Length)
            {
                GenerateCity(currentStringPosition);
                Debug.Log(currentString[currentStringPosition]);
                currentStringPosition++;
            }
        }
    }

    public void setRules(KeyValuePair<string, Dictionary<char, string>> passedSystem)
    {
        rules = passedSystem.Value;
        GenerateString();
    }

    private void GenerateString()
    {
        currentString = axiom;
        //Debug.Log("got here after axiom");
        //Debug.Log(currentString);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }

            currentString = sb.ToString();
            sb = new StringBuilder();
        }

        //currentString = "BFF]";

        Debug.Log("generating string ");
        Debug.Log(currentString);
    }

    private void GenerateCity(int stringPosition)
    {
        switch(currentString[currentStringPosition])
        {
            case 'F':
                tempObject = Instantiate(cityBlockPrefab, transform.position, transform.rotation);

                largestOverlapDistance = 0f;
                foreach (GameObject go in prefabList)
                {
                    bool overlapped = Physics.ComputePenetration(tempObject.GetComponent<BoxCollider>(), tempObject.transform.position, tempObject.transform.rotation, go.GetComponent<BoxCollider>(), go.transform.position, go.transform.rotation, out overlapDirection, out overlapDistance);
                    if (overlapped && overlapDistance > largestOverlapDistance)
                    {
                        largestOverlapDistance = overlapDistance;

                    }

                }
                if (largestOverlapDistance < overlapAllowance)
                {
                    prefabList.Add(tempObject);
                    tempObject.transform.parent = parentCity.transform;
                }
                else
                {
                    Destroy(tempObject);
                }

                transform.Translate(Vector3.forward * 118);

                //tempObject.transform.parent = parentCity.transform;
                break;
            case 'X':
                break;
            case '+':
                transform.RotateAround(transform.position, transform.up, angle * (1 + variance / 100 * randomRotationValues[stringPosition % randomRotationValues.Length]));
                break;
            case '-':
                transform.RotateAround(transform.position, transform.up, angle * (1 + variance / 100 * randomRotationValues[stringPosition % randomRotationValues.Length]));
                break;
            case '[':
                transformStack.Push(new TransformInfo()
                {
                    position = transform.position,
                    rotation = transform.rotation
                });
                break;
            case ']':
                TransformInfo ti = transformStack.Pop();
                transform.position = ti.position;
                transform.rotation = ti.rotation;
                break;

            default:
                break;

        }
    }

}
