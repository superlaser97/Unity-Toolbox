### Unity-Toolbox
Documentation on what and how to use these tools

## Requirements: 
-	Unity 2018.1 and above
-	Half a brain

## SimpleUIDialog
Easy way to show beautifully designed dialog with deletaged buttons

# Usage
In your script call `SimpleUIDialog.ShowDialog()`
You do not need to any prefab on the scene, it will be auto loaded by the script itself and assigned to do not be destroyed.

# Functions
Parameters you can set in ShowDialog()
-	Title
-	Content
-	Buttons (Delegates and Button Text)
-	Show “X” Button
-	Highlighted Button
After any button is clicked, the dialog will automatically close

Additional Parameters that is set on the dialog prefab at Assets/Resource/SUPERLASER/SimpleUIPrompt/SimpleUIPromptCanvas
-	Accent Color (Highlight color)
-	Dialog Scale (Size of the dialog)
-	Animation Spd
