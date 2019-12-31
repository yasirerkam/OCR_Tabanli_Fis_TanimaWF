using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace OCR_Tabanli_Fis_TanimaWF
{
    public enum Dil
    {
        TURKISH,
        ENGLISH
    }

    public class TesseractOCR
    {
        public static List<string> Doit(Bitmap image, string path, Dil dil, Button btn, ref string txtbox, ref List<List<string>> blocksOut)
        {
            btn.Enabled = false;
            List<string> strBlocks = new List<string>();
            List<List<string>> blocks = new List<List<string>>();

            string lng = "eng";
            if (dil == Dil.TURKISH)
                lng = "tur";

            var testImagePath = path;
            string FisinTamami = "";
            try
            {
                var engine = new TesseractEngine(@"./tessdata", lng, EngineMode.Default);
                var img = Pix.LoadFromFile(testImagePath);
                var page = engine.Process(img);

                FisinTamami = page.GetText();
                txtbox = FisinTamami;
                Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                Console.WriteLine("Text (iterator):");
                var iter = page.GetIterator();

                iter.Begin();

                List<string> satirlar = new List<string>();
                string satir = "";
                string block = "";

                do
                {
                    do
                    {
                        do
                        {
                            do
                            {
                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                {
                                    Console.WriteLine("<BLOCK>");
                                    satirlar = new List<string>();
                                    satir = "";
                                    block = "";
                                }

                                satir += iter.GetText(PageIteratorLevel.Word) + " ";
                                Console.Write(satir);

                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                {
                                    satirlar.Add(satir);
                                    block += satir;
                                    Console.WriteLine();
                                }
                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                            {
                                Console.WriteLine();
                                blocks.Add(satirlar);
                                strBlocks.Add(block);
                            }
                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                } while (iter.Next(PageIteratorLevel.Block));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Beklenmeyen hata: " + e.Message);
                Console.WriteLine("Detayları : ");
                Console.WriteLine(e.ToString());
            }

            btn.Enabled = true;

            blocksOut = blocks;
            //return FisinTamami;
            return strBlocks;
        }
    }
}