using Quartz;

namespace Service.Quartz
{
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            string code = string.Empty;
            string mess = string.Empty;
            try
            {
                //Console.WriteLine("hello");
            }
            catch (Exception)
            {
                //log.ErrorFormat("departmentset ", "Department object sent from client is {error} ", ex.ToString());
            }
            // return Task.CompletedTask;
        }
    }


}
