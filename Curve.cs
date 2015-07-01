using UnityEngine;

public abstract class Curve : ICurve {
	protected abstract Vector3 GetPositionImpl(float u);

	protected abstract float LengthImpl { get; }

	public static readonly Vector3 DefaultUpVector = Vector3.up;

	protected bool Invalid { get; private set; }

	private float _length;

	public float Length { get {
			Validate();
			return _length;
		}
	}

	protected void Invalidate() {
		Invalid = true;
	}

	protected void Validate() {
		if (Invalid) {
			Update();
			Invalid = false;
		}
	}

	protected virtual void Update() {
		_length = LengthImpl;
	}

	public Vector3 GetPosition(float u) {
		Validate();
		return GetPositionImpl(u);
	}

	public Vector3 GetForward(float u) {
		u = Mathf.Clamp01(u);
		const float epsilon = 0.01f;
		Validate();
		return (GetPosition(Mathf.Clamp01(u + epsilon)) - 
			GetPosition(Mathf.Clamp01(u - epsilon))).normalized;
	}

	public Vector3 GetNormal(float u, Vector3 up) {
		return Vector3.Cross(GetForward(u), up);
	}

	public Vector3 GetNormal(float u) {
		return GetNormal(u, DefaultUpVector);
	}
}
