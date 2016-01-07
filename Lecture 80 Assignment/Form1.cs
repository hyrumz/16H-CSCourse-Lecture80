using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Lecture_80_Assignment
{
    public partial class Form1 : Form
    {
        // flag variables to assist with selection of text in text boxes.
        bool box1Hasfocus;
        bool box2HasFocus;

        //Dictionary for storing pairs of values as key,value.
        Dictionary<string, string> dict = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();

            //event to assist with selection of text in text boxes.
            textBox1.GotFocus += textBox1_GotFocus;
            textBox1.MouseUp += textBox1_MouseUp;
            textBox1.Leave += textBox1_Leave;
            textBox2.GotFocus += textBox2_GotFocus;
            textBox2.MouseUp += textBox2_MouseUp;
            textBox2.Leave += textBox2_Leave;

            //clear the status label until something actually happens.
            lblStatus.Text = "";
        }


        void textBox1_Leave(object sender, EventArgs e)
        {
            box1Hasfocus = false;
            // deselect text on the way out
            textBox1.SelectionLength = 0;
        }

        void textBox2_Leave(object sender, EventArgs e)
        {
            box2HasFocus = false;

            // deselect text on the way out
            textBox2.SelectionLength = 0;
        }

        void textBox1_GotFocus(object sender, EventArgs e)
        {
            // Select all text only if the mouse isn't down.
            // This makes tabbing to the textbox give focus.
            if (MouseButtons == MouseButtons.None)
            {
                textBox1.SelectAll();
                box1Hasfocus = true;
            }
        }

        void textBox2_GotFocus(object sender, EventArgs e)
        {
            // Select all text only if the mouse isn't down.
            // This makes tabbing to the textbox give focus.
            if (MouseButtons == MouseButtons.None)
            {
                textBox2.SelectAll();
                box2HasFocus = true;
            }
        }

        void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // only select the text on a mouse up event if we are just
            // entering the box and if there is not any text selected.
            if (!box1Hasfocus && textBox1.SelectionLength == 0)
            {
                box1Hasfocus = true;
                textBox1.SelectAll();
            }
        }

        void textBox2_MouseUp(object sender, MouseEventArgs e)
        {
            // only select the text on a mouse up event if we are just
            // entering the box and if there is not any text selected.
            if (!box2HasFocus && textBox2.SelectionLength == 0)
            {
                box2HasFocus = true;
                textBox2.SelectAll();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // check the key and value text boxes to ensure that
            // they are not blank. 
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                // key or value field is blank
                lblStatus.Text = "Key and Value fields cannot be blank.";
            }
            else
            {
                // both the key and the value are not blank
                try
                {
                    // add values to dictionary
                    // if a key already exists with the same name
                    // it will through an error.
                    dict.Add(textBox1.Text, textBox2.Text);

                    // show the user that the add was successful.
                    lblStatus.Text = "Value added, " + dict.Count + " entries total.";
                }
                catch (Exception ex)
                {
                    // it is likely that the user entered a value that is
                    // already used.
                    lblStatus.Text = "Error adding value.\n" + ex.Message;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // opening a file as "AppendText" will create a file if one 
                // does not exist.
                using (StreamWriter writer = File.AppendText(textBoxFile.Text))
                {
                    // write each pair of values starting at the bottom 
                    // of the exiting file, or top of a new file.
                    foreach (KeyValuePair<string, string> pair in dict)
                    {
                        writer.WriteLine(pair.Key + ", " + pair.Value);
                    }
                }
                dict.Clear();
                lblStatus.Text = "File saved. Dictionary cleared.";
            }
            catch (Exception ex)
            {
                // likely the user did not provided a valid path.
                lblStatus.Text = "Error:\n" + ex.Message;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // check to see if the user has ented valid filename before
            // launching notepad. If not, inform the user.
            if (File.Exists(textBoxFile.Text))
            { Process.Start(@"notepad.exe", textBoxFile.Text); }
            else
            { lblStatus.Text = "File not found, check name."; }
        }
    }
}
