using System;
using SupaFabulus.Dev.Foundation.Core.Components;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Components.UI
{
    [Serializable]
    public abstract class UIComponentBase : ComponentBase
    {
       
        [Header("UI Component Base")] [Space(2)]
        
        [SerializeField]
        protected bool _isEnabled = false;
        [SerializeField]
        protected bool _isVisible = false;
        [SerializeField]
        protected bool _isFocused = false;
        [SerializeField]
        protected bool _isHighlighted = false;
        [SerializeField]
        protected bool _isActive = false;
        [SerializeField]
        protected bool _isAnimated = false;
        [SerializeField]
        protected UIComponentBaseEvents _baseUIEvents;



        public bool IsAnimated
        {
            get => _isAnimated;
            set => _isAnimated = value;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value)
                {
                    Show(_isAnimated);
                }
                else
                {
                    Hide(_isAnimated);
                }
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value)
                {
                    Enable(_isAnimated);
                }
                else
                {
                    Disable(_isAnimated);
                }
            }
        }

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (value)
                {
                    Focus(_isAnimated);
                }
                else
                {
                    UnFocus(_isAnimated);
                }
            }
        }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                if (value)
                {
                    Highlight(_isAnimated);
                }
                else
                {
                    UnHighlight(_isAnimated);
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value)
                {
                    Activate(_isAnimated);
                }
                else
                {
                    DeActivate(_isAnimated);
                }
            }
        }



        private void Awake()
        {
            if (_initOnAwake)
            {
                Init();
            }
        }




        public virtual void Init()
        {
            if (!_isInitialized)
            {
                CacheComponents();
                _isInitialized = true;
            }
        }

        public virtual void DeInit()
        {
            ClearComponents();
            _isInitialized = false;
        }

        public virtual void Enable(bool animate = true)
        {
            _isEnabled = true;
        }

        public virtual void Disable(bool animate = true)
        {
            _isEnabled = false;
        }

        public virtual void Show(bool animate = true)
        {
            _isVisible = true;
            NotifyShown();
        }

        public virtual void Hide(bool animate = true)
        {
            _isVisible = false;
            NotifyHidden();
        }

        public virtual void Focus(bool animate = true)
        {
            _isFocused = true;
            NotifyFocused();
        }

        public virtual void UnFocus(bool animate = true)
        {
            _isFocused = false;
            NotifyUnFocused();
        }

        public virtual void Highlight(bool animate = true)
        {
            _isHighlighted = true;
        }

        public virtual void UnHighlight(bool animate = true)
        {
            _isHighlighted = false;
        }

        public virtual void Activate(bool animate = true)
        {
            _isActive = true;
            NotifyActivated();
        }

        public virtual void DeActivate(bool animate = true)
        {
            _isActive = false;
        }

        protected void NotifyShown()
        {
            _baseUIEvents.OnShow.Broadcast();
        }

        protected void NotifyHidden()
        {
            _baseUIEvents.OnHide.Broadcast();
        }

        protected void NotifyFocused()
        {
            _baseUIEvents.OnFocus.Broadcast();
        }

        protected void NotifyUnFocused()
        {
            _baseUIEvents.OnUnFocus.Broadcast();
        }

        protected void NotifyActivated()
        {
            _baseUIEvents.OnActivate.Broadcast();
        }
    }
}