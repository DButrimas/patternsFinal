using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;

namespace TanksMP_Client
{

    public partial class GameForm : Form
    {
        Player mementoP;
        PlayerMemory mm;

        private const string apiUrl = "https://localhost:44319/";
        Player mePlayer = new Player();
        static HttpClient client = new HttpClient();
        public List<Player> players = new List<Player>();
        public Map map;
        List<Block> blocks = new List<Block>();
        Random rnd = new Random();
        //Color pColor;
        int mapSizeX = 20;
        Pen p = new Pen(Color.Black);
        SolidBrush sb = new SolidBrush(Color.Red);
        Graphics g = null;
        PictureBox[] playerPics = new PictureBox[50];
        //PictureBox playerPic;
        RichTextBox chatLog;
        AbstractLogger loggerChain;

        Collection collection = new Collection();
        Iterator iterator;

        Message m;
        TerminalExpression terminal = new TerminalExpression();
        

        public delegate void MessageReceivedEventHandler(string message, int id);

        private void GameForm_Load(object sender, EventArgs e)
        {
            AllocConsole();
            chatLog = gameLog;
            iterator = collection.CreateIterator();
            iterator.Step = 1;
            m = new Message("");
            //loggerChain = getChainOfLoggers();
            //loggerChain.logMessage(AbstractLogger.DEBUG, "Form loaded");


        }

        private AbstractLogger getChainOfLoggers()
        {
            AbstractLogger errorLogger = new ErrorLogger();
            AbstractLogger fileLogger = new FileLogger();
            AbstractLogger consoleLogger = new ConsoleLogger();
            AbstractLogger chatLogger = new ChatLogger(chatLog);

            errorLogger.setNextLogger(fileLogger);
            fileLogger.setNextLogger(consoleLogger);
            consoleLogger.setNextLogger(chatLogger);

            return errorLogger;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public GameForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ControlBox = false;
            g = panel2.CreateGraphics();
            p = new Pen(Color.Black);
            sb = new SolidBrush(Color.Red);
        }

        //Event handlers
        private async void joinBtn_Click(object sender, EventArgs e)
        {
         
            await joinGameAsync();
            Timer timer = new Timer();
            timer.Interval = (10); // 0.5 secs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            //loggerChain.logMessage(AbstractLogger.DEBUG, "Player joined the game");
            //loggerChain.logMessage(AbstractLogger.FILE, "Player joined the game");
        }
        
        private async void timer_Tick(object sender, EventArgs e)
        {
            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            players = playersTemp.ToList();
            int count = 0;
            foreach (var item in players)
            {
                collection[count] = item;
                count++;
            }
            UpdatePlayers();
            string text = "";
            var asss = await GetMessagesAsync(client.BaseAddress.PathAndQuery);
            List<Msg> msgs = asss.ToList();
            foreach (var item in msgs)
            {
                if (item.Name != "Player" + mePlayer.getId().ToString())
                {
                    text += "Player" + mePlayer.getId().ToString() + " received " + item.Text + " from " + item.Name + "\n";
                }
            }
            gameLog.Text = text;
        }


        //ITERATOR EXAMPLE
        private void UpdatePlayers()
        {

            //for (Player pl = iterator.First(); !iterator.IsDone; pl = iterator.Next())
            //{
            //    if (!isPlayerPicCreated(pl.Id))
            //    {
            //        PictureBox pb = createPlayerPic(pl);
            //        panel2.Controls.Add(pb);
            //        playerPics[pl.Id] = pb;
            //    }
            //    playerPics[pl.Id].Location = new Point(pl.getPosX() * mapSizeX, pl.getPosY() * mapSizeX);
            //    playerPics[pl.Id].Image = Image.FromFile("..\\..\\Images\\tank" + pl.Rotation + ".png");
            //}

            foreach (Player p in players)
            {
                if (!isPlayerPicCreated(p.Id))
                {
                    PictureBox pb = createPlayerPic(p);
                    panel2.Controls.Add(pb);
                    playerPics[p.Id] = pb;
                }
                playerPics[p.Id].Location = new Point(p.getPosX() * mapSizeX, p.getPosY() * mapSizeX);
                playerPics[p.Id].Image = Image.FromFile("..\\..\\Images\\tank" + p.Rotation + ".png");
            }
        }

