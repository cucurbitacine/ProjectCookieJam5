using System.Collections;

namespace Game.Scripts
{
    public abstract class YieldProcess
    {
        public bool InProgress { get; private set; }
        public bool Done { get; private set; }
        public bool Canceled { get; private set; }

        public IEnumerator Process()
        {
            InProgress = true;
            Done = false;
            
            yield return OnProcess();
            
            InProgress = false;
            Done = true;
        }

        public void Cancel()
        {
            Canceled = true;
        }
        
        protected abstract IEnumerator OnProcess();
    }

    public abstract class YieldProcess<T> : YieldProcess
    {
        public T Result { get; private set; }

        protected sealed override IEnumerator OnProcess()
        {
            yield return OnProcessWithResult();

            Result = GetResult();
        }
        
        protected abstract IEnumerator OnProcessWithResult();
        protected abstract T GetResult();
    }
}