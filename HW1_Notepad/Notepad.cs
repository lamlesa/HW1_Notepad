using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace HW1_Notepad
{
    public partial class Notepad : Form
    {
        public string file_name;
        public bool is_file_changed;
        public int font_size = 10;
        public FontStyle font_style = FontStyle.Regular;
        public Color color = Color.Black;
        public Notepad()
        {
            InitializeComponent();
            file_name = "";
            is_file_changed = false;
            WriteNewHeading();
        }
        public void WriteNewHeading()
        {
            if (file_name != "")
            {
                Text = file_name + " - Блокнот";
            }
            else
            {
                Text = "Безымянный - Блокнот";
            }
        }
        public void CreateNewFile(object sender, EventArgs e)
        {
            SaveUnsavedFile();
            richTextBox1.Text = "";
            file_name = "";
            is_file_changed = false;
            WriteNewHeading();
        }
        public void SaveUnsavedFile()
        {
            if (is_file_changed)
            {
                DialogResult result = MessageBox.Show("Сохранить изменения в файле ?", "★", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    SaveFile(file_name);
                }
            }
        }
        public void SaveFile(string filename)
        {
            if (filename == "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    file_name = saveFileDialog1.FileName;
                }
            }
            try
            {
                StreamWriter writer = new StreamWriter(filename + ".txt", true, Encoding.Default);
                file_name = filename;
                writer.Write(richTextBox1.Text);
                writer.Close();
                is_file_changed = false;
            }
            catch
            {
                MessageBox.Show("Не удаётся сохранить файл.");
            }
            WriteNewHeading();
        }
        public void Save(object sender, EventArgs e)
        {
            SaveFile(file_name);
        }
        public void SaveAs(object sender, EventArgs e)
        {
            SaveFile("");
        }
        public void OpenFile(object sender, EventArgs e)
        {
            SaveUnsavedFile();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader reader = new StreamReader(openFileDialog1.FileName, Encoding.ASCII);
                    file_name = openFileDialog1.FileName;
                    richTextBox1.Text = reader.ReadToEnd();
                    reader.Close();
                    is_file_changed = false;
                }
                catch
                {
                    MessageBox.Show("Не удаётся открыть файл.");
                }
            }
            WriteNewHeading();
        }
        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!is_file_changed)
            {
                Text = Text.Replace("𓆏 ", "");
                is_file_changed = true;
                this.Text = "𓆏 " + this.Text;
            }
        }
        public void OnPrint(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPageHandler;
            PrintDialog p_dialog = new PrintDialog();
            if (p_dialog.ShowDialog() == DialogResult.OK)
            {
                p_dialog.Document.Print();
            }
        }
        public void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 0, 0);
        }
        public void CutText()
        {
            Clipboard.SetText(richTextBox1.SelectedText);
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart, richTextBox1.SelectionLength);
        }
        public void CopyText()
        {
            Clipboard.SetText(richTextBox1.SelectedText);
        }
        public void PasteText()
        {
            richTextBox1.Text += Clipboard.GetText();
        }
        private void OnClickCut(object sender, EventArgs e)
        {
            CutText();
        }
        private void OnClickCopy(object sender, EventArgs e)
        {
            CopyText();
        }
        private void OnClickPaste(object sender, EventArgs e)
        {
            PasteText();
        }
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUnsavedFile();
        }
        public void ClickFont(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
            }
        }
        public void ClickColor(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog1.Color;
            }
        }
        public void ClickHelp(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }
        private void ClickExit(object sender, EventArgs e)
        {
            SaveUnsavedFile();
            Application.Exit();
        }
    }
}