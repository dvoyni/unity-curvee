using UnityEngine;

[AddComponentMenu("Curvee/Bezier3")]
public class Bezier3CurveComponent : SegmentedCurveComponent {

	public override int AvailablePointsCount { get { return Bezier3Curve.PointsCount; } }

	protected override Curve InstantinateCurve() {
		var curve = new Bezier3Curve(initialPoints);
		curve.SegmentsCount = initialSegmentsCount;
		return curve;
	}
}
