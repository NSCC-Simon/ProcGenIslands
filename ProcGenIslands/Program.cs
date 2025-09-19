using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcGenIslands
{
    enum TileType
    {
        Water,
        Grass,
        Sand,
        DeepWater,
        None

    }


    internal static class Program
    {

        public static Dictionary<TileType, ConsoleColor> _tileColors = new Dictionary<TileType, ConsoleColor>
        {
             { TileType.Water, ConsoleColor.Blue },
             { TileType.Grass, ConsoleColor.DarkGreen },
             { TileType.Sand, ConsoleColor.White },
             { TileType.DeepWater, ConsoleColor.DarkBlue},
             { TileType.None, ConsoleColor.Cyan}

        };
        static Random random = new Random();


        static int mapWidth = 20;
        static int mapHeight = 20;
        static TileType[,] mapBaseLayer = new TileType[mapWidth, mapHeight];
        static TileType[,] mapIslandLayer = new TileType[mapWidth, mapHeight];
        static int minStartingIslands = 3;
        static int maxStartingIslands = 7;
        static int amountOfIslands;

        static List<(int, int)> islandStartingCoords = new List<(int, int)>();

        static void InitializeMap()
        {
            for(int i = 0; i < mapBaseLayer.GetLength(0); i++)
            {
                for(int j = 0; j < mapBaseLayer.GetLength(1); j++)
                {

                    mapBaseLayer[j, i] = TileType.DeepWater;
                    mapIslandLayer[j, i] = TileType.None;

                }
            }

        }

        static void Main(string[] args)
        {
            InitializeMap();
            IMapRenderer mapRenderer = new CoolRenderer();
            
            
            CreateIslands(TileType.Water, TileType.DeepWater, (2,5),(8,12));

            CreateIslands(TileType.Grass, TileType.Water, (3, 7), (3, 35));

            CreateIslands(TileType.Sand, TileType.None, (1, 3), (0, 1));



            //DrawMap(new TileType[][,] {mapBaseLayer });
            mapRenderer.DrawMap(new TileType[][,] { mapBaseLayer });

            Console.ReadKey();

        }


        static void CreateBridge(TileType typeToConnect)
        {
            List<(int, int)> tilePositions = new List<(int, int)>();
            for (int i = 0; i < mapBaseLayer.GetLength(0); i++)
            {
                for(int j = 0; j < mapBaseLayer.GetLength(1); j++)
                {
                    if (mapBaseLayer[j, i] == typeToConnect) tilePositions.Add((j, i));
                }
            }


        }


        static void CreateIslands(TileType massTile, TileType trimTile, (int, int) islandAmtRange, (int, int) islandGrowthRange)
        {
            islandStartingCoords = new List<(int, int)>();

            amountOfIslands = random.Next(islandAmtRange.Item1, islandAmtRange.Item2);

            // create islands with 1 size
            for (int i = 0; i < amountOfIslands; i++)
            {
                islandStartingCoords.Add(SeedIsland(massTile));
            }


            // for each island, expand it
            ExpandIslands(massTile, islandGrowthRange);
            AddTrimToIslands(trimTile, massTile);
            MergeLayers(mapBaseLayer, mapIslandLayer);


           

        }


        // copy contents of one layer onto another
        static void MergeLayers(TileType[,] destination, TileType[,] source)
        {
            if(source.GetLength(0) != destination.GetLength(0) || source.GetLength(1) != destination.GetLength(1))
            {
                throw new ArgumentException("source ");
            }

            for(int i = 0; i < destination.GetLength(0); i++)
            {
                for(int j = 0; j < destination.GetLength(1); j++)
                {
                    if (source[j, i] == TileType.None) continue;

                    destination[j, i] = source[j, i];

                }
            }



        }

        static void ExpandIslands(TileType massTile, (int, int) islandGrowthRange)
        {
            foreach ((int, int) coord in islandStartingCoords)
            {
                (int, int) currentCoord = coord;
                int expansion = random.Next(islandGrowthRange.Item1, islandGrowthRange.Item2);
                for (int i = 0; i < expansion; i++)
                {

                    (int, int) expDir = (random.Next(-1, 2), random.Next(-1, 2));

                    // range check map size 
                    if (expDir.Item1 + currentCoord.Item1 < 0) continue;
                    if (expDir.Item1 + currentCoord.Item1 > mapWidth - 1) continue;
                    if (expDir.Item2 + currentCoord.Item2 < 0) continue;
                    if (expDir.Item2 + currentCoord.Item2 > mapHeight -1) continue;

                    currentCoord = (currentCoord.Item1 + expDir.Item1, currentCoord.Item2 + expDir.Item2);
                    mapIslandLayer[currentCoord.Item1, currentCoord.Item2] = massTile;

                }

            }
        }

        static void AddTrimToIslands(TileType trimType, TileType islandType)
        {
            for(int i = 0; i < mapIslandLayer.GetLength(0); i ++)
            {
                for(int j = 0; j < mapIslandLayer.GetLength(1); j++)
                {
                    if (mapIslandLayer[j,i] == islandType)
                    {
                        List<(int, int)> neighbours = GetNeighbourCoords((j, i));
                        foreach((int,int) coord in neighbours)
                        {
                            //Console.WriteLine((j + coord.Item1, i + coord.Item2));

                            if (mapIslandLayer[j + coord.Item1, i + coord.Item2] != islandType)
                            {
                                mapIslandLayer[j + coord.Item1, i + coord.Item2] = trimType;
                            }
                        }
                    }
                }
            }
        }

        static List<(int,int)> GetNeighbourCoords((int,int) inputCoord)
        {
            List<(int, int)> neighbours = new List<(int, int)>();

            for(int i = -1; i < 2; i++)
            {
                for( int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (inputCoord.Item1 + j < 0) continue;
                    if (inputCoord.Item1 + j > mapWidth - 1) continue;
                    if (inputCoord.Item2 + i < 0) continue;
                    if (inputCoord.Item2 + i > mapHeight - 1) continue;

                    neighbours.Add((j, i));

                }
            }

            return neighbours;

        }


        // TODO
       // (int, int)[] dirs = { (0, -1), (0, 1), (-1, 0), (1, 0) };

        static (int, int) SeedIsland(TileType tileType)
        {
            int randX = random.Next(0, mapWidth - 1);
            int randY = random.Next(0, mapHeight - 1);

            mapIslandLayer[randX, randY] = tileType;


            return (randX, randY);

        }

        static void DrawMap(TileType[][,] mapLayer)
        {

            foreach (TileType[,] map in mapLayer)
            {
                for(int i = 0; i < map.GetLength(0); i++)
                {
                    for(int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[j, i] == TileType.None) continue;
                        Console.SetCursorPosition(j * 2, i);
                        Console.BackgroundColor = _tileColors[map[j,i]];
                        Console.Write("  ");

                    }
                }

            }

        }



    }
}
