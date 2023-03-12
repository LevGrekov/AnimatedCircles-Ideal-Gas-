using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimatedCircles_Ideal_Gas_
{
    internal class AnimatedCircle
    {
        private Random random = new();
        private double x;
        private double y;
        private bool is_cklicked = false;
        private bool pause = false;
        public bool Pause
        {
            get => pause;
            set => pause = value;
        }
        public double X => x;
        public double Y => y;

        private int radius;
        public double Radius => radius;

        private double speedX;
        private double speedY;
        private Color color;

        private Thread? t = null;
        public bool IsAlive => t == null || t.IsAlive;

        public bool isClicked
        {
            get => is_cklicked;
            set => is_cklicked = value;
        }
        public Size ContainerSize { get; set; }

        public AnimatedCircle(int x, int y, int radius, Size ContainerSize)
        {
            this.radius = radius;
            this.x = x;
            this.y = y;
            while (Math.Abs(speedX) < 0.5 && Math.Abs(speedY) < 0.5)
            {
                speedX = random.Next(-5, 5) * random.NextDouble();
                speedY = random.Next(-5, 5) * random.NextDouble();
            }

            color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            this.ContainerSize = ContainerSize;
        }

        public AnimatedCircle(Size ContainerSize)
        {
            this.radius = random.Next(20, 50);

            this.x = random.Next((radius*2),ContainerSize.Width - (radius*2));
            this.y = random.Next((radius * 2), ContainerSize.Height - (radius*2));

            while(Math.Abs(speedX) < 0.5 && Math.Abs(speedY) < 0.5)
            {
                speedX = random.Next(-5, 5) * random.NextDouble();
                speedY = random.Next(-5, 5) * random.NextDouble();
            }

            color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            this.ContainerSize = ContainerSize;
        }
        public void Start()
        {
            t = new Thread(() =>
            {
                while (!isClicked)
                {
                    Reflect();
                    Thread.Sleep(30);
                    Move();
                    while (pause)
                    {
                        Thread.Sleep(1);
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void Move()
        {
            x += speedX;
            y += speedY;
        }

        public void Paint(Graphics g)
        {
            var brush = new SolidBrush(color);
            g.FillEllipse(brush, (float)x, (float)y, radius*2, radius*2);
        }

        public void Reflect()
        {
            if (this.x + this.radius * 2 >= ContainerSize.Width)
            {
                speedX = -speedX;
            }
            if (this.x <= 0)
            {
                speedX = -speedX;
            }
            if (this.y + this.radius * 2 >= ContainerSize.Height)
            {
                speedY = -speedY;
            }
            if (this.y <= 0)
            {
                speedY = -speedY;
            }
        }

    }
}
