using Enums;
using UnityEngine;

namespace Interfaces
{
    public interface IDimensional 
    {
        public DimensionType DimensionType { get; }

        void Show();

        void Hide();

    }
}
