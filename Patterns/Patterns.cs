using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    public class Program
    {
        static void Main(string[] args)
        {
            GameManager gm = new GameManager();
            BlockFactory f = new BlockFactory();
            IBlock b = f.GetBlock(BlockType.Water);
            Console.WriteLine(b.ToString() + "\n\n\n\n");
            gm.method(new DefensiveItemFactory());
            Player p = new Player();
            p.direction = new MoveLeft();
            p.changeDirection();
            Console.WriteLine("Direction: " + p.direction.ToString() + "\n");
            p.direction = new MoveUp();
            p.changeDirection();
            Console.WriteLine("Direction: " + p.direction.ToString() + "\n");


            //Builder builder = new ConcreteBuilder();
            //Director director = new Director(builder);
            //director.Construct();
            //Product product = builder.GetResult();

            MapBuilder bb = new SmalMapBuilder();
            bb.CreateMap(2, 4);
            bb.Map.CreateBlocksArray();

            MapDirector d = new MapDirector();
            d.BuildMap(bb);

            Console.ReadKey();
        }
    }
    public class GameManager
    {
        public void method(ItemFactory factory)
        {
            var a = factory.createConsumablePowerUp();
            var b = factory.createStatusPowerUp();
            Console.WriteLine("abstract factory created: " + a.ToString() + "\n and: " + b.ToString() + "\n");
        }
    }
    /// <summary> ///////////////////////// SINGLETON
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
        Ground
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
                default:
                    throw new NotSupportedException();
            }
        }
    }

    public interface IBlock
    {
        void setPosX(int x);
        void setPosY(int y);
        int getPosX();
        int getPosY();
        string getType();

    }
    public class Brick : IBlock
    {
        private int PosX { get; set; }
        private int PosY { get; set; }
        private string Type { get; } = "Brick";

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

        public string getType()
        {
            return Type;
        }
    }
    public class Ground : IBlock
    {
        private int PosX { get; set; }
        private int PosY { get; set; }
        private string Type { get; } = "Ground";

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
            PosY = y;
        }

        public string getType()
        {
            return Type;
        }
    }
    public class Water : IBlock
    {
        private int PosX { get; set; }
        private int PosY { get; set; }

        private string Type { get; } = "Water";


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
        private int PosX { get; set; }
        private int PosY { get; set; }

        private string Type { get; } = "Grass";

        public Grass(int PosX, int PosY, string Type)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            this.Type = Type;
        }
        public Grass()
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
        private int PosX { get; set; }
        private int PosY { get; set; }

        private int Speed { get; set; }

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
            var a = this.BlockFactory.GetBlock(BlockType.Water);
            this.Map.Blocks[0, 0] = a;
            // throw new NotImplementedException();
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
        public static int Info = 0;
        public static int Debug = 1;
        public static int File = 2;
        public static int Chat = 3;

        public int type;
        AbstractLogger nextLogger;

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
            this.type = Info;
        }

        protected override void write(string message)
        {
            logMessage(this.type, message);
        }
    }

    public class ErrorLogger : AbstractLogger
    {
        public ErrorLogger()
        {
            this.type = Debug;
        }

        protected override void write(string message)
        {
            logMessage(this.type, message);
        }
    }

    public class FileLogger : AbstractLogger
    {
        public FileLogger()
        {
            this.type = File;
        }

        protected override void write(string message)
        {
            logMessage(this.type, message);
        }
    }

    public class ChatLogger : AbstractLogger
    {
        public ChatLogger()
        {
            this.type = Chat;
        }

        protected override void write(string message)
        {
            logMessage(this.type, message);
        }
    }
}
