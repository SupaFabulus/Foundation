using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buganamo.MainFrame.Utils
{
    public static class PathUtils
    {
        public static Vector3 ClosestPointOnLineSegmentToPosition
        (
            Vector3 lineStart, 
            Vector3 lineEnd, 
            Vector3 targetPosition
        )
        {
            Vector3 result = lineStart + Vector3.Project(targetPosition - lineStart, lineEnd - lineStart);

            return result;
        }

        public static float NormalizedLocationOfPointOnSegment
        (
            Vector3 lineStart, 
            Vector3 lineEnd, 
            Vector3 targetPosition
        )
        {
            /*
            Vector3 deltaV = lineEnd - lineStart;
            Vector3 targetFromStart = targetPosition - lineStart;
            Vector3 targetFromEnd = targetPosition - lineEnd;

            float deltaDist = deltaV.magnitude;
            float fromStartDist = targetFromStart.magnitude;
            float fromEndDist = targetFromEnd.magnitude;

            return (fromStartDist / deltaDist) * ((fromEndDist > fromStartDist && fromEndDist > deltaDist) ? -1f : 1f);
            */

            Vector3 deltaV = lineEnd - lineStart;
            Vector3 intersect = ClosestPointOnLineSegmentToPosition(lineStart, lineEnd, targetPosition);
            
            Vector3 intersectFromStart = intersect - lineStart;
            Vector3 intersectFromEnd = intersect - lineEnd;
            
            float deltaDist = deltaV.magnitude;
            float intersectFromStartDist = intersectFromStart.magnitude;
            float intersectFromEndDist = intersectFromEnd.magnitude;

            return (intersectFromStartDist / deltaDist) *
                   ((intersectFromEndDist > deltaDist && intersectFromEndDist > intersectFromStartDist)
                ? -1f
                : 1f);
            
            /*
            if (intersectFromEndDist > deltaDist)
            {
                return -(intersectFromStartDist / deltaDist);
            }
            else
            {
                return intersectFromStartDist / deltaDist;
            }
            */
        }
        
        
        public static Vector3 FindLocationOnPathForDistance(List<Vector3> path, float distance)
        {
            if (path == null || path.Count < 2)
            {
                return default(Vector3);
            }

            float accumulated = 0f;
            float nextAccumulation;
            float remainder;
            float delta;
            Vector3 deltaVector;
            int i;
            int limit = path.Count - 1;
            
            Vector3 currentNode;
            Vector3 nextNode;

            for (i = 0; i < limit; i++)
            {
                currentNode = path[i];
                nextNode = path[i + 1];
                deltaVector = (nextNode - currentNode);
                delta = deltaVector.magnitude;
                nextAccumulation = accumulated + delta;

                if (nextAccumulation == distance)
                {
                    return nextNode;
                }

                if (nextAccumulation > distance)
                {
                    remainder = distance - accumulated;

                    return currentNode + (deltaVector.normalized * remainder);
                }
                else
                {
                    accumulated = nextAccumulation;
                }
            }

            return default(Vector3);
        }
        
        
        public static Vector3 FindLocationOnPathForDistance(List<Transform> path, float distance)
        {
            if (path == null || path.Count < 2)
            {
                return default(Vector3);
            }

            float accumulated = 0f;
            float nextAccumulation;
            float remainder;
            float delta;
            Vector3 deltaVector;
            int i;
            int limit = path.Count - 1;
            
            Transform currentNode;
            Transform nextNode;

            for (i = 0; i < limit; i++)
            {
                currentNode = path[i];
                nextNode = path[i + 1];
                deltaVector = (nextNode.position - currentNode.position);
                delta = deltaVector.magnitude;
                nextAccumulation = accumulated + delta;

                if (nextAccumulation == distance)
                {
                    return nextNode.position;
                }

                if (nextAccumulation > distance)
                {
                    remainder = distance - accumulated;

                    return currentNode.position + (deltaVector.normalized * remainder);
                }
                else
                {
                    accumulated = nextAccumulation;
                }
            }

            return default(Vector3);
        }
        
        
        public static Vector3 FindLocationOnPathForDistance<TPathNode>(List<TPathNode> path, float distance)
            where TPathNode : MonoBehaviour
        {
            if (path == null || path.Count < 2)
            {
                return default(Vector3);
            }

            float accumulated = 0f;
            float nextAccumulation;
            float remainder;
            float delta;
            Vector3 deltaVector;
            int i;
            int limit = path.Count - 1;
            
            TPathNode currentNode;
            TPathNode nextNode;

            for (i = 0; i < limit; i++)
            {
                currentNode = path[i];
                nextNode = path[i + 1];
                deltaVector = (nextNode.transform.position - currentNode.transform.position);
                delta = deltaVector.magnitude;
                nextAccumulation = accumulated + delta;

                if (nextAccumulation == distance)
                {
                    return nextNode.transform.position;
                }

                if (nextAccumulation > distance)
                {
                    remainder = distance - accumulated;

                    return currentNode.transform.position + (deltaVector.normalized * remainder);
                }
                else
                {
                    accumulated = nextAccumulation;
                }
            }

            return default(Vector3);
        }
        
        public static int IndexOfClosestPrecedingNodeOnPathToPosition(List<Transform> nodes, Vector3 position)
        {
            if (nodes == null || nodes.Count < 2)
            {
                return -1;
            }

            int result = -1;

            int i;
            int count = nodes.Count;
            
            Transform node = null;
            Transform nextNode = null;
            Transform closestNode = null;

            Vector3 startPos = Vector3.zero;
            Vector3 nodePos = Vector3.zero;
            Vector3 prevNodePos = Vector3.zero;
            Vector3 closestNodePos = Vector3.zero;
            Vector3 segmentV = Vector3.zero;
            
            float closestNodeDistanceFromActionPos = 0f;
            float nodeDistanceFromActionPos = 0f;
            float closestNodeDistanceFromPrevNode = 0f;
            float nodeDistanceFromPrevNode = 0f;
            
            float distance = 0f;
            
            //float actionPosDistance = 0f;
            float actionPosTotal = 0f;
            
            Vector3 actionPos = position;

            
            prevNodePos = startPos;
            
            for (i = 0; i < count; i++)
            {
                node = nodes[i];
                nodePos = node.position;

                if (i < count - 1)
                {
                    nextNode = nodes[i + 1];
                    segmentV = nextNode.position - node.position;
                    distance = segmentV.magnitude;
                }

                if (closestNode == null)
                {
                    closestNode = node;
                    result = i;
                    
                    float nodeDist = Vector3.Distance(nodePos, startPos);
                    float actionDist = Vector3.Distance(actionPos, startPos);

                    if (nodeDist < actionDist)
                    {
                        closestNodeDistanceFromActionPos = Vector3.Distance(actionPos, nodePos);
                    }
                    else
                    {
                        closestNodeDistanceFromActionPos = actionDist;
                    }
                    
                    actionPosTotal = distance + closestNodeDistanceFromActionPos;
                }
                else
                {
                    nodeDistanceFromActionPos = Vector3.Distance(nodePos, actionPos);
                    nodeDistanceFromPrevNode = Vector3.Distance(nodePos, prevNodePos);
                    closestNodeDistanceFromPrevNode = Vector3.Distance(closestNodePos, prevNodePos);
                    
                    if (nodeDistanceFromActionPos < closestNodeDistanceFromActionPos && nodeDistanceFromPrevNode < closestNodeDistanceFromActionPos)
                    {
                        closestNode = node;
                        result = i;
                        //actionPosTotal = _pathLength + Vector3.Distance(actionPos, prevNodePos);
                    }
                }
                
                closestNodePos = closestNode.transform.position;
                closestNodeDistanceFromActionPos = Vector3.Distance(closestNodePos, actionPos);
                
                //_pathLength += distance;
                //_nodePositions.Add(node.transform.position);

                prevNodePos = node.transform.position;
            }

            //_closestActionNodePos = closestNodePos;
            //_distanceToActionLocation = actionPosTotal;

            return result;
        }
        
        
        /*
         * With some help from:
         * https://stackoverflow.com/questions/59449628/check-when-two-vector3-lines-intersect-unity3d
         */
        public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
            Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){

            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parallel
            if( Mathf.Abs(planarFactor) < 0.0001f 
                && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) 
                          / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }



        public struct LineSegmentSolution
        {
            public Transform[] fullPath;
            public Transform startNode;
            public Transform endNode;
            public Transform targetPosition;
            public Vector3 closestSegmentPosition;
            public float normalizedPosition;
            public float intersectionDistance;
            public float totalPathDistance;

        }

        public static LineSegmentSolution Test(Transform[] nodes, Transform target)
        {
            LineSegmentSolution result = new LineSegmentSolution();
            result.targetPosition = target;

            Transform node;
            Transform nextNode;
            Transform closestNode;
            Vector3 deltaV;
            Vector3 targetV;
            Vector3 targetPos = target.position;
            Vector3 nodePos;
            Vector3 nextNodePos;
            float dot;
            float closestDistance = float.PositiveInfinity;
            float targetDistance = 0f;
            float intersectionDistance = 0f;
            float totalDistance = 0f;
            //float normalizedDistance = 0f;
            float segmentLength = 0f;
            

            int i;
            int count = nodes.Length;
            int closestIndex = 0;
            
            for (i = 0; i < count-1; i++)
            {
                node = nodes[i];
                nextNode = nodes[i + 1];
                nodePos = node.position;
                nextNodePos = nextNode.position;
                deltaV = nextNodePos - nodePos;
                targetV = targetPos - nodePos;
                targetDistance = targetV.magnitude;
                dot = Vector3.Dot(deltaV, targetV);
                
                targetDistance = Vector3.Distance(target.position, node.position);
                segmentLength = (nextNode.position - node.position).magnitude;
                
                if (targetDistance < closestDistance && (dot > 0f || i == 0 || i == count-1))
                {
                    closestDistance = targetDistance;
                    closestNode = node;
                    closestIndex = i;
                    result.startNode = node;
                    result.endNode = nextNode;
                    result.closestSegmentPosition =
                        PathUtils.ClosestPointOnLineSegmentToPosition(nodePos, nextNodePos, targetPos);

                    result.normalizedPosition = PathUtils.NormalizedLocationOfPointOnSegment(nodePos, nextNodePos,targetPos);
                    
                    intersectionDistance = totalDistance + (result.normalizedPosition * segmentLength);
                }
                
                totalDistance += segmentLength;
            }

            result.totalPathDistance = totalDistance;
            result.intersectionDistance = intersectionDistance;
            
            /*
            if (closestIndex == 0 && result.normalizedPosition < 0f)
            {
                result.startNode = nodes[0];
                result.endNode = nodes[1];
                result.closestSegmentPosition = result.startNode.position;
            }
            else if (closestIndex == count - 2 && result.normalizedPosition > 1f)
            {
                result.startNode = nodes[count - 2];
                result.endNode = nodes[count - 1];
                result.closestSegmentPosition = result.endNode.position;
            }
            */
            return result;
        }

        public static Tuple<Vector3, Vector3> ShortestConnectingSegment(Vector3 oa, Vector3 va, Vector3 ob, Vector3 vb)
        {
            Tuple<Vector3, Vector3> result;
            Vector3 d;
            Vector3 e;
            Vector3 vc = va - vb;
            Func<Vector3, Vector3, float> dot = Vector3.Dot;

            d = oa + (va * (
                ((-dot(va, vb) * dot(vb, vc)) + (dot(va, vc))) /
                ((dot(va, va) * dot(vb, vb)) - (dot(va, vb) * dot(va, vb)))
            ));

            e = ob + (vb * (
                ((dot(va, vb) * dot(va, vc)) - (dot(vb, vc) * dot(va, va))) /
                ((dot(va, va) * dot(vb, vb)) - (dot(va, vb) * dot(va, vb)))
            ));
            
            result = new(d, e);

            return result;
        }

        public static LineSegmentSolution GetClosestSegmentAndLinePositionToTargetLocation(Transform[] nodes,
            Transform target)
        {
            LineSegmentSolution result = new LineSegmentSolution();
            result.targetPosition = target;

            int i;
            int count = nodes.Length - 1;
            Transform node = null;
            Transform nextNode = null;
            float distance = 0f;
            float closestDistance = float.PositiveInfinity;
            float secondClosestDistance = float.PositiveInfinity;
            Transform closestNode = null;
            Transform secondClosestNode = null;
            int closestIndex = -1;
            int secondClosestIndex = -1;
            float segmentPosition;
            Vector3 closestPoint;
            Transform currentSegmentStart;
            Transform currentSegmentEnd;

            for (i = 0; i < count; i++)
            {
                node = nodes[i];
                nextNode = nodes[i + 1];
                distance = Vector3.Distance(target.position, node.position);
                //Debug.Log(node.name + ": " + distance);

                closestPoint =
                    PathUtils.ClosestPointOnLineSegmentToPosition(node.position, nextNode.position,
                        target.position);
                
                segmentPosition =
                    PathUtils.NormalizedLocationOfPointOnSegment(node.position, nextNode.position,
                        target.position);

                result.normalizedPosition = segmentPosition;
                

                if (segmentPosition >= 0f && segmentPosition <= 1f)
                {
                    currentSegmentStart = node;
                    currentSegmentEnd = nextNode;
                    //Debug.Log("POS: " + segmentPosition);
                }
                else if (segmentPosition < 0f && i == 0)
                {
                    closestPoint = node.position;
                    currentSegmentStart = node;
                    currentSegmentEnd = nextNode;
                    result.closestSegmentPosition = closestPoint;
                    result.endNode = nextNode;
                    result.startNode = node;
                    return result;
                }
                else if (segmentPosition > 1f && i == count - 2)
                {
                    
                    closestPoint = nextNode.position;
                }

                if (distance < closestDistance)
                {
                    secondClosestDistance = closestDistance;
                    closestDistance = distance;
                    secondClosestNode = closestNode;
                    closestNode = node;
                    secondClosestIndex = closestIndex;
                    closestIndex = i;
                    //Debug.Log(closestNode.name + ", " + secondClosestNode?.name);
                    
                    
                    result.closestSegmentPosition = closestPoint;
                    result.endNode = nextNode;
                    result.startNode = node;
                }
                
                    
                
            }
            
            

            return result;
        }

    }
}