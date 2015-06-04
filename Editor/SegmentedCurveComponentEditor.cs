using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SegmentedCurveComponentEditor : Editor {

	private HashSet<int> _toggledAnchors;

	override public void OnInspectorGUI() {
		var curve = target as SegmentedCurveComponent;

		if (curve.initialPoints == null) {
			curve.initialPoints = new List<Vector3>();
		}

		if (curve.anchorPoints == null) {
			curve.anchorPoints = new List<Transform>();
		}

		while (curve.initialPoints.Count < curve.AvailablePointsCount) {
			curve.initialPoints.Add(Random.onUnitSphere * 10);
		}

		while (curve.anchorPoints.Count < curve.AvailablePointsCount) {
			curve.anchorPoints.Add(null);
		}

		if (_toggledAnchors == null) {
			_toggledAnchors = new HashSet<int>();
			for (int i = 0; i < curve.anchorPoints.Count; i++) {
				if (curve.anchorPoints[i] != null) {
					_toggledAnchors.Add(i);
				}
			}
		}

		for (var i = 0; i < curve.AvailablePointsCount; i++) {
			string label;
			if (i == 0) {
				label = "Begin Point";
			}
			else if (i == curve.AvailablePointsCount - 1) {
				label = "End Point";
			}
			else if (curve.AvailablePointsCount == 3) {
				label = "Control Point";
			}
			else {
				label = "Control Point " + i;
			}

			var toggleAnchor = EditorGUILayout.ToggleLeft(label, _toggledAnchors.Contains(i));
			if (toggleAnchor) {
				_toggledAnchors.Add(i);
				curve.anchorPoints[i] = EditorGUILayout.ObjectField(
					curve.anchorPoints[i], typeof(Transform), true) as Transform;
				if (curve.anchorPoints[i] != null) {
					curve.initialPoints[i] = curve.transform.worldToLocalMatrix.MultiplyPoint(
						curve.anchorPoints[i].position);
				}
			}
			else {
				_toggledAnchors.Remove(i);
				curve.anchorPoints[i] = null;
				curve.initialPoints[i] = EditorGUILayout.Vector3Field(label, curve.initialPoints[i]);
			}
		}

		GUILayout.Space(15);

		curve.initialSegmentsCount = EditorGUILayout.IntField("Segments Count", curve.initialSegmentsCount);
		curve.color = EditorGUILayout.ColorField("Editor color", curve.color);

		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
}

[CustomEditor(typeof(Bezier2CurveComponent))]
public class Bezier2CurveComponentEditor : SegmentedCurveComponentEditor {
}

[CustomEditor(typeof(Bezier3CurveComponent))]
public class Bezier3CurveComponentEditor : SegmentedCurveComponentEditor {
}