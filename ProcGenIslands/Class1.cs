using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcGenIslands
{
    internal class CoolRenderer : IMapRenderer
    {
        public void DrawMap(TileType[][,] mapLayers)
        {
            int diag = 0;
            foreach (TileType[,] map in mapLayers)
            {
                
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    diag++;
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[j, i] == TileType.None) continue;
                        Console.SetCursorPosition(j * 2 + diag, i);
                        Console.BackgroundColor = Program._tileColors[map[j, i]];
                        Console.Write("  ");

                    }
                }

            }
        }
    }


    internal class BasicRenderer : IMapRenderer
    {
        public void DrawMap(TileType[][,] mapLayers)
        {
            foreach (TileType[,] map in mapLayers)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[j, i] == TileType.None) continue;
                        Console.SetCursorPosition(j * 2, i);
                        Console.BackgroundColor = Program._tileColors[map[j, i]];
                        Console.Write("  ");

                    }
                }

            }


        }
    }
}
