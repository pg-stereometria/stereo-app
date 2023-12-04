using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StereoApp.UIHandlers
{
    public class Coordinates : MonoBehaviour
    {
        public Model.Point point;
        public TMP_Dropdown dropdown;
        public TMP_InputField xCoordinate;
        public TMP_InputField yCoordinate;
        public TMP_InputField zCoordinate;

        private Model.SolidFigure _currentSolid;
        public Model.SolidFigure CurrentSolid
        {
            get => _currentSolid;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                _currentSolid = value;
                dropdown.ClearOptions();
                dropdown.AddOptions(new List<string>() { "New Point" });
                dropdown.AddOptions(value.Points.Select(s => s.ToString()).ToList());
                dropdown.value = 0;
            }
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
                return;
            }
            point = CurrentSolid.Points.Where(s => s.ToString() == value).Single();
            string pattern = @"-?\d+\.?\d*";
            Match match = Regex.Match(value, pattern);
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
            xCoordinate.text = point.X.ToString();
            yCoordinate.text = point.Y.ToString();
            zCoordinate.text = point.Z.ToString();
        }
    }
}
