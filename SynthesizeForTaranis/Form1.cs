﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;

namespace SynthesizeForTaranis
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer speaker;
        List<string> voices = new List<string>();
        string folderName;

        public Form1()
        {
            InitializeComponent();
            speaker = new SpeechSynthesizer();
            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            initialize();
        }

        private void initialize()
        {
            
            foreach (InstalledVoice voice in speaker.GetInstalledVoices())
            {
                voices.Add(voice.VoiceInfo.Name);
            }

            comboBox1.DataSource = voices;
        }

        private void speakButton_Click(object sender, EventArgs e)
        {
            string text = textBox2.Text;
            speaker.SelectVoice(voices[comboBox1.SelectedIndex]);
            speaker.SetOutputToDefaultAudioDevice();
            saveButton.Enabled = false;
            speakButton.Enabled = false;
            speaker.Speak(text);
            saveButton.Enabled = true;
            speakButton.Enabled = true; ;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string fileName = textBox1.Text.Trim();

            if (fileName.Length != 0)
            {
                if (fileName.EndsWith(".wav"))
                {
                    fileName = fileName.Substring(0, fileName.IndexOf(".wav"));
                }
                else
                {
                    textBox1.Text = fileName + ".wav";
                }
                
                if (fileName.Length > 8)
                {
                    string oldName = fileName;
                    fileName = fileName.Substring(0, 8);
                    fileName += ".wav";
                    string warning = "The filename: " + oldName + " is too long.\nMax length for Taranis is 12 characters including file ending.\nFile will be renamed to " + fileName;
                    MessageBox.Show(this, warning);
                }

                if (folderName == null)
                {
                    folderName = Environment.CurrentDirectory;
                }

                string text = textBox2.Text;
                speaker.SelectVoice(voices[comboBox1.SelectedIndex]);
                var speechAudioFormatInfo = new SpeechAudioFormatInfo(EncodingFormat.ULaw, 32000, 8, 1, 16000, 2, null);
                speaker.SetOutputToWaveFile(folderName + "\\" + fileName, speechAudioFormatInfo);
                speaker.Speak(text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                label5.Text = folderName;
            }
        }
    }
}
