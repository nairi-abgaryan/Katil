using Quartz;

namespace Katil.Scheduler.Task.Infrastructure
{
    public abstract class JobBase : IJob
    {
        public JobBase()
        {
        }

        public virtual System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
