using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Dashboard
{
    public class OverviewStats
    {
        #region Getters & Setters

            public int TotalUsers { get; set; }

            public int Resellers { get; set; }

            public int Companies { get; set; }

            public int Mailboxes { get; set; }

            public int CitrixUsers { get; set; }

            public int LyncUsers { get; set; }

        #endregion

        #region Percentage Methods

        /// <summary>
        /// Gets the percent of mailboxes to total users
        /// </summary>
        public int MailboxPercent
        {
            get
            {
                if (Mailboxes > 0)
                    return (200 * Mailboxes + 1) / (TotalUsers * 2);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the percent of citrix users to total users
        /// </summary>
        public int CitrixPercent
        {
            get
            {
                if (CitrixUsers > 0)
                    return (200 * Mailboxes + 1) / (TotalUsers * 2);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the percent of lync users to total users
        /// </summary>
        public int LyncPercent
        {
            get
            {
                if (LyncUsers > 0)
                    return (200 * Mailboxes + 1) / (TotalUsers * 2);
                else
                    return 0;
            }
        }

        #endregion
    }
}
