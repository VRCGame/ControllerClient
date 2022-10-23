using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime;
using System.Runtime.CompilerServices;

#pragma warning disable CS8618

namespace VRCGame.ControllerClient
{
    internal class Program
    {
        public static StreamWriter writer;
        public static string Callsign;
        public static int ID;
        public static int Rating;
        public static string Frequency;
        public static double Lat;
        public static double Lng;

        static async void Main(string callsign, string frequency, double lat, double lng, int id)
        {
            if (callsign == null || frequency == null || lat = null || lng == null || id == null)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            Callsign = callsign;
            ID = id;
            Frequency = frequency;
            Lat = lat;
            Lng = lng;

            if (Callsign.EndsWith("_GND"))
            {
                Rating = 2;
                //S1 

            } else if (Callsign.EndsWith("_TWR"))
            {
                Rating = 3;
                //S2

            }
            else if (Callsign.EndsWith("_APP") || Callsign.EndsWith("_DEP"))
            {
                Rating = 4;
                //S3

            } else if (Callsign.EndsWith("_CTR"))
            {
                Rating = 5;
                //C1

            }

            TcpClient Client = new("127.0.0.1", 6809);
            NetworkStream stream = Client.GetStream();
            StreamReader reader = new(stream);
            writer = new(stream) { AutoFlush = true };

            while (true)
            {
                var info = await reader.ReadLineAsync();
                
                if (await ProcessLine(info))
                {
                    break;
                }
            }

        }

        public static async Task<bool> ProcessLine(string info)
        {
            if (info == null)
            {
                
                throw new ArgumentNullException(nameof(info));
            }
            
            if (info.StartsWith("$DI"))
            {
                //  Send ID Packet
                await Send($"ID{Callsign}:SERVER:de1e:VRC:3:2:{ID}:123456789 #AA{Callsign}:SERVER:Simulated Controller:{ID}:{ID}:{Rating}:100 %{Callsign}:{Frequency}:0:150:{Rating}:{Lat}:{Lng}");
            }

            return false;
        }

        public static async Task Send(string data)
        {
            if (writer != null)
            {
                await writer.WriteLineAsync(data);
            }
        }

        
    }
}