        private PictureBox createPlayerPic(Player p)
        {
            string path = "..\\..\\Images\\tank" + p.Rotation + ".png";
            PictureBox pic = new PictureBox
            {
                Location = new Point(10, 10),
                Image = Image.FromFile(path),
                Width = mapSizeX,
                Height = mapSizeX,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Name = p.Id.ToString(),
                BackColor = Color.Transparent
            };
            return pic;
        }

        private bool isPlayerPicCreated(int id)
        {
            if (playerPics[id] != null)
            {
                return true;
            }
            return false;
        }
        public int bulletCounter = 1;
        private void fireBtn_Click(object sender, EventArgs e)
        {
            if (bulletCounter <= 10)
            {
                testBox1.Text += "Total Bullets hits to the player: " + bulletCounter + "\n" + mePlayer.Bullethit(bulletCounter);
                bulletCounter += 1;
            } else
            {
                return;
            }
  
            /*PictureBox bulletPic = new PictureBox
            {
                Location = new Point(0, 0),
                Image = Image.FromFile("..\\..\\Images\\bullet.png"),
                Width = mapSizeX,
                Height = mapSizeX,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            panel2.Controls.Add(bulletPic);
            while (bulletPic.Location.X < 20)
            {
                gameLog.AppendText((bulletPic.Location.X).ToString() + "\n");
                bulletPic.Location = new Point((bulletPic.Location.X + 1) * mapSizeX, bulletPic.Location.Y * mapSizeX);
                System.Threading.Thread.Sleep(1000);
            }*/
        }

        private async void leaveBtn_Click(object sender, EventArgs e)
        {
            await removePlayerAsync();
            Close();
        }

        private async void upBtn_Click(object sender, EventArgs e)
        {
            foreach (var item in blocks)
            {
                if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX && item.getPosY() == mePlayer.PosY - 1)
                {
                    return;
                }
            }
            mePlayer.Rotation = 0;
            if(mePlayer.PosY -1 >= 0)
            {
                mePlayer.PosY -= 1;
            }else
            {
                return;
            }
            
            await UpdateProductAsync(mePlayer);
            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            players = playersTemp.ToList();
        }

