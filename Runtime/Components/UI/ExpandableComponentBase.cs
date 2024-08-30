using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.UI
{
    [Serializable]
    public abstract class ExpandableComponentBase : UIComponentBase
    {
        [Space(10)][Header("Expansion")][Space(2)]
        [SerializeField]
        protected bool _isExpanded = false;
        [SerializeField]
        protected ExpandableComponentEvents _expandableComponentEvents;

        public bool IsExpanded
        {
            get => _isVisible;
            set
            {
                if (value)
                {
                    Expand(_isAnimated);
                }
                else
                {
                    Contract(_isAnimated);
                }
            }
        }

        public virtual void Expand(bool animate = true)
        {
            _isExpanded = true;
            NotifyExpanded();
        }
        public virtual void Contract(bool animate = true)
        {
            _isExpanded = false;
            NotifyContracted();
        }

        protected void NotifyExpanded()
        {
            _expandableComponentEvents.OnExpand.Broadcast();
        }

        protected void NotifyContracted()
        {
            _expandableComponentEvents.OnContract.Broadcast();
        }
    }
}