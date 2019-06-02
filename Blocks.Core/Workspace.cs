using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Blocks.Core
{
    public class Workspace
    {
        public Block RootBlock { get; }

        public Workspace()
        {
            RootBlock = new Block("Workspace");
        }
    }
}