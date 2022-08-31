using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

public class LSystem : MonoBehaviour
{
    private static int iterations;
    private int currentStringPosition;
    private static float angle;
    private float width;
    private float length;
    private float variance;
    private float overlapDistance;
    private static float overlapAllowance;
    private float largestOverlapDistance;
    private static string currentString;
    private Vector3 overlapDirection;
    private static string axiom;
    private Stack<TransformInfo> transformStack;
    private static Dictionary<char, string> rules;
    private float[] randomRotationValues;
    private List<GameObject> prefabList;
    private GameObject tempObject;
    private GameObject parentBuilding;


    public GameObject roomPrefab;
    public GameObject hallwayPrefab;
    public GameObject entrancePrefab;

    void initializeValues()
    {
        currentStringPosition = 0;
        currentString = string.Empty;
        overlapDirection = Vector3.zero;
        
        transformStack = new Stack<TransformInfo>();
        
    }

    //void deInitializeValues()
    //{
    //    Destroy(prefabList);
    //}



    // Start is called before the first frame update
    void Start()
    {
        //iterations = 4;
        
        //angle = 25f;
        width = 1f;
        length = 3f;
        variance = 10f;

        randomRotationValues = new float[100];

        prefabList = new List<GameObject>();
        tempObject = new GameObject();
        parentBuilding = new GameObject();

        for (int i = 0; i < randomRotationValues.Length; i++)
        {
            randomRotationValues[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        initializeValues();
        //GenerateString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            while(currentStringPosition<currentString.Length)
            {
                Generate(currentStringPosition);
                Debug.Log(currentString[currentStringPosition]);
                currentStringPosition++;
            }
        }
    }

   

    public void setValuesAndGenerate(string passedAxiom, int passedIterations, float passedAngle, float passedOverlapAllowance)
    {
        initializeValues();
        //Debug.Log(axiom + " " + iterations + " " + angle + " " + overlapAllowance);
        axiom = passedAxiom;
        iterations = passedIterations;
        angle = passedAngle;
        overlapAllowance = passedOverlapAllowance;

        GenerateString();
    }

    public void setRules(KeyValuePair<string, Dictionary<char, string>> passedSystem)
    {
        rules = passedSystem.Value;
        
    }

    private static void GenerateString()
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

    private void Generate(int stringPosition)
    {
        

            switch (currentString[stringPosition])
            {
                case 'F':
                tempObject = Instantiate(hallwayPrefab, transform.position, transform.rotation);

                

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
                    tempObject.transform.parent = parentBuilding.transform;
                }
                else
                {
                    Debug.Log("Destroying");
                    Destroy(tempObject);
                    //StringBuilder newString = new StringBuilder();
                    //int index, stackPopCount = 0;
                    //for(index = 0; index < stringPosition; index++)
                    //{
                    //    newString.Append(currentString[index]);
                    //}
                    //while(index < currentString.Length)
                    //{
                    //    if(currentString[index] == '[')
                    //    {
                    //        stackPopCount++;
                    //    }
                    //    if(currentString[index] == ']')
                    //    {
                    //        //index++;
                    //        if (stackPopCount == 0)
                    //        {
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            stackPopCount--;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        index++;
                    //    }
                        
                    //}
                    //for(index = index; index < currentString.Length; index++)
                    //{
                    //    newString.Append(currentString[index]);
                    //}
                    //currentString = newString.ToString();
                }

                transform.Translate(Vector3.up * 118);

                    break;

                case 'B':
                tempObject = Instantiate(entrancePrefab, transform.position, transform.rotation);
                    prefabList.Add(tempObject);
                tempObject.transform.parent = parentBuilding.transform;
                transform.Translate(new Vector3(20, 158, -20));
                    break;
                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle * (1 + variance / 100 * randomRotationValues[stringPosition % randomRotationValues.Length]));
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle * (1 + variance / 100 * randomRotationValues[stringPosition % randomRotationValues.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.up * (angle * 2) * (1 + variance / 100 * randomRotationValues[stringPosition % randomRotationValues.Length]));
                    break;

                case '/':
                    transform.Rotate(Vector3.down * (angle * 2) * (1 + variance / 100 * randomRotationValues[stringPosition % randomRotationValues.Length]));
                    break;

                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                 
                transform.Translate(new Vector3(-1, 0, -1));
                tempObject = Instantiate(roomPrefab, transform.position, transform.rotation);
                //transform.Translate(Vector3.up * length);

                largestOverlapDistance = 0f;
                foreach(GameObject go in prefabList)
                {
                    bool overlapped = Physics.ComputePenetration(tempObject.GetComponent<BoxCollider>(), tempObject.transform.position, tempObject.transform.rotation, go.GetComponent<BoxCollider>(), go.transform.position, go.transform.rotation, out overlapDirection, out overlapDistance);
                    if(overlapped && overlapDistance > largestOverlapDistance)
                    {
                        largestOverlapDistance = overlapDistance;

                    }
                    
                }
                if(largestOverlapDistance < overlapAllowance)
                {
                    prefabList.Add(tempObject);
                    tempObject.transform.parent = parentBuilding.transform;
                }
                else
                {
                    Destroy(tempObject);
                }

                TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;

                default:
                    break;
                
                    
            }
        

    }

    public void setTransformBeforeGenerating(Vector3 blockPosition, Quaternion blockRotation)
    {
        transform.position = blockPosition;
        transform.rotation = blockRotation;
    }
}
