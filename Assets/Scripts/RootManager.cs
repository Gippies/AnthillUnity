using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootManager : MonoBehaviour {

    private static readonly int INITIAL_AMOUNT_OF_GATHERERS = 40;
    private static readonly int INITIAL_AMOUNT_OF_DIGGERS = 10;
    private static readonly int INITIAL_AMOUNT_OF_LEAVES = 100;
    private static readonly int GROUND_WIDTH = 100;
    private static readonly int GROUND_LENGTH = 100;
    private static readonly float LEAFY_SEARCH_RADIUS = 1.0f;
    private static readonly float DIRT_SEARCH_RADIUS = 10.0f;

    public GameObject gathererPrefab;
    public GameObject diggerPrefab;
    public GameObject leafyPrefab;
    public GameObject groundPrefab;
    public GameObject dirtPrefab;

    private List<GameObject> leafies;
    private List<GameObject> dirts;

    public List<GameObject> GetLeafies() {
        return leafies;
    }

    public List<GameObject> GetDirts() {
        return dirts;
    }

    private void GroundSetup() {
        // Instantiate the ground
        for (int i = 0; i < GROUND_WIDTH; i++) {
            for (int j = 0; j < GROUND_LENGTH; j++) {
                int xPos = i - GROUND_WIDTH / 2;
                int zPos = j - GROUND_LENGTH / 2;
                if (xPos != 0 || zPos != 0)
                    Instantiate(groundPrefab, new Vector3(xPos, -0.5f, zPos), Quaternion.identity);
            }
        }

        // Instantiate the dirt
        dirts = new List<GameObject>();
        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                GameObject dirt = Instantiate(dirtPrefab, new Vector3(i, -1.5f, j), Quaternion.identity);
                dirt.GetComponent<CarriableBehavior>().searchRadius = DIRT_SEARCH_RADIUS;
                dirts.Add(dirt);
            }
        }
    }

    private void AntSetup() {
        int oneSide = (int) Mathf.Sqrt(INITIAL_AMOUNT_OF_GATHERERS);
        for (int i = 0; i < oneSide; i++) {
            for (int j = 0; j < oneSide; j++) {
                GameObject newGatherer = Instantiate(gathererPrefab, new Vector3(i - oneSide / 2, 1, j - oneSide / 2), Quaternion.identity);
                newGatherer.GetComponent<AntBehavior>().rootManager = this;
            }
        }

        oneSide = (int) Mathf.Sqrt(INITIAL_AMOUNT_OF_DIGGERS);
        for (int i = 0; i < oneSide; i++) {
            for (int j = 0; j < oneSide; j++) {
                GameObject newDigger = Instantiate(diggerPrefab, new Vector3(i + oneSide / 2, 1, j + oneSide / 2), Quaternion.identity);
                newDigger.GetComponent<AntBehavior>().rootManager = this;
            }
        }
    }

    private void PlantSetup() {
        leafies = new List<GameObject>();
        for (int i = 0; i < INITIAL_AMOUNT_OF_LEAVES; i++) {
            GameObject newLeafy = Instantiate(leafyPrefab, new Vector3(Random.Range(-20.0f, 20.0f), 0.25f / 2.0f, Random.Range(-20.0f, 20.0f)), Quaternion.identity);
            newLeafy.GetComponent<CarriableBehavior>().searchRadius = LEAFY_SEARCH_RADIUS;
            leafies.Add(newLeafy);
        }
    }

    // Start is called before the first frame update
    void Start() {
        GroundSetup();
        AntSetup();
        PlantSetup();
    }
}
