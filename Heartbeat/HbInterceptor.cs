using System;
using System.Diagnostics;
using Castle.DynamicProxy;


namespace Heartbeat
{ 
    public class HbInterceptor : IInterceptor
    {
        private readonly ClientProcessor _clientProcessor;

        public HbInterceptor(ClientProcessor clientProcessor)
        {
            _clientProcessor = clientProcessor;
        }

        public void Intercept(IInvocation invocation)
        {
            MethodExecutionInfo context = new MethodExecutionInfo();
            context.MethodName = invocation.Method.Name;
            context.ExecutionDate = DateTime.Now;
            context.HasException = false;
            var watch = Stopwatch.StartNew();
            try
            {
                invocation.Proceed();    
            }
            catch (Exception ex)
            {
                try
                {
                    watch.Stop();
                    context.Duration = watch.ElapsedMilliseconds;
                    context.HasException = true;
                    _clientProcessor.AddHeartbeatContext(context);
                }
                catch
                {
                    // ignored
                }
                throw;
            }

            watch.Stop();
            context.Duration = watch.ElapsedMilliseconds;
            _clientProcessor.AddHeartbeatContext(context);
        }
    }
}