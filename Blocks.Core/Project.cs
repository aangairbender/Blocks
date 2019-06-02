namespace Blocks.Core
{
    public class Project
    {
        public string Name { get; }

        public string Path { get; }

        public Workspace Workspace { get; }

        public Project(string name, string path)
        {
            Name = name;
            Path = path;
            Workspace = new Workspace();
        }
    }
}