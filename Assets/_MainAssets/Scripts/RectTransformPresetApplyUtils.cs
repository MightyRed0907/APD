using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: CG-Tespy
/// </summary>

namespace CGTespy.UI
{
	public static class RectTransformPresetApplyUtils 
	{
		#region For converting text anchors to Vector2s
		static float upper = 							1f;
		static float middle = 							0.5f;
		static float lower = 							0f;

		static float right = 							1f;
		static float center = 							0.5f;
		static float left = 							0f;

		public static Vector2 ToViewportCoords(this TextAnchor textAnchor)
		{

			switch (textAnchor)
				{
					case TextAnchor.UpperLeft:
						return new Vector2(left, upper);
					case TextAnchor.UpperCenter:
						return new Vector2(center, upper);
					case TextAnchor.UpperRight:
						return new Vector2(right, upper);

					case TextAnchor.MiddleLeft:
						return new Vector2(left, middle);
					case TextAnchor.MiddleCenter:
						return new Vector2(center, middle);
					case TextAnchor.MiddleRight:
						return new Vector2(right, middle);

					case TextAnchor.LowerLeft:
						return new Vector2(left, lower);
					case TextAnchor.LowerCenter:
						return new Vector2(center, lower);
					case TextAnchor.LowerRight:
						return new Vector2(right, lower);

					default:
						throw new System.NotImplementedException("Didn't account for " + textAnchor.ToString());

				}
		}


		#endregion
		
		#region Various utility funcs that help with applying the preset.

		#region Pivot and anchor-setting functions.

		/// <summary>
		/// Sets the pivot without changing the rect trans' physical position in the scene.
		/// Credit to jmorhart at Unity Answers for this function.
		/// </summary>
		public static void SetPivot(this RectTransform rectTransform, Vector2 newPivot)
		{
			Vector2 size =                                      rectTransform.rect.size;
			Vector2 deltaPivot =                                rectTransform.pivot - newPivot;
			Vector3 deltaPosition =                             new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
			rectTransform.pivot =                               newPivot;
			rectTransform.localPosition -=                      deltaPosition;
		}

		public static void SetPivot(this RectTransform rectTransform, TextAnchor newPivot)
		{
			Vector2 vpCoords = 									newPivot.ToViewportCoords();
			rectTransform.SetPivot(vpCoords);
		}

		public static void SetAnchors(this RectTransform rectTransform,
										Vector2 anchorMin, Vector2 anchorMax)
		{
			// Make sure this doesn't change the size of the rect transform. 
			Vector2 prevSize =                      rectTransform.rect.size;
			rectTransform.anchorMin =               anchorMin;
			rectTransform.anchorMax =               anchorMax;
			rectTransform.sizeDelta =               prevSize;
		}

		/// <summary>
		/// Sets the rectTransform's anchors to a given point.
		/// </summary>
		public static void AnchorToPoint(this RectTransform rectTransform, Vector2 anchorPoint)
		{
			rectTransform.SetAnchors(anchorPoint, anchorPoint);
		}

		/// <summary>
		/// Sets the rectTransform's anchors and pivot to a given point.
		/// </summary>
		public static void AnchorAndPivotToPoint(this RectTransform rectTransform, Vector2 point)
		{
			rectTransform.AnchorToPoint(point);
			rectTransform.SetPivot(point);
		}

		#endregion

		#region Rect Transform Edge coordinates	
		public static float RightEdgeX(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			float xCoord =          				rectTransform.rect.xMax;

			if (inWorldCoordinates)
				xCoord =            				rectTransform.TransformPoint(new Vector3(xCoord, 0, 0)).x;

			return xCoord;
		}

		public static float LeftEdgeX(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			float xCoord =          				rectTransform.rect.xMin;

			if (inWorldCoordinates)
				xCoord =            				rectTransform.TransformPoint(new Vector3(xCoord, 0, 0)).x;

			return xCoord;
		}

		public static float UpperEdgeY(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			float yCoord =         	 				rectTransform.rect.yMax;

			if (inWorldCoordinates)
				yCoord =            				rectTransform.TransformPoint(new Vector3(0, yCoord, 0)).y;

			return yCoord;
		}

		public static float LowerEdgeY(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			float yCoord =          				rectTransform.rect.yMin;

			if (inWorldCoordinates)
				yCoord = 							rectTransform.TransformPoint(new Vector3(0, yCoord, 0)).y;

			return yCoord;
		}

		#endregion

		#region RectTransform corner and center coordinates
		public static Vector2 LowerLeftCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			Vector2 lowerLeftCorner =               new Vector2(rectTransform.LeftEdgeX(), 
														rectTransform.LowerEdgeY());

			if (inWorldCoordinates)
				lowerLeftCorner =                   rectTransform.TransformPoint(lowerLeftCorner);
			
