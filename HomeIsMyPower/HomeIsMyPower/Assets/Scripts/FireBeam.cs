using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBeam : MonoBehaviour {

    public GameObject bulletObj;
    public List<GameObject> bulletList;
    public List<float> destroyTimes;
    public float bulletDestroyDelay = 2.0f;
    public int bulletSpeed;
    private float currentTime;
    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        // set current time
        currentTime = Time.time;
        // check if the use presses the f key, then fire bullet
        if (Input.GetKeyDown(KeyCode.F)) {
            CreateBullet();
            bulletList[bulletList.Count - 1].transform.forward = GameObject.FindGameObjectWithTag("Player").transform.forward;
        }
        // check if there are any bullets in the list
        if (bulletList.Count > 0) {
            for (int i = 0; i < bulletList.Count; i++) {
                MoveBullet(i);
                
            }
        }

        DestroyBullets();
	}

    void CreateBullet() {

        bulletList.Add(Object.Instantiate(bulletObj, gameObject.transform.GetChild(3).gameObject.transform.position, Quaternion.identity));
        Debug.Log(gameObject.transform.GetChild(3).gameObject.transform.position);
        // bulletList[bulletList.Count].transform.forward = GameObject.FindGameObjectWithTag("Player").transform.forward;
        destroyTimes.Add(Time.time);
    }

    void MoveBullet(int bulletToMove) {

        // bulletList[bulletToMove].transform.forward = GameObject.FindGameObjectWithTag("Player").transform.forward;
        bulletList[bulletToMove].transform.Translate(Vector3.forward * (Time.deltaTime * bulletSpeed));
        
    }

    void DestroyBullets() {

        for (int i = 0; i < bulletList.Count; i++) {
            if (currentTime - destroyTimes[i] >= bulletDestroyDelay){
                Destroy(bulletList[i]);
                bulletList.Remove(bulletList[i]);

                destroyTimes.Remove(destroyTimes[i]);
            }
        }
        
    }
}
