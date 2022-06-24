using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
[RequireComponent(typeof(LineRenderer))]
public class RaycastReflection : MonoBehaviour
{
	public int reflections;
	public float maxLength;
	public float nextPuzzleWaitTime;
	public bool isLastPuzzle;

	public VideoPlayer video;
	public RawImage videoTexture;
	public GameObject circuitBoard;
	public GameObject nextPuzzleCam;
	[SerializeField] private LayerMask whatIsMirror;

	private LineRenderer lineRenderer;
	private Ray2D ray;
	private RaycastHit2D hit;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		ray = new Ray2D(transform.position, Vector2.right);

		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, transform.position);
		float remainingLength = maxLength;

		for (int i = 0; i < reflections; i++)
		{
			hit = Physics2D.Raycast(ray.origin, ray.direction, remainingLength, whatIsMirror);
			if (hit)
			{
				lineRenderer.positionCount += 1;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
				remainingLength -= Vector2.Distance(ray.origin, hit.point);
				ray = new Ray2D(hit.point - ray.direction * 0.01f, Vector2.Reflect(ray.direction, hit.normal));
			
				if(hit.collider.GetComponent<DoorTrigger>())
                {
					hit.collider.GetComponent<DoorTrigger>().OnTriggeredDoor();
					circuitBoard.GetComponent<Animator>().enabled = true;

					ActivateNextPuzzle();
				}
			}
			else
			{
				lineRenderer.positionCount += 1;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
			}
		}
	}
	
	public void ActivateNextPuzzle()
    {

		if (!isLastPuzzle)
		{
			nextPuzzleWaitTime -= Time.deltaTime;
			if (nextPuzzleWaitTime <= 0f)
				nextPuzzleCam.SetActive(true);
		}
		else
        {
			Invoke(nameof(PlayVideo), 2);
        }
		
		//Invoke(nameof(DeactivateCurrentPuzzle), 3);
    }

	public void PlayVideo()
    {
		video.enabled = true;
		video.Play();
		videoTexture.enabled = true;

    }

	public void DeactivateCurrentPuzzle()
    {
		transform.parent.gameObject.SetActive(false);
    }
}