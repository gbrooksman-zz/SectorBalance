using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SectorBalanceClient.Shared
{
    public static class ColorPicker
    {
        public static string Get(int id)
        {
            string colorName = string.Empty;
            switch (id)
            {
                case 0:
                case 1:
                    colorName = "Red";
                    break;
                case 2:
                    colorName = "Blue";
                    break;
                case 3:
                    colorName = "Green";
                    break;
                case 4:
                    colorName = "Yellow";
                    break;
                case 5:
                    colorName = "Gray";
                    break;
                case 6:
                    colorName = "LightRed";
                    break;
                case 7:
                    colorName = "LightBlue";
                    break;
                case 8:
                    colorName = "LightGreen";
                    break;
                case 9:
                    colorName = "LightYellow";
                    break;
                case 10:
                    colorName = "AliceBlue";
                    break;
                default:
                    colorName = "White";
                    break;
            }

            return colorName;

        }

    }
}
