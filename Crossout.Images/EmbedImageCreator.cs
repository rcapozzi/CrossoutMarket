﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Crossout.Model.Items;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Crossout.Images
{
    public class EmbedImageCreator
    {
        Item item;
        string itemNameString;
        string sellPriceString;
        string buyPriceString;
        string branding;
        string backgroundPath = Path.GetFullPath(@"Sources\background.png");
        string itemImagePath;
        IList<DataPoint> itemData;

        public EmbedImageCreator(Item imageItem, IList<DataPoint> imageItemData, string imageBranding = "CrossoutDB.com")
        {
            item = imageItem;
            itemData = imageItemData;
            itemNameString = item.Name;
            sellPriceString = "Sell Price: " + item.FormatSellPrice;
            buyPriceString = "Buy Price: " + item.FormatBuyPrice;
            branding = imageBranding;

            itemImagePath = Path.Combine(@"img\items\" + item.Id + ".png");

            if (!File.Exists(itemImagePath))
            {
                itemImagePath = Path.GetFullPath(@"img\NoImage.png");
            }
        }

        PointF overlayLocation = new PointF(0f, 0f);
        PointF itemNameLocation = new PointF(80f, 1f);
        PointF sellPriceLocation = new PointF(80f, 20f);
        PointF buyPriceLocation = new PointF(80f, 35f);
        PointF chartLocation = new PointF(200f, 5f);
        PointF brandingLocation = new PointF(200f, 45f);

        Brush RarityColor(int rarityNumber)
        {
            Brush b;
            switch (rarityNumber)
            {
                case 1:
                    b = Brushes.White;
                    break;
                case 2:
                    b = Brushes.Blue;
                    break;
                case 3:
                    b = Brushes.Purple;
                    break;
                case 4:
                    b = Brushes.Gold;
                    break;
                case 5:
                    b = Brushes.OrangeRed;
                    break;
                default:
                    b = Brushes.LightGray;
                    break;
            }
            
            return b;
        }

        public Image CreateEmbedImage()
        {
            Bitmap bitmap = (Bitmap)Image.FromFile(backgroundPath);//load the image file
            Bitmap overlay = (Bitmap)Image.FromFile(itemImagePath);
            var chartStream = new MemoryStream();
            ChartImageCreator cic = new ChartImageCreator();
            cic.GenerateMinimalChart(itemData, chartStream);
            

            Graphics graphics = Graphics.FromImage(bitmap);
            Font arialFont = new Font("Arial", 8);
            Font arialBoldFont = new Font("Arial", 8, FontStyle.Bold);

            graphics.DrawString(itemNameString, arialBoldFont, RarityColor(item.RarityId), itemNameLocation);
            graphics.DrawString(sellPriceString, arialFont, Brushes.White, sellPriceLocation);
            graphics.DrawString(buyPriceString, arialFont, Brushes.White, buyPriceLocation);
            graphics.DrawString(branding, arialFont, Brushes.White, brandingLocation);

            graphics.DrawImage(overlay, new RectangleF(overlayLocation, new SizeF(64,64)));
            graphics.DrawImage(Image.FromStream(chartStream), chartLocation);

            Image img = bitmap;

            return img;
        }
    }
}
