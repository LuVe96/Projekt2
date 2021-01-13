using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileOnCollisionHandler : MonoBehaviour
{

    public GameObject particlesOnCollision;
    public bool stayAtCollisionPosition = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Arrow")
        {
            if( particlesOnCollision != null)
            {
                Instantiate(particlesOnCollision, transform.position, new Quaternion(0, 0, 0, 0));
                Destroy(gameObject);
            }  

        }
        //else if (collision.gameObject.tag == "Arrow" || collision.gameObject.tag == "EnemyProjectile")
        //{
        //    Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());  // ignore collision with arrow 

        //}

        if (stayAtCollisionPosition)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<ProjectileHandler>().StopShoot();
            transform.position = collision.GetContact(0).point;
            transform.localPosition += transform.forward * -0.4f; // new Vector3(0, 0, -0.4f);
            transform.SetParent(collision.transform);
        }

    }
}
