using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Collector
{
    public partial class FMain : Form
    {
        private const string TITLE = "Collector";
        private readonly string CONFIG_PATH;
        private INIManager manager;

        private bool isLoadFailed, isSubfolders, isFilterEnabled;

        private enum EState { Default = 0, Working = 1, Cancelling = 2 };

        private List<string> filter;

        private CCyclicalBuffer buffer;
        private System.Windows.Forms.Timer timer;

        private int lastProgress;
        private Font progressBarFont;
        //BackgroundWorker bw;

        string source, destination;

        private volatile bool cancel;

        private Color colorBegin;
        private Color colorEnd;

        public FMain()
        {
            InitializeComponent();

            CONFIG_PATH = Environment.CurrentDirectory + "\\config.ini";
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!File.Exists(CONFIG_PATH))
            {
                try
                {
                    File.Create(CONFIG_PATH);
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    isLoadFailed = true;
                    return;
                }
            }

            buffer = new CCyclicalBuffer();
            buffer.AddImage(new Bitmap(Properties.Resources.l0, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l1, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l2, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l3, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l4, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l5, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l6, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l7, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l8, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l9, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l10, new Size(20, 20)));
            buffer.AddImage(new Bitmap(Properties.Resources.l11, new Size(20, 20)));

            timer = new System.Windows.Forms.Timer() { Interval = 50 };
            timer.Tick += (ss, ee) => { pb_Spinner.Invalidate(); };

            //colorBegin = Color.FromArgb(52, 209, 180);
            //colorEnd = Color.FromArgb(58, 235, 202);

            colorBegin = Color.FromArgb(0, 229, 0);
            colorEnd = Color.FromArgb(0, 255, 0);

            progressBarFont = new Font("Courier New", 10, FontStyle.Bold);

            manager = new INIManager(CONFIG_PATH);

            string temp = manager.GetPrivateString("Main", "IsSubfolders");
            isSubfolders = (temp == string.Empty) ? true : Convert.ToBoolean(temp);

            cb_Subfolders.Checked = isSubfolders;

            temp = manager.GetPrivateString("Main", "Filter");
            tb_Filter.Text = temp;

            temp = manager.GetPrivateString("Main", "LastSource");
            cb_Sources.Items.Add(temp);

            temp = manager.GetPrivateString("Main", "LastDestination");
            cb_Destinations.Items.Add(temp);

            rb_AllTypes.Checked = true;

            SetState(EState.Default);

            cb_Sources_TextChanged(cb_Sources, new EventArgs());

            pb_Progress.Image = Properties.Resources.progressBar;
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;

            if (isLoadFailed)
                Application.Exit();
        }

        private void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (isLoadFailed)
                return;

            manager.WritePrivateString("Main", "IsSubfolders", isSubfolders.ToString());
            manager.WritePrivateString("Main", "Filter", tb_Filter.Text);

            string temp = cb_Sources.Text;
            if (temp.Length > 0)
                manager.WritePrivateString("Main", "LastSource", temp);

            temp = cb_Destinations.Text;
            if (temp.Length > 0)
                manager.WritePrivateString("Main", "LastDestination", temp);

            if (buffer != null)
                buffer.Clear();

            timer.Dispose();
            progressBarFont.Dispose();

            Cursor.Current = Cursors.Default;
        }

        private void b_Source_Click(object sender, EventArgs e)
        {
            string path = cb_Sources.Text;

            cb_Sources.Text = ShowFolderDialog(path);
            cb_Sources.Focus();
        }

        private void cb_Sources_TextChanged(object sender, EventArgs e)
        {
            bool success = cb_Sources.Text.Length > 0 & cb_Destinations.Text.Length > 0;
            b_Start.Enabled = success;
        }

        private void cb_Sources_Leave(object sender, EventArgs e)
        {
            ComboBox cb = (sender as ComboBox);
            string text = cb.Text;
            if (text == string.Empty)
                return;

            bool success = CheckPath(text);
            //b_Start.Enabled = success;
            if (!success)
            {
                cb.BackColor = Color.Crimson;
                cb.ForeColor = Color.White;
                MessageBox.Show("Path does not exist", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                cb.BackColor = Color.White;
                cb.ForeColor = Color.Black;
            }
        }

        private void cb_Subfolders_CheckedChanged(object sender, EventArgs e)
        {
            isSubfolders = cb_Subfolders.Checked;
        }

        private void b_Destination_Click(object sender, EventArgs e)
        {
            string path = cb_Destinations.Text;

            cb_Destinations.Text = ShowFolderDialog(path);
            cb_Destinations.Focus();
        }

        private void rb_AllTypes_CheckedChanged(object sender, EventArgs e)
        {
            isFilterEnabled = !rb_AllTypes.Checked;

            tb_Filter.Enabled = isFilterEnabled;
        }

        private void tb_Filter_Leave(object sender, EventArgs e)
        {
            if (!CheckFilter(tb_Filter.Text))
            {
                MessageBox.Show("File types is incorrect", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tb_Filter.SelectAll();
            }
            else
            {
                string text = null;
                for (int i = 0; i < filter.Count; i++)
                    text += (i < filter.Count - 1) ? filter[i] + "; " : filter[i];
                
                tb_Filter.Text = text;
            }
        }

        private void pb_Spinner_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(buffer.GetFrame(), 0, 0);
        }

        private void b_Start_Click(object sender, EventArgs e)
        {
            cancel = false;

            SetState(EState.Working);
            SetStatusText("Verification...");

            source = cb_Sources.Text;
            destination = cb_Destinations.Text;

            bool success = CheckPath(source);
            if (!success)
            {
                MessageBox.Show("Source path does not exist", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cb_Sources.Focus();
                cb_Sources.SelectAll();

                SetState(EState.Default);
                SetStatusText("");
                b_Start.Enabled = success;
                return;
            }
            success = CheckPath(destination);
            if (!success)
            {
                MessageBox.Show("Destination path does not exist", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cb_Destinations.Focus();
                cb_Destinations.SelectAll();

                SetState(EState.Default);
                SetStatusText("");
                b_Start.Enabled = success;
                return;
            }
            if (isFilterEnabled)
            {
                success = CheckFilter(tb_Filter.Text);
                if (!success)
                {
                    MessageBox.Show("File types is incorrect", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tb_Filter.Focus();
                    tb_Filter.SelectAll();

                    SetState(EState.Default);
                    SetStatusText("");
                    b_Start.Enabled = success;
                    return;
                }
            }

            tb_Filter_Leave(tb_Filter, new EventArgs());

            SetState(EState.Working);
            SetStatusText("Analyze...");

            List<string> files = (isSubfolders) ?
                new List<string>(GetFilesFromDirectory(source)) :
                files = new List<string>(Directory.GetFiles(source));

            if (isFilterEnabled)
                files.RemoveAll(f => !filter.Contains(Path.GetExtension(f)));

            List<int> failedFiles = new List<int>();

            int workerThreads, completionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            ThreadPool.SetMaxThreads(workerThreads, completionPortThreads);

            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
            int progress = 0, lastProgress = 0;

            int count = files.Count;
            for (int j = 0; j < count; j++)
            {
                ThreadPool.QueueUserWorkItem
                (
                    (param) =>
                    {
                        if (cancel)
                        {
                            Interlocked.Increment(ref progress);
                            return;
                        }

                        int i = (int)param;

                        string sourceFileName = files[i];
                        string fileName = Path.GetFileName(sourceFileName);
                        string destFileName = destination + "\\" + fileName;

                        int tries = 0;
                        bool copySuccess = false;
                        bool fileExists = false;
                        do
                        {
                            try
                            {
                                File.Copy(sourceFileName, destFileName);
                            }
                            catch (IOException ioe)
                            {
                                byte[] original = GetFileMD5(sourceFileName);
                                byte[] copy = GetFileMD5(destFileName);

                                fileExists = Equals(original, copy);

                                //если такой же файл существует, то пропускаем его
                                if (fileExists)
                                    copySuccess = true;
                                //если имена одинаковые, но файлы разные
                                else
                                {
                                    //считаем, сколько таких же файлов
                                    int cnt = 0;
                                    string[] suspiciousFiles = System.IO.Directory.GetFiles(destination);
                                    for (int k = 0; k < suspiciousFiles.Length; k++)
                                    {
                                        byte[] bytes = GetFileMD5(suspiciousFiles[k]);

                                        if (Equals(original, bytes))
                                        {
                                            copySuccess = true;
                                            break;
                                        }
                                        else if (suspiciousFiles[k].Contains(Path.GetFileNameWithoutExtension(fileName)))
                                            cnt++;

                                        bytes = null;
                                    }

                                    if (!copySuccess && cnt > 0)
                                    {
                                        //меняем имя файла и путь
                                        fileName = Path.GetFileNameWithoutExtension(fileName) + "_" + cnt.ToString() + Path.GetExtension(fileName);
                                        destFileName = destination + "\\" + fileName;
                                        //tries++;
                                    }

                                    suspiciousFiles = null;
                                }

                                copy = null;
                                original = null;
                                continue;
                            }

                            //если файл скопирован неудачно, то удаляем его
                            copySuccess = CheckFilesMD5(sourceFileName, destFileName);
                            if (!copySuccess)
                            {
                                try
                                {
                                    File.Delete(destFileName);
                                }
                                catch { }

                                tries++;
                            }
                        }
                        while (!copySuccess && tries < 2);

                        if (fileExists)
                        {
                            Interlocked.Increment(ref progress);
                            return;
                        }

                        if (tries == 2)
                            lock (failedFiles)
                                failedFiles.Add(i);

                        Interlocked.Increment(ref progress);
                    },
                    j
                );
            }

            while (Interlocked.CompareExchange(ref progress, count, count) != count)
            {
                if (lastProgress != progress)
                {
                    lastProgress = progress;

                    int prgs = (int)(100.0 * (progress + 1) / count);
                    SetProgress(prgs, colorBegin, colorEnd);
                    TaskbarManager.Instance.SetProgressValue(prgs, 100);
                    SetStatusText(string.Format("Copying files: {0} / {1}", progress + 1, count));
                }

                Application.DoEvents();
                Thread.Sleep(5);
            }

            SetState(EState.Default);
            SetStatusText("");

            if (cancel)
                MessageBox.Show("Operation cancelled", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (failedFiles.Count > 0)
            {
                string text = "Next files cannot be copied:\n";
                for (int i = 0; i < failedFiles.Count; i++)
                    text += files[failedFiles[i]] + "\n";
                MessageBox.Show(text, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("Successfully completed!\n\nOpen destination folder?", TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    Process.Start(destination);
            }

            SetProgress(0, colorBegin, colorEnd);
            TaskbarManager.Instance.SetProgressValue(0, 100);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);

            files.Clear();
            files = null;


















            /*if (bw == null)
            {
                bw = new BackgroundWorker()
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

                bw.DoWork += (ss, ee) =>
                {
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    BackgroundWorker worker = ss as BackgroundWorker;

                    int count = files.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (cancel)
                            break;

                        string sourceFileName = files[i];
                        string fileName = Path.GetFileName(sourceFileName);
                        string destFileName = destination + "\\" + fileName;

                        int tries = 0;
                        bool copySuccess = false;
                        bool fileExists = false;
                        do
                        {
                            try
                            {
                                File.Copy(sourceFileName, destFileName);
                            }
                            catch (IOException ioe)
                            {
                                byte[] original = GetFileMD5(sourceFileName);
                                byte[] copy = GetFileMD5(destFileName);

                                fileExists = Equals(original, copy);

                                //если такой же файл существует, то пропускаем его
                                if (fileExists)
                                    copySuccess = true;
                                //если имена одинаковые, но файлы разные
                                else
                                {
                                    //считаем, сколько таких же файлов
                                    int cnt = 0;
                                    string[] suspiciousFiles = System.IO.Directory.GetFiles(destination);
                                    for (int j = 0; j < suspiciousFiles.Length; j++)
                                    {
                                        byte[] bytes = GetFileMD5(suspiciousFiles[j]);

                                        if (Equals(original, bytes))
                                        {
                                            copySuccess = true;
                                            break;
                                        }
                                        else if (suspiciousFiles[j].Contains(Path.GetFileNameWithoutExtension(fileName)))
                                            cnt++;

                                        bytes = null;
                                    }

                                    if (!copySuccess && cnt > 0)
                                    {
                                        //меняем имя файла и путь
                                        fileName = Path.GetFileNameWithoutExtension(fileName) + "_" + cnt.ToString() + Path.GetExtension(fileName);
                                        destFileName = destination + "\\" + fileName;
                                        //tries++;
                                    }

                                    suspiciousFiles = null;
                                }

                                copy = null;
                                original = null;
                                continue;
                            }

                            //если файл скопирован неудачно, то удаляем его
                            copySuccess = CheckFilesMD5(sourceFileName, destFileName);
                            if (!copySuccess)
                            {
                                try
                                {
                                    File.Delete(destFileName);
                                }
                                catch { }
                                
                                tries++;
                            }
                        }
                        while (!copySuccess && tries < 2);

                        worker.ReportProgress((int)(100.0 * (i + 1) / count), new Point(i + 1, count));
                        
                        if (fileExists)
                            continue;

                        if (tries == 2)
                            failedFiles.Add(i);
                    }
                };

                bw.ProgressChanged += (ss, ee) =>
                {
                    int progress = ee.ProgressPercentage;
                    SetProgress(progress, colorBegin, colorEnd);
                    TaskbarManager.Instance.SetProgressValue(progress, 100);
                    Point p = (Point)ee.UserState;
                    SetStatusText(string.Format("Copying files: {0} / {1}", p.X, p.Y));
                };

                bw.RunWorkerCompleted += (ss, ee) =>
                {
                    SetState(EState.Default);
                    SetStatusText("");

                    if (ee.Error != null)
                        MessageBox.Show(ee.Error.Message, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else if (cancel)
                        MessageBox.Show("Operation cancelled", TITLE, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else if (failedFiles.Count > 0)
                    {
                        string text = "Next files cannot be copied:\n";
                        for (int i = 0; i < failedFiles.Count; i++)
                            text += files[failedFiles[i]] + "\n";
                        MessageBox.Show(text, TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (DialogResult.Yes == MessageBox.Show("Successfully completed!\n\nOpen destination folder?", TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                            Process.Start(destination);
                    }

                    SetProgress(0, colorBegin, colorEnd);
                    TaskbarManager.Instance.SetProgressValue(0, 100);
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);

                    files.Clear();
                    files = null;

                    bw.Dispose();
                    bw = null;
                };
            }

            SetStatusText("Copying files...");

            bw.RunWorkerAsync();*/
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("Cancel operation?", TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                return;

            cancel = true;
            SetState(EState.Cancelling);
        }

        private void b_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string ShowFolderDialog(string selectedPath = "")
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog() { RootFolder = Environment.SpecialFolder.Desktop };
            if (selectedPath != string.Empty)
                fbd.SelectedPath = selectedPath;

            if (fbd.ShowDialog() != DialogResult.OK)
                return string.Empty;

            string path = fbd.SelectedPath;
            fbd.Dispose();

            return path;
        }

        private bool CheckPath(string path)
        {
            return Directory.Exists(path);
        }

        private void SetState(EState state)
        {
            bool _default = state == EState.Default;

            gb_Source.Enabled = _default;
            gb_Destination.Enabled = _default;
            gb_Filter.Enabled = _default;

            pb_Spinner.Visible = !_default;
            l_Status.Visible = !_default;

            b_Cancel.Enabled = !_default && state != EState.Cancelling;
            b_Cancel.Visible = !_default;
            b_Start.Enabled = b_Start.Visible = _default;
            b_Exit.Enabled = _default;

            if (!_default)
                timer.Start();
            else
                timer.Stop();
        }

        private bool CheckFilter(string filter)
        {
            string pattern = @"\.\w+";
            var matches = Regex.Matches(filter, pattern);

            if (matches.Count > 0)
            {
                this.filter = new List<string>();

                foreach (Match m in matches)
                    this.filter.Add(m.Value);

                return true;
            }
            else
                return false;
        }

        private void SetProgress(int progress, Color begin, Color end)
        {
            lastProgress = progress;

            int width = pb_Progress.Width;
            int height = pb_Progress.Height;

            int readyWidth = width * progress / 100;

            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            if (progress > 0)
            {
                LinearGradientMode direction = LinearGradientMode.Horizontal;
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, readyWidth, height), begin, end, direction);

                g.FillRectangle(new SolidBrush(pb_Progress.Parent.BackColor), readyWidth, 0, width - readyWidth, height);
                g.FillRectangle(brush, 0, 0, readyWidth, height);
            }
            else
                g.FillRectangle(new SolidBrush(pb_Progress.Parent.BackColor), 0, 0, width, height);

            if (progress > 0)
            {
                string text = progress.ToString() + "%";
                SizeF size = g.MeasureString(text, progressBarFont);
                g.DrawString(text, progressBarFont, Brushes.Black, width / 2 - size.Width / 2, height / 2 - size.Height / 2);
            }

            pb_Progress.BackgroundImage = bmp;
            pb_Progress.Image = Properties.Resources.progressBar;

            g.Dispose();
        }

        private string[] GetFilesFromDirectory(string path)
        {
            List<string> files = new List<string>(Directory.GetFiles(path));

            string[] subDirs = Directory.GetDirectories(path);
            for (int i = 0; i < subDirs.Length; i++)
                files.AddRange(GetFilesFromDirectory(subDirs[i]));

            return files.ToArray();
        }

        private void SetStatusText(string text)
        {
            l_Status.Text = text;
        }

        private byte[] GetFileMD5(string path)
        {
            byte[] result = null;

            using (Stream stream1 = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (MD5 md5 = MD5.Create())
                result = md5.ComputeHash(stream1);

            return result;
        }

        private bool CheckFilesMD5(string file1, string file2)
        {
            bool result = false;

            using (Stream stream1 = new FileStream(file1, FileMode.Open, FileAccess.Read))
            {
                using (Stream stream2 = new FileStream(file2, FileMode.Open, FileAccess.Read))
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        byte[] bytes1 = md5.ComputeHash(stream1);
                        byte[] bytes2 = md5.ComputeHash(stream2);

                        result = Equals(bytes1, bytes2);

                        bytes1 = null;
                        bytes2 = null;
                    }
                }
            }

            return result;
        }

        private bool Equals(byte[] arr1, byte[] arr2)
        {
            bool result = true;

            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i])
                {
                    result = false;
                    break;
                }

            return result;
        }
    }
}
