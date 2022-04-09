using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MaterialSkin.Controls;

namespace Gallery
{
    public partial class GalleryForm : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialSkinManager;
        public PictureBox pictureBox = new PictureBox();
        public DbGalleryEntities db = new DbGalleryEntities();
        public string _comboVal = "Nature";
        public GalleryForm()
        {
            InitializeComponent();
            materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Indigo500, 
                                                                           MaterialSkin.Primary.Indigo700, 
                                                                           MaterialSkin.Primary.Indigo100, 
                                                                           MaterialSkin.Accent.Pink200, 
                                                                           MaterialSkin.TextShade.WHITE);
            List<string> categories = new List<string>
            {
                "Nature",
                "Human",
                "Technology",
                "Abstract"
            };
            /*
            for (int i = 0; i < categories.Count; i++)
            {
                    FileCategoryTbl fileCategoryTbl = new FileCategoryTbl();
                    fileCategoryTbl.categoryName = categories[i];
                    db.FileCategoryTbls.Add(fileCategoryTbl);
                    db.SaveChanges();
            }
            */
            int buttonHeight = 30;
            int buttonWidth = 100;
            MaterialButton allImagesButton = new MaterialButton()
            {
                Text = "All images",
                Location = new Point(30, 100)
            };
            allImagesButton.Click += new EventHandler(allImagesButton_Click);
            MaterialButton natureButton = new MaterialButton()
            {
                Text = "Nature",
                Location = new Point(140, 100)
            };
            natureButton.Click += new EventHandler(natureButton_Click);
            MaterialButton humanButton = new MaterialButton()
            {
                Text = "Human",
                Location = new Point(225, 100),
                Height = buttonHeight,
                Width = buttonWidth
            };
            MaterialButton technologyButton = new MaterialButton()
            {
                Text = "Technology",
                Location = new Point(305, 100),
                Height = buttonHeight,
                Width = buttonWidth
            };
            MaterialButton abstractButton = new MaterialButton()
            {
                Text = "Abstract",
                Location = new Point(430, 100),
                Height = buttonHeight,
                Width = buttonWidth
            };
            MaterialButton newImageButton = new MaterialButton()
            {
                Text = "New Image",
                Location = new Point(this.Width - buttonWidth - 30, 100),
                Height = buttonHeight,
                Width = buttonWidth
            };
            
            MaterialComboBox comboBox = new MaterialComboBox()
            {
                Location = new Point(this.Width - 300, 100),
                Height = buttonHeight,
                Width = 160
            };
            comboBox.SelectedValueChanged += new EventHandler(comboBox_SelectedValueChanged);
            comboBox.DataSource = categories;
            newImageButton.Click += new EventHandler(newImageButton_Click);
            this.Controls.Add(allImagesButton);
            this.Controls.Add(natureButton);
            this.Controls.Add(humanButton);
            this.Controls.Add(technologyButton);
            this.Controls.Add(abstractButton);
            this.Controls.Add(newImageButton);
            this.Controls.Add(comboBox);
        }
        public void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            _comboVal = cb.Text;
        }

        //  Test
        public void removePicture_Click(object sender, EventArgs e)
        {
            
            var pictures = sender as PictureBox;
            string message = "Do you want to remove picture";
            string caption = "Delete";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons);
            if(result == DialogResult.Yes)
            {
                /*
                var file = pictures.ImageLocation;
                using (var s = new FileStream(file, FileMode.Open))
                {
                    pictures.Image = Image.FromStream(s);
                }
                File.Delete(file);
                */
                MessageBox.Show("Deleted");
            }
            
        }
        public void allImagesButton_Click(object sender, EventArgs e)
        {
            foreach(var item in this.Controls.OfType<PictureBox>())
            {
                this.Controls.Remove(item);
            }
            int x = 10;
            int y = 200;
            for (int i = 0; i < Directory.GetFiles(@"..\..\Images", "*", SearchOption.TopDirectoryOnly).Length; i++)
            {
                PictureBox pictures = new PictureBox();
                pictures.Image = Image.FromFile(Directory.GetFiles(@"..\..\Images", "*", SearchOption.TopDirectoryOnly)[i]);
                pictures.Location = new Point(x, y);
                pictures.SizeMode = PictureBoxSizeMode.StretchImage;
                pictures.Height = 100;
                pictures.Width = 100;
                pictures.Click += new EventHandler(removePicture_Click);
                x += 110;
                this.Controls.Add(pictures);
            }
        }
        public void natureButton_Click(object sender, EventArgs e)
        {

            foreach (var item in this.Controls.OfType<PictureBox>())
            {
                this.Controls.Remove(item);
            }
            int x = 10;
            int y = 200;
            foreach (var item in db.FileTbls)
            {
                if (item.fileCategoryId == 1)
                {
                    PictureBox naturePictureBox = new PictureBox();
                    naturePictureBox.Image = Image.FromFile(item.fileLocation);
                    naturePictureBox.Location = new Point(x, y);
                    naturePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    naturePictureBox.Height = 100;
                    naturePictureBox.Width = 100;
                    this.Controls.Add(naturePictureBox);
                }
            }
        }
        public void newImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            Image File;
            if(open.ShowDialog() == DialogResult.OK)
            {
                File = Image.FromFile(open.FileName);
                pictureBox.Image = File;
                string folderPath = @"..\..\Images\\";
                pictureBox.Image.Save(folderPath + open.SafeFileName);
                /*
                FileTbl fileTbl = new FileTbl();
                fileTbl.fileName = Path.GetFileNameWithoutExtension(open.FileName);
                FileInfo fi = new FileInfo(open.FileName);
                fileTbl.fileSize = (Convert.ToDouble(fi.Length) / (1024.0 * 1024.0)).ToString() + " MB";
                fileTbl.fileExtention = Path.GetExtension(open.SafeFileName);
                fileTbl.fileAddDate = DateTime.Now;
                fileTbl.fileLocation = Path.GetDirectoryName(open.FileName);
                FileCategoryTbl fileCategoryTbl = new FileCategoryTbl();
                foreach (var item in db.FileCategoryTbls)
                {
                    if (item.categoryName == _comboVal)
                        fileCategoryTbl.id = item.id;
                }
                db.FileTbls.Add(fileTbl);
                db.SaveChanges();
                */
            }
        }
    }
}
