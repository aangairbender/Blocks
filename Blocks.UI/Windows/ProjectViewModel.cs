using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Blocks.Core;
using Blocks.Library.Components;
using Blocks.Library.Components.Physics;
using Blocks.Library.Components.Rendering;
using Blocks.UI.Annotations;
using Blocks.UI.Controls;

namespace Blocks.UI.Windows
{
    public class ProjectViewModel : ViewModelBase
    {
        private Project _project;

        public string ProjectName => _project.Name;

        public Workspace Workspace => _project.Workspace;

        public ObservableCollection<NodeViewModel> Blocks => new ObservableCollection<NodeViewModel>()
            {new NodeViewModel(_project.Workspace.RootBlock, null)};

        private Block _selectedBlock;

        public Block SelectedBlock
        {
            get => _selectedBlock;
            set
            {
                _selectedBlock = value;
                OnPropertyChanged(nameof(SelectedBlock));
            }
        }

        public ProjectViewModel(Project project)
        {
            _project = project;
        }
    }
}