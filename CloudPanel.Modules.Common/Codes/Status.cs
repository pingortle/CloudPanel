using CloudPanel.Modules.Base.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.Codes
{
    public class Status<T>
    {
        public readonly T Object;
        public readonly String Message;

        public Status(ErrorID _code, string _message)
        {
            this.Message = _message;
        }        
    }
}
