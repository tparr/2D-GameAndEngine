using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _2D_Game;

namespace AnimationEditorForms
{
    public partial class ColliderForm : Form
    {
        PictureBox colliderPicture;
        public ColliderForm(Collidable collider)
        {
            InitializeComponent();
            var RectImage = Properties.Resources.Rectangle;
            var circleImage = Properties.Resources.Circle;
            var enemyImage = Properties.Resources.enemy;

            this.Size = new Size(400, 500);
            this.Location = new Point(100, 100);
            this.Bounds = new Rectangle(this.Location,this.Size);
            colliderPicture = new PictureBox();

            if (collider.GetType() == typeof(_2D_Game.RectangleF))
            {
                colliderPicture.Image = enemyImage;
            }
            if (collider.GetType() == typeof(Circle))
            {
                colliderPicture.Image = circleImage;
            }
            this.Controls.Add(colliderPicture);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //empty implementation
        }
    }
}
