using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(LineRenderer))]
public class Beam: MonoBehaviour
{
	public int reflections;
	public float maxLength;

	public GameObject circuitBoard;
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

				if (hit.collider.GetComponent<DoorTrigger>())
				{
					hit.collider.GetComponent<DoorTrigger>().OnTriggeredDoor();
					circuitBoard.GetComponent<Animator>().enabled = true;

				}
			}
			else
			{
				lineRenderer.positionCount += 1;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
			}
		}
	}

}
