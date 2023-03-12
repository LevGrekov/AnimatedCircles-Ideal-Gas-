using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimatedCircles_Ideal_Gas_
{
    public class Painter
    {
        private object locker = new();
        private List<AnimatedCircle> animators = new();
        private Size containerSize;
        private Thread t;
        private Graphics mainGraphics;
        private BufferedGraphics bg;
        private bool isAlive;
        private bool pause = false;

        public bool Pause
        {
            get => pause;
            set => pause = value;
        }

        public bool PauseMethod()
        {
            return Pause;
        }

        public delegate bool Paused();

        private volatile int objectsPainted = 0;
        public Thread PainterThread => t;
        public Graphics MainGraphics
        {
            get => mainGraphics;
            set
            {
                lock (locker)
                {
                    mainGraphics = value;
                    ContainerSize = mainGraphics.VisibleClipBounds.Size.ToSize();
                    bg = BufferedGraphicsManager.Current.Allocate(
                        mainGraphics, new Rectangle(new Point(0, 0), ContainerSize)
                    );
                    objectsPainted = 0;
                }
            }
        }
        public Size ContainerSize
        {
            get => containerSize;
            set
            {
                containerSize = value;
                foreach (var animator in animators)
                {
                    animator.ContainerSize = ContainerSize;
                }
            }
        }

        public Painter(Graphics mainGraphics)
        {
            MainGraphics = mainGraphics;
        }

        public void AddNew()
        {
            var a = new AnimatedCircle(ContainerSize);
            animators.Add(a);
            a.Start();
            //Thread.Sleep(500);
        }

        public void Start()
        {
            isAlive = true;
            t = new Thread(() =>
            {
                try
                {
                    
                    while (isAlive)
                    {
                        if (pause)
                        {
                            foreach (var animator in animators)
                            {
                                animator.Pause = true;
                            }
                        }
                        else
                        {
                            var paused = new Paused(PauseMethod);
                            animators.RemoveAll(it => !it.IsAlive);
                            lock (locker)
                            {
                                if (PaintOnBuffer())
                                {
                                    bg.Render(MainGraphics);
                                }
                            }
                            if (isAlive) Thread.Sleep(30);
                        }
                    }
                }
                catch (ThreadInterruptedException e)
                {
                    
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private bool PaintOnBuffer()
        {
            objectsPainted = 0;
            var objectsCount = animators.Count;
            bg.Graphics.Clear(Color.White);
            foreach (var animator in animators)
            {
                animator.Paint(bg.Graphics);
                objectsPainted++;
            }

            return objectsPainted == objectsCount;
        }

        public void AddOrDeleteInPoint(MouseEventArgs point)
        {
            foreach (var animator in animators)
            {
                if( Math.Pow(animator.X-(point.X-animator.Radius),2) + 
                    Math.Pow(animator.Y-(point.Y-animator.Radius),2) <= 
                    Math.Pow(animator.Radius,2))
                {
                    animator.isClicked = true;
                    return;
                }
            }
            var rand = new Random();
            int radius = rand.Next(20, 50);
            var a = new AnimatedCircle(point.X-radius, point.Y-radius,radius, ContainerSize);
            animators.Add(a);
            a.Start();
        }
        
    }
}
