using UnityEngine;

namespace StereoApp
{
    public class Point : MonoBehaviour
    {
        private Color _baseColor = Color.blue;
        private Color _selectionColor = Color.red;

        private Renderer _renderer;

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UpdateMaterialColor();
            }
        }

        void Start()
        {
            _renderer = GetComponent<Renderer>();
            UpdateMaterialColor();
        }

        private void UpdateMaterialColor()
        {
            var color = _baseColor;
            if (_isSelected)
            {
                color = _selectionColor;
            }
            _renderer.material.color = color;
        }
    }
}
