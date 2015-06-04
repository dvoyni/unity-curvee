using UnityEngine;
using System.Collections.Generic;

public class Bezier3Curve : SegmentedCurve {

	public static readonly int PointsCount = 4;

	public Bezier3Curve(IEnumerable<Vector3> points):
		base(points) {
	}

	public Bezier3Curve(Vector3 begin, Vector3 controlPointA, Vector3 controlPointB, Vector3 end):
		this(new Vector3[] { begin, controlPointA, controlPointB, end }) {
	}

	public override int AvailablePointsCount { get { return PointsCount; } }

	protected override Vector3 GetCurvePosition(float u) {
		var begin = GetPoint(0);
		var controlPointA = GetPoint(1);
		var controlPointB = GetPoint(2);
		var end = GetPoint(3);

		return begin * (1 - u) * (1 - u) * (1 - u) + 
				controlPointA * (1 - u) * (1 - u) * u * 3 +
				controlPointB * (1 - u) * u * u * 3 +
				end * u * u * u;
	}

	public Vector3 Begin {
		get {
			return GetPoint(0);
		}
		set {
			SetPoint(0, value);
		}
	}

	public Vector3 ControlPointA {
		get {
			return GetPoint(1);
		}
		set {
			SetPoint(1, value);
		}
	}

	public Vector3 ControlPointB {
		get {
			return GetPoint(2);
		}
		set {
			SetPoint(2, value);
		}
	}

	public Vector3 End {
		get {
			return GetPoint(3);
		}
		set {
			SetPoint(3, value);
		}
	}
}
