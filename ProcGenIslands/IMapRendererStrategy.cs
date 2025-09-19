using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcGenIslands
{
    internal interface IMapRendererStrategy
    {

           
        void DrawMap(TileType[][,] mapLayers);
       

    }
}
