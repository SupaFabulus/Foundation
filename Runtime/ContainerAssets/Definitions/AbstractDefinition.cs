using System;
using UnityEngine;

namespace SupaFabulus.Dev.Definitions
{
    [Serializable]
    public struct DefinitionDisplayInfo
    {
        [SerializeField]
        private string _shortName;
        [SerializeField]
        private string _fullName;
        [SerializeField]
        private string _abbreviation;
        [SerializeField][Multiline]
        private string _description;
        
        [SerializeField]
        private Texture2D _iconImage;
        [SerializeField]
        private GameObject _iconPrefab;
        
        
        public string ShortName => _shortName;
        public string FullName => _fullName;
        public string Abbreviation => _abbreviation;
        public string Description => _description;
     
        public Texture2D IconImage => _iconImage;
        public GameObject IconPrefab => _iconPrefab;

        public DefinitionDisplayInfo
        (
            string shortName, 
            string fullName, 
            string abbreviation, 
            string description, 
            Texture2D iconImage, 
            GameObject iconPrefab
        )
        {
            _shortName = shortName;
            _fullName = fullName;
            _abbreviation = abbreviation;
            _description = description;
            _iconImage = iconImage;
            _iconPrefab = iconPrefab;
        }
    }


    [Serializable]
    public abstract class AbstractDefinition : ScriptableObject
    {
        [SerializeField]
        protected string _id;
        [SerializeField]
        protected DefinitionDisplayInfo _info;
        [SerializeField]
        protected bool _autoValidateContent = false;

        public string ID => _id;
        public DefinitionDisplayInfo Info => _info;
        public bool AutoValidateContent => _autoValidateContent;
    }
    
    [Serializable]
    public abstract class AbstractDefinition<TContent> : AbstractDefinition
    {
        [SerializeField]
        protected TContent _content;
        public virtual TContent Content => _content;
    }
}