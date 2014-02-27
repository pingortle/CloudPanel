using CloudPanel.Modules.Base.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class IViewModel
    {
        public delegate void ViewModelEventHandler(AlertID errorID, string message);
        public event ViewModelEventHandler ViewModelEvent;

        public void ThrowEvent(AlertID errorID, string message)
        {
            if (ViewModelEvent != null)
                ViewModelEvent(errorID, message);
        }
    }
}
