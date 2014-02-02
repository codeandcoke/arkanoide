using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arkanoide
{
    public class Util
    {
        public static double GradosARadianes(double angulo)
        {
            return Math.PI * angulo / 180;
        }
    }
}
