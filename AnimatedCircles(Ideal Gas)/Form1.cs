namespace AnimatedCircles_Ideal_Gas_
{
    public partial class Form1 : Form
    {
        private Painter? p;
        public Form1()
        {

            InitializeComponent();
            this.Text = "Model of Ideal Gas";
            p = new Painter(pictureBox1.CreateGraphics());
            p.Start();
            for(int i =0; i < 10; i++)
            {
                p.AddNew();
                Thread.Sleep(30);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            p.AddNew();
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            if (p != null)
            {
                p.MainGraphics = pictureBox1.CreateGraphics();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            p.AddOrDeleteInPoint(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Стоп")
            {
                button2.Text = "Старт";
                button2.BackColor = Color.Green;
                p.Pause = true;
            }
            else
            {
                button2.Text = "Стоп";
                button2.BackColor = Color.Red;
                p.Pause = false;
            }
        }
    }
}