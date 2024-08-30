#if UNITY_EDITOR
using System;
using SupaFabulus.Dev.Foundation.Utils;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
#if UNITY_EDITOR

    public static class GizMo
    {

        public static void DrawPick(Vector3 center, float radius, Color color)
        {
            Gizmos.color = color;
            Vector3 x = Vector3.right * radius;
            Vector3 y = Vector3.up * radius;
            Vector3 z = Vector3.forward * radius;
            Gizmos.DrawLine(center - x, center + x);
            Gizmos.DrawLine(center - y, center + y);
            Gizmos.DrawLine(center - z, center + z);
        }

        public static void DrawTrident(Vector3 pos, Quaternion rot, Vector3 pickLength, Color tint = default)
        {
            if(tint == default) tint = Color.white;
            
            Vector3 right = rot * Vector3.right;
            Vector3 up = rot * Vector3.up;
            Vector3 fwd = rot * Vector3.forward;

            Vector3 rightV = right * pickLength.x;
            Vector3 upV = up * pickLength.y;
            Vector3 fwdV = fwd * pickLength.z;

            Color x = tint * Color.red;
            Color y = tint * Color.green;
            Color z = tint * Color.blue;

            Gizmos.color = x;
            Gizmos.DrawLine(pos, pos + rightV);
            Gizmos.color = y;
            Gizmos.DrawLine(pos, pos + upV);
            Gizmos.color = z;
            Gizmos.DrawLine(pos, pos + fwdV);
        }

        public static void Arrow(Vector3 startPos, Vector3 vector, Color color, float arrowheadSize = 0.25f)
        {
            DrawArrow(startPos, startPos + vector, vector.normalized, vector.magnitude, color, arrowheadSize);
        }
        
        public static void ArrowFromTo(Vector3 startPos, Vector3 endPoint, Color color, float arrowheadSize = 0.25f)
        {
            Vector3 delta = (endPoint - startPos);
            DrawArrow(startPos, endPoint, delta.normalized, delta.magnitude, color, arrowheadSize);
        }
        
        public static void DrawArrow(Vector3 startPos, Vector3 endPos, Vector3 direction, float length, Color color, float arrowheadSize = 0.25f)
        {
            Vector3 leftPoint;
            Vector3 rightPoint;
            Vector3 rightDirection;
            Vector3 arrowheadBasePos;
            float arrowheadBaseDistance;
            
            arrowheadBaseDistance = Mathf.Abs(length - (arrowheadSize * 2f));
            arrowheadBasePos = startPos + (direction * arrowheadBaseDistance);
            rightDirection = Vector3.Cross(Vector3.up, direction).normalized * arrowheadSize;
            
            rightPoint = arrowheadBasePos + rightDirection;
            leftPoint = arrowheadBasePos - rightDirection;

            Gizmos.color = color;
            
            Gizmos.DrawLine(startPos, arrowheadBasePos);
            Gizmos.DrawLine(leftPoint, rightPoint);
            Gizmos.DrawLine(leftPoint, endPos);
            Gizmos.DrawLine(rightPoint, endPos);
        }

        public static int GlobalCurveResolution = 64;


        public static void DrawPerpendicularLine
        (
            Vector3 center,
            Vector3 direction,
            Vector3 up,
            float width = 0.5f,
            float offset = 0f,
            float thickness = 0f
        )
        {
            //Vector3 right = Vector3.Cross(direction, up).normalized;
            Vector3 right = Quaternion.LookRotation(Vector3.right, up) * direction;
            Vector3 halfLine = width * right;
            Vector3 offsetPoint = center + (right * offset);
            
            DrawLine
            (
                offsetPoint - halfLine,
                offsetPoint + halfLine,
                thickness
            );
        }

        public static void DrawGradientStrip
        (
            Gradient gradient, 
            Vector3 start, 
            Vector3 end,
            Vector3 up,
            float gradientStart = 0f,
            float gradientEnd = 1f,
            float highlightStart = 0f,
            float highlightEnd = 1f,
            float alpha = 0.75f,
            float alphaHighlight = 1f,
            float width = 0.75f,
            float widthHighlight = 1f,
            float thickness = 0f,
            float thicknessHighlight = 0.001f,
            int density = 10,
            bool mirror = false
        )
        {
            Vector3 deltaV = end - start;
            float distance = deltaV.magnitude;

            if (distance < 0.0000001f)
            {
                Debug.Log("Too Short!: " + distance);
                return;
            }
            
            Vector3 deltaN = deltaV.normalized;
            
            int i;
            Vector3 segmentCenter;
            Vector3 segmentStart;
            Vector3 segmentEnd;
            Color color;
            float time;
            float gradientPos;
            bool isHighlight = false;

            int count = Mathf.CeilToInt(distance * density);
            Quaternion rotation = Quaternion.LookRotation(deltaN, up);
            Vector3 right = Quaternion.LookRotation(Vector3.right, up) * deltaN;
            float step = distance / (count);
            float size;
            Vector3 segmentHalf;
            
            for(i = 0; i <= count; i++)
            {
                time = (float) i / (float) count;
                
                isHighlight = time >= highlightStart && time <= highlightEnd;
                
                gradientPos = Mathf.Lerp(gradientStart, gradientEnd, time);
                
                //Debug.Log(time);
                color = gradient.Evaluate(mirror ? Math.Abs(gradientPos) : gradientPos);
                color = new Color(color.r, color.g, color.b, (isHighlight ? alphaHighlight : alpha));
                size = ((isHighlight ? widthHighlight : width) * 0.5f);
                segmentHalf = right * size;
                segmentCenter = start + (i * deltaN * step);
                segmentStart = segmentCenter - segmentHalf;
                segmentEnd = segmentCenter + segmentHalf;
                
                

                Gizmos.color = color;
                DrawPerpendicularLine(segmentCenter, deltaN, Vector3.up, size, 0f, isHighlight ? thicknessHighlight : thickness);
            }
        }

        public static void DrawRing(Vector3 center, float radius, Color color)
        {
            Gizmos.color = color;
            int i;
            int c = GlobalCurveResolution;
            float a, _a;
            float d = (360f * Mathf.Deg2Rad) / GlobalCurveResolution;
            
            for (i = 0; i < c; i++)
            {
                a = i * d;
                _a = (i + 1) * d;
                
                Gizmos.DrawLine
                (
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(a))
                    ),
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(_a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(_a))
                    )
                );
            }
        }
        
        public static void DrawRing(Vector3 center, float radius, Color color, float thickness = 0.001f)
        {
            Gizmos.color = color;
            int i;
            int c = GlobalCurveResolution;
            float a, _a;
            float d = (360f * Mathf.Deg2Rad) / GlobalCurveResolution;
            
            for (i = 0; i < c; i++)
            {
                a = i * d;
                _a = (i + 1) * d;
                
                DrawLine
                (
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(a))
                    ),
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(_a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(_a))
                    ),
                    thickness
                );
            }
        }

        public static void DrawRingHue(Vector3 center, float radius, Hue hue, Vector3 direction, float thickness = 0.001f,
            float strength = 1f, float threshold = 0.5f)
        {
            int i;
            int c = GlobalCurveResolution;
            float a, _a;
            float d = (360f * Mathf.Deg2Rad) / GlobalCurveResolution;
            
            for (i = 0; i < c; i++)
            {
                a = i * d;
                _a = (i + 1) * d;
                float angle = ((a + _a) * 0.5f);

                Vector3 v = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                float m = ((Vector3.Dot(v, direction) + 1f) * 0.5f) * threshold;

                Gizmos.color = Colors.GetMultipliedTransparentColor(hue, (m * strength) - (strength * 1.05f), strength);
                    
                DrawLine
                (
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(a))
                    ),
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(_a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(_a))
                    ),
                    thickness
                );
            }
        }
        
        public static void DrawRingColor32(Vector3 center, float radius, Color32 color, Vector3 direction, float thickness = 0.001f,
            float strength = 1f, float threshold = 0.5f)
        {
            int i;
            int c = GlobalCurveResolution;
            float a, _a;
            float d = (360f * Mathf.Deg2Rad) / GlobalCurveResolution;
            
            for (i = 0; i < c; i++)
            {
                a = i * d;
                _a = (i + 1) * d;
                float angle = ((a + _a) * 0.5f);

                Vector3 v = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                float m = ((Vector3.Dot(v, direction) + 1f) * 0.5f) * threshold;

                Gizmos.color = Colors.GetMultipliedTransparentColor(color, (m * strength) - (strength * 1.05f), strength);
                    
                DrawLine
                (
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(a))
                    ),
                    new Vector3
                    (
                        center.x + (radius * Mathf.Cos(_a)),
                        center.y,
                        center.z + (radius * Mathf.Sin(_a))
                    ),
                    thickness
                );
            }
        }


        // By Bunny83@UnityForums
        public static void DrawLine(Vector3 p1, Vector3 p2, float width = 0.001f)
        {
            int count = Mathf.CeilToInt(width / 0.001f); // how many lines are needed.

            if (width <= 0f)
            {
                Gizmos.DrawLine(p1,p2);
            }

            if(count ==1)
                Gizmos.DrawLine(p1,p2);
            else
            {
                Camera c = Camera.current;
                if (c == null)
                {
                    Debug.LogError("Camera.current is null");
                    return;
                }
                Vector3 v1 = (p2 - p1).normalized; // line direction
                Vector3 v2 = (c.transform.position - p1).normalized; // direction to camera
                Vector3 n = Vector3.Cross(v1,v2); // normal vector
                for(int i = 0; i < count; i++)
                {
                    Vector3 o = n * width * ((float)i/(count-1) - 0.5f);
                    Gizmos.DrawLine(p1+o,p2+o);
                }
            }
        }
        
        public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
        {
            if (_color != default(Color))
                Handles.color = _color;
            Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (_height - (_radius * 2)) / 2;
 
                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
                Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
                Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);
 
            }
        }

        public static bool DrawSpatialLabel
        (
            Vector3 position,
            string text,
            int textSize,
            Hue hue,
            FontStyle style = FontStyle.Bold,
            TextAnchor alignment = TextAnchor.MiddleLeft,
            float maxOverScaleFactor = 2f
        )
        {
            return DrawSpatialLabel
            (
                position,
                text,
                textSize,
                Colors.GetColor(hue),
                style,
                alignment,
                maxOverScaleFactor
            );
        }


        public static bool DrawSpatialLabel
        (
            Vector3 position,
            string text,
            int textSize, 
            Color textColor,
            FontStyle style = FontStyle.Bold,
            TextAnchor alignment = TextAnchor.MiddleLeft,
            float maxOverScaleFactor = 2f
        )
        {
            if (textSize < 1) return false;
            
            float sizeNormalized = textSize;
            int maxSize = Mathf.FloorToInt(sizeNormalized * maxOverScaleFactor);
            float camDist = (SceneView.lastActiveSceneView.camera.transform.position - position).magnitude;
            float adjustedFOV = (SceneView.lastActiveSceneView.camera.fieldOfView * 0.5f) +
                                (SceneView.lastActiveSceneView.camera.fieldOfView * 0.5f);
            float fovFactor = 1f - Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * SceneView.lastActiveSceneView.camera.fieldOfView * 0.75f));
            int rawSize = Mathf.FloorToInt(sizeNormalized / (camDist * fovFactor));
            int fontSize = Mathf.Clamp(rawSize, 0, maxSize);
                 
            if (fontSize >= 1)
            {
                GUIStyle s = new GUIStyle();
                        
                s.fontSize = fontSize;
                s.fontStyle = style;
                s.alignment = alignment;
                s.normal.textColor = textColor;

                Handles.Label(position, text, s);

                return true;
            }

            return false;
        }
        
    }
    
    
    


#endif
}
#endif