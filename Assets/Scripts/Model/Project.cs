using System;
using System.IO;
using StereoApp.Model.Interfaces;
using UnityEngine;

namespace StereoApp.Model
{
    public class Project : ISerializableTo<Project, SerializedProject>
    {
        public static readonly string ProjectsDir = Path.Join(
            Application.persistentDataPath,
            "projects"
        );

        public SolidFigure Figure { get; set; }
        public string ProjectName { get; set; }

        public Project(SolidFigure figure)
        {
            Figure = figure;
        }

        public SerializedProject ToSerializable()
        {
            return new SerializedProject { figure = Figure.ToSerializableFigure() };
        }

        public static Project FromProjectName(string projectName)
        {
            var projectPath = Path.Join(ProjectsDir, projectName);
            using var fs = new StreamReader(projectPath);
            var project = JsonSerializable.FromJson<Project, SerializedProject>(fs.ReadToEnd());
            project.ProjectName = projectName;
            return project;
        }

        public void Save()
        {
            if (ProjectName is null)
            {
                throw new InvalidOperationException("Project name is not set.");
            }

            var projectPath = Path.Join(ProjectsDir, ProjectName);
            var tmpProjectPath = projectPath + ".tmp";

            Directory.CreateDirectory(Path.GetDirectoryName(projectPath)!);

            try
            {
                using var fs = new StreamWriter(tmpProjectPath);
                fs.Write(this.ToJson());
                fs.Close();

                // atomic operation
                File.Replace(tmpProjectPath, projectPath, null);
                // Unity is still on .NET Standard 2.1 and doesn't support File.Move() with
                // override argument that I should be using here instead but I guess this works...
            }
            finally
            {
                File.Delete(tmpProjectPath);
            }
        }
    }

    [Serializable]
    public class SerializedProject : ISerializableFrom<SerializedProject, Project>
    {
        public SerializedSolidFigure figure;

        public Project ToActualType()
        {
            return new Project(figure.ToActualFigure());
        }
    }
}
