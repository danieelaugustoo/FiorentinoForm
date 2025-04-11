﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiorentinoForm
{
    public class CircleBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0,0, this.Width - 1, this.Height - 1);
            this.Region = new Region(gp);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
