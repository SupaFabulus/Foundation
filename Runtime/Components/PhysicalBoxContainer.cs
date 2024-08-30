using System;
using System.Collections.Generic;
using SupaFabulus.Dev.Foundation.Core.Enums;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components
{
    #if UNITY_EDITOR
    [ExecuteInEditMode]
    #endif
    [Serializable]
    public class PhysicalBoxContainer : MonoBehaviour
    {
        
        private const float _kMAX_VOLUME_SIZE = 32768f;
        private const string _kBOUNDARY_PREFIX = "Boundary_";

        [Serializable]
        public class BoundaryEntry
        {
            [SerializeField]
            public BoundarySide Side;
            [SerializeField]
            public BoxCollider Boundary;
        }

        [Serializable]
        public class BoundaryCreationSettings
        {
            [SerializeField]
            public BoundarySide SidesToCreate = ALL_SIDES;
        
            [SerializeField]
            public BoundarySide MarkedStatic = 0;
        
            [SerializeField]
            public BoundarySide MarkedTrigger = 0;

            [SerializeField]
            public PhysicsMaterial PhysicalMaterial;
        }

        [SerializeField]
        public Vector3 DefaultVolumeSize = new Vector3(1, 1, 1);
       
        [SerializeField]
        public BoxCollider SourceVolume;
       
        [SerializeField]
        public Transform CustomContainer;

        [SerializeField]
        public BoundaryCreationSettings CreationSettings;
        
        [SerializeField]
        public BoundaryCenterAlignment CenterAlignment = BoundaryCenterAlignment.Outer;

        [SerializeField]
        [Range(0.01f, 10f)]
        public float Thickness = 1f;

        [SerializeField]
        public bool ShowPreview = true;

        [SerializeField] public Color PreviewWireColor = new Color(1f, 0.6f, 0f, 0.25f);
        [SerializeField] public Color PreviewSolidColor = new Color(1f, 0.6f, 0f, 0.25f);

        
        protected Dictionary<BoundarySide, BoxCollider> __boundaries;

        [HideInInspector]
        [SerializeField]
        public List<BoundaryEntry> _boundaries;
        
        protected Dictionary<BoundarySide, Vector3> _boundaryDirections;
        protected bool _boundaryDirectionsInitialized = false;

        internal static readonly BoundarySide[] ORDERED_SIDES = new BoundarySide[]
        {
            BoundarySide.Right,
            BoundarySide.Left,
            BoundarySide.Top,
            BoundarySide.Bottom,
            BoundarySide.Front,
            BoundarySide.Back
        };

        internal static readonly BoundarySide ALL_SIDES =
            BoundarySide.Right |
            BoundarySide.Left |
            BoundarySide.Top |
            BoundarySide.Bottom |
            BoundarySide.Front |
            BoundarySide.Back;


        protected void BuildContainer()
        {
            RemoveBoundaries();
            BuildBoundaryDirections();
            AddBoundaries();
            PositionBoundaries();
        }


        protected bool ValidateBoundaryDirections()
        {
            if
            (
                _boundaryDirections == null ||
                _boundaryDirections == default ||
                _boundaryDirections.Count != ORDERED_SIDES.Length ||
                _boundaryDirectionsInitialized == false
            )
            {
                return false;
            }

            return true;
        }

        
        
        
        protected void BuildBoundaryDirections()
        {
            if (!ValidateBoundaryDirections())
            {
                ClearBoundaryDirectionMap();

                _boundaryDirections = new Dictionary<BoundarySide, Vector3>(6);

                _boundaryDirections.Add(BoundarySide.Right, Vector3.right);
                _boundaryDirections.Add(BoundarySide.Left, Vector3.left);
                _boundaryDirections.Add(BoundarySide.Top, Vector3.up);
                _boundaryDirections.Add(BoundarySide.Bottom, Vector3.down);
                _boundaryDirections.Add(BoundarySide.Front, Vector3.forward);
                _boundaryDirections.Add(BoundarySide.Back, Vector3.back);

                _boundaryDirectionsInitialized = true;
            }
        }

        protected void ClearBoundaryDirectionMap()
        {
            if (_boundaryDirections != null)
            {
                _boundaryDirections.Clear();
                _boundaryDirections = null;
            }
        }

        protected void AddBoundaries()
        {
            int i;
            int count = ORDERED_SIDES.Length;
            BoxCollider collider;
            GameObject gObj;
            Transform tx;
            BoundarySide side;
            BoundaryEntry entry;
            string sideName;
            
            Transform parent;
            parent = CustomContainer != null ? CustomContainer : transform;

            if (_boundaries == null || _boundaries == default)
            {
                _boundaries = new List<BoundaryEntry>();
            }

            for (i = 0; i < count; i++)
            {
                side = ORDERED_SIDES[i];
                
                if((CreationSettings.SidesToCreate & side) != side)
                {
                    continue;
                }

                sideName = side.ToString();
                
                gObj = new GameObject(_kBOUNDARY_PREFIX + sideName);
                tx = gObj.transform;
                
                tx.localPosition = Vector3.zero;
                tx.localEulerAngles = Vector3.zero;
                tx.localScale = Vector3.one;

                collider = gObj.AddComponent<BoxCollider>();
                collider.material = CreationSettings.PhysicalMaterial;
                collider.isTrigger = (CreationSettings.MarkedTrigger & side) == side;
                
                gObj.isStatic = (CreationSettings.MarkedStatic & side) == side;
                gObj.transform.SetParent(parent);

                entry = new BoundaryEntry();
                entry.Side = side;
                entry.Boundary = collider;

                _boundaries.Add(entry);
            }
        }

        protected void RemoveBoundaries()
        {
            int i;
            //int id;
            int count;
            BoxCollider collider;
            GameObject gObj;
            Transform child;
            string name;

            Transform parent;
            parent = CustomContainer != null ? CustomContainer : transform;
            
            if (_boundaries != null)
            {
                _boundaries.Clear();
                _boundaries = null;
            }
            
            count = parent.childCount;

            for (i = count - 1; i >= 0; i--)
            {

                child = parent.GetChild(i);
                gObj = child.gameObject;
                collider = gObj.GetComponent<BoxCollider>();
                name = gObj.name;

                if (collider != null && name.Substring(0, _kBOUNDARY_PREFIX.Length) == _kBOUNDARY_PREFIX)
                {
                    #if UNITY_EDITOR
                    DestroyImmediate(collider);
                    DestroyImmediate(gObj);
                    #else
                    Destroy(collider);
                    Destroy(gObj);
                    #endif
                }
            }
        }

        
        protected void PositionBoundaries()
        {
            
            if (_boundaries == null || _boundaries == default || _boundaries.Count < 1)
            {
                return;
            }
            
            if(!ValidateBoundaryDirections())
            {
                BuildBoundaryDirections();
            }

            int i;
            //int id;
            int count = _boundaries.Count;
            BoundarySide side;
            BoundaryEntry entry;
            BoxCollider collider;
            //GameObject gObj;
            Transform tx;
            Vector3 direction;
            Vector3 extentDirection;
            Vector3 size = Vector3.one;
            Vector3 extents = size * 0.5f;
            Vector3 center = Vector3.zero;
            float offset = (float)CenterAlignment * (Thickness * 1f);


            if (SourceVolume != null)
            {
                size = SourceVolume.size;
                size.Scale(SourceVolume.transform.localScale);
            }
            else
            {
                size = DefaultVolumeSize;
            }

            size = new Vector3
            (
                Mathf.Clamp(size.x + offset, 0f, _kMAX_VOLUME_SIZE),
                Mathf.Clamp(size.y + offset, 0f, _kMAX_VOLUME_SIZE),
                Mathf.Clamp(size.z + offset, 0f, _kMAX_VOLUME_SIZE)
            );

            extents = size * 0.5f;
            center = Vector3.zero;


            for (i = 0; i < count; i++)
            {
                entry = _boundaries[i];

                if (entry == null) continue;

                collider = entry.Boundary;
                side = entry.Side;
                //id = (int)side;
                
                /*
                if (_boundaries.ContainsKey(side))
                {
                    collider = _boundaries[side];
                }
                */
                
                if (collider != null)
                {
                    tx = collider.transform;
                    direction = _boundaryDirections[side];
                    extentDirection = direction;
                    extentDirection.Scale(extents);
                    
                    tx.localRotation = Quaternion.identity;
                    tx.localPosition = center + extentDirection;

                    collider.size = GetBoundaryDimensions
                        (
                            Thickness, 
                            direction, 
                            size
                        );
                }
            }
        }



        protected bool ValidateBoundaries()
        {
            return _boundaries != null && _boundaries != default && _boundaries.Count > 0;
        }

        protected Vector3 GetBoundaryDimensions
        (
            float thickness, 
            Vector3 sideDirectionality, 
            Vector3 containerSize
        )
        {
            return new Vector3
            (
                GetConstrainedDimension(thickness, sideDirectionality.x, containerSize.x),
                GetConstrainedDimension(thickness, sideDirectionality.y, containerSize.y),
                GetConstrainedDimension(thickness, sideDirectionality.z, containerSize.z)
            );
        }


        protected float GetConstrainedDimension(float thickness, float directionalComponent, float sizeComponent)
        {
            return thickness + (Mathf.Abs(1f - Mathf.Abs(directionalComponent)) * sizeComponent);
        }


#if UNITY_EDITOR

        private void DrawBoundaryPreviews(bool solid = false)
        {
            int i;
            int count;
            BoundaryEntry boundary;
            BoxCollider collider;
            Transform tx;
            BoundarySide side;

            count = _boundaries.Count;

            Gizmos.color = PreviewSolidColor;

            for (i = 0; i < count; i++)
            {
                boundary = _boundaries[i];
                if (boundary != null)
                {
                    collider = boundary.Boundary;
                    side = boundary.Side;

                    if (collider != null)
                    {
                        tx = collider.transform;
                        Matrix4x4 m = Gizmos.matrix;
                        Gizmos.matrix = tx.localToWorldMatrix;

                        if (solid)
                        {
                            Gizmos.DrawCube(Vector3.zero, collider.size);
                        }
                        else
                        {
                            Gizmos.DrawWireCube(Vector3.zero, collider.size);
                        }

                        
                        Gizmos.matrix = m;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (ValidateBoundaries())
            {
                PositionBoundaries();
                DrawBoundaryPreviews(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (ValidateBoundaries())
            {
                PositionBoundaries();
                DrawBoundaryPreviews(true);
            }
        }
        #endif
    }
}