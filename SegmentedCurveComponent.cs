using UnityEngine;
using System.Collections.Generic;

public abstract class SegmentedCurveComponent : CurveComponent, ISegmentedCurve {

	public Color32 color = Color.white;
	public List<Vector3> initialPoints;
	public List<Transform> anchorPoints;
	public int initialSegmentsCount = SegmentedCurve.DefaultSegmentsCount;

	private SegmentedCurve Curve { get { return curve as SegmentedCurve; } }

	public abstract int AvailablePointsCount { get; }

	public int SegmentsCount { 
		get { 
			return Curve.SegmentsCount; 
		}
		set {
			Curve.SegmentsCount = value;
		}
	}

	public Vector3 GetPoint(int index) {
		return Curve.GetPoint(index);
	}
	
	public void SetPoint(int index, Vector3 point) {
		Curve.SetPoint(index, point);
	}
	
	public IEnumerable<Vector3> GetPoints() {
		return Curve.GetPoints();
	}
	
	public void SetPoints(IEnumerable<Vector3> points) {
		Curve.SetPoints(points);
	}

	private void UpdateAnchors(SegmentedCurve curve) {
		
        for(var i = 0; i < anchorPoints.Count; i++) {
            var anchorPoint = anchorPoints[i];
			if (anchorPoint != null) {
				if (anchorPoint.position != curve.GetPoint(i)) {
					curve.SetPoint(i, transform.worldToLocalMatrix.MultiplyPoint(
						anchorPoint.position));
				}
			}
		}
	}

	void Update() {
		UpdateAnchors(Curve);
	}

	void OnDrawGizmos() {
		var curve = InstantinateCurve() as SegmentedCurve;
		UpdateAnchors(curve);
		var step = 1f / curve.SegmentsCount;
		var mx = transform.localToWorldMatrix;
		var prevPoint = mx.MultiplyPoint(curve.GetPosition(0));

		Gizmos.color = color;
		for (var i = 1; i <= curve.SegmentsCount; i++) {
			var point = mx.MultiplyPoint(curve.GetPosition(step * i));
			Gizmos.DrawLine(prevPoint, point);
			prevPoint = point;
		}

		var handleColor = color;
		handleColor.a /= 2;

		var lineColor = color;
		lineColor.a /= 4;

		var size = curve.Length * .002f;

		for (var i = 0; i < curve.AvailablePointsCount; i++) {
			if (i > 0) {
				Gizmos.color = lineColor;
				Gizmos.DrawLine(mx.MultiplyPoint(curve.GetPoint(i - 1)),
				                mx.MultiplyPoint(curve.GetPoint(i)));
			}

			Gizmos.color = handleColor;
			Gizmos.DrawWireSphere(mx.MultiplyPoint(curve.GetPoint(i)), size);
		}
	}
}
