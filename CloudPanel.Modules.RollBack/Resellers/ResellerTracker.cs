using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.RollBack.Resellers
{
    public class ResellerTracker
    {
        private readonly List<RollBackTracker> Actions;

        public ResellerTracker()
        {
            Actions = new List<RollBackTracker>();
        }

        #region Add Actions

        /// <summary>
        /// That we completed creating the new organizational unit for the reseller
        /// </summary>
        /// <param name="distinguishedName"></param>
        public void CompletedNewOU(string distinguishedName)
        {
            Actions.Add(new RollBackTracker()
            {
                Action = Base.Enumerations.RollBackAction.CreateResellerOU,
                Variable1 = distinguishedName
            });
        }

        /// <summary>
        /// That we completed creating the GPOAccess security group for the reseller
        /// </summary>
        /// <param name="groupName"></param>
        public void CompletedNewGPOAccess(string groupName)
        {
            Actions.Add(new RollBackTracker()
            {
                Action = Base.Enumerations.RollBackAction.CreateResellerGPOAccessGroup,
                Variable1 = groupName
            });
        }

        #endregion
    }
}
