using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Task.DB;
using Task.TestHelpers;

namespace ConsoleTets
{
    class Program
    {
        static void Main(string[] args)
        {
            var  dbContext = new Northwind();

            dbContext.Configuration.ProxyCreationEnabled = true;
            dbContext.Configuration.LazyLoadingEnabled = true;

            StreamingContext sc = new StreamingContext(
                StreamingContextStates.CrossProcess, (dbContext as IObjectContextAdapter).ObjectContext);

            SurrogateSelector ss = new SurrogateSelector();

            ss.AddSurrogate(typeof(Order_Detail),
                sc,
                new Order_DetailSerializationSurrogate());

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(new DataContractSerializer(typeof(IEnumerable<Order>), new DataContractSerializerSettings { DataContractSurrogate = new OrderDataContractSurrogate(), PreserveObjectReferences = false}), true);
            var orders = dbContext.Orders.ToList();

            tester.SerializeAndDeserialize(orders);

            Console.ReadKey();
        }
    }
}
