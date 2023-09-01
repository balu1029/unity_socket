using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Vector3 position = Vector3.zero;
    [SerializeField] public GameObject[] atomPrefabs;
    public Transform parentTransform; // Set this in the Inspector
    public GameObject bondPrefab;


    Dictionary<int,int> atomPrefabMap = new Dictionary<int, int>(){
	{1, 0},
	{6, 1},
	{7, 2},
    {8, 3}
    };

    private List<int> elements = new List<int>(){1,6,1};
    private List<Vector3> coordinates = new List<Vector3>
    {
        new Vector3(1f, 0f, 0f),
        new Vector3(0f, 1f, 0f),
        new Vector3(1f, 0f, 1f)
    };



    // Start is called before the first frame update
    void Start()
    {
        // if(gameObject.GetComponent<Rigidbody>() == null){
        //         createMolecule(dataReceived);
        //     } 

        createMolecule();   
        makeBonds();     
    }

    public void createMolecule(){

        for (int i = 0; i < coordinates.Count; i++)
        {
            // Get the current Vector3 instance using the index
            Vector3 vector = coordinates[i];

            // Perform actions on the current Vector3 instance
            Debug.Log("Vector3 value at index " + i + ": " + vector);

            GameObject instantiatedPrefab = Instantiate(atomPrefabs[atomPrefabMap[elements[i]]]);
            instantiatedPrefab.transform.SetParent(parentTransform);

            instantiatedPrefab.transform.localPosition = vector;
        }
    }

    void makeBonds(){
        GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom"); // Adjust the tag
        
        for (int i = 0; i < atoms.Length - 1; i++)
        {
            for (int j = i + 1; j < atoms.Length; j++)
                {
                    float distance = Vector3.Distance(atoms[i].transform.position, atoms[j].transform.position);

                    if (distance < 1.5f)
                    {
                        CreateBond(atoms[i].transform.position, atoms[j].transform.position);
                    }
                }
        }
        
}

    void CreateBond(Vector3 start, Vector3 end)
    {
        Vector3 bondPosition = (start + end) / 2f;
        Vector3 bondScale = new Vector3(0.05f, Vector3.Distance(start, end) / 2f, 0.05f);

        GameObject bond = Instantiate(bondPrefab, bondPosition, Quaternion.identity);
        bond.transform.SetParent(parentTransform);


        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, end - start);
        bond.transform.rotation = rotation;

        bond.transform.localScale = bondScale;


    }

    void Update()
    {
        // Set this object's position in the scene according to the position received
        transform.position = position;
    }
}
