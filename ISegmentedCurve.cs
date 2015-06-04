using UnityEngine;
using System.Collections.Generic;

public interface ISegmentedCurve : ICurve {
	int AvailablePointsCount { get; }
	int SegmentsCount { get; set;}
	Vector3 GetPoint(int index);
	void SetPoint(int index, Vector3 point);
	IEnumerable<Vector3> GetPoints();
	void SetPoints(IEnumerable<Vector3> points);
}
