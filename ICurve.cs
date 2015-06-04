using UnityEngine;

public interface ICurve {
	float Length { get; }
	Vector3 GetPosition(float u);	
	Vector3 GetForward(float u);
	Vector3 GetNormal(float u, Vector3 up);
	Vector3 GetNormal(float u);
}