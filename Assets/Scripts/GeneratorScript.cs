using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour {
    public GameObject[] availableMountains;
    public List<GameObject> currentMountains;
    private float screenWidthInPoints;

    public GameObject[] availableObjects;
    public List<GameObject> objects;

    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    public float objectsMinRotation = -45.0f;
    public float objectsMaxRotation = 45.0f;

    void AddObject(float lastObjectX)
    {
        int randomIndex = Random.Range(0, availableObjects.Length);
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);

        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);

        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

        objects.Add(obj);
    }

    void GenerateObjectsIfRequired()
    {
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in objects)
        {
            float objX = obj.transform.position.x;

            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            if (objX < removeObjectsX)
                objectsToRemove.Add(obj);
        }

        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        if (farthestObjectX < addObjectX)
            AddObject(farthestObjectX);
    }

	// Use this for initialization
	void Start () {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AddMountains(float farthestMountainEndX)
    {
        int randomIndex = Random.Range(0, availableMountains.Length);
        GameObject mt = (GameObject)Instantiate(availableMountains[randomIndex]);
        float mtWidth = mt.transform.FindChild("Floor").localScale.x;
        float mtCenter = farthestMountainEndX + mtWidth * 0.275f;
        mt.transform.position = new Vector3(mtCenter, 0, 0);
        currentMountains.Add(mt);
    }

    void GenerateMountainIfRequired()
    {
        List<GameObject> mountainsToRemove = new List<GameObject>();
        bool addMountains = true;
        float playerX = transform.position.x;
        float removeMountainX = playerX - screenWidthInPoints;
        float addMountainX = playerX + screenWidthInPoints;
        float farthestMountainEndX = 0;

        foreach (var mt in currentMountains)
        {
            float mtWidth = mt.transform.FindChild("Floor").localScale.x;
            float mtStartX = mt.transform.position.x - (mtWidth * 0.5f);
            float mtEndX = mtStartX + mtWidth;

            if (mtStartX > addMountainX)
                addMountains = false;

            if (mtEndX < removeMountainX)
                mountainsToRemove.Add(mt);

            farthestMountainEndX = Mathf.Max(farthestMountainEndX, mtEndX);
        }

        foreach (var mt in mountainsToRemove)
        {
            currentMountains.Remove(mt);
            Destroy(mt);
        }

        if (addMountains)
            AddMountains(farthestMountainEndX);
    }

    void FixedUpdate()
    {
        GenerateMountainIfRequired();

        GenerateObjectsIfRequired();
    }
}
