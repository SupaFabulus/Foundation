using System;
using SupaFabulus.Dev.Foundation.Core.Enums;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Symbols
{
    [Serializable]
    [CreateAssetMenu(
        fileName    = "BasicValueTypeID Symbol",
        menuName    = "Buganamo/MainFrame/Core/Symbols/" +
                      "Basic Value Type Identifier"
    )]
    public class BasicValueTypeSymbol : AbstractSymbol<BasicValueType> { }
}