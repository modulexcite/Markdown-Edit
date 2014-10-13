﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MarkdownEdit.Properties;

namespace MarkdownEdit
{
    public partial class Editor
    {
        private string TextSwap { get; set; }

        public Editor()
        {
            InitializeComponent();
            EditorBox.Loaded += EditorBoxOnLoaded;
        }

        private void EditorBoxOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            LoadFile(Settings.Default.LastOpenFile);
            EditorBox.TextChanged += EditorBoxOnTextChanged;
            EditorBox.Dispatcher.InvokeAsync(() => EditorBoxOnTextChanged(this, null));
        }

        private void EditorBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            MainWindow.UpdatePreviewCommand.Execute(EditorBox.Text, this);
        }

        public void OpenFileHandler()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) return;
            LoadFile(dialog.FileNames[0]);
        }

        private void LoadFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return;
            EditorBox.Text = File.ReadAllText(file);
            Settings.Default.LastOpenFile = file;
        }

        public void ToggleHelp()
        {
            if (TextSwap == null)
            {
                TextSwap = EditorBox.Text;
                EditorBox.Text = Properties.Resources.Help;
                EditorBox.IsReadOnly = true;
            }
            else
            {
                EditorBox.IsReadOnly = false;
                EditorBox.Text = TextSwap;
                TextSwap = null;
            }
        }

        public void WordWrapHandler()
        {
            var wrap = EditorBox.TextWrapping;
            EditorBox.TextWrapping = wrap == TextWrapping.Wrap ? TextWrapping.NoWrap : TextWrapping.Wrap;
        }
    
        private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs scrollChangedEventArgs)
        {
            MainWindow.ScrollPreviewCommand.Execute(EditorBox.VerticalOffset, this);
        }
    }
}