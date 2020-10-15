using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace ADCP
{
    public class ClassPrint
    {
        public ClassPrint()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            this.docToPrint.PrintPage += new PrintPageEventHandler(docToPrint_PrintPage);
        }//将事件处理函数添加到PrintDocument的PrintPage中 

        // Declare the PrintDocument object.
        private System.Drawing.Printing.PrintDocument docToPrint =  new System.Drawing.Printing.PrintDocument();//创建一个PrintDocument的实例 

        private System.IO.Stream streamToPrint;
        string streamType;

        // This method will set properties on the PrintDialog object and
        // then display the dialog.
        public void StartPrint(Stream streamToPrint, string streamType)
        {
            this.streamToPrint = streamToPrint;
            this.streamType = streamType;
            // Allow the user to choose the page range he or she would
            // like to print.
            System.Windows.Forms.PrintDialog PrintDialog1 = new PrintDialog();//创建一个PrintDialog的实例。
            PrintDialog1.AllowSomePages = true;

            // Show the help button.
            PrintDialog1.ShowHelp = true;

            //将纸张按照横向打印
            PrinterSettings pss = new System.Drawing.Printing.PrinterSettings(); 
            pss.DefaultPageSettings.Landscape = true;
            docToPrint.PrinterSettings = pss;

            // Set the Document property to the PrintDocument for 
            // which the PrintPage Event has been handled. To display the
            // dialog, either this property or the PrinterSettings property 
            // must be set 
            PrintDialog1.Document = docToPrint;//把PrintDialog的Document属性设为上面配置好的PrintDocument的实例 
            DialogResult result = PrintDialog1.ShowDialog();//调用PrintDialog的ShowDialog函数显示打印对话框 

            // If the result is OK then print the document.
            if (result == DialogResult.OK)
            {
                docToPrint.Print();//开始打印
            }

        }

        // The PrintDialog will print the document
        // by handling the document's PrintPage event.
        private void docToPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)//设置打印机开始打印的事件处理函数
        {

            // Insert code to render the page here.
            // This code will be called when the control is drawn. 

            // The following code will render a simple
            // message on the printed document
            switch (this.streamType)
            {
                case "txt":
                    string text = null;
                    System.Drawing.Font printFont = new System.Drawing.Font
                     ("Arial", 9, System.Drawing.FontStyle.Regular);

                    // Draw the content.
                    System.IO.StreamReader streamReader = new StreamReader(this.streamToPrint);
                    text = streamReader.ReadToEnd();
                    e.Graphics.DrawString(text, printFont, System.Drawing.Brushes.Black, e.MarginBounds.X, e.MarginBounds.Y);
                    break;
                case "image":
                    System.Drawing.Image image = System.Drawing.Image.FromStream(this.streamToPrint);
                    int x = e.MarginBounds.X;

                    int y = e.MarginBounds.Y;
                    int width = image.Width;
                    int height = image.Height;
                    if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height))
                    {
                        width = e.MarginBounds.Width;
                        height = image.Height * e.MarginBounds.Width / image.Width;
                    }
                    else
                    {
                        height = e.MarginBounds.Height;
                        width = image.Width * e.MarginBounds.Height / image.Height;
                    }
                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, width, height);
                    e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
                    break;
                default:
                    break;
            }

        }
    }
}
