using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using C1.Win.C1SplitContainer;
using C1.Win.C1Tile;
using C1.C1Pdf;
using System.IO;

namespace ImageGallery_Main_
{
    public partial class ImageGallery : Form
    {
        public ImageGallery()
        {
            InitializeComponent();
            this.MaximumSize = new Size(810, 810);
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.Size = new Size(780, 780);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ImageGallery";
        }
        C1SplitContainer sc = new C1SplitContainer();
        C1SplitterPanel Spanel1 = new C1SplitterPanel();
        C1SplitterPanel Spanel2 = new C1SplitterPanel();
        TableLayoutPanel DtableLayoutPanel = new TableLayoutPanel();
        Panel searchPanel = new Panel();
        Panel searchbtnPanel = new Panel();
        TextBox search_TB = new TextBox();
        Button search_BTN = new Button();
        PictureBox search_PB = new PictureBox();
        PictureBox export_PB = new PictureBox();
        PictureBox exportJpg_PB = new PictureBox();
        C1TileControl imageTileControl = new C1TileControl();
        C1PdfDocument pdfdoc = new C1PdfDocument();
        ToolStrip ts = new ToolStrip();
        ToolStripProgressBar pBar = new ToolStripProgressBar();
        DataFetcher dataFetch = new DataFetcher();
        List<ImageItem> imagesList;
        Group group = new Group();
        int checkedTiles = 0;
        StatusStrip statusStrip1 = new StatusStrip();
        ToolStripProgressBar toolStripProgressBar1 = new ToolStripProgressBar();
        Label title = new Label();

