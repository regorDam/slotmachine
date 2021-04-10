using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
	public float scrollSpeed;
	public float tileSizeZ;

	[SerializeField]
	float maxY = 6;
	[SerializeField]
	float minY = -6;
	private Vector3 startPosition;

	public bool isStopped = true;

	int stoppedValue;

	public int GetResult() { return stoppedValue; }

	Dictionary<int, float> symbols;

	// Use this for initialization
	void Start()
	{
		startPosition = transform.position;
		scrollSpeed = 0;

		symbols = new Dictionary<int, float>();

		symbols.Add(1, 0.61f);
		symbols.Add(2, 4.86f);
		symbols.Add(3, 3.44f);
		symbols.Add(4, 2.02f);
	}

	// Update is called once per frame
	void Update()
	{
		if (isStopped) return;
		
		float newPosition = Time.deltaTime * scrollSpeed * 5;
		transform.position = transform.position + Vector3.up * newPosition;

		transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f);

		if(transform.position.y < minY)
        {
			transform.position = new Vector3(startPosition.x, maxY, startPosition.z);
        }
	}


	public void Stop()
    {
		isStopped = true;

		int randomKey = Random.Range(1, 5);
		stoppedValue = randomKey;
		float value;
		symbols.TryGetValue(randomKey, out value);
		transform.localPosition = new Vector3(transform.localPosition.x, value);
	}

	public void Run() 
	{
		isStopped = false;
	}

}
