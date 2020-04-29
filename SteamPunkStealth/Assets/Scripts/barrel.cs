using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrel : MonoBehaviour
{

    public bool lit;

    public GameObject fuse;

    public GameObject explosion;
    void Start()
    {

    }


    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerSword")
        {
            if (lit != true)
            {
                lit = true;
                StartCoroutine(Execute());
            }
        }

    }

    IEnumerator Execute()
    {
        fuse.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);

    }

}