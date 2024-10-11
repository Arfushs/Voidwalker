using Enums;
using UnityEngine;

namespace Interfaces
{
    public interface IDimensional
    {

        void Show();

        void Hide();

        DimensionType GetDimensionType();
    }
}
