using System;

namespace SupaFabulus.Dev.Foundation.Common.DataModels
{
    public interface IInjectionKeyObject { }
    
    // [Serializable]
    public struct InjectionKey<TKeyObj, TKeyOption>
    where TKeyObj : IInjectionKeyObject
    where TKeyOption : struct, Enum
    {
        // [SerializeField]
        private TKeyObj _keyID;
        // [SerializeField]
        private TKeyOption _optionID;
        // [SerializeField]
        private string _optionTypeName;
        private Type _optionType;

        public InjectionKey(TKeyOption optionID, TKeyObj keyID, Type optionType)
        {
            _keyID = keyID;
            _optionID = optionID;
            _optionType = optionType;
            _optionTypeName = optionType.FullName;
        }

        public TKeyObj keyID => _keyID;
        public TKeyOption optionID => _optionID;
        public string optionTypeName => _optionTypeName;
        public Type optionType => _optionType;
    }
}