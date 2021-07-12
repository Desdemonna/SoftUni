﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClassBoxData
{
    public class Box
    {
        private double length;
        private double width;
        private double height;

        public Box(double length, double width, double height)
        {
            this.Length = length;
            this.Width = width;
            this.Height = height;
        }

        public double Length
        {
            get 
            { 
                return this.length; 
            }
            private set 
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Length cannot be zero or negative.");
                }
                this.length = value;
            }
        }

        public double Width
        {
            get
            {
                return this.width;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Width cannot be zero or negative.");
                }

                this.width = value;
            }
        }

        public double Height
        {
            get
            {
                return this.height;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Height cannot be zero or negative.");
                }
                this.height = value;
            }
        }
        /// <summary>
        /// Volume = lwh
        /// </summary>
        /// <returns></returns>
        public string Volume()
        {
            double result = this.Length * this.Width * this.Height;
            return $"Volume - {result:F2}";
        }
        /// <summary>
        /// LiteralSurfaceArea = 2lh + 2wh
        /// </summary>
        /// <returns></returns>
        public string LiteralSurfaceArea()
        {
            double result = (2 * this.Length * this.Height) + (2 * this.Width * this.Height);
            return $"Lateral Surface Area - {result:F2}"; 
        }
        /// <summary>
        /// SurfaceArea = 2lw + 2lh + 2wh
        /// </summary>
        /// <returns></returns>
        public string SurfaceArea()
        {
            double result = (2 * this.Length * this.Width) + (2 * this.Length * this.Height) + (2 * this.Width * this.Height);
            return $"Surface Area - {result:F2}";
        }
    }
}
