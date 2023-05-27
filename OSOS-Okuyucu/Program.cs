using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using DataAccess.Concrete.EntityFramework;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

namespace OSOS_Okuyucu
{
    internal class Program
    {
        static MongoClient _client = new MongoClient("mongodb://localhost/");

        static async Task Main(string[] args)
        {
            System.Timers.Timer timerHourly = new System.Timers.Timer(new TimeSpan(0, 0, 0, 30).TotalMilliseconds);
            timerHourly.AutoReset = true;
            timerHourly.Elapsed += TimerHourly_Elapsed;
            timerHourly.Start();
            System.Timers.Timer timerDaily = new System.Timers.Timer(new TimeSpan(0, 0, 1, 0).TotalMilliseconds);
            timerDaily.AutoReset = true;
            timerDaily.Elapsed += TimerDaily_Elapsed;
            timerDaily.Start();
            System.Timers.Timer timerWeekly = new System.Timers.Timer(new TimeSpan(7, 0, 0, 0).TotalMilliseconds);
            timerWeekly.AutoReset = true;
            timerWeekly.Elapsed += TimerWeekly_Elapsed;
            timerWeekly.Start();


                while (true)
                {

                }
        }

        private async static void TimerWeekly_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Weekly");
            var weeklyId = new EfReadingTimeDal().Get(t => t.Name == "Weekly").Id;
            var weeklyMeters = new EfMeterDal().GetAll(m => m.ReadingTimeId == weeklyId);
            Parallel.For(0, weeklyMeters.Count, new ParallelOptions { MaxDegreeOfParallelism = 15 }, async index =>
            {
                var reading = await FetchReadingAsync(weeklyMeters[index].SerialNo);
                InsertValue(reading);
            });
        }

        private async static void TimerDaily_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Daily");
            var dailyId = new EfReadingTimeDal().Get(t => t.Name == "Daily").Id;
            var dailyMeters = new EfMeterDal().GetAll(m => m.ReadingTimeId == dailyId);
            Parallel.For(0, dailyMeters.Count, new ParallelOptions { MaxDegreeOfParallelism = 15 }, async index =>
            {
                var reading = await FetchReadingAsync(dailyMeters[index].SerialNo);
                InsertValue(reading);
            });
        }

        private async static void TimerHourly_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
         {
             Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
             Console.WriteLine("Hourly");
             var hourlyId = new EfReadingTimeDal().Get(t => t.Name == "Hourly").Id;
             var hourlyMeters = new EfMeterDal().GetAll(m => m.ReadingTimeId == hourlyId);

             Parallel.For(0, hourlyMeters.Count, new ParallelOptions { MaxDegreeOfParallelism = 15 }, async index =>
             {
                 var reading =await FetchReadingAsync(hourlyMeters[index].SerialNo);
                 InsertValue(reading);
             });
         }

        /*
        private static void TimerHourly_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Hourly");

            Parallel.For(1, 101, new ParallelOptions { MaxDegreeOfParallelism = 15 }, async index =>
            {
                FetchReading(index);
            });
        }*/

        static async Task<Reading> FetchReadingAsync(int Obis000)
        {
            //Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId + " Value:" + Obis000);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"http://localhost:8000/?Obis000={Obis000}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var reading = JsonConvert.DeserializeObject<Reading>(responseBody);
                        return reading;
                    }
                }
                catch (Exception ex)
                {
                    
                }
                finally { }
                return null;
            }
        }
        /*static void FetchReading(int Obis000)
        {
            //Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId+" Value:"+Obis000);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = client.GetAsync($"http://localhost:8000/?Obis000={Obis000}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        var reading = JsonConvert.DeserializeObject<Reading>(responseBody);
                        InsertValue(reading);
                    }
                }
                catch (Exception ex)
                {

                }
                finally { }
            }
        }*/
        static void InsertValue(Reading reading)
        {
            IMongoDatabase _db = _client.GetDatabase("Readings");
            IMongoCollection<Reading> Collection = _db.GetCollection<Reading>("Reading");
            Collection.InsertOne(reading);
        }

    }
}