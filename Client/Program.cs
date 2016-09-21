using FlatBuffers;
using GameObject;
using SuperSocket.ClientEngine;
using System;
using System.Net;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1);
            var weaponOneName = builder.CreateString("Sword");
            var weaponOneDamage = 3;
            var weaponTwoName = builder.CreateString("Axe");
            var weaponTwoDamage = 5;
            // Use the `CreateWeapon()` helper function to create the weapons, since we set every field.
            var sword = Weapon.CreateWeapon(builder, weaponOneName, (short)weaponOneDamage);
            var axe = Weapon.CreateWeapon(builder, weaponTwoName, (short)weaponTwoDamage);

            var name = builder.CreateString("Orc");

            Monster.StartInventoryVector(builder, 10);
            for (int i = 9; i >= 0; i--)
            {
                builder.AddByte((byte)i);
            }
            var inv = builder.EndVector();

            var weaps = new Offset<Weapon>[2];
            weaps[0] = sword;
            weaps[1] = axe;

            // Pass the `weaps` array into the `CreateWeaponsVector()` method to create a FlatBuffer vector.
            var weapons = Monster.CreateWeaponsVector(builder, weaps);
            var pos = Vec3.CreateVec3(builder, 1.0f, 2.0f, 3.0f);

            Monster.StartMonster(builder);
            Monster.AddPos(builder, pos);
            Monster.AddHp(builder, (short)300);

            Monster.AddName(builder, name);
            Monster.AddInventory(builder, inv);
            Monster.AddColor(builder, Color.Red);

            Monster.AddWeapons(builder, weapons);
            Monster.AddEquippedType(builder, Equipment.Weapon);
            Monster.AddEquipped(builder, axe.Value); // Axe

            var orc = Monster.EndMonster(builder);
            builder.Finish(orc.Value);
            var buf = builder.SizedByteArray();
            
            // Initialize the client with the receive filter and request handler
            foo(buf);
            while (true) ;
        }

        static private async void foo(byte[] buf)
        {
            var client = new EasyClient();
            client.Initialize(new MyReceiveFilter(4), (request) =>
            {
                // handle the received request
                Console.WriteLine(request.Key);
            });
            while(true)
            {
                if(!client.IsConnected)
                {
                    var connected = false;
                    while (!connected)
                    {
                        connected = await client.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
                    }
                    //while (!client.IsConnected) ;
                    if (connected)
                    {
                        Console.WriteLine("Connected to Server");
                        Console.WriteLine("Buffer length: " + buf.Length);
                        string key = "hi";
                        int length = buf.Length;


                        byte[] data = AppendByte(AppendByte(GetBytes(key), BitConverter.GetBytes(length)), buf);
                        client.Send(buf);
                        Console.WriteLine("Sent to Server");
                    }
                }
            }
        }
        static private byte[] GetBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str.ToCharArray());
        }

        private static byte[] AppendByte(byte[] current, byte[] after)
        {
            var bytes = new byte[current.Length + after.Length];
            current.CopyTo(bytes, 0);
            after.CopyTo(bytes, current.Length);
            return bytes;
        }

    }
}
