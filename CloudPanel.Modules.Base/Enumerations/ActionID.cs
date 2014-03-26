using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Enumerations
{
    public enum ActionID
    {
        CreateUser = 1,
        DeleteUser = 2,
        UpdateUser = 3,
        EnableMailbox = 4,
        DisableMailbox = 5,
        UpdateMailbox = 6,
        EnableExcahnge = 7,
        DisableExchange = 8,
        EnableAcceptedDomain = 9,
        DisableAcceptedDomain = 10,
        AddDomain = 11,
        DeleteDomain = 12,
        CreateContact = 13,
        DeleteContact = 14,
        CreateCompany = 15,
        DeleteComapny = 16,
        CreateReseller = 17,
        DeleteReseller = 18,
        SaveSettings = 19,
        UpdateCompany = 20,
        UpdateReseller = 21,
        UpdateDomain = 22,
        ResetPassword = 23
    }
}
