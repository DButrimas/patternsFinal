using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TanksMP_Client.Models;

namespace TanksMP_Client
{
    public partial class Form1 : Form
    {
        public List<Player> players = new List<Player>();
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await RunAsync();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        Player myPlayer = new Player();


        static HttpClient client = new HttpClient();

        static void ShowProduct(Player player)
        {
            //Console.WriteLine($"Name: {player.Name}");

        }

        static async Task<Uri> CreateProductAsync(Player player)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/players", player);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Player> GetProductAsync(string path)
        {
            Player player = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                player = await response.Content.ReadAsAsync<Player>();
            }
            return player;
        }


        static async Task<Player> UpdateProductAsync(Player player)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/players/", player);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            player = await response.Content.ReadAsAsync<Player>();
            return player;
        }

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/players/{id}");
            return response.StatusCode;
        }



        static async Task<ICollection<Player>> GetAllPlayerAsync(string path)
        {
            ICollection<Player> players = null;
            HttpResponseMessage response = await client.GetAsync(path + "api/players");
            if (response.IsSuccessStatusCode)
            {
                players = await response.Content.ReadAsAsync<ICollection<Player>>();
            }
            return players;
        }



        public async Task<string> BuildMap(string path)
        {

            string m = "";
            HttpResponseMessage response = await client.GetAsync(path + "api/map/");
            if (response.IsSuccessStatusCode)
            {
                m = await response.Content.ReadAsAsync<string>();
            }
            return m;
        }
        public async Task<string> UpdateMap(string path)
        {

            string m = "";
            HttpResponseMessage response = await client.GetAsync(path + "api/map/update/");
            if (response.IsSuccessStatusCode)
            {
                m = await response.Content.ReadAsAsync<string>();
            }
            return m;
        }

        public async Task<Player> createPlayer(string path)
        {
            Player p = null;
            HttpResponseMessage response = await client.PostAsync(path + "api/players/", null);
            if (response.IsSuccessStatusCode)
            {
                p = await response.Content.ReadAsAsync<Player>();
            }
            return p;
        }

        public async Task<int> createMessage(string path, Message m)
        {
            int p = 1;
            HttpResponseMessage response = await client.PostAsJsonAsync(path + "api/Chat/", m);
            if (response.IsSuccessStatusCode)
            {
                p = await response.Content.ReadAsAsync<int>();
            }
            return p;
        }
        static async Task<ICollection<Message>> GetMessagesAsync(string path)
        {
            ICollection<Message> messages = null;
            HttpResponseMessage response = await client.GetAsync(path + "api/chat/");
            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<ICollection<Message>>();
            }
            return messages;
        }



        public Map map;
        List<IBlock> blocks = new List<IBlock>();

        Player mePlayer = new Player();
        async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:44319/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            char[] ss = { '[', ']' };
            String asd = await BuildMap(client.BaseAddress.PathAndQuery);
            List<String> splited = asd.Split(ss).ToList();
            splited = splited.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            string jsonCorrect = "";
            string jsonCorrect2 = "";
            string jsonCorrect3 = "";
            int cnt = splited.Count();

            if (cnt != 3)
            {
                return;
            }
            jsonCorrect += "[" + splited[0] + "]";
            var model = JsonConvert.DeserializeObject<List<Brick>>(jsonCorrect);
            foreach (var item in model)
            {
                blocks.Add(item);
            }
            jsonCorrect2 += "[" + splited[1] + "]";
            var modelw = JsonConvert.DeserializeObject<List<Water>>(jsonCorrect2);
            foreach (var item in modelw)
            {
                blocks.Add((Water)item);
            }
            jsonCorrect3 += "[" + splited[2] + "]";
            var model3 = JsonConvert.DeserializeObject<List<Ground>>(jsonCorrect3);
            foreach (var item in model3)
            {
                blocks.Add((Ground)item);
            }



            // textBox1.Text = map.SizeX.ToString();
            // ICollection<Player> playersList = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            //    foreach (Player p in playersList)
            //{
            //Console.WriteLine(p.Name + "\n");

            // }

            //var url = await CreateProductAsync(product);
            //textBox1.Text += "Created player: " + url + "\n";
            ////  Console.WriteLine($"Created at {url}");
            //button1.Enabled = false;

            //// Console.WriteLine(cbb);
            ////  var url = await CreateProductAsync(product);
            ////Console.WriteLine($"Created at {url}");
            ////  string temp = $"{cbb} Create at {url}";
            ////  cbb = temp;

            //// Get the product
            //product = await GetProductAsync(url.PathAndQuery);
            //// ShowProduct(product);

            ////// Update the product
            //textBox1.Text += $"Updating name: {product.Name}\n";
            ////    Console.WriteLine("Updating Name...");
            //product.Name = "UpdatedName";
            //await UpdateProductAsync(product);

            //////// Get the updated product
            //product = await GetProductAsync(url.PathAndQuery);
            //textBox1.Text += $"New name: {product.Name} \n";
            ////ShowProduct(product);

            //myPlayer = product;
            // Hook up the Elapsed event for the timer.

            mePlayer = await createPlayer(client.BaseAddress.PathAndQuery);

            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);

            players = playersTemp.ToList();

            pColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

            //   panel2.Invalidate();
            timer1.Interval = 1;

            timer1.Start();
        }

        Random rnd = new Random();
        Color pColor;

        int mapSizeX = 20;
        /// <summary>
        /// MAP 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = panel1.CreateGraphics();

            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.Red);

            foreach (var item in players)
            {
                p.Color = Color.Black;
                sb.Color = pColor;
                g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX + 5, mapSizeX, 20);
                g.FillRectangle(sb, (item.getPosX() * mapSizeX), (item.getPosY() * mapSizeX) + 5, mapSizeX + 5, mapSizeX + 5);
            }
            foreach (var item in blocks)
            {
                if (item != null)
                {
                    if (BlockType.Brick.ToString() == item.getType())
                    {
                        p.Color = Color.Black;
                        sb.Color = Color.Red;
                        g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, mapSizeX);
                        g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);
                    }
                    else if (BlockType.Water.ToString() == item.getType())
                    {
                        p.Color = Color.Black;
                        sb.Color = Color.Blue;
                        g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, mapSizeX);
                        g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX, mapSizeX - 1);

                    }
                    else if (BlockType.Ground.ToString() == item.getType())
                    {
                        p.Color = Color.BurlyWood;
                        sb.Color = Color.BurlyWood;
                        g.DrawRectangle(p, item.getPosX() * 20, item.getPosY() * mapSizeX, mapSizeX, 20);
                        g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);

                    }
                    else if (BlockType.Iron.ToString() == item.getType())
                    {
                        p.Color = Color.Black;
                        sb.Color = Color.DimGray;
                        g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, 20);
                        g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);

                    }
                    else if (BlockType.Border.ToString() == item.getType())
                    {
                        p.Color = Color.DarkSlateGray;
                        sb.Color = Color.Black;
                        g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, 20);
                        g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);

                    }
                }

            }



        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        int asd2 = 0;

        Pen p = new Pen(Color.Black);
        SolidBrush sb = new SolidBrush(Color.Red);
        private async void timer1_Tick(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            string text = "";
            var asss = await GetMessagesAsync(client.BaseAddress.PathAndQuery);
            List<Message> msgs = asss.ToList();

            foreach (var item in msgs)
            {
                text += item.Name + " :-" + item.Text + '\n';
            }
            textBox2.Text = text;
            players = playersTemp.ToList();
            //   var players = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            //foreach (var item in players)
            //{
            //    p.Color = Color.Black;
            //    sb.Color = Color.Black;
            //    g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX + 5, mapSizeX, 20);
            //    g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX + 4, mapSizeX - 1);
            //}


            //foreach (var item in players)
            //{
            //    p.Color = Color.Black;
            //    sb.Color = Color.Pink;
            //    g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, 20);
            //    g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);
            //}

            //char[] ss = { '[', ']' };
            //String asd = await UpdateMap(client.BaseAddress.PathAndQuery);
            //// var bbb3 = Newtonsoft.Json.JsonConvert.DeserializeObject(asd);
            //// textBox2.Text = asd.ToString();
            //List<String> splited = asd.Split(ss).ToList();
            //splited = splited.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            //string jsonCorrect = "";
            //string jsonCorrect2 = "";
            //string jsonCorrect3 = "";
            //int cnt = splited.Count();
            //if (cnt != 3)
            //{
            //    return;
            //}
            //jsonCorrect += "[" + splited[0] + "]";
            //var model = JsonConvert.DeserializeObject<List<Brick>>(jsonCorrect);
            //foreach (var item in model)
            //{
            //    blocks.Add(item);
            //}
            //jsonCorrect2 += "[" + splited[1] + "]";
            //var modelw = JsonConvert.DeserializeObject<List<Water>>(jsonCorrect2);
            //foreach (var item in modelw)
            //{
            //    blocks.Add((Water)item);
            //}
            //jsonCorrect3 += "[" + splited[2] + "]";
            //var model3 = JsonConvert.DeserializeObject<List<Ground>>(jsonCorrect3);
            //foreach (var item in model3)
            //{
            //    blocks.Add((Ground)item);
            //}
            //textBox2.Text = "im h3333ere";
            //asd2++;

            //mePlayer.PosX += 2;
            //await UpdateProductAsync(mePlayer);

            asd2++;
            panel1.Invalidate();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            foreach (var item in blocks)
            {
                if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX + 1 && item.getPosY() == mePlayer.PosY)
                {
                    return;
                }
            }
            mePlayer.PosX += 1;
            await UpdateProductAsync(mePlayer);
        }

        private async void button4_Click(object sender, EventArgs e)
        {

            {
                foreach (var item in blocks)
                {
                    if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX && item.getPosY() == mePlayer.PosY - 1)
                    {
                        return;
                    }
                }
                mePlayer.PosY -= 1;
                await UpdateProductAsync(mePlayer);
            }
        }

        /// <summary> ///////////////////////// SINGLETON
        /// 
        public class Message
        {
            public string Name { get; set; }
            public string Text { get; set; }

            public Message()
            {

            }
        }

        public sealed class Chat
        {
            private static Chat _chat;
            private static readonly object _syncLock = new object();

            private Chat()
            {
            }

            public static Chat GetChat()
            {
                if (_chat == null)
                {
                    lock (_syncLock)
                    {
                        if (_chat == null)
                        {
                            _chat = new Chat();
                        }
                    }
                }

                return _chat;
            }

            public void WriteMessage(string message)
            {
                throw new NotImplementedException();
            }
        }
        /// </summary>
        ///////////////////////////////////        FACTORY                ////////////////////////////////////////////////////////////
        public enum BlockType
        {
            Water,
            Brick,
            Grass,
            Ground,
            Iron,
            Border
        }
        public class BlockFactory
        {
            public IBlock GetBlock(BlockType type)
            {
                switch (type)
                {
                    case BlockType.Brick:
                        return new Brick();
                    case BlockType.Grass:
                        return new Grass();
                    case BlockType.Water:
                        return new Water();
                    case BlockType.Ground:
                        return new Ground();
                    case BlockType.Iron:
                        return new Iron();
                    case BlockType.Border:
                        return new Border();
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public interface IBlock
        {
            void setPosX(int x);
            void setPosY(int y);
            void setPosXY(int x, int y);
            int getPosX();
            int getPosY();
            string getType();

        }
        public class Brick : IBlock
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
            public string Type { get; } = "Brick";

            public Brick(int PosX, int PosY, string Type)
            {
                this.PosX = PosX;
                this.PosY = PosY;
                this.Type = Type;
            }
            public Brick()
            {

            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }
            public void setPosXY(int x, int y)
            {
                PosY = y;
                PosX = x;
            }


            public string getType()
            {
                return Type;
            }
        }
        public class Iron : IBlock
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
            public string Type { get; } = "Iron";

            public Iron(int PosX, int PosY, string Type)
            {
                this.PosX = PosX;
                this.PosY = PosY;
                this.Type = Type;
            }
            public Iron()
            {

            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }
            public void setPosXY(int x, int y)
            {
                PosY = y;
                PosX = x;
            }


            public string getType()
            {
                return Type;
            }
        }
        public class Border : IBlock
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
            public string Type { get; } = "Border";

            public Border(int PosX, int PosY, string Type)
            {
                this.PosX = PosX;
                this.PosY = PosY;
                this.Type = Type;
            }
            public Border()
            {

            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }
            public void setPosXY(int x, int y)
            {
                PosY = y;
                PosX = x;
            }


            public string getType()
            {
                return Type;
            }
        }
        public class Ground : IBlock
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
            public string Type { get; } = "Ground";

            public Ground(int PosX, int PosY, string Type)
            {
                this.PosX = PosX;
                this.PosY = PosY;
                this.Type = Type;
            }
            public Ground()
            {

            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {

            }

            public string getType()
            {
                return Type;
            }

            public void setPosXY(int x, int y)
            {
                PosX = x;
                PosY = y;
            }
        }
        public class Water : IBlock
        {
            public int PosX { get; set; }
            public int PosY { get; set; }

            public string Type { get; } = "Water";


            public Water(int PosX, int PosY, string Type)
            {
                this.PosX = PosX;
                this.PosY = PosY;
                this.Type = Type;
            }
            public Water()
            {

            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }
            public void setPosXY(int x, int y)
            {
                PosX = x;
                PosY = y;
            }
            public void setPosY(int y)
            {
                PosY = y;
            }
            public string getType()
            {
                return Type;
            }
        }
        public class Grass : IBlock
        {
            public int PosX { get; set; }
            public int PosY { get; set; }

            public string Type { get; } = "Grass";

            public Grass(int PosX, int PosY, string Type)
            {
                this.PosX = PosX;
                this.PosY = PosY;
                this.Type = Type;
            }
            public Grass()
            {

            }
            public void setPosXY(int x, int y)
            {
                PosX = x;
                PosY = y;
            }
            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }
            public string getType()
            {
                return Type;
            }



        }
        /////////////////////////////////////// ABSTRACT FACTORY /////////////////////////////////////////


        public abstract class ItemFactory
        {
            public abstract IStatusPowerUp createStatusPowerUp();
            public abstract IConsumablePowerUp createConsumablePowerUp();
        }

        public class OffensiveItemFactory : ItemFactory
        {
            public override IConsumablePowerUp createConsumablePowerUp()
            {
                return new Mine();
            }

            public override IStatusPowerUp createStatusPowerUp()
            {
                return new DamagePowerUp();
            }
        }
        public class DefensiveItemFactory : ItemFactory
        {
            public override IConsumablePowerUp createConsumablePowerUp()
            {
                return new Shield();
            }

            public override IStatusPowerUp createStatusPowerUp()
            {
                return new HealthPowerUp();
            }
        }

        public interface IPickUpItem
        {
            void setPosX(int x);
            void setPosY(int y);
            int getPosX();
            int getPosY();
        }
        public interface IStatusPowerUp : IPickUpItem
        {
            void increaseStats();
        }
        public interface IConsumablePowerUp : IPickUpItem
        {
            void activateConsumable();
        }

        public class Shield : IConsumablePowerUp
        {

            private int PosX { get; set; }
            private int PosY { get; set; }


            public Shield(int PosX, int PosY)
            {
                this.PosX = PosX;
                this.PosY = PosY;
            }
            public Shield()
            {

            }


            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }

            public void activateConsumable()
            {
                throw new NotImplementedException();
            }
        }

        public class Mine : IConsumablePowerUp
        {

            private int PosX { get; set; }
            private int PosY { get; set; }


            public Mine(int PosX, int PosY)
            {
                this.PosX = PosX;
                this.PosY = PosY;
            }
            public Mine()
            {

            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }

            public void activateConsumable()
            {
                throw new NotImplementedException();
            }
        }


        public class DamagePowerUp : IStatusPowerUp
        {

            private int PosX { get; set; }
            private int PosY { get; set; }


            public DamagePowerUp(int PosX, int PosY)
            {
                this.PosX = PosX;
                this.PosY = PosY;
            }
            public DamagePowerUp()
            {

            }


            public void increaseStats()
            {
                throw new NotImplementedException();
            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }
        }
        public class HealthPowerUp : IStatusPowerUp
        {
            private int PosX { get; set; }
            private int PosY { get; set; }



            public HealthPowerUp(int PosX, int PosY)
            {
                this.PosX = PosX;
                this.PosY = PosY;
            }
            public HealthPowerUp()
            {

            }


            public void increaseStats()
            {
                throw new NotImplementedException();
            }

            public int getPosX()
            {
                return PosX;
            }

            public int getPosY()
            {
                return PosY;
            }

            public void setPosX(int x)
            {
                PosX = x;
            }

            public void setPosY(int y)
            {
                PosY = y;
            }
        }





        /// <summary>
        /// /////////////////////////////////////    STRATEGY //////////////////////////////////////////
        /// </summary>
        public interface IMoveAlgorithm
        {
            void changeDirection(Player p);
        }

        public class MoveUp : IMoveAlgorithm
        {
            public void changeDirection(Player p)
            {
                if (p.getPosY() != 100)
                {
                    p.addPosY(1);
                }
            }

        }
        public class MoveDown : IMoveAlgorithm
        {
            public void changeDirection(Player p)
            {
                if (p.getPosY() != 0)
                {
                    p.addPosY(-1);
                }
            }
        }


        public class MoveLeft : IMoveAlgorithm
        {
            public void changeDirection(Player p)
            {
                if (p.getPosX() != 0)
                {
                    p.addPosX(-1);
                }
            }

        }
        public class MoveRight : IMoveAlgorithm
        {
            public void changeDirection(Player p)
            {
                if (p.getPosX() != 100)
                {
                    p.addPosY(1);
                }
            }

        }


        public class Player
        {

            public int Id { get; set; }
            public int PosX { get; set; }
            public int PosY { get; set; }

            public int Speed { get; set; }

            public Player(int posX, int posY, int speed)
            {
                PosX = posX;
                PosY = posY;
                Speed = speed;
            }
            public Player()
            {

            }
            public void setPosX(int posX)
            {
                PosX = posX;
            }
            public void setPosY(int posY)
            {
                PosY = posY;
            }
            public int getPosY()
            {
                return PosY;
            }
            public int getPosX()
            {
                return PosX;
            }
            public void addPosY(int x)
            {
                PosY += x * Speed;
            }
            public void addPosX(int x)
            {
                PosX += x * Speed;
            }

            public IMoveAlgorithm direction;
            public Player(IMoveAlgorithm iMoveAlgorithm)
            {
                this.direction = iMoveAlgorithm;
            }
            public void changeDirection()
            {
                direction.changeDirection(this);
            }

        }

        ////////////////////// BUILDER //////////////////////////

        public class MapDirector
        {
            public Map BuildMap(MapBuilder mapBuilder)
            {
                mapBuilder.AddBlocks();
                mapBuilder.AddItems();

                return mapBuilder.Map;
            }
        }



        public class Map
        {
            public List<IPickUpItem> Items { get; set; } = new List<IPickUpItem>();

            //    public List<IBlock> Blocks { get; set; } = new List<IBlock>();

            public IBlock[,] Blocks { get; set; }


            public int SizeX { get; set; }

            public int SizeY { get; set; }

            public Map(int SizeX, int SizeY)
            {
                this.SizeX = SizeX;
                this.SizeY = SizeY;
            }
            public Map()
            {

            }

            public void CreateBlocksArray()
            {
                Blocks = new IBlock[SizeX, SizeY];
            }

        }
        public abstract class MapBuilder
        {
            public BlockFactory BlockFactory = new BlockFactory();

            public ItemFactory Itemfactory;


            public Map Map { get; private set; }
            public void CreateMap(int SizeX, int SizeY)
            {
                Map = new Map(SizeX, SizeY);
                Map.CreateBlocksArray();
            }

            public abstract void AddBlocks();
            public abstract void AddItems();
        }
        public class SmalMapBuilder : MapBuilder
        {
            public override void AddBlocks()
            {// Tik kaip pvz. Reikia sugalvoti algoritma kuris sugeneruos visus blokus zemelapije.
                for (int i = 0; i < Map.SizeX; i++)
                {
                    this.Map.Blocks[i, 0] = BlockFactory.GetBlock(BlockType.Border);
                    this.Map.Blocks[i, 0].setPosXY(i, 0);

                    this.Map.Blocks[0, i] = BlockFactory.GetBlock(BlockType.Border);
                    this.Map.Blocks[0, i].setPosXY(0, i);

                    this.Map.Blocks[Map.SizeX - 1, i] = BlockFactory.GetBlock(BlockType.Border);
                    this.Map.Blocks[Map.SizeX - 1, i].setPosXY(Map.SizeX - 1, i);

                    this.Map.Blocks[i, Map.SizeY - 1] = BlockFactory.GetBlock(BlockType.Border);
                    this.Map.Blocks[i, Map.SizeY - 1].setPosXY(i, Map.SizeY - 1);

                }
                Random rnd = new Random();
                int waterCnt = rnd.Next(1, 3);

                for (int x = 0; x < waterCnt; x++)
                {
                    int x1 = rnd.Next(1, 19);
                    int x2 = rnd.Next(1, 19);
                    int y1 = rnd.Next(1, 19);
                    int y2 = rnd.Next(1, 19);

                    this.Map.Blocks[x1, y1] = BlockFactory.GetBlock(BlockType.Water);
                    this.Map.Blocks[x1, y1].setPosXY(x1, y1);

                    this.Map.Blocks[x2, y2] = BlockFactory.GetBlock(BlockType.Water);
                    this.Map.Blocks[x2, y2].setPosXY(x2, y2);

                    int numOfBlocksTodrawX = 0;
                    int numOfBlocksTodrawY = 0;
                    int tempX = 0;

                    if (x1 < x2)
                    {
                        numOfBlocksTodrawX = x2 - x1;
                        for (int i = 1; i < numOfBlocksTodrawX + 1; i++)
                        {
                            if (this.Map.Blocks[x1 + i, y1] == null)
                            {
                                this.Map.Blocks[x1 + i, y1] = BlockFactory.GetBlock(BlockType.Water);
                                this.Map.Blocks[x1 + i, y1].setPosXY(x1 + i, y1);
                            }
                        }
                        tempX = x2;
                    }
                    else
                    {
                        numOfBlocksTodrawX = x1 - x2;
                        for (int i = 1; i < numOfBlocksTodrawX + 1; i++)
                        {
                            if (this.Map.Blocks[x2 + i, y1] == null)
                            {
                                this.Map.Blocks[x2 + i, y2] = BlockFactory.GetBlock(BlockType.Water);
                                this.Map.Blocks[x2 + i, y2].setPosXY(x2 + i, y2);
                            }
                        }
                        tempX = x1;
                    }

                    if (y1 < y2)
                    {
                        numOfBlocksTodrawY = y2 - y1;
                        for (int i = 1; i < numOfBlocksTodrawY; i++)
                        {
                            if (this.Map.Blocks[tempX, y1 + i] == null)
                            {
                                this.Map.Blocks[tempX, y1 + i] = BlockFactory.GetBlock(BlockType.Water);
                                this.Map.Blocks[tempX, y1 + i].setPosXY(tempX, y1 + i);
                            }
                        }
                    }
                    else
                    {
                        numOfBlocksTodrawY = y1 - y2;
                        for (int i = 1; i < numOfBlocksTodrawY; i++)
                        {
                            if (this.Map.Blocks[tempX, y2 + i] == null)
                            {
                                this.Map.Blocks[tempX, y2 + i] = BlockFactory.GetBlock(BlockType.Water);
                                this.Map.Blocks[tempX, y2 + i].setPosXY(tempX, y2 + i);
                            }
                        }
                    }
                }
                int bricksPatter = rnd.Next(0, 2);

                if (bricksPatter == 0)
                {
                    for (int i = 2; i < Map.SizeX - 2; i++)
                    {
                        this.Map.Blocks[i, i] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[i, i].setPosXY(i, i);

                        this.Map.Blocks[Map.SizeX - i, i] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[Map.SizeX - i, i].setPosXY(Map.SizeX - i, i);
                    }

                }
                if (bricksPatter == 1)
                {
                    for (int i = Map.SizeX / 5; i < (Map.SizeX - Map.SizeX / 5) + 1; i++)
                    {
                        this.Map.Blocks[i, Map.SizeX / 5] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[i, Map.SizeX / 5].setPosXY(i, Map.SizeX / 5);

                        this.Map.Blocks[Map.SizeX / 5, i] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[Map.SizeX / 5, i].setPosXY(Map.SizeX / 5, i);

                        this.Map.Blocks[i, Map.SizeX - Map.SizeX / 5] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[i, Map.SizeX - Map.SizeX / 5].setPosXY(i, Map.SizeX - Map.SizeX / 5);

                        this.Map.Blocks[Map.SizeX - Map.SizeX / 5, i] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[Map.SizeX - Map.SizeX / 5, i].setPosXY(Map.SizeX - Map.SizeX / 5, i);
                    }

                    for (int i = 1; i < Map.SizeX - 1; i++)
                    {

                        this.Map.Blocks[i, Map.SizeX - Map.SizeX / 2] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[i, Map.SizeX - Map.SizeX / 2].setPosXY(i, Map.SizeX - Map.SizeX / 2);


                        this.Map.Blocks[Map.SizeX - Map.SizeX / 2, i] = BlockFactory.GetBlock(BlockType.Brick);
                        this.Map.Blocks[Map.SizeX - Map.SizeX / 2, i].setPosXY(Map.SizeX - Map.SizeX / 2, i);
                    }
                }

                for (int i = 0; i < Map.SizeX; i++)
                {
                    for (int j = 0; j < Map.SizeY; j++)
                    {
                        if (Map.Blocks[i, j] == null)
                        {
                            Map.Blocks[i, j] = BlockFactory.GetBlock(BlockType.Ground);
                            Map.Blocks[i, j].setPosXY(i, j);
                        }
                    }
                }
            }
            public override void AddItems()
            {
                Itemfactory = new DefensiveItemFactory();
                this.Map.Items.Add(Itemfactory.createStatusPowerUp());
                //throw new NotImplementedException();
            }

        }
        public class LargeMapBuilder : MapBuilder
        {
            public override void AddBlocks()
            {

                throw new NotImplementedException();
            }

            public override void AddItems()
            {

                throw new NotImplementedException();
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            foreach (var item in blocks)
            {
                if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX && item.getPosY() == mePlayer.PosY + 1)
                {
                    return;
                }
            }
            mePlayer.PosY += 1;
            await UpdateProductAsync(mePlayer);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            foreach (var item in blocks)
            {
                if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX - 1 && item.getPosY() == mePlayer.PosY)
                {
                    return;
                }
            }
            mePlayer.PosX -= 1;
            await UpdateProductAsync(mePlayer);
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            Message m = new Message();
            m.Text = textBox1.Text;
            m.Name = "Player" + players.Count();

            int asd = await createMessage(client.BaseAddress.PathAndQuery, m);
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}



