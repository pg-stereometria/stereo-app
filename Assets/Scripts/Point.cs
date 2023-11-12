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
        private bool _isMouseOver = false;
        public bool IsMouseOver
        {
            get => _isMouseOver;
            private set
            {
                _isMouseOver = value;
                UpdateMaterialColor();
            }
        }

        void Start()
        {
            _renderer = GetComponent<Renderer>();
            UpdateMaterialColor();
        }

        //void OnMouseEnter()
        //{
        //    IsMouseOver = true;
        //    EditSpaceController.Instance.OnPointMouseEnter(this);
        //}

        //void OnMouseExit()
        //{
        //    IsMouseOver = false;
        //    EditSpaceController.Instance.OnPointMouseExit(this);
        //}

        //void OnMouseOver()
        //{
        //    EditSpaceController.Instance.OnPointMouseOver(this);
        //}

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
