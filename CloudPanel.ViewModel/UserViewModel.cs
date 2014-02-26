using System;
using CloudPanel.Modules.Base;
using CloudPanel.Modules.ActiveDirectory;

namespace CloudPanel.ViewModel
{
    public class UserViewModel
    {
        public void Save()
        {
            // This is a list of steps required to save a new user.
            var stepsToSave = new[]
            {
                // You may use lambdas to pass your logic into Undoables.
                new Undoable(
                    () => CreateUser(),
                    () => UndoCreateUser()
                ),

                // Or you can just pass methods that match the Action signature.
                new Undoable(AddToSecurityGroup, UndoAddToSecurityGroup),
                new Undoable(SaveToSQL, UndoSaveToSQL),
            };

            var transaction = new Transaction(stepsToSave);

            if (transaction.TryExecute())
            {
                // It worked!
            }
            else
            {
                // It didn't work.
            }
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public string Department { get; set; }

        public string LoginName { get; set; }

        public bool CompanyAdmin { get; set; }
        public bool ReselerAdmin { get; set; }

        public bool PasswordExpires { get; set; }

        #region Private Utility
        // Empty example funcitons:
        private void CreateUser() {}
        private void AddToSecurityGroup() {}
        private void SaveToSQL() {}
        private void UndoCreateUser() {}
        private void UndoAddToSecurityGroup() {}
        private void UndoSaveToSQL() {}
        #endregion
    }
}