			return lowerLeftCorner;
			
		}

		public static Vector2 LowerRightCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			Vector2 lowerRightCorner = 				new Vector2( rectTransform.RightEdgeX(), rectTransform.LowerEdgeY());
			
			if (inWorldCoordinates)
				return rectTransform.TransformPoint(lowerRightCorner);
			
			return lowerRightCorner;
		}

		public static Vector2 UpperRightCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			Vector2 upperRightCorner = 				new Vector2( rectTransform.RightEdgeX(), rectTransform.UpperEdgeY());
			
			if (inWorldCoordinates)
				return rectTransform.TransformPoint(upperRightCorner);
			
			return upperRightCorner;
		}

		public static Vector2 UpperLeftCorner(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			Vector2 upperLeftCorner = 				new Vector2(  rectTransform.LeftEdgeX(), rectTransform.UpperEdgeY());

			if (inWorldCoordinates)
				return rectTransform.TransformPoint(upperLeftCorner);

			return upperLeftCorner;
		}

		public static Vector2 Center(this RectTransform rectTransform, bool inWorldCoordinates = false)
		{
			// calculate effective rect dimensions

			Vector2 center = 					rectTransform.rect.center;

			if (inWorldCoordinates)
				center = 						rectTransform.TransformPoint(center);

			return center;
		}

		#endregion

		#region  RectTransform dimensions
		public static float Width(this RectTransform rectTransform)
		{
			return rectTransform.rect.size.x;
		}

		public static float Height(this RectTransform rectTransform)
		{
			return rectTransform.rect.size.y;
		}

		#endregion
		
		#region  RectTransform-positioning
		/// <summary>
		/// Returns a position on the rect transform based on the anchor position passed. Say, if the anchor position is 
		/// (1, 1), this returns the position of the rect's upper right corner.
		/// </summary>
		public static Vector2 GetPositionOnRect(this RectTransform rectTransform, Vector2 anchorPos, bool inWorldCoordinates = false)
		{
			// Use the lower left corner as a starting point.
			Vector2 posOnRect =                            rectTransform.LowerLeftCorner(inWorldCoordinates);

			// Use some simple vector math to get the exact point we want.
			posOnRect.x +=                                 rectTransform.Width() * anchorPos.x;
			posOnRect.y +=                                 rectTransform.Height() * anchorPos.y;
			
			return posOnRect;
		}

		public static void PositionRelativeToParent(this RectTransform rectTransform, Vector2 anchorPos)
		{
			// Get the exact world position on the parent rect where the anchor pos is, then adjust based on 
			// the current rect's dimensions, pivot, and the anchor pos.
			RectTransform parentRect =                  rectTransform.parent.GetComponent<RectTransform>();

			if (parentRect == null) // Safety.
			{
				string format = 						"{0} needs to have a parent to be positioned relative to it.";
				string errMessage = 					string.Format(format, rectTransform.name);
				throw new System.NullReferenceException(errMessage);
			}

			Vector2 newPos =                            parentRect.GetPositionOnRect(anchorPos, inWorldCoordinates: true);

			newPos.x -=                                 rectTransform.Width() * (anchorPos.x - rectTransform.pivot.x);
			newPos.y -=                                 rectTransform.Height() * (anchorPos.y - rectTransform.pivot.y);

			rectTransform.position =                    newPos;
		}

		public static void PositionRelativeToParent(this RectTransform rectTransform, TextAnchor preset)
		{
			Vector2 vpCoords =                          preset.ToViewportCoords();

			rectTransform.PositionRelativeToParent(vpCoords);
		}

		#endregion

		#region The preset-applying functions
		public static void ApplyAnchorPreset(this RectTransform rectTransform, 
												TextAnchor presetToApply,
												bool alsoSetPivot = false, 
												bool alsoSetPosition = false)
		{
			
			Vector2 anchorsToSet =              presetToApply.ToViewportCoords();
			rectTransform.SetAnchors(anchorsToSet, anchorsToSet);
			
			if (alsoSetPivot)
				rectTransform.SetPivot(anchorsToSet);
			
			if (alsoSetPosition)
				rectTransform.PositionRelativeToParent(anchorsToSet);

		}

		public static void ApplyAnchorPresetRecursively(this RectTransform rectTransform, 
														TextAnchor presetToApply, 
														bool alsoSetPivot = false, 
														bool alsoSetPosition = false)
		{
			rectTransform.ApplyAnchorPreset (presetToApply, alsoSetPivot, alsoSetPosition);

			foreach (RectTransform child in rectTransform) 
				child.ApplyAnchorPresetRecursively (presetToApply, alsoSetPivot, alsoSetPosition);

		}
		#endregion

		#endregion
	}
}