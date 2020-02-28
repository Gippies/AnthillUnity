using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour {

    private static readonly int INITIAL_AMOUNT_OF_GATHERERS = 42;
    private static readonly int INITIAL_AMOUNT_OF_LEAVES = 100;
    private static readonly int GROUND_WIDTH = 100;
    private static readonly int GROUND_LENGTH = 100;

    public GameObject gathererPrefab;
    public GameObject leafyPrefab;
    public GameObject groundPrefab;

    private List<GameObject> leafies;

    public List<GameObject> GetLeafies() {
        return leafies;
    }

    private void GroundSetup() {
        for (int i = 0; i < GROUND_WIDTH; i++) {
            for (int j = 0; j < GROUND_LENGTH; j++) {
                Instantiate(groundPrefab, new Vector3(i - GROUND_WIDTH / 2, (float) -0.5, j - GROUND_LENGTH / 2), Quaternion.identity);
            }
        }
    }

    private void AntSetup() {
        GameObject newGatherer;
        int oneSide = (int)Mathf.Sqrt(INITIAL_AMOUNT_OF_GATHERERS);
        for (int i = 0; i < oneSide; i++) {
            for (int j = 0; j < oneSide; j++) {
                newGatherer = Instantiate(gathererPrefab, new Vector3(i - oneSide / 2, 1, j - oneSide / 2), Quaternion.identity);
                newGatherer.GetComponent<AntBehavior>().rootManager = this;
            }
        }
    }

    private void PlantSetup() {
        leafies = new List<GameObject>();
        GameObject newLeafy;
        for (int i = 0; i < INITIAL_AMOUNT_OF_LEAVES; i++) {
            newLeafy = Instantiate(leafyPrefab, new Vector3(Random.Range(-20.0f, 20.0f), 0.25f / 2.0f, Random.Range(-20.0f, 20.0f)), Quaternion.identity);
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
