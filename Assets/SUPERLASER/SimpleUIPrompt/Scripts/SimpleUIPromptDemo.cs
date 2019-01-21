using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Author: Low Zhi Heng
/// Date of Creation: 1 Nov 2018
/// Last Updated: 19 Jan 2019
/// </summary>

namespace SUPERLASER
{
    public class SimpleUIPromptDemo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI numbersTxt;

        private int a = 0;
        private int b = 0;

        private void Update()
        {
            numbersTxt.text =
                $"A: {a}" + "\n" +
                $"B: {b}" + "\n";
        }

        // Simple dialog that only needs the title and content parameter
        public void Dialog_Without_Actions()
        {
            SimpleUIPrompt.ShowDialog("Simple UI Dialog", "Hello There!");
        }

        // Simple dialog that has a single button with empty delegates (no action)
        public void Dialog_With_OK_Button()
        {
            List<SimpleUIDialogAction> dialogActions = new List<SimpleUIDialogAction>
            {
                new SimpleUIDialogAction(delegate { }, "Ok")
            };

            SimpleUIPrompt.ShowDialog("Simple UI Dialog", "Hello There again!", dialogActions);
        }

        // Dialog with multiple buttons with delegates
        public void Dialog_With_Actions()
        {
            List<SimpleUIDialogAction> dialogActions = new List<SimpleUIDialogAction>
            {
                new SimpleUIDialogAction(delegate { a++; }, "Increment A"),
                new SimpleUIDialogAction(delegate { a--; }, "Decrement A"),
                new SimpleUIDialogAction(delegate { b++; }, "Increment B"),
                new SimpleUIDialogAction(delegate { b--; }, "Decrement B")
            };

            SimpleUIPrompt.ShowDialog("Simple UI Dialog", "Increment A by 1", dialogActions);
        }

        // Dialog with highlighted button and "X" disabled
        // Highlight a button by writing its index (starts with 1)
        public void Dialog_With_X_Turned_Off_And_Highlighted_Btn()
        {
            List<SimpleUIDialogAction> dialogActions = new List<SimpleUIDialogAction>
            {
                new SimpleUIDialogAction(delegate { b++; }, "Increment B"), // <- Highlighting this Button
                new SimpleUIDialogAction(delegate { b--; }, "Decrement B")
            };

            SimpleUIPrompt.ShowDialog("Simple UI Dialog", "Select your options", dialogActions, 1, false);
        }

        // Dialog with multiple delegates in a button
        public void Dialog_With_Multiple_Delegates()
        {
            List<SimpleUIDialogAction> dialogActions = new List<SimpleUIDialogAction>
            {
                new SimpleUIDialogAction(delegate { a++; b++; }, "YES")
            };

            SimpleUIPrompt.ShowDialog("Simple UI Dialog", "Increment A & B by 1?", dialogActions);
        }

        // Dialog that opens another dialog right after
        public void Dialog_That_Opens_Another_Dialog()
        {
            List<SimpleUIDialogAction> dialogActions = new List<SimpleUIDialogAction>
            {
                new SimpleUIDialogAction(delegate { Dialog_With_OK_Button(); }, "Open another Dialog")
            };

            SimpleUIPrompt.ShowDialog("Simple UI Dialog", "Increment A by 1", dialogActions, 1, false);
        }

        // Multiple calls to ShowDialog()
        public void Multiple_Calls_ToOpen_Dialog()
        {
            Dialog_That_Opens_Another_Dialog();
            Dialog_With_Multiple_Delegates();
        }
    }
}


