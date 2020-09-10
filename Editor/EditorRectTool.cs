using UnityEditor;
using UnityEngine;

public class EditorRectTool : MonoBehaviour
{
	const string _shortcuts_modifier = "%"; //CTRL

	[MenuItem(nameof(EditorRectTool)+"/Anchors to Corners "+ _shortcuts_modifier + "[")]
	static void AnchorsToCorners()
	{
		foreach (var transform in Selection.transforms)
		{
			var rect_transform = transform as RectTransform;

			Undo.RecordObject(rect_transform, $"{rect_transform.name}: {nameof(EditorRectTool)}. Anchors to Corners");

			var parent = Selection.activeTransform.parent as RectTransform;

			if (!rect_transform || !parent) return;

			var anchors_min_offset = new Vector2(rect_transform.offsetMin.x / parent.rect.width, rect_transform.offsetMin.y / parent.rect.height);
			var anchors_max_offset = new Vector2(rect_transform.offsetMax.x / parent.rect.width, rect_transform.offsetMax.y / parent.rect.height);

			var newAnchorsMin = rect_transform.anchorMin + anchors_min_offset;
			var newAnchorsMax = rect_transform.anchorMax + anchors_max_offset;

			rect_transform.anchorMin = newAnchorsMin;
			rect_transform.anchorMax = newAnchorsMax;
			rect_transform.offsetMin = rect_transform.offsetMax = Vector2.zero;

			EditorUtility.SetDirty(rect_transform);
		}
	}

	[MenuItem(nameof(EditorRectTool)+"/Corners to Anchors "+ _shortcuts_modifier + "]")]
	static void CornersToAnchors()
	{
		foreach (var transform in Selection.transforms)
		{
			var rect_transform = transform as RectTransform;
			Undo.RecordObject(rect_transform, $"{rect_transform.name}: {nameof(EditorRectTool)}. Corners to Anchors");

			if (!rect_transform) return;

			rect_transform.offsetMin = rect_transform.offsetMax = Vector2.zero;
			EditorUtility.SetDirty(rect_transform);

		}
	}

	[MenuItem(nameof(EditorRectTool)+"/Mirror Horizontally Around Anchors "+ _shortcuts_modifier + ";")]
	static void MirrorHorizontallyAnchors()
	{
		MirrorHorizontally(false);
	}

	[MenuItem(nameof(EditorRectTool)+"/Mirror Horizontally Around Parent Center "+ _shortcuts_modifier + ":")]
	static void MirrorHorizontallyParent()
	{
		MirrorHorizontally(true);
	}

	static void MirrorHorizontally(bool mirrorAnchors)
	{
		foreach (var transform in Selection.transforms)
		{
			var rect_transform = transform as RectTransform;
			var parent = Selection.activeTransform.parent as RectTransform;

			if (!rect_transform || !parent) return;


			Undo.RecordObject(rect_transform, $"{rect_transform.name}: {nameof(EditorRectTool)}. Horizontal Mirror {(mirrorAnchors ? "Anchors" : "Around Parent")}");

			if (mirrorAnchors)
			{
				var temp_anchor_min = rect_transform.anchorMin;
				rect_transform.anchorMin = new Vector2(1 - rect_transform.anchorMax.x, rect_transform.anchorMin.y);
				rect_transform.anchorMax = new Vector2(1 - temp_anchor_min.x, rect_transform.anchorMax.y);
			}

			var temp_offset_min = rect_transform.offsetMin;
			rect_transform.offsetMin = new Vector2(-rect_transform.offsetMax.x, rect_transform.offsetMin.y);
			rect_transform.offsetMax = new Vector2(-temp_offset_min.x, rect_transform.offsetMax.y);

			rect_transform.localScale = new Vector3(-rect_transform.localScale.x, rect_transform.localScale.y, rect_transform.localScale.z);

			EditorUtility.SetDirty(rect_transform);
		}
	}

	[MenuItem(nameof(EditorRectTool)+"/Mirror Vertically Around Anchors "+ _shortcuts_modifier + "'")]
	static void MirrorVerticallyAnchors()
	{
		MirrorVertically(false);
	}

	[MenuItem(nameof(EditorRectTool)+"/Mirror Vertically Around Parent Center "+ _shortcuts_modifier + "\"")]
	static void MirrorVerticallyParent()
	{
		MirrorVertically(true);
	}

	static void MirrorVertically(bool mirrorAnchors)
	{
		foreach (var transform in Selection.transforms)
		{
			var rect_transform = transform as RectTransform;
			var parent = Selection.activeTransform.parent as RectTransform;

			if (!rect_transform || !parent) return;

			Undo.RecordObject(rect_transform, $"{rect_transform.name}: {nameof(EditorRectTool)}. Vertical Mirror {(mirrorAnchors?"Anchors":"Around Parent")}");

			if (mirrorAnchors)
			{
				var temp_anchor_min = rect_transform.anchorMin;
				rect_transform.anchorMin = new Vector2(rect_transform.anchorMin.x, 1 - rect_transform.anchorMax.y);
				rect_transform.anchorMax = new Vector2(rect_transform.anchorMax.x, 1 - temp_anchor_min.y);
			}

			var temp_offset_min = rect_transform.offsetMin;
			rect_transform.offsetMin = new Vector2(rect_transform.offsetMin.x, -rect_transform.offsetMax.y);
			rect_transform.offsetMax = new Vector2(rect_transform.offsetMax.x, -temp_offset_min.y);

			rect_transform.localScale = new Vector3(rect_transform.localScale.x, -rect_transform.localScale.y, rect_transform.localScale.z);

			EditorUtility.SetDirty(rect_transform);
		}
	}
}
