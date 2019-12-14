using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public GameObject[] layers;
    private Camera mainCamera;
    private Vector2 cameraBoundary;
    public float choke;

    private void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        cameraBoundary = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach(GameObject obj in layers)
        {
            LoadChildren(obj);
        }
    }

    void LoadChildren(GameObject obj)
    {
        //Getting the width of the current background sprite
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;

        //Finding how many clones are needed to fill the screen
        int childNeeded = (int)Mathf.Ceil(cameraBoundary.x * 2 / objectWidth);

        //Create clones
        GameObject clone = Instantiate(obj) as GameObject;
        for(int i = 0; i <= childNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);

            //Keeping it clean
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    //Bring the child leaving the edge of the screen to the other end of the screen
    void RepositionChildren(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;

            //Moving the children
            if (transform.position.x + cameraBoundary.x > lastChild.transform.position.x + halfWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }

            //In the rare case that the screen goes left
            else if(transform.position.x - cameraBoundary.x < firstChild.transform.position.x - halfWidth)
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfWidth * 2, firstChild.transform.position.y, lastChild.transform.position.z);
            }

        }
    }

    void LateUpdate()
    {
        foreach(GameObject obj in layers)
        {
            RepositionChildren(obj);
        }
    }
}
