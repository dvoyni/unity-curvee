using UnityEngine;

[AddComponentMenu("Curvee/Bezier2")]
public class Bezier2CurveComponent : SegmentedCurveComponent {

	public override int AvailablePointsCount { get { return Bezier2Curve.PointsCount; } }

	protected override Curve InstantinateCurve() {
		var curve = new Bezier2Curve(initialPoints);
		curve.SegmentsCount = initialSegmentsCount;
		return curve;
	}
}
