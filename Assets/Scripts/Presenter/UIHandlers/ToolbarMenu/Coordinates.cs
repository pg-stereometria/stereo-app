using StereoApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace StereoApp.Presenter.UIHandlers.ToolbarMenu
{
    public class Coordinates : MonoBehaviour
    {
        public Model.Point point;
        public TMP_Dropdown dropdown;
        public TMP_InputField xCoordinate;
        public TMP_InputField yCoordinate;
        public TMP_InputField zCoordinate;
        public bool noChangingPoints = false;
        public UnityEvent onValueChanged;

        private Model.Polyhedron _currentSolid;
        public Model.Polyhedron CurrentSolid
        {
            get => _currentSolid;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _currentSolid = value;
                Initialize();
            }
        }

        public void Initialize()
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(new List<string>() { "New Point" });
            dropdown.AddOptions(AppManager.Instance.points.Select(s => s.ToString()).ToList());
            dropdown.value = 0;
            UpdateDataFromDropdown();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Initialize();
        }

        public void OnChange()
        {
            onValueChanged.Invoke();
        }

        public void UpdateDataFromDropdown()
        {
            var value = dropdown.options[dropdown.value].text;
            
            if (value == "New Point")
            {
                point = null;
                xCoordinate.text = "";
                yCoordinate.text = "";
                zCoordinate.text = "";
                xCoordinate.interactable = true;
                yCoordinate.interactable = true;
                zCoordinate.interactable = true;
                return;
            }

            if(noChangingPoints)
            {
                xCoordinate.interactable = false;
                yCoordinate.interactable = false;
                zCoordinate.interactable = false;
            }

            point = AppManager.Instance.points.Where(s => s.ToString() == value).Single();
            var pattern = @"-?\d+\.?\d*";
            var match = Regex.Match(value, pattern);
            xCoordinate.text = match.Value;
            match = match.NextMatch();
            yCoordinate.text = match.Value;
            match = match.NextMatch();
            zCoordinate.text = match.Value;
        }

        public void SelectPoint(Model.Point point)
        {
            this.point = point;
            dropdown.value = dropdown.options.FindIndex(s => s.text == point.ToString());
            xCoordinate.text = point.X.ToString("0.##");
            yCoordinate.text = point.Y.ToString("0.##");
            zCoordinate.text = point.Z.ToString("0.##");
        }
    }
}
