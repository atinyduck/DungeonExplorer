using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal interface ISaveable
    {
        string GetSaveIdentifier();
    }
}