        private async void leftBtn_Click(object sender, EventArgs e)
        {
            foreach (var item in blocks)
            {
                if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX - 1 && item.getPosY() == mePlayer.PosY)
                {
                    return;
                }
            }
            mePlayer.Rotation = 1;
            if (mePlayer.PosX - 1 >= 0)
            {
                mePlayer.PosX -= 1;
            }
            else
            {
                return;
            }
            await UpdateProductAsync(mePlayer);
            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            players = playersTemp.ToList();
        }

        private async void downBtn_Click(object sender, EventArgs e)
        {
            foreach (var item in blocks)
            {
                if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX && item.getPosY() == mePlayer.PosY + 1)
                {
                    return;
                }
            }
            mePlayer.Rotation = 2;
            if (mePlayer.PosY + 1 < 20)
            {
                mePlayer.PosY += 1;
            }
            else
            {
                return;
            }     
            await UpdateProductAsync(mePlayer);
            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            players = playersTemp.ToList();
        }

        private async void rightBtn_Click(object sender, EventArgs e)
        {
            foreach (var item in blocks)
            {
                if (BlockType.Ground.ToString() != item.getType() && item.getPosX() == mePlayer.PosX + 1 && item.getPosY() == mePlayer.PosY)
                {
                    return;
                }
            }
            mePlayer.Rotation = 3;
            if (mePlayer.PosX + 1 <= 20)
            {
                mePlayer.PosX += 1;
            }
            else
            {
                return;
            }
            await UpdateProductAsync(mePlayer);
            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            players = playersTemp.ToList();
        }

        //Async tasks
        async Task removePlayerAsync()
        {
            ICollection<Player> playersTemp = await RemovePlayer(client.BaseAddress.PathAndQuery, mePlayer.Id);
            players = playersTemp != null ? playersTemp.ToList() : players;
            mePlayer = null;
        }

        async Task joinGameAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            char[] ss = { '[', ']' };
            String stringData = await BuildMap(client.BaseAddress.PathAndQuery);
            List<String> splited = stringData.Split(ss).ToList();
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

            mePlayer = await createPlayer(client.BaseAddress.PathAndQuery);
            ICollection<Player> playersTemp = await GetAllPlayerAsync(client.BaseAddress.PathAndQuery);
            players = playersTemp.ToList();
            paintMap();
        }

        private void paintMap()
        {
            foreach (var item in blocks)
            {
                if (item != null)
                {
                    if (BlockType.Brick.ToString() == item.getType())
                    {
                        string path = "..\\..\\Images\\brick.jpeg";
                        Image img = Image.FromFile(path);
                        g.DrawImage(img, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, mapSizeX);
                    }
                    else if (BlockType.Water.ToString() == item.getType())
                    {
                        string path = "..\\..\\Images\\water.jpg";
                        Image img = Image.FromFile(path);
                        g.DrawImage(img, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, mapSizeX);
                    }
                    else if (BlockType.Ground.ToString() == item.getType())
                    {
                        //p.Color = Color.BurlyWood;
                        //sb.Color = Color.BurlyWood;
                        //g.DrawRectangle(p, item.getPosX() * 20, item.getPosY() * mapSizeX, mapSizeX, 20);
                        //g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);
                    }
                    else if (BlockType.Iron.ToString() == item.getType())
                    {
                        //p.Color = Color.Black;
                        //sb.Color = Color.DimGray;
                        //g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, 20);
                        //g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);
                    }
                    else if (BlockType.Border.ToString() == item.getType())
                    {
                        //p.Color = Color.DarkSlateGray;
                        //sb.Color = Color.Black;
                        //g.DrawRectangle(p, item.getPosX() * mapSizeX, item.getPosY() * mapSizeX, mapSizeX, 20);
                        //g.FillRectangle(sb, (item.getPosX() * mapSizeX) + 1, (item.getPosY() * mapSizeX) + 1, mapSizeX - 1, mapSizeX - 1);
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

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

        static async Task<ICollection<Player>> RemovePlayer(string path, long playerId)
        {
            ICollection<Player> players = null;
            HttpResponseMessage response = await client.DeleteAsync(path + $"api/players/{playerId}");
            if (response.IsSuccessStatusCode)
            {
                players = await response.Content.ReadAsAsync<ICollection<Player>>();
            }
            return players;
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

        // PATTERNS

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
            public Block GetBlock(BlockType type)
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

        public abstract class Block
        {
            public abstract void setPosX(int x);
            public abstract void setPosY(int y);
            public abstract void setPosXY(int x, int y);
            public abstract int getPosX();
            public abstract int getPosY();
            public abstract string getType();
            public abstract void checkHp();

            public abstract void changePicture();



            public void Destroy()
            {
                checkHp();
                changePicture();
            }

        }
        // ITERATOR
        /// <summary>

        /// The 'Aggregate' interface

        /// </summary>

        public interface IAbstractCollection

        {
            Iterator CreateIterator();
        }

        /// <summary>

        /// The 'ConcreteAggregate' class

        /// </summary>

        public class Collection : IAbstractCollection

        {
            private ArrayList _items = new ArrayList();

            public Iterator CreateIterator()
            {
                return new Iterator(this);
            }

            // Gets item count

            public int Count
            {
                get { return _items.Count; }
            }

            // Indexer

            public object this[int index]
            {
                get { return _items[index]; }
                set { _items.Add(value); }
            }
        }

        /// <summary>

        /// The 'Iterator' interface

        /// </summary>

        public interface IAbstractIterator

        {
            Player First();
            Player Next();
            bool IsDone { get; }
            Player CurrentItem { get; }
        }

        /// <summary>

        /// The 'ConcreteIterator' class

        /// </summary>

        public class Iterator : IAbstractIterator

        {
            private Collection _collection;
            private int _current = 0;
            private int _step = 1;

            // Constructor

            public Iterator(Collection collection)
            {
                this._collection = collection;
            }

            // Gets first item

            public Player First()
            {
                _current = 0;
                return _collection[_current] as Player;
            }

            // Gets next item

            public Player Next()
            {
                _current += _step;
                if (!IsDone)
                    return _collection[_current] as Player;
                else

                    return null;
            }

            // Gets or sets stepsize

            public int Step
            {
                get { return _step; }
                set { _step = value; }
            }

            // Gets current iterator item

            public Player CurrentItem
            {
                get { return _collection[_current] as Player; }
            }

            // Gets whether iteration is complete

            public bool IsDone
            {
                get { return _current >= _collection.Count; }
            }
        }



        public class Brick : Block
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

            public override int getPosX()
            {
                return PosX;
            }

            public override int getPosY()
            {
                return PosY;
            }

            public override void setPosX(int x)
            {
                PosX = x;
            }

            public override void setPosY(int y)
            {
                PosY = y;
            }
            public override void setPosXY(int x, int y)
            {
                PosY = y;
                PosX = x;
            }


            public override string getType()
            {
                return Type;
            }

            public override void checkHp()
            {
                throw new NotImplementedException();
            }

            public override void changePicture()
            {
                throw new NotImplementedException();
            }
        }
        public class Iron : Block
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

            public override int getPosX()
            {
                return PosX;
            }

            public override int getPosY()
            {
                return PosY;
            }

            public override void setPosX(int x)
            {
                PosX = x;
            }

            public override void setPosY(int y)
            {
                PosY = y;
            }
            public override void setPosXY(int x, int y)
            {
                PosY = y;
                PosX = x;
            }

            public override void checkHp()
            {
                throw new NotImplementedException();
            }
            public override string getType()
            {
                return Type;
            }

            public override void changePicture()
            {
                throw new NotImplementedException();
            }
        }
        public class Border : Block
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

            public override int getPosX()
            {
                return PosX;
            }

            public override int getPosY()
            {
                return PosY;
            }

            public override void setPosX(int x)
            {
                PosX = x;
            }

            public override void setPosY(int y)
            {
                PosY = y;
            }
            public override void setPosXY(int x, int y)
            {
                PosY = y;
                PosX = x;
            }
            public override void checkHp()
            {
                throw new NotImplementedException();
            }

            public override string getType()
            {
                return Type;
            }

            public override void changePicture()
            {
                throw new NotImplementedException();
            }
        }
        public class Ground : Block
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

            public override int getPosX()
            {
                return PosX;
            }

            public override int getPosY()
            {
                return PosY;
            }

            public override void setPosX(int x)
            {
                PosX = x;
            }

            public override void setPosY(int y)
            {

            }

            public override string getType()
            {
                return Type;
            }

            public override void setPosXY(int x, int y)
            {
                PosX = x;
                PosY = y;
            }

            public override void checkHp()
            {
                throw new NotImplementedException();
            }

            public override void changePicture()
            {
                throw new NotImplementedException();
            }
        }
        public class Water : Block
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

            public override int getPosX()
            {
                return PosX;
            }

            public override int getPosY()
            {
                return PosY;
            }

            public override void setPosX(int x)
            {
                PosX = x;
            }
            public override void setPosXY(int x, int y)
            {
                PosX = x;
                PosY = y;
            }
            public override void setPosY(int y)
            {
                PosY = y;
            }
            public override string getType()
            {
                return Type;
            }
            public override void checkHp()
            {
                throw new NotImplementedException();
            }

            public override void changePicture()
            {
                throw new NotImplementedException();
            }
        }
        public class Grass : Block
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
            public override void setPosXY(int x, int y)
            {
                PosX = x;
                PosY = y;
            }
            public override int getPosX()
            {
                return PosX;
            }

            public override int getPosY()
            {
                return PosY;
            }

            public override void setPosX(int x)
            {
                PosX = x;
            }

            public override void setPosY(int y)
            {
                PosY = y;
            }
            public override string getType()
            {
                return Type;
            }

            public override void checkHp()
            {
                throw new NotImplementedException();
            }

            public override void changePicture()
            {
                throw new NotImplementedException();
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

        // 'State' interface
        public interface State
        {
            public string ExecuteCommand(Player player);

        }
        // 'ConcreteStateA' class
        public class HealthyState : State
        {

            public string ExecuteCommand(Player player)
            {
                return "The Player is in Healthy State. \n";
            }
        }
        // 'ConcreteStateB' class
        public class HurtState : State
        {
            public string ExecuteCommand(Player player)
            {
                return "The player is wounded. Please search health points \n";
            }
        }
        // 'ConcreteStateC' class
        public class DeadState : State
        {
            public string ExecuteCommand(Player player)
            {
                return "The Player is dead. Game Over. \n";
            }
        }
        /// <summary>
        /// Mediator
        /// </summary>
        /// <param name="message"></param>
        /// <param name="from"></param>


        public class Mediator
        {
            public event MessageReceivedEventHandler MessageReceived;
            public void Send(string message, int id)
            {
                if (MessageReceived != null)
                {
                    Console.WriteLine("Sending '{0}' from Player{1}", message, id);
                    MessageReceived(message, id);
                }
            }
        }

        public class Player
        {
            State currentState;
            public int Id { get; set; }
            public int PosX { get; set; }
            public int PosY { get; set; }
            public int Speed { get; set; }
            public int Rotation { get; set; }
            public bool Dead {get; set;}
            private Mediator _mediator;
            public Player(Mediator mediator, int id,int posX, int posY, int speed)
            {
                Id = id;
                _mediator = mediator;
                _mediator.MessageReceived += new MessageReceivedEventHandler(Receive);
                PosX = posX;
                PosY = posY;
                Speed = speed;
            }
            private void Receive(string message, int id)
            {
                if (id != Id)
                    Console.WriteLine("Player{0} received '{1}' from Player{2}", Id, message, id);
            }
            public void Send(string message)
            {
                _mediator.Send(message, Id);
            }
            public Player()
            {
                this.currentState = new HealthyState();
            }
            public string Bullethit(int bullets)
            {
                if (bullets < 5)
                    this.currentState = new HealthyState();
                if (bullets >= 5 && bullets < 10)
                    this.currentState = new HurtState();
                if (bullets >= 10)
                    this.currentState = new DeadState();
                    
                return currentState.ExecuteCommand(this);
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
            public int getId()
            {
                return Id;
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

            public PlayerMemento SaveMemento()
            {
                return new PlayerMemento(Speed, PosX, PosY, Id, Rotation);
            }

            public void RestoreMemento(PlayerMemento memento)
            {
                Id = memento.Id;
                PosX = memento.PosX;
                PosY = memento.PosY;
                Speed = memento.Speed;
                Rotation = memento.Rotation;
            }


        }

        /// <summary>
        /// ///////////////////////     MEMENTO
        /// </summary>
        public class PlayerMemento
        {
            public int Id { get; set; }
            public int PosX { get; set; }
            public int PosY { get; set; }
            public int Speed { get; set; }
            public int Rotation { get; set; }

            public PlayerMemento(int Id, int PosX, int PosY, int Speed, int Rotation)
            {
                this.Id = Id;
                this.PosX = PosX;
                this.PosY = PosY;
                this.Speed = Speed;
                this.Rotation = Rotation;
            }

        }
        public class PlayerMemory
        {
            private PlayerMemento _memento;

            public PlayerMemento Memento
            {
                get { return _memento; }
                set { _memento = value; }
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

            public Block[,] Blocks { get; set; }


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
                Blocks = new Block[SizeX, SizeY];
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

        /////////////////////// CHAIN OF RESPONSIBILITY ///////////////

        public abstract class AbstractLogger
        {
            public static int INFO = 0;
            public static int DEBUG = 1;
            public static int FILE = 2;
            public static int CHAT = 3;

            public int type;
            AbstractLogger nextLogger;
            public void setNextLogger(AbstractLogger nextLogger)
            {
                this.nextLogger = nextLogger;
            }

            public void logMessage(int type, String message)
            {
                if (this.type <= type)
                {
                    write(message);
                }
                if (nextLogger != null)
                {
                    nextLogger.logMessage(type, message);
                }
            }

            protected abstract void write(String message);
        }

        public class ConsoleLogger : AbstractLogger
        {
            public ConsoleLogger()
            {
                this.type = INFO;
            }

            protected override void write(string message)
            {
                Console.WriteLine($"INFO: {message}");
            }
        }

        public class ErrorLogger : AbstractLogger
        {
            public ErrorLogger()
            {
                this.type = DEBUG;
            }

            protected override void write(string message)
            {
                Console.WriteLine($"DEBUG: {message}");
            }
        }

        public class FileLogger : AbstractLogger
        {
            public FileLogger()
            {
                this.type = FILE;
            }

            protected override void write(string message)
            {
                using (StreamWriter w = File.AppendText("log.txt"))
                {
                    w.WriteLine(message);
                }
            }
        }
       
        public class Msg
        {
            public string Name { get; set; }
            public string Text { get; set; }

            public Msg()
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
        public class ChatLogger : AbstractLogger
        {
            RichTextBox richTextBox;
            public ChatLogger(RichTextBox richTextBox)
            {
                this.type = CHAT;
                this.richTextBox = richTextBox;
            }

            protected override void write(string message)
            {
                richTextBox.AppendText($"{message}\n");
            }
        }

        private void gameLog_TextChanged(object sender, EventArgs e)
        {

        }/// <summary>
         /// //////////////////////////////////////PROXY           //////////////////////////////////////////////////////
         /// </summary>

        //  Panaudojimas new GetUriStringProxy().GetUriString(); ///////// 

        public interface GetUriString
        {
            string GetUriString();
        }
        public class GetUriStringProxy : GetUriString
        {
            public string GetUriString()
            {
                return new GetUriStringTheSring().GetUriString();
            }
        }
        public class GetUriStringTheSring : GetUriString
        {
            public string GetUriString()
            {
                return "https://localhost:44319/";
            }
        }


        ///////////////////////////////////////                           INTERPRETER



        public class Message
        {
            public string Text { get; set; }

            public Message(string text)
            {
                Text = text;
            }
        }

        public interface IExpression
        {
            int Interpret(Message context);
        }

        public class TerminalExpression : IExpression
        {
            public int Interpret(Message context)
            {
                if (context.Text.Contains("CheatCode:"))
                {


                    int score = 0;

                    byte[] ASCIIValues = Encoding.ASCII.GetBytes(context.Text);
                    foreach (byte b in ASCIIValues)
                    {
                        score += int.Parse(b.ToString());
                    }
                    return score;
                }
                else
                {
                    return 0;
                }
            }

            public class NonterminalExpression : IExpression
            {
                public IExpression Expression1 { get; set; }

                public IExpression Expression2 { get; set; }

                public int Interpret(Message context)
                {
                    int newScore = 0;
                    int a = Expression1.Interpret(context);
                    int b = Expression2.Interpret(context);
                    return newScore = a + b;

                }
            }
        }

        public async Task<int> createMessage(string path, Msg m)
        {
            int p = 1;
            HttpResponseMessage response = await client.PostAsJsonAsync(path + "api/Chat/", m);
            if (response.IsSuccessStatusCode)
            {
                p = await response.Content.ReadAsAsync<int>();
            }
            return p;
        }
        static async Task<ICollection<Msg>> GetMessagesAsync(string path)
        {
            ICollection<Msg> messages = null;
            HttpResponseMessage response = await client.GetAsync(path + "api/chat/");
            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<ICollection<Msg>>();
            }
            return messages;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            Msg m = new Msg();
            m.Text = textBox1.Text;
            m.Name = "Player" + mePlayer.Id;

            int asd = await createMessage(client.BaseAddress.PathAndQuery, m);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void testBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            m.Text = textBox1.Text;
            int sc = terminal.Interpret(m);
            gameLog.Text += "\n Score: " + sc.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            mm = new PlayerMemory();
            mementoP = new Player();
            gameLog.Text += "\n created player posX:" + mementoP.getPosX().ToString();
            gameLog.Text += "\n saved player memento";
            mm.Memento = mementoP.SaveMemento();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mementoP.RestoreMemento(mm.Memento);
            gameLog.Text += "\n restored memento, posx: " + mementoP.PosX;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mementoP.PosX = 10;
            gameLog.Text += "\n changed posX to: " + mementoP.PosX;
        }
    }
}


