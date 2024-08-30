
using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Symbols
{
    [Serializable]
    [CreateAssetMenu(
        fileName    = "Boolean Symbol",
        menuName    = "Buganamo/MainFrame/Core/Symbols/" +
                      "Boolean"
    )]
    public class BoolSymbol : AbstractSymbol<bool> { }
}