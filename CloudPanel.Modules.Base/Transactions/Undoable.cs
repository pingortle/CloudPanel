using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudPanel.Modules.Base
{
    public class Undoable : IUndoable
    {
        private bool _hasDone = false;
        private bool _hasUndone = false;
        private readonly Action _do;
        private readonly Action _undo;

        public Undoable(Action dodo, Action undo)
        {
            if (dodo == null) throw new ArgumentNullException("Must have a Do.");
            if (undo == null) throw new ArgumentNullException("Must have an Undo.");

            _do = dodo;
            _undo = undo;
        }

        public void Do()
        {
            if (_hasDone) throw new InvalidOperationException("Don't do it again!");

            _do();
            _hasDone = true;
        }

        public void Undo()
        {
            if (!_hasDone) throw new InvalidOperationException("One cannot undo what has not yet been done. Make sure you successfully Do() before you Undo().");
            if (_hasUndone) throw new InvalidOperationException("Don't undo it again!");

            _undo();
            _hasUndone = true;
        }
    }

    public class IndependentUndoableGroup : IUndoable
    {
        protected IEnumerable<IUndoable> Undoables { get; private set; }
        public IndependentUndoableGroup(IEnumerable<IUndoable> undoables)
        {
            Undoables = undoables.ToList();
        }

        public virtual void Do()
        {
            foreach (IUndoable u in Undoables)
                u.Do();
        }

        public virtual void Undo()
        {
            foreach (IUndoable u in Undoables)
                u.Undo();
        }
    }

    public class ParalellUndoableGroup : IndependentUndoableGroup
    {
        public ParalellUndoableGroup(IEnumerable<IUndoable> undoables) : base(undoables) { }

        public override void Do()
        {
            Undoables.AsParallel().ForAll(u => u.Do());
        }

        public override void Undo()
        {
            Undoables.AsParallel().ForAll(u => u.Undo());
        }
    }
}
