using UnityEngine;

public abstract class CurveComponent : MonoBehaviour, ICurve {

	protected abstract Curve InstantinateCurve();

	public Curve curve;

	void Awake() {
		curve = InstantinateCurve();
	}

	public float Length { get { return curve.Length; } }

	public Vector3 GetPosition(float u) {
		return transform.localToWorldMatrix.MultiplyPoint(curve.GetPosition(u));
	}

	public Vector3 GetForward(float u) {
		return transform.localToWorldMatrix.MultiplyVector(curve.GetForward(u));
	}

	public Vector3 GetNormal(float u, Vector3 up) {
		return transform.localToWorldMatrix.MultiplyVector(
			curve.GetNormal(u, transform.worldToLocalMatrix.MultiplyVector(up)));
	}

	public Vector3 GetNormal(float u) {
		return GetNormal(u, Curve.DefaultUpVector);
	}
}
