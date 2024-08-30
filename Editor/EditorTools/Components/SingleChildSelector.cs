using System;
using SupaFabulus.Dev.Foundation.Core.Signals;
using UnityEngine;

namespace SupaFabulus.Dev.EditorTools.Components
{
    [Serializable]
    public class SingleChildSelector : MonoBehaviour
    {
        [SerializeField]
        public bool NotifyOnSelection = true;
        [SerializeField]
        public bool EnableChildrenOnAdd = true;
        [SerializeField]
        public bool revalidateOnAdd = true;
        [SerializeField]
        public bool keepInspectorUpdated = true;

        [SerializeField]
        public Transform CustomContainer;
        
        [SerializeField]
        protected Signal<Transform> _onChildAdded = new();
        public Signal<Transform> OnChildAdded => _onChildAdded;
        
        [SerializeField]
        protected Signal<Transform> _onSelectionChanged = new();
        public Signal<Transform> OnSelectionChanged => _onSelectionChanged;
        

        
        protected Transform _selectedChild = null;
        public Transform SelectedChild => _selectedChild;


        public Transform Container => (CustomContainer != null)
            ? CustomContainer
            : transform;
        
        public int SelectedChildIndex
        {
            get
            {
                if (_selectedChild == null) return -1;
                
                int i;
                int c = Container.childCount;
                Transform child;
                
                for (i = 0; i < c; i++)
                {
                    child = Container.GetChild(i);
                    if (_selectedChild == child)
                    {
                        return i;
                    }
                }

                return -1;
            }
            
            set
            {
                bool changed = false;
                if (value >= 0 && value < Container.childCount)
                {
                    Debug.Log($"Getting Child: {value}/{Container.childCount}");
                    _swap = Container.GetChild(value);
                    changed = _swap != _selectedChild;
                    _selectedChild = _swap;
                }
                else if(value < 0)
                {
                    _selectedChild = null;
                }

                if (changed)
                {
                    UpdateSelection();
                    if (NotifyOnSelection)
                    {
                        NotifySelectionChanged();
                    }
                }
            }
        }

        public void ShowChild(Transform child)
        {
            if (child != null)
            {
                Debug.Log($"Selected child: [{child.name}]");
            }
            else
            {
                Debug.Log($"No selected child received!");
            }
        }

        private Transform _swap;

        protected void UpdateSelection()
        {
            if (_selectedChild == null) return;
                
            Debug.Log("Updating Selection");
            int i;
            int c = Container.childCount;
            Transform child;
            bool selected;

            for (i = 0; i < c; i++)
            {
                child = Container.GetChild(i);
                selected = (_selectedChild == child);
                child.gameObject.SetActive(selected);
            }
        }

        protected void NotifySelectionChanged()
        {
            _onSelectionChanged.Broadcast(_selectedChild);
        }

        public void AddChild(Transform child)
        {
            //Debug.Log($"Received: {child}");
            if (child != null)
            {
                child.SetParent(Container);
                child.gameObject.SetActive(EnableChildrenOnAdd);
                UpdateSelection();
                _onChildAdded.Broadcast(child);

                if (revalidateOnAdd)
                {
                    
                }
            }
        }
    }
}