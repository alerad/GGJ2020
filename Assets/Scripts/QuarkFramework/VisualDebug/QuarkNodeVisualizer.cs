using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace QuarkFramework.Debugging {
    //Not very performant. Needs to implement polling
    public class QuarkNodeVisualizer : MonoBehaviour {

        public GameObject customSpherePrefab;
        public int limit = -1;
        LinkedList<GameObject> allNodes;
        Transform nodesParent;
        // Start is called before the first frame update
        void Start() {
            nodesParent = new GameObject(this.name + "QuarkVisualNodes").transform;
            allNodes = new LinkedList<GameObject>();
            GetComponent<NodeCreatedSystem>().nodeCreatedStream.Subscribe(NewNodeCreated);
        }

        public void NewNodeCreated(QuarkNode newNode) {
            if (newNode != null)
                InstantiateNewNode(newNode.Position);
        }

        public void ClearAll() {
            if (allNodes != null && allNodes.Count > 0) {
                foreach (GameObject g in allNodes) {
                    Destroy(g, 0.1f);
                }
            }
        }

        void InstantiateNewNode(Vector3 pos) {
            if (limit > 1 && allNodes.Count >= limit) {
                Destroy(allNodes.First.Value);
                allNodes.RemoveFirst();
            }
            if (customSpherePrefab != null) {
                allNodes.AddLast(Instantiate(customSpherePrefab, pos, Quaternion.identity, nodesParent));
            } else {
                GameObject aux = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                aux.transform.position = pos;
                aux.transform.parent = nodesParent;
                aux.transform.localScale = Vector3.one * 0.065f;
                allNodes.AddLast(aux);
            }

        }
    }
}
