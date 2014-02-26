using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Base
{
	public interface IUndoable
	{
        void Do();
        void Undo();
	}
}
