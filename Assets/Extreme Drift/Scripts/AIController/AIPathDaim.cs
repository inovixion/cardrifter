using UnityEngine;
using System.Collections;

public class AIPathDaim : MonoBehaviour
{

    public Color pathColor = new Color(1, 0.5f, 0);

    void OnDrawGizmos()
    {
        Gizmos.color = pathColor;

        int count = 1;

        foreach (Transform node in transform)
        {
            node.GetComponent<AINodeDaim>().GizmosColor = pathColor;

            if (!node.GetComponent<AINodeDaim>())
                node.gameObject.AddComponent<AINodeDaim>();

            if (node.name != count.ToString())
                node.name = count.ToString();


            Transform NextNode = transform.Find((count + 1).ToString());
            Transform PreviousNode = transform.Find((count - 1).ToString());

            if (NextNode)
            {
                Gizmos.DrawLine(node.position, NextNode.position);
                node.GetComponent<AINodeDaim>().nextNode = NextNode;
            }

            if (PreviousNode)
                node.GetComponent<AINodeDaim>().previousNode = PreviousNode;

            count++;
        }
    }
}
