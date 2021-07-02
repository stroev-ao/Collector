namespace Collector
{
    partial class FMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.gb_Source = new System.Windows.Forms.GroupBox();
            this.cb_Subfolders = new System.Windows.Forms.CheckBox();
            this.cb_Sources = new System.Windows.Forms.ComboBox();
            this.gb_Destination = new System.Windows.Forms.GroupBox();
            this.cb_Destinations = new System.Windows.Forms.ComboBox();
            this.gb_Filter = new System.Windows.Forms.GroupBox();
            this.tb_Filter = new System.Windows.Forms.TextBox();
            this.rb_FilteredTypes = new System.Windows.Forms.RadioButton();
            this.rb_AllTypes = new System.Windows.Forms.RadioButton();
            this.l_Status = new System.Windows.Forms.Label();
            this.b_Start = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.b_Exit = new System.Windows.Forms.Button();
            this.pb_Spinner = new System.Windows.Forms.PictureBox();
            this.pb_Progress = new System.Windows.Forms.PictureBox();
            this.b_Destination = new System.Windows.Forms.Button();
            this.b_Source = new System.Windows.Forms.Button();
            this.gb_Source.SuspendLayout();
            this.gb_Destination.SuspendLayout();
            this.gb_Filter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Spinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Progress)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_Source
            // 
            this.gb_Source.Controls.Add(this.cb_Subfolders);
            this.gb_Source.Controls.Add(this.cb_Sources);
            this.gb_Source.Controls.Add(this.b_Source);
            this.gb_Source.Location = new System.Drawing.Point(12, 12);
            this.gb_Source.Name = "gb_Source";
            this.gb_Source.Size = new System.Drawing.Size(360, 69);
            this.gb_Source.TabIndex = 0;
            this.gb_Source.TabStop = false;
            this.gb_Source.Text = "Source";
            // 
            // cb_Subfolders
            // 
            this.cb_Subfolders.AutoSize = true;
            this.cb_Subfolders.Location = new System.Drawing.Point(36, 46);
            this.cb_Subfolders.Name = "cb_Subfolders";
            this.cb_Subfolders.Size = new System.Drawing.Size(125, 17);
            this.cb_Subfolders.TabIndex = 2;
            this.cb_Subfolders.Text = "Include all subfolders";
            this.cb_Subfolders.UseVisualStyleBackColor = true;
            this.cb_Subfolders.CheckedChanged += new System.EventHandler(this.cb_Subfolders_CheckedChanged);
            // 
            // cb_Sources
            // 
            this.cb_Sources.FormattingEnabled = true;
            this.cb_Sources.Location = new System.Drawing.Point(36, 19);
            this.cb_Sources.Name = "cb_Sources";
            this.cb_Sources.Size = new System.Drawing.Size(318, 21);
            this.cb_Sources.TabIndex = 1;
            this.cb_Sources.TextChanged += new System.EventHandler(this.cb_Sources_TextChanged);
            this.cb_Sources.Leave += new System.EventHandler(this.cb_Sources_Leave);
            // 
            // gb_Destination
            // 
            this.gb_Destination.Controls.Add(this.cb_Destinations);
            this.gb_Destination.Controls.Add(this.b_Destination);
            this.gb_Destination.Location = new System.Drawing.Point(12, 87);
            this.gb_Destination.Name = "gb_Destination";
            this.gb_Destination.Size = new System.Drawing.Size(360, 46);
            this.gb_Destination.TabIndex = 1;
            this.gb_Destination.TabStop = false;
            this.gb_Destination.Text = "Destination";
            // 
            // cb_Destinations
            // 
            this.cb_Destinations.FormattingEnabled = true;
            this.cb_Destinations.Location = new System.Drawing.Point(36, 19);
            this.cb_Destinations.Name = "cb_Destinations";
            this.cb_Destinations.Size = new System.Drawing.Size(318, 21);
            this.cb_Destinations.TabIndex = 1;
            this.cb_Destinations.TextChanged += new System.EventHandler(this.cb_Sources_TextChanged);
            this.cb_Destinations.Leave += new System.EventHandler(this.cb_Sources_Leave);
            // 
            // gb_Filter
            // 
            this.gb_Filter.Controls.Add(this.tb_Filter);
            this.gb_Filter.Controls.Add(this.rb_FilteredTypes);
            this.gb_Filter.Controls.Add(this.rb_AllTypes);
            this.gb_Filter.Location = new System.Drawing.Point(12, 139);
            this.gb_Filter.Name = "gb_Filter";
            this.gb_Filter.Size = new System.Drawing.Size(360, 89);
            this.gb_Filter.TabIndex = 2;
            this.gb_Filter.TabStop = false;
            this.gb_Filter.Text = "File type filter";
            // 
            // tb_Filter
            // 
            this.tb_Filter.Location = new System.Drawing.Point(6, 65);
            this.tb_Filter.Name = "tb_Filter";
            this.tb_Filter.Size = new System.Drawing.Size(348, 20);
            this.tb_Filter.TabIndex = 2;
            this.tb_Filter.Leave += new System.EventHandler(this.tb_Filter_Leave);
            // 
            // rb_FilteredTypes
            // 
            this.rb_FilteredTypes.AutoSize = true;
            this.rb_FilteredTypes.Location = new System.Drawing.Point(6, 42);
            this.rb_FilteredTypes.Name = "rb_FilteredTypes";
            this.rb_FilteredTypes.Size = new System.Drawing.Size(87, 17);
            this.rb_FilteredTypes.TabIndex = 1;
            this.rb_FilteredTypes.TabStop = true;
            this.rb_FilteredTypes.Text = "Filtered types";
            this.rb_FilteredTypes.UseVisualStyleBackColor = true;
            // 
            // rb_AllTypes
            // 
            this.rb_AllTypes.AutoSize = true;
            this.rb_AllTypes.Location = new System.Drawing.Point(6, 19);
            this.rb_AllTypes.Name = "rb_AllTypes";
            this.rb_AllTypes.Size = new System.Drawing.Size(64, 17);
            this.rb_AllTypes.TabIndex = 0;
            this.rb_AllTypes.TabStop = true;
            this.rb_AllTypes.Text = "All types";
            this.rb_AllTypes.UseVisualStyleBackColor = true;
            this.rb_AllTypes.CheckedChanged += new System.EventHandler(this.rb_AllTypes_CheckedChanged);
            // 
            // l_Status
            // 
            this.l_Status.AutoSize = true;
            this.l_Status.Location = new System.Drawing.Point(38, 265);
            this.l_Status.Name = "l_Status";
            this.l_Status.Size = new System.Drawing.Size(35, 13);
            this.l_Status.TabIndex = 3;
            this.l_Status.Text = "label1";
            // 
            // b_Start
            // 
            this.b_Start.Location = new System.Drawing.Point(216, 260);
            this.b_Start.Name = "b_Start";
            this.b_Start.Size = new System.Drawing.Size(75, 23);
            this.b_Start.TabIndex = 4;
            this.b_Start.Text = "Start";
            this.b_Start.UseVisualStyleBackColor = true;
            this.b_Start.Click += new System.EventHandler(this.b_Start_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.Location = new System.Drawing.Point(216, 260);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 5;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // b_Exit
            // 
            this.b_Exit.Location = new System.Drawing.Point(297, 260);
            this.b_Exit.Name = "b_Exit";
            this.b_Exit.Size = new System.Drawing.Size(75, 23);
            this.b_Exit.TabIndex = 6;
            this.b_Exit.Text = "Exit";
            this.b_Exit.UseVisualStyleBackColor = true;
            this.b_Exit.Click += new System.EventHandler(this.b_Exit_Click);
            // 
            // pb_Spinner
            // 
            this.pb_Spinner.Location = new System.Drawing.Point(12, 260);
            this.pb_Spinner.Name = "pb_Spinner";
            this.pb_Spinner.Size = new System.Drawing.Size(20, 20);
            this.pb_Spinner.TabIndex = 4;
            this.pb_Spinner.TabStop = false;
            this.pb_Spinner.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_Spinner_Paint);
            // 
            // pb_Progress
            // 
            this.pb_Progress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_Progress.Location = new System.Drawing.Point(12, 234);
            this.pb_Progress.Name = "pb_Progress";
            this.pb_Progress.Size = new System.Drawing.Size(360, 20);
            this.pb_Progress.TabIndex = 3;
            this.pb_Progress.TabStop = false;
            // 
            // b_Destination
            // 
            this.b_Destination.Image = global::Collector.Properties.Resources.folder;
            this.b_Destination.Location = new System.Drawing.Point(6, 19);
            this.b_Destination.Name = "b_Destination";
            this.b_Destination.Size = new System.Drawing.Size(21, 21);
            this.b_Destination.TabIndex = 0;
            this.b_Destination.UseVisualStyleBackColor = true;
            this.b_Destination.Click += new System.EventHandler(this.b_Destination_Click);
            // 
            // b_Source
            // 
            this.b_Source.Image = global::Collector.Properties.Resources.folder;
            this.b_Source.Location = new System.Drawing.Point(6, 19);
            this.b_Source.Name = "b_Source";
            this.b_Source.Size = new System.Drawing.Size(21, 21);
            this.b_Source.TabIndex = 0;
            this.b_Source.UseVisualStyleBackColor = true;
            this.b_Source.Click += new System.EventHandler(this.b_Source_Click);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 295);
            this.Controls.Add(this.b_Exit);
            this.Controls.Add(this.l_Status);
            this.Controls.Add(this.pb_Spinner);
            this.Controls.Add(this.pb_Progress);
            this.Controls.Add(this.gb_Filter);
            this.Controls.Add(this.gb_Destination);
            this.Controls.Add(this.gb_Source);
            this.Controls.Add(this.b_Start);
            this.Controls.Add(this.b_Cancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 334);
            this.MinimumSize = new System.Drawing.Size(400, 334);
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            this.Load += new System.EventHandler(this.FMain_Load);
            this.Shown += new System.EventHandler(this.FMain_Shown);
            this.gb_Source.ResumeLayout(false);
            this.gb_Source.PerformLayout();
            this.gb_Destination.ResumeLayout(false);
            this.gb_Filter.ResumeLayout(false);
            this.gb_Filter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Spinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Progress)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Source;
        private System.Windows.Forms.GroupBox gb_Destination;
        private System.Windows.Forms.Button b_Source;
        private System.Windows.Forms.ComboBox cb_Sources;
        private System.Windows.Forms.CheckBox cb_Subfolders;
        private System.Windows.Forms.ComboBox cb_Destinations;
        private System.Windows.Forms.Button b_Destination;
        private System.Windows.Forms.GroupBox gb_Filter;
        private System.Windows.Forms.TextBox tb_Filter;
        private System.Windows.Forms.RadioButton rb_FilteredTypes;
        private System.Windows.Forms.RadioButton rb_AllTypes;
        private System.Windows.Forms.PictureBox pb_Progress;
        private System.Windows.Forms.PictureBox pb_Spinner;
        private System.Windows.Forms.Label l_Status;
        private System.Windows.Forms.Button b_Start;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.Button b_Exit;
    }
}

