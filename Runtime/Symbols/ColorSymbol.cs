using System;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.Symbols
{
    [Serializable]
    [CreateAssetMenu(
        fileName = "Color Symbol",
        menuName = "Buganamo/MainFrame/Core/Symbols/" +
                   "Color"
    )]
    public class ColorSymbol : AbstractSymbol<Color>
    {
        
    }
}