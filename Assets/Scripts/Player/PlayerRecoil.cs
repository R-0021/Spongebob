using System.Collections;
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    public float recoilTime = 0.3f;
    public float recoilForce = 1.0f;

    public AnimationCurve xCurve;
    public float xScale = 0f;
    public AnimationCurve yCurve;
    public float yScale = 0f;
	public AnimationCurve zCurve;
	public float zScale = 0f;

	Vector3 firstPos;

	private void Start()
	{
		firstPos = transform.localPosition;
	}

	public void Apply()
    {
        StartCoroutine(ApplyRecoil());
    }

    public void SetForce(float force) => recoilForce = force;
    public void SetTime(float time) => recoilTime = time;


	IEnumerator ApplyRecoil()
    {
        float time = 0f;

        float x, y, z;
        float percent;

        while(time < recoilTime)
        {
            percent = time / recoilTime;

			time += Time.deltaTime;
            x = xCurve.Evaluate(percent) * xScale * recoilForce;
            y = yCurve.Evaluate(percent) * yScale * recoilForce;
            z = zCurve.Evaluate(percent) * zScale * recoilForce;

			transform.localPosition = new Vector3(x, y, z);
            yield return null;
        }

        transform.localPosition = firstPos;
    }
}
