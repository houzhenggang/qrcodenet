﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Gma.QrCodeNet.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            qrCodeImgControl1.DarkBrush = Brushes.Yellow;
            qrCodeImgControl1.LightBrush = Brushes.MediumVioletRed;
            qrCodeGraphicControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            qrCodeGraphicControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
        	
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|Encapsuled PostScript (*.eps)|*.eps|SVG (*.svg)|*.svg";
            saveFileDialog.FileName = Path.GetFileName(GetFileNameProposal());
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

			if (saveFileDialog.FileName.EndsWith("eps"))
			{
                BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();

                // Initialize the EPS renderer
                var renderer = new EncapsulatedPostScriptRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new EPSFormColor(Color.Black), new EPSFormColor(Color.White));

                using (var file = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                {
                    renderer.WriteToStream(matrix, file);
                }
			}
            else if (saveFileDialog.FileName.EndsWith("svg"))
            {
                BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();

                // Initialize the EPS renderer
                var renderer = new SVGRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new EPSFormColor(Color.FromArgb(150, 200, 200, 210)), new EPSFormColor(Color.FromArgb(200, 255, 155, 0)));

                using (var file = File.OpenWrite(saveFileDialog.FileName))
                {
                    renderer.WriteToStream(matrix, file, false);
                }
            }
            else
            {

                //DrawingBrushRenderer dRender = new DrawingBrushRenderer(new FixedModuleSize(5, QuietZoneModules.Four));
                //BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();
                //using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                //{
                //    dRender.WriteToStream(matrix, ImageFormatEnum.PNG, stream);
                //}

                //WriteableBitmapRenderer wRender = new WriteableBitmapRenderer(new FixedModuleSize(15, QuietZoneModules.Four));
                //BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();
                //using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                //{
                //    wRender.WriteToStream(matrix, ImageFormatEnum.PNG, stream);
                //}

                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four));
                BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    gRender.WriteToStream(matrix, ImageFormat.Png, stream, new Point(600, 600));
                }
            }
           

        }

        private string GetFileNameProposal()
        {
            return textBoxInput.Text.Length > 10 ? textBoxInput.Text.Substring(0, 10) : textBoxInput.Text;
        }

        private void checkBoxArtistic_CheckedChanged(object sender, EventArgs e)
        {
            //qrCodeControl1.Artistic = checkBoxArtistic.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|Encapsuled PostScript (*.eps)|*.eps|SVG (*.svg)|*.svg";
            saveFileDialog.FileName = Path.GetFileName(GetFileNameProposal());
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            QrEncoder encoder = new QrEncoder();
            QrCode qrCode;
            byte[] byteArray = new byte[] { 34, 54, 90, 200 };
            if (!encoder.TryEncode(byteArray, out qrCode))
                return;
            if (saveFileDialog.FileName.EndsWith("eps"))
            {
                BitMatrix matrix = qrCode.Matrix;

                // Initialize the EPS renderer
                var renderer = new EncapsulatedPostScriptRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new EPSFormColor(Color.Black), new EPSFormColor(Color.White));

                using (var file = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                {
                    renderer.WriteToStream(matrix, file);
                }
            }
            else if (saveFileDialog.FileName.EndsWith("svg"))
            {
                BitMatrix matrix = qrCode.Matrix;

                // Initialize the EPS renderer
                var renderer = new SVGRenderer(
                    new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 6/72th inch (72 points = 1 inch)
                    new EPSFormColor(Color.FromArgb(150, 200, 200, 210)), new EPSFormColor(Color.FromArgb(200, 255, 155, 0)));

                using (var file = File.OpenWrite(saveFileDialog.FileName))
                {
                    renderer.WriteToStream(matrix, file, false);
                }
            }
            else
            {
                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four));
                BitMatrix matrix = qrCode.Matrix;
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    gRender.WriteToStream(matrix, ImageFormat.Png, stream, new Point(600, 600));
                }
            }
        }
    }
}
