using System.Drawing;
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

            Size = new Size(400, 500);
            Location = new Point(100, 100);
            Bounds = new Rectangle(Location, Size);
            colliderPicture = new PictureBox();

            if (collider.GetType() == typeof(_2D_Game.RectangleF))
            {
                colliderPicture.Image = enemyImage;
            }
            if (collider.GetType() == typeof(Circle))
            {
                colliderPicture.Image = circleImage;
            }
            Controls.Add(colliderPicture);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //empty implementation
        }
    }
}
