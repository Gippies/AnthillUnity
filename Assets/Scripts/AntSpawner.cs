using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour {
    
    // Note that 42 was chosen because its square root is 7
    private static readonly int INITIAL_AMOUNT_OF_GATHERERS = 42;

    public GameObject gathererPrefab;

    // Start is called before the first frame update
    void Start() {
        GameObject newGatherer;
        int oneSide = (int) Mathf.Sqrt(INITIAL_AMOUNT_OF_GATHERERS);
        for (int i = 0; i < oneSide; i++) {
            for (int j = 0; j < oneSide; j++) {
                newGatherer = Instantiate(gathererPrefab, new Vector3(i - oneSide / 2, 1, j - oneSide / 2), Quaternion.identity);
                newGatherer.transform.parent = this.transform;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
