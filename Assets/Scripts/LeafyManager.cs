using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafyManager : MonoBehaviour {

    // Note that 42 was chosen because its square root is 7
    private static readonly int INITIAL_AMOUNT_OF_LEAVES = 100;

    public GameObject leafyPrefab;
    public static List<GameObject> leafies;

    // Start is called before the first frame update
    void Start() {
        leafies = new List<GameObject>();
        GameObject newLeafy;
        for (int i = 0; i < INITIAL_AMOUNT_OF_LEAVES; i++) {
            newLeafy = Instantiate(leafyPrefab, new Vector3(Random.Range(-20.0f, 20.0f), 0.25f / 2.0f, Random.Range(-20.0f, 20.0f)), Quaternion.identity);
            newLeafy.transform.parent = this.transform;
            leafies.Add(newLeafy);
        }
    }

}
