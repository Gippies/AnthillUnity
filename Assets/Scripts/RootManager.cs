using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour {

    private static readonly int INITIAL_AMOUNT_OF_GATHERERS = 42;
    private static readonly int INITIAL_AMOUNT_OF_LEAVES = 100;

    public GameObject gathererPrefab;
    public GameObject leafyPrefab;

    private List<GameObject> leafies;

    public List<GameObject> GetLeafies() {
        return leafies;
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
        AntSetup();
        PlantSetup();
    }
}
