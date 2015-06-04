using UnityEngine;
using System.Collections.Generic;

public abstract class SegmentedCurve : Curve, ISegmentedCurve {
	public abstract int AvailablePointsCount { get; }
	protected abstract Vector3 GetCurvePosition(float u);

	public static readonly int DefaultSegmentsCount = 25;

	private class Segment {
		public Vector3 start;
		public Vector3 end;
		public float magnitude;
		public float length;
		public float offset;
	}
	
	private Vector3[] _points;
	private IList<Segment> _segments;
	private int _segmentsCount = DefaultSegmentsCount;
	private float _length;
	private float _stepLength;

	public int SegmentsCount { 
		get { 
			Validate();
			return _segmentsCount; 
		}
		set {
			_segmentsCount = value;
			Invalidate();
		}
	}

	override protected float LengthImpl { get { return _length; } }

	public SegmentedCurve(IEnumerable<Vector3> points) {
		_points = new Vector3[AvailablePointsCount];
		SetPoints(points);
	}

	public Vector3 GetPoint(int index) {
		return _points[index];
	}

	public void SetPoint(int index, Vector3 point) {
		_points[index] = point;
		Invalidate();
	}

	public IEnumerable<Vector3> GetPoints() {
		return _points;
	}

	public void SetPoints(IEnumerable<Vector3> points) {
		var count = 0;
		foreach(var point in points) {
			_points[count++] = point;
		}		
		Invalidate();
	}

	private void Rasterize() {
		if ((_segments == null) || (_segments.Count != _segmentsCount + 1)) {
			_segments = new List<Segment>();
			for (var i = 0; i < _segmentsCount + 1; i++) {
				_segments.Add(new Segment());
			}
		}

		_stepLength = 1f / (_segmentsCount+1);
		var position = _stepLength;
		var prevPoint = _points[0];
		_length = 0f;

		foreach(var segment in _segments) {
			var point = GetCurvePosition(position);

			segment.start = prevPoint;
			segment.end = point;
			segment.magnitude = (point - prevPoint).magnitude;
			segment.length = segment.magnitude;
			segment.offset = _length;

			_length += segment.length;
			position += _stepLength;
			prevPoint = point;
		}

		foreach(var segment in _segments) {
			segment.offset /= _length;
			segment.length /= _length;
		}
	}

	protected override void Update() {
		Rasterize();
		base.Update();
	}

	protected override Vector3 GetPositionImpl(float u) {
		var index = Mathf.Clamp(Mathf.RoundToInt(u * SegmentsCount), 0, SegmentsCount);
		var segment = _segments[index];

		while (segment.offset > u) {
			if (--index == -1) {
				break;
			}
			segment = _segments[index];
		}

		while (segment.offset + segment.length < u) {
			if (++index == _segments.Count) {
				break;
			}
			segment = _segments[index];
		}

		var t = segment.length == 0 ? 0 : (u - segment.offset) / segment.length;
		var position = Vector3.Lerp(segment.start, segment.end, t);
		if (float.IsNaN(position.x)) {
			Debug.DebugBreak();
		}
		return position;
	}
}
