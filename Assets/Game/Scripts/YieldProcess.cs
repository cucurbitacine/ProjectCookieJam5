using System.Collections;

namespace Game.Scripts
{
    public abstract class YieldProcess
    {
        public abstract IEnumerator Process();
    }
    
    public abstract class YieldProcess<T> : YieldProcess
    {
        public T Result { get; private set; }

        public override IEnumerator Process()
        {
            yield return OnProcess();

            Result = GetResult();
        }
        
        protected abstract IEnumerator OnProcess();
        protected abstract T GetResult();
    }
}