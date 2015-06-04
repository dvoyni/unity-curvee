using UnityEngine;
using System.Collections.Generic;

public class Bezier2Curve : SegmentedCurve {

	public static readonly int PointsCount = 3;

	public Bezier2Curve(IEnumerable<Vector3> points):
		base(points) {

	}

	public Bezier2Curve(Vector3 begin, Vector3 controlPoint, Vector3 end):
		this(new Vector3[] { begin, controlPoint, end }) {
	}

	public override int AvailablePointsCount { get { return PointsCount; } }

	protected override Vector3 GetCurvePosition(float u) {
		var begin = GetPoint(0);
		var controlPoint = GetPoint(1);
		var end = GetPoint(2);

		return begin * (1 - u) * (1 - u) + controlPoint * (1 - u) * u * 2 + end * u * u;
	}

	public Vector3 Begin {
		get {
			return GetPoint(0);
		}
		set {
			SetPoint(0, value);
		}
	}

	public Vector3 ControlPoint {
		get {
			return GetPoint(1);
		}
		set {
			SetPoint(1, value);
		}
	}

	public Vector3 End {
		get {
			return GetPoint(2);
		}
		set {
			SetPoint(2, value);
		}
	}
}
