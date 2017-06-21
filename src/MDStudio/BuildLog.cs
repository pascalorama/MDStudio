﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDStudio
{
    public partial class BuildLog : Form
    {
        MainForm m_Parent;
        string rawLog;

        public BuildLog(MainForm parent)
        {
            m_Parent = parent;

            InitializeComponent();

            listErrors.MultiSelect = false;

        }

        public void Clear()
        {
            listErrors.Items.Clear();
        }

        public void AddError(int line, string message)
        {
            listErrors.Items.Add(new ListViewItem(new[] { line.ToString(), "", message }));
        }

        public void AddRaw(string line)
        {
            rawLog += line + Environment.NewLine;
        }

        private void BuildLog_Load(object sender, EventArgs e)
        {
        }

        private void listErrors_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selection = listErrors.SelectedItems;

            if (selection.Count > 0)
            {
                int lineNumber;

                if (int.TryParse(selection[0].SubItems[0].Text, out lineNumber))
                {
                    m_Parent.GoTo(lineNumber-1);
                }
            }
        }

        private void BuildLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void BuildLog_Shown(object sender, EventArgs e)
        {
            m_Parent.UpdateViewBuildLog(Visible);
        }

        private void listErrors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveLogDialog.ShowDialog();
        }

        private void saveLogDialog_FileOk(object sender, CancelEventArgs e)
        {
            using (System.IO.FileStream file = System.IO.File.OpenWrite(saveLogDialog.FileName))
            {
                file.Write(Encoding.ASCII.GetBytes(rawLog), 0, rawLog.Count());
            }
        }
    }
}