        private void ImageGallery_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(18,18,18);
            sc.Panels.Add(Spanel1);
            sc.Panels.Add(Spanel2);
            sc.Size = new Size(780, 730);
            Spanel1.Resizable = false;
            Spanel1.Height = 101;
            DtableLayoutPanel.ColumnCount = 3;
            DtableLayoutPanel.RowCount = 1;
            DtableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            DtableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5f));
            DtableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5f));
            DtableLayoutPanel.Dock = DockStyle.Fill;
            DtableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            DtableLayoutPanel.Size = new Size(800, 40);
            searchPanel.Location = new System.Drawing.Point(477, 0);
            searchPanel.Size = new Size(287, 10);
            searchPanel.Dock = DockStyle.Fill;
            searchPanel.Paint += new PaintEventHandler(OnSearchPanelPaint);
            search_TB.BorderStyle = BorderStyle.Fixed3D;
            search_TB.Location = new System.Drawing.Point(16, 60);
            search_TB.Size = new Size(244,50);
            search_TB.Font = new Font("Palatino Linotype",9F);
            search_TB.Text = "Search Image";
            search_TB.GotFocus += new EventHandler(RemoveText);
            search_TB.LostFocus += new EventHandler(AddText);
            searchbtnPanel.Location = new System.Drawing.Point(479, 12);
            searchbtnPanel.Margin = new Padding(2, 12, 45, 12);
            searchbtnPanel.Size = new Size(40,80);
            searchbtnPanel.TabIndex = 1;
            
            search_PB.Location = new System.Drawing.Point(0,53);
            search_PB.Margin = new Padding(0);
            search_PB.Size = new Size(40, 16);
            search_PB.SizeMode = PictureBoxSizeMode.Zoom;
            search_PB.Image = new Bitmap(Properties.Resources.Search);
            search_PB.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            search_PB.Click += new EventHandler(OnSearchClick);
            export_PB.Location = new System.Drawing.Point(29, 3);
            export_PB.Size = new Size(50,50);
            export_PB.SizeMode = PictureBoxSizeMode.StretchImage;
            export_PB.TabIndex = 2;
            export_PB.Image = new Bitmap(Properties.Resources.export);
            export_PB.Paint += new PaintEventHandler(OnExportPaint);
            export_PB.Click += new EventHandler(OnExportClick);
            export_PB.MouseHover += new EventHandler(OnPdfMouseHover);
            export_PB.Visible = false;
            export_PB.BackColor = Color.White;
            exportJpg_PB.BackColor = Color.White;
            exportJpg_PB.Location = new System.Drawing.Point(229, 3);
            exportJpg_PB.Size = new Size(50,50);
            exportJpg_PB.SizeMode = PictureBoxSizeMode.StretchImage;
            exportJpg_PB.TabIndex = 2;
            exportJpg_PB.Image = new Bitmap(Properties.Resources.exportJpg);
            exportJpg_PB.Paint += new PaintEventHandler(OnExportJpgPaint);
            exportJpg_PB.Click += new EventHandler(OnExportJpgClick);
            export_PB.MouseHover += new EventHandler(OnJpgMouseHover);
            exportJpg_PB.Visible = false;
            imageTileControl.AllowRearranging = true;
            imageTileControl.CellHeight = 78;
            imageTileControl.CellWidth = 78;
            imageTileControl.CellSpacing = 11;
            imageTileControl.Dock = DockStyle.Fill;
            imageTileControl.Size = new Size(764, 718);
            imageTileControl.SurfacePadding = new Padding(12, 4, 12, 4);
            imageTileControl.SwipeDistance = 20;
            imageTileControl.SwipeRearrangeDistance = 98;
            imageTileControl.AllowChecking = true;
            imageTileControl.TabIndex = 0;
            imageTileControl.TileChecked += new EventHandler<TileEventArgs>(OnTileChecked);
            imageTileControl.TileUnchecked += new EventHandler<TileEventArgs>(OnTileUnchecked);
            imageTileControl.Paint += new PaintEventHandler(OnTilePaint);
            imageTileControl.Groups.Add(group);
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;           
            statusStrip1.Items.Add(toolStripProgressBar1);
            statusStrip1.BackColor = Color.Transparent;
            statusStrip1.Visible = false;
            title.Text = "Image Gallery";
            title.Size = new Size(330, 50);
            title.Font = new Font("Algerian",16.5F);
            title.ForeColor = Color.White;
            title.Location = new Point(50,15);
            searchbtnPanel.Controls.Add(search_PB);
            searchPanel.Controls.Add(search_TB);
            searchPanel.Controls.Add(title);
            DtableLayoutPanel.Controls.Add(searchPanel, 1, 0);
            DtableLayoutPanel.Controls.Add(searchbtnPanel, 2, 0);
            Spanel1.Controls.Add(DtableLayoutPanel);
            Spanel2.Controls.Add(export_PB);
            Spanel2.Controls.Add(exportJpg_PB);
            Spanel2.Controls.Add(imageTileControl);
            Spanel2.Controls.Add(statusStrip1);
            this.Controls.Add(sc);
        }    
        
        private void OnExportJpgClick(object sender, EventArgs e) 
        {
            try
            {
                List<Image> img = new List<Image>();
                foreach (Tile tile in imageTileControl.Groups[0].Tiles)
                {
                    if (tile.Checked)
                    {
                        SaveFileDialog saveFile = new SaveFileDialog();
                        saveFile.DefaultExt = "Jpeg";
                        saveFile.Filter = "Image Files (*.jpg)|*.jpg*|Bitmap Image|*.bmp|Png Image|*.png";
                        if (saveFile.ShowDialog() == DialogResult.OK)
                        {
                            switch (saveFile.FilterIndex) 
                            {
                             case 1:
                                tile.Image.Save(saveFile.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                MessageBox.Show("Your File is Saved Successfully");
                                break;
                             case 2:
                                tile.Image.Save(saveFile.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                MessageBox.Show("Your File is Saved Successfully");
                                break;
                             case 3:
                                tile.Image.Save(saveFile.FileName, System.Drawing.Imaging.ImageFormat.Png);
                                MessageBox.Show("Your File is Saved Successfully");
                                break;

                            }
                        }
                    }
                }
            }
            catch(Exception z)
            {
                MessageBox.Show("Sorry we cant save your Image Due to" + z.Message);
            }
        }
       
        private void OnExportClick(object sender, EventArgs e)
        {
            try
            {
                List<Image> img = new List<Image>();
                foreach (Tile tile in imageTileControl.Groups[0].Tiles)
                {
                    if (tile.Checked)
                    {
                        img.Add(tile.Image);
                    }
                }
                ConvertPdf(img);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "pdf";
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf*";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    pdfdoc.Save(saveFile.FileName);
                    MessageBox.Show("Your File is Saved Successfully");
                }
            }
            catch (Exception z)
            {
                MessageBox.Show("Sorry we cant save your Image Due to" + z.Message);
            }
        }
        private void ConvertPdf(List<Image> images) 
        {
            RectangleF rect = pdfdoc.PageRectangle;
            bool fp = true;
            foreach (var sImg in images)
            {
                if (fp == false)
                {
                    pdfdoc.NewPage();
                }
                fp = false;
                rect.Inflate(-80, -80);
                pdfdoc.DrawImage(sImg, rect);
            }
        
        }
        
        private void OnTileChecked(object sender, TileEventArgs e)
        {
            checkedTiles++;
            export_PB.Visible = true;
            exportJpg_PB.Visible = true;
        }
        private void OnTileUnchecked(object sender, TileEventArgs e)
        {
            checkedTiles--;
            if (checkedTiles >= 1)
            {
                export_PB.Visible = true;
                exportJpg_PB.Visible = true;
            }
            else
            {
                export_PB.Visible = false;
                exportJpg_PB.Visible = false;
            }
        }
        public async void OnSearchClick(object sender, EventArgs e)
        {

            statusStrip1.Visible = true;
            imagesList = await dataFetch.GetImageData(search_TB.Text);
            AddTiles(imagesList);
            statusStrip1.Visible = false;
        }
        private void AddTiles(List<ImageItem> imagesList)
        {
            imageTileControl.Groups[0].Tiles.Clear();
            foreach (var imageitem in imagesList)
            {
                Tile tile = new Tile();
                tile.HorizontalSize = 2;
                tile.VerticalSize = 2;
                imageTileControl.Groups[0].Tiles.Insert(0,tile);
                Image img = Image.FromStream(new
               MemoryStream(imageitem.Base64));
                Template tl = new Template();
                ImageElement ie = new ImageElement();
                ie.ImageLayout = ForeImageLayout.Stretch;
                tl.Elements.Add(ie);
                tile.Template = tl;
                tile.Image = img;
            }
        }
        private void OnTilePaint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.Red);
            e.Graphics.DrawLine(p, 0, 60, 800, 60);

        }
        private void OnSearchPanelPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = searchbtnPanel.Bounds;
            r.Inflate(3, 3);
            Pen p = new Pen(Color.Aqua);
            e.Graphics.DrawRectangle(p, r);
         }
        private void OnExportPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(export_PB.Location.X, export_PB.Location.Y, export_PB.Width, export_PB.Height);
            r.X -= 29;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.Aqua);
            e.Graphics.DrawRectangle(p, r);
            
        }
        private void OnExportJpgPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(exportJpg_PB.Location.X, exportJpg_PB.Location.Y, exportJpg_PB.Width, exportJpg_PB.Height);
            r.X -= 229;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.Aqua);
            e.Graphics.DrawRectangle(p, r);
           
        }
        private void RemoveText(object sender, EventArgs e)
        {
            if (search_TB.Text.Equals("Search Image"))
            {
                search_TB.Text = "";
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (search_TB.Text.Equals(""))
                search_TB.Text = "Search Image";
        }
        private void OnPdfMouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(export_PB, "Export Your Images as PDF");
        
        }
        private void OnJpgMouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(exportJpg_PB, "Export Your Images as Jpg");

        }
    }
}

