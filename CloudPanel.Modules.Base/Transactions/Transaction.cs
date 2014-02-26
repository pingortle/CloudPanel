using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudPanel.Modules.Base
{
    public class Transaction
    {
        private IEnumerable<IUndoable> _steps;

        public Transaction(IEnumerable<IUndoable> steps)
        {
            _steps = steps;
        }

        public bool TryExecute()
        {
            var successes = new List<IUndoable>();
            foreach (IUndoable step in _steps)
            {
                try
                {
                    step.Do();
                    successes.Add(step);

                    System.Diagnostics.Debug.Print("Step succeeded!");
                }
                catch
                {
                    System.Diagnostics.Debug.Print("Step failed! Rolling back...");

                    foreach (IUndoable success in successes.AsEnumerable().Reverse())
                    {
                        success.Undo();
                    }

                    System.Diagnostics.Debug.Print("Rollback complete!");
                    break;
                }
            }

            return successes.Count == _steps.Count();
        }

    }
}
