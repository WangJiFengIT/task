using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("第一步：沟通需求");
            Console.WriteLine("第二步：开始研发");

            //Calculate("架构", "系统设计");
            //Calculate("DBA", "数据库设计");
            //Calculate("前端", "编写页面");
            //Calculate("后端", "服务开发");

            TaskFactory taskFactory = new TaskFactory();
            List<Task> taskList = new List<Task>();

            Action ac1 = new Action(() => Calculate("架构", "系统设计"));
            Action ac2 = new Action(() => Calculate("DBA", "数据库设计"));
            Action ac3 = new Action(() => Calculate("前端", "编写页面"));
            Action ac4 = new Action(() => Calculate("后端", "服务开发"));

            //创建并开启一个线程
            taskList.Add(taskFactory.StartNew(ac1));
            taskList.Add(taskFactory.StartNew(ac2));
            taskList.Add(taskFactory.StartNew(ac3));
            taskList.Add(taskFactory.StartNew(ac4));

            //多线程执行结束后 另一任务的开始
            Action<Task[]> actAll = new Action<Task[]>(t => Console.WriteLine($"第三步：开发完成，进行联调 线程ID{Thread.CurrentThread.ManagedThreadId}"));
            Task taskAll = taskFactory.ContinueWhenAll(taskList.ToArray(), actAll);

            //等待某个线程执行完成
            Task.WaitAny(taskList.ToArray());

            //将（多线程执行结束后 另一任务的开始） 这个线程放入线程集合
            taskList.Add(taskAll);
            //等待所有线程执行完成
            Task.WaitAll(taskList.ToArray());



            Console.WriteLine("第四步：研发完毕");

            /// <summary>
            /// 耗时的计算方法
            /// </summary>
            /// <param name="name"></param>
            void Calculate(string name, string method)
            {
                int result = 0;
                for (int i = 0; i < 999999999; i++)
                {
                    result += i;
                }
                Console.WriteLine($"{name}执行了{method}，ID={Thread.CurrentThread.ManagedThreadId},result={result}");
            }
        }


    }
}
