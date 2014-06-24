using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IViewState
    {
        // View state name
        string Name { get; }

        // Isdefault property
        bool IsDefaultViewState { get; }
    }
}
