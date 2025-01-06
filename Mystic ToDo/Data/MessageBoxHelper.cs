using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using Point = System.Drawing.Point;

public static class MessageBoxHelper
{
    public static void ShowMessageBox(string message)
    {
        // Create a new form to act as the parent of the message box
        Form dummyForm = new Form();
        dummyForm.StartPosition = FormStartPosition.Manual;

        // Get the mouse cursor position
        Point mousePos = Control.MousePosition;

        // Set the form location to the mouse position
        dummyForm.Location = mousePos;

        // Display the message box with the dummy form as its owner
        System.Windows.Forms.MessageBox.Show(dummyForm, message);

        // Dispose of the dummy form
        dummyForm.Dispose();
    }

    public static string PromptMessageBox(string message, string title = "Input")
    {
        // Create a new form to act as the parent of the message box
        Form dummyForm = new Form();
        dummyForm.StartPosition = FormStartPosition.Manual;

        // Get the mouse cursor position
        Point mousePos = Control.MousePosition;

        // Set the form location to the mouse position
        dummyForm.Location = mousePos;

        // Create a new input form
        Form inputForm = new Form();
        inputForm.Text = title;
        inputForm.StartPosition = FormStartPosition.Manual;
        inputForm.Location = dummyForm.Location;
        inputForm.Width = 300;
        inputForm.Height = 150;

        // Create a label for the prompt message
        System.Windows.Forms.Label label = new System.Windows.Forms.Label() { Left = 10, Top = 20, Text = message, Width = 260 };
        inputForm.Controls.Add(label);

        // Create a textbox for user input
        System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox() { Left = 10, Top = 50, Width = 260 };
        inputForm.Controls.Add(textBox);

        // Create OK and Cancel buttons
        System.Windows.Forms.Button okButton = new System.Windows.Forms.Button() { Text = "OK", Left = 100, Width = 80, Top = 80 };
        System.Windows.Forms.Button cancelButton = new System.Windows.Forms.Button() { Text = "Cancel", Left = 190, Width = 80, Top = 80 };
        okButton.DialogResult = DialogResult.OK;
        cancelButton.DialogResult = DialogResult.Cancel;
        inputForm.Controls.Add(okButton);
        inputForm.Controls.Add(cancelButton);

        // Set the input form's accept and cancel buttons
        inputForm.AcceptButton = okButton;
        inputForm.CancelButton = cancelButton;

        // Display the input form and get the result
        DialogResult result = inputForm.ShowDialog(dummyForm);

        // Dispose of the dummy form
        dummyForm.Dispose();

        // Return the user input if OK was clicked, otherwise return null
        if (result == DialogResult.OK)
        {
            return textBox.Text;
        }
        else
        {
            return null;
        }
    }
